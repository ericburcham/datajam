namespace DataJam.Testing;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

internal sealed class RepresentationRepository
{
    public Dictionary<Type, Action<object>> IdentityStrategies { get; set; } = new();

    internal IList<Representation> Representations { get; } = new List<Representation>();

    internal void Add<T>(T item)
        where T : class
    {
        if (Exists(item))
        {
            return;
        }

        var representation = new Representation(item, AddRelatedObjects(item));

        Representations.Add(representation);
        UpdateExistingRepresentations(representation);
    }

    internal void Commit()
    {
        RemoveOrphans();
        FindChanges();
        ApplyIdentities();
    }

    internal bool Exists(object item)
    {
        return Representations.Any(x => x.Entity == item);
    }

    internal IQueryable<T> GetRepresentations<T>()
    {
        return Representations.Where(x => x.Entity is T).Select(x => x.Entity).Cast<T>().AsQueryable();
    }

    internal bool Remove<T>(T item)
        where T : class
    {
        var success = false;
        var matchingRepresentations = Representations.Where(x => x.Entity == item).ToList();

        foreach (var representation in matchingRepresentations)
        {
            success = Representations.Remove(representation);

            if (!success)
            {
                throw new InvalidDataException("Object was not removed");
            }

            foreach (var parent in representation.Parents)
            {
                parent.Value.Remover();
            }

            foreach (var related in representation.GetRelated())
            {
                if (related.Parents.Count == 1)
                {
                    success = Representations.Remove(related);
                }
                else
                {
                    related.Parents[item].Remover();
                }

                if (!success)
                {
                    throw new InvalidDataException("Dependent Object was not removed");
                }
            }
        }

        return success;
    }

    private IEnumerable<Representation> AddRelatedObjects<T>(T item)
    {
        var reps = GetSingleRelationship(item).ToList();
        reps.AddRange(GetEnumerableRelationships(item));

        return reps;
    }

    private void ApplyIdentities()
    {
        foreach (var entity in Representations.Select(x => x.Entity))
        {
            ApplyIdentity(entity);
        }
    }

    private void ApplyIdentity<T>(T item)
        where T : class
    {
        var itemType = item.GetType();
        var itemTypes = new List<Type>(itemType.GetInterfaces()) { itemType };

        var identityStrategy = IdentityStrategies.Keys.Intersect(itemTypes).FirstOrDefault();

        if (identityStrategy != null)
        {
            IdentityStrategies[identityStrategy](item);
        }
    }

    private Representation CreateChildObjectRepresentation(object item, object parent, Action removeAction, Func<object, object, object> getterFunc)
    {
        if (Exists(item))
        {
            var objectRepresentation = Representations.Single(x => x.Entity == item);

            if (!objectRepresentation.Parents.ContainsKey(parent))
            {
                objectRepresentation.Parents.Add(parent, new(removeAction, getterFunc));
            }

            return objectRepresentation;
        }
        else
        {
            var objectRepresentation = new Representation(item, AddRelatedObjects(item)) { Parents = new() { { parent, new(removeAction, getterFunc) } } };

            Representations.Add(objectRepresentation);

            return objectRepresentation;
        }
    }

    private object CreateGenericList(Type type)
    {
        var listType = typeof(List<>);
        Type[] typeArgs = [type];
        var genericType = listType.MakeGenericType(typeArgs);

        return Activator.CreateInstance(genericType)!;
    }

    private Func<object, object, object> CreateGetterFromCollectionFunc(PropertyInfo propertyInfo, object childItem)
    {
        return (parent, child) =>
        {
            var value = propertyInfo.GetValue(parent, null);

            if (value == null)
            {
                return null!;
            }

            var collection = (IEnumerable)value;

            return collection.Cast<object>().FirstOrDefault(item => item == child)!;
        };
    }

    private Action CreateRemoveFromCollectionAction(PropertyInfo propertyInfo, object item, object childItem)
    {
        return () =>
        {
            var items = propertyInfo.GetValue(item, null);

            if (items == null)
            {
                return;
            }

            var list = CreateGenericList(childItem.GetType());
            var addMethod = list.GetType().GetMethod("Add");
            var childItems = (IEnumerable)items;

            foreach (var itemInList in childItems)
            {
                if (itemInList != childItem)
                {
                    addMethod!.Invoke(list, [itemInList]);
                }
            }

            propertyInfo.SetValue(item, list, null);
        };
    }

    private void FindChanges()
    {
        var objectRepresentations = Representations.Where(x => x.Parents.Count == 0).ToList();

        foreach (var root in objectRepresentations)
        {
            root.RelatedEntities = AddRelatedObjects(root.Entity);

            foreach (var objRep in root.GetRelated().Where(x => x.Parents.Count == 1 && !Representations.Contains(x)))
            {
                Representations.Add(objRep);
            }
        }
    }

    private IEnumerable<Representation> GetEnumerableRelationships<T>(T item)
    {
        var representations = new List<Representation>();

        var enumerableProperties = item!.GetType()
                                        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                        .Where(x => x.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(x.PropertyType) && x.GetValue(item, null) != null);

        foreach (var property in enumerableProperties)
        {
            var childCollection = (IEnumerable)property.GetValue(item, null)!;

            foreach (var child in childCollection)
            {
                var remover = CreateRemoveFromCollectionAction(property, item, child);
                var getter = CreateGetterFromCollectionFunc(property, child);
                var childTypeRepresentation = CreateChildObjectRepresentation(child, item, remover, getter);
                representations.Add(childTypeRepresentation);
            }
        }

        return representations;
    }

    private IEnumerable<Representation> GetSingleRelationship<T>(T item)
    {
        var representations = new List<Representation>();

        var singleProperties = item!.GetType()
                                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                    .Where(x => x.PropertyType.IsClass && !typeof(IEnumerable).IsAssignableFrom(x.PropertyType) && x.GetValue(item, null) != null);

        foreach (var property in singleProperties)
        {
            object Getter(object parent, object kid)
            {
                return property.GetValue(parent, null)!;
            }

            void Remover()
            {
                property.SetValue(item, null, null);
            }

            var childValue = property.GetValue(item, null);
            var childTypeRepresentation = CreateChildObjectRepresentation(childValue!, item, Remover, Getter);
            representations.Add(childTypeRepresentation);
        }

        return representations;
    }

    private void RemoveOrphans()
    {
        var roots = Representations.Where(x => x.Parents.None()).ToList();

        foreach (var root in roots)
        {
            var orphans = root.GetRelatedOrphans();

            foreach (var orphan in orphans)
            {
                Representations.Remove(orphan);
            }
        }
    }

    private void UpdateExistingRepresentations(Representation representation)
    {
        var entityType = representation.Entity.GetType();
        var nonPrimitiveProperties = entityType.GetProperties().Where(x => !x.PropertyType.IsPrimitive).ToList();
        var typesCurrentlyStored = representation.RelatedEntities.Select(x => x.Entity.GetType()).ToList();
        var referencedProperties = new List<object>();

        foreach (var property in nonPrimitiveProperties)
        {
            if (typesCurrentlyStored.Contains(property.PropertyType.ToSingleType()))
            {
                if (property.PropertyType.IsEnumerable())
                {
                    var values = (IEnumerable)property.GetValue(representation.Entity, null)!;
                    referencedProperties.AddRange(values.Cast<object>());
                }
                else
                {
                    referencedProperties.Add(property.GetValue(representation.Entity, null)!);
                }
            }
        }

        foreach (var data in representation.RelatedEntities.Where(x => typesCurrentlyStored.Contains(x.Entity.GetType())))
        {
            if (!referencedProperties.Contains(data.Entity))
            {
                continue;
            }

            var collectionType = typeof(ICollection<>).MakeGenericType(entityType);

            var propertiesThatReferToRepresentation =
                data.Entity.GetType().GetProperties().Where(x => x.PropertyType == entityType || x.PropertyType.IsAssignableFrom(collectionType));

            var addMethod = collectionType.GetMethod("Add");
            var propertyInfos = propertiesThatReferToRepresentation.ToList();

            if (!propertyInfos.Any() || propertyInfos.Count > 1)
            {
                return;
            }

            var referencingProperty = propertyInfos.Single();

            if (referencingProperty.PropertyType.IsAssignableFrom(collectionType))
            {
                var collection = referencingProperty.GetValue(data.Entity, null);

                if (collection == null)
                {
                    var listType = typeof(List<>).MakeGenericType(entityType);
                    referencingProperty.SetValue(data.Entity, Activator.CreateInstance(listType), null);
                    collection = referencingProperty.GetValue(data.Entity, null);
                }

                addMethod!.Invoke(collection, [representation.Entity]);
            }
            else
            {
                referencingProperty.SetValue(data.Entity, representation.Entity, null);
            }
        }
    }
}

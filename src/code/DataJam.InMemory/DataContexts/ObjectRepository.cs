using System.Collections;
using System.Reflection;
using DataJam.Extensions;

namespace DataJam.InMemory;

internal sealed class ObjectRepository
{
    private readonly IList<Representation> _representations = new List<Representation>();

    internal void Add<T>(T? item) where T : class?
    {
        if (EntityExistsInRepository(item))
        {
            return;
        }

        var representation = new Representation
        {
            Entity = item
        };
        
        _representations.Add(representation);
        representation.RelatedEntities = AddRelatedObjects(item);
        UpdateExistingRepresentations(representation);
    }

    private IEnumerable<Representation> AddRelatedObjects<T>(T item)
    {
        var representations = new List<Representation>();

        representations.AddRange(GetSingularRelationships(item));
        
        representations.AddRange(GetMultipleRelationships(item));

        return representations;
    }

    private void ApplyIdentityStrategies()
    {
        foreach (var entity in _representations.Select(x => x.Entity))
        {
            ApplyIdentityStrategy(entity);
        }
    }

    private void ApplyIdentityStrategy<T>(T item)
        where T : class
    {
        var type = item.GetType();
        var types = new List<Type>(type.GetInterfaces())
        {
            type
        };

        var intersectingType = IdentityStrategies.Keys.Intersect(types).FirstOrDefault();
        if (intersectingType != null)
        {
            IdentityStrategies[intersectingType](item);
        }
    }

    private void CleanGraph()
    {
        var objectRepresentations = _representations.Where(x => x.Parents.Count == 0).ToList();
        foreach (var root in objectRepresentations)
        {
            var orphans = root.GetObjectRepresentationsToPrune();
            foreach (var objectRepresentation in orphans)
            {
                _representations.Remove(objectRepresentation);
            }
        }
    }

    public void Commit()
    {
        CleanGraph();
        FindChanges();
        ApplyIdentityStrategies();
    }

    private Representation CreateChildObjectRepresentation(
        object? item,
        object? parent = null,
        Action? removeAction = null,
        Func<object, object, object>? getterFunc = null)
    {
        throw new NotImplementedException();
    }

    private object? CreateGenericList(Type type)
    {
        var listType = typeof(List<>);
        Type[] typeArgs = { type };
        var genericType = listType.MakeGenericType(typeArgs);

        return Activator.CreateInstance(genericType);
    }

    private Func<object, object, object?> CreateGetterFromCollectionFunc(PropertyInfo propertyInfo, object childItem)
    {
        return (parent, child) =>
        {
            var value = propertyInfo.GetValue(parent, null);
            if (value == null)
            {
                return null;
            }

            var collection = (IEnumerable)value;

            return collection.Cast<object>().FirstOrDefault(item => item == child);
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
            var mListAdd = list!.GetType().GetMethod("Add");
            var childItems = (IEnumerable)items;
            foreach (var itemInList in childItems)
            {
                if (itemInList != childItem)
                {
                    mListAdd!.Invoke(list, new[] { itemInList });
                }
            }

            propertyInfo.SetValue(item, list, null);
        };
    }

    private bool EntityExistsInRepository(object? item)
    {
        return _representations.Any(x => x.Entity == item);
    }

    private void FindChanges()
    {
        var objectRepresentations = _representations.Where(x => x.Parents.Count == 0).ToList();
        foreach (var root in objectRepresentations)
        {
            root.RelatedEntities = AddRelatedObjects(root.Entity);
            foreach (var objRep in root.AllRelated().Where(x => x.Parents.Count == 1 && !_representations.Contains(x)))
            {
                _representations.Add(objRep);
            }
        }
    }

    private IEnumerable<Representation> GetMultipleRelationships<T>(T item)
    {
        if (item is null)
        {
            return Enumerable.Empty<Representation>();
        }
        
        var reps = new List<Representation>();
        var properties =
            item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(
                    x => x.PropertyType != typeof(string)
                         && typeof(IEnumerable).IsAssignableFrom(x.PropertyType)
                         && x.GetValue(item, null) != null);

        foreach (var propertyInfo in properties)
        {
            var childCollection = (IEnumerable)propertyInfo.GetValue(item, null)!;
            foreach (var child in childCollection)
            {
                var removeAction = CreateRemoveFromCollectionAction(propertyInfo, item, child);
                var getterFunc = CreateGetterFromCollectionFunc(propertyInfo, child);
                var childTypeRepresentation = CreateChildObjectRepresentation(child, item, removeAction, getterFunc!);
                reps.Add(childTypeRepresentation);
            }
        }

        return reps;
    }

    private IEnumerable<Representation> GetSingularRelationships<T>(T item)
    {
        if (item is null)
        {
            return Enumerable.Empty<Representation>();
        }
        
        var representations = new List<Representation>();
        var properties = item.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(x => x.PropertyType.IsClass
                        && !typeof(IEnumerable).IsAssignableFrom(x.PropertyType)
                        && x.GetValue(item, null) != null);

        foreach (var property in properties)
        {
            object? Getter(object parent, object kid)
            {
                return property.GetValue(parent, null);
            }

            void Remover()
            {
                property.SetValue(item, null, null);
            }

            var child = property.GetValue(item, null);
            var childTypeRepresentation = CreateChildObjectRepresentation(child, item, Remover, Getter!);
            representations.Add(childTypeRepresentation);
        }

        return representations;
    }

    public Dictionary<Type, Action<object>> IdentityStrategies { get; } = new();

    private void UpdateExistingRepresentations(Representation rep)
        {
            var type = rep.Entity!.GetType();
            var nonPrimitivePropertiesFromObject = type.GetProperties().Where(x => !x.PropertyType.IsPrimitive).ToList();
            var typesCurrentlyStored = rep.RelatedEntities!.Select(x => x.Entity!.GetType()).ToList();
            var referencedProperties = new List<object>();
            foreach (var info in nonPrimitivePropertiesFromObject)
            {
                if (typesCurrentlyStored.Contains(info.PropertyType.ToSingleType()))
                {
                    if (info.PropertyType.IsEnumerable())
                    {
                        var values = (IEnumerable)info.GetValue(rep.Entity, null)!;
                        referencedProperties.AddRange(values.Cast<object>());
                    }
                    else
                    {
                        referencedProperties.Add(info.GetValue(rep.Entity, null)!);
                    }
                }
            }

            foreach (var data in rep.RelatedEntities!.Where(x => typesCurrentlyStored.Contains(x.Entity!.GetType())))
            {
                if (!referencedProperties.Contains(data.Entity!))
                {
                    continue;
                }

                var collectionType = typeof(ICollection<>).MakeGenericType(type);
                var propertiesThatReferToRepresentation =
                    data.Entity!.GetType()
                        .GetProperties()
                        .Where(x => x.PropertyType == type || x.PropertyType.IsAssignableFrom(collectionType));

                var addMethod = collectionType.GetMethod("Add");
                var propertyInfos = propertiesThatReferToRepresentation.ToList();
                if (!propertyInfos.Any() || propertyInfos.Count() > 1)
                {
                    return;
                }

                var referencingProperty = propertyInfos.Single();
                if (referencingProperty.PropertyType.IsAssignableFrom(collectionType))
                {
                    var collection = referencingProperty.GetValue(data.Entity, null);
                    if (collection == null)
                    {
                        var listType = typeof(List<>).MakeGenericType(type);
                        referencingProperty.SetValue(data.Entity, Activator.CreateInstance(listType), null);
                        collection = referencingProperty.GetValue(data.Entity, null);
                    }

                    addMethod!.Invoke(collection, new[] { rep.Entity });
                }
                else
                {
                    referencingProperty.SetValue(data.Entity, rep.Entity, null);
                }
            }
        }
}
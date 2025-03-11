namespace DataJam.Testing;

using System.Collections.Generic;
using System.Linq;

internal class Representation(object entity, IEnumerable<Representation> relatedEntities)
{
    internal object Entity { get; } = entity;

    internal Dictionary<object, Accessor> Parents { get; set; } = new();

    internal IEnumerable<Representation> RelatedEntities { get; set; } = relatedEntities;

    public List<Representation> GetRelatedOrphans()
    {
        return GetRelated().Where(x => x.IsOrphaned()).ToList();
    }

    public bool IsOrphaned()
    {
        return !Parents.Any() || Parents.All(accessor => accessor.Value.Getter(accessor.Key, Entity) == null);
    }

    public bool IsType<T>()
    {
        return Entity.GetType() is T;
    }

    internal IEnumerable<Representation> GetRelated()
    {
        var evaluatedObjects = new List<Representation>();

        return GetRelated(evaluatedObjects);
    }

    internal IEnumerable<Representation> GetRelated(List<Representation> evaluatedObjects)
    {
        var items = RelatedEntities.ToList();

        foreach (var objectRepresentationBase in RelatedEntities)
        {
            if (evaluatedObjects.Contains(objectRepresentationBase))
            {
                continue;
            }

            evaluatedObjects.Add(objectRepresentationBase);
            items.AddRange(objectRepresentationBase.GetRelated(evaluatedObjects));
        }

        return items;
    }
}

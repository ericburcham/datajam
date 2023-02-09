namespace DataJam.InMemory;

internal class Representation
{
    public object? Entity { get; set; }
    public IEnumerable<Representation>? RelatedEntities { get; set; }

    internal Dictionary<object, Accessor> Parents { get; set; } = new();

    public List<Representation> GetObjectRepresentationsToPrune()
    {
        return AllRelated().Where(x => x.Orphaned()).ToList();
    }

    public bool Orphaned()
    {
        if (!Parents.Any())
        {
            return true;
        }

        return
            Parents.All(
                accessor =>
                    accessor.Value == null || accessor.Value.GetterFunc == null ||
                    accessor.Value.GetterFunc(accessor.Key, Entity) == null);
    }

    internal IEnumerable<Representation> AllRelated()
    {
        var evaluatedObjects = new List<Representation>();

        return AllRelated(evaluatedObjects);
    }
    
    internal IEnumerable<Representation> AllRelated(List<Representation> evaluatedObjects)
    {
        var items = RelatedEntities.ToList();
        foreach (var objectRepresentationBase in RelatedEntities)
        {
            if (evaluatedObjects.Contains(objectRepresentationBase))
            {
                continue;
            }

            evaluatedObjects.Add(objectRepresentationBase);
            items.AddRange(objectRepresentationBase.AllRelated(evaluatedObjects));
        }

        return items;
    }


}
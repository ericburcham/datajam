namespace DataJam.TestSupport.TestPatterns.Family;

public class Child : Person
{
    public Father Father { get; private set; } = null!;

    public long FatherId { get; set; }

    public long MotherId { get; set; }

    public Mother Mother { get; private set; } = null!;

    public void AddParents(Father father, Mother mother)
    {
        father.AddChild(this);
        mother.AddChild(this);

        Father = father;
        Mother = mother;
    }
}

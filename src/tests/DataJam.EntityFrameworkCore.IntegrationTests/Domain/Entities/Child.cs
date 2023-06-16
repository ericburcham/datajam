namespace DataJam.EntityFrameworkCore.IntegrationTests.Domain;

public class Child : Person
{
    public Father Father { get; set; }

    public Mother Mother { get; set; }
}
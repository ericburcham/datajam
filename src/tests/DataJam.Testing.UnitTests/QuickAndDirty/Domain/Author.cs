namespace DataJam.Testing.UnitTests.QuickAndDirty.Domain;

using System;

public class Author
{
    public string? Email { get; set; }

    public string? FirstName { get; set; }

    public Guid Id { get; set; } = Guid.NewGuid();

    public string? LastName { get; set; }

    public string? TwitterHandle { get; set; }
}

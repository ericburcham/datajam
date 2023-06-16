﻿namespace DataJam.EntityFrameworkCore.IntegrationTests.Domain;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ChildMapping : FamilyMapping<Child>
{
    public override void Configure(EntityTypeBuilder<Child> builder)
    {
        builder.ToTable(nameof(Child), SCHEMA).HasKey(child => child.Id);
    }
}
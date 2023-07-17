﻿namespace DataJam.EntityFrameworkCore.IntegrationTests.Domains.Family;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TestSupport.EntityFrameworkCore.Domains.Family;

public class FatherMapping : FamilyMapping<Father>
{
    public override void Configure(EntityTypeBuilder<Father> builder)
    {
        builder.ToTable(nameof(Father), SCHEMA).HasKey(father => father.Id);
    }
}

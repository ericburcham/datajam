namespace DataJam.EntityFramework.MySql.IntegrationTests;

using System.Data.Entity;

using Domains;

using global::MySql.Data.EntityFramework;

[DbConfigurationType(typeof(MySqlEFConfiguration))]
internal class MySqlDomainContext(Domain domain) : DomainContext<Domain>(domain);

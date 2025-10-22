# DataJam Python

DataJam provides abstraction patterns for data access across different ORM technologies in Python. This is a Python port of the .NET DataJam library, following domain-driven design principles with clear separation of concerns.

## Features

- **Domain-Driven Architecture**: Organize data access around domains that encapsulate related entities and configuration
- **Multi-ORM Support**: Currently supports SQLAlchemy with extensible architecture for other ORMs
- **Command/Query Pattern**: Separate command objects for operations that don't return values
- **Repository Pattern**: Abstract data access through repository interfaces
- **Unit of Work Pattern**: Manage transaction boundaries and change tracking
- **Oracle Support**: First-class support for Oracle databases with proper case sensitivity handling
- **Async/Await Support**: Modern Python async patterns throughout the stack
- **Type Safety**: Full type hints and mypy compatibility

## Quick Start

```python
import asyncio
from sqlalchemy.ext.asyncio import create_async_engine
from datajam_sqlalchemy import Domain, DataContext
from datajam import IQuery

# Define your entities using SQLAlchemy
from sqlalchemy import Column, Integer, String
from sqlalchemy.ext.declarative import declarative_base

Base = declarative_base()

class User(Base):
    __tablename__ = 'users'
    id = Column(Integer, primary_key=True)
    name = Column(String(50))

# Create your domain
class UserDomain(Domain):
    def __init__(self, engine):
        super().__init__(
            connection_string=str(engine.url),
            engine=engine,
            metadata=Base.metadata
        )

# Define queries
class GetAllUsers(IQuery[User]):
    async def execute(self, data_source):
        queryable = data_source.create_query(User)
        return await queryable.to_list()

# Usage
async def main():
    engine = create_async_engine("oracle+oracledb_async://user:pass@host/service")
    domain = UserDomain(engine)

    # Create schema
    await domain.create_tables()

    # Use repository
    async with await domain.create_data_context() as context:
        repository = Repository(context)

        # Add a user
        user = User(name="John Doe")
        context.add(user)
        await context.commit()

        # Query users
        users = await repository.find(GetAllUsers())
        print(f"Found {len(users)} users")

asyncio.run(main())
```

## Installation

```bash
# Basic installation
pip install datajam

# With SQLAlchemy support
pip install datajam[sqlalchemy]

# For development
pip install datajam[dev]
```

## Development

This project uses Nox for build automation (equivalent to NUKE in .NET):

```bash
# Setup development environment
nox -s dev-setup

# Run tests
nox -s test

# Run integration tests (requires Docker)
nox -s test-integration

# Run linting
nox -s lint

# Format code
nox -s format

# Full build pipeline
nox -s datajam
```

## Architecture

The library follows the same architectural patterns as the .NET DataJam:

### Core Abstractions (`datajam` package)
- `IDataContext` - Primary abstraction combining data source, unit of work, and context management
- `IRepository` - Repository pattern for command and query execution
- `IUnitOfWork` - Transaction boundary management
- `IQuery[T]` / `IScalar[T]` - Query pattern interfaces
- `ICommand` - Command pattern for operations without return values
- `IDomain` - Domain configuration and mapping

### SQLAlchemy Implementation (`datajam_sqlalchemy` package)
- `DataContext` - SQLAlchemy-based implementation with async session management
- `Domain` - SQLAlchemy domain configuration
- `Repository` - Repository implementation
- `QueryableImpl` - Fluent query API implementation
- Oracle-specific utilities for case sensitivity and sequences

### Testing Infrastructure
- TestContainers integration for database testing
- Family domain test entities (Person, Father, Mother, Child)
- Pytest fixtures and async test support

## Oracle Database Support

DataJam Python provides first-class Oracle support with:

```python
from datajam_sqlalchemy import create_oracle_connection_string, OracleNamingConvention

# Oracle connection
connection_string = create_oracle_connection_string(
    host="localhost",
    port=1521,
    service_name="XE",
    username="system",
    password="oracle"
)

# Oracle naming conventions (automatic uppercase conversion)
class OracleDomain(Domain):
    def __init__(self, engine):
        super().__init__(
            connection_string=str(engine.url),
            engine=engine,
            mapping_configurator=OracleMappingConfigurator()
        )

class OracleMappingConfigurator(IConfigureDomainMappings[MetaData]):
    def configure(self, metadata):
        OracleNamingConvention.configure_metadata_for_oracle(metadata)
```

## Testing

Integration tests use TestContainers for database provisioning:

```bash
# Run all tests
nox -s test-all

# Run only Oracle integration tests
nox -s test-integration -- tests/integration/oracle/

# Run with coverage
nox -s test-all -- --cov-report=html
```

### Running Oracle Tests

The Oracle integration tests require Docker and will automatically:
1. Start an Oracle Free container
2. Create test schema and tables
3. Run integration tests
4. Clean up containers

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Run the full test suite: `nox -s datajam`
5. Submit a pull request

## License

MIT License - see LICENSE file for details.
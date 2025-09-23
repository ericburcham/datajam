# DataJam Python

DataJam provides abstraction patterns for data access across different ORM technologies in Python. This is a Python port of the .NET DataJam library, following domain-driven design principles with clear separation of concerns.

## Features

- **Domain-Driven Architecture**: Organize data access around domains that encapsulate related entities and configuration
- **Multi-ORM Support**: Currently supports SQLAlchemy with extensible architecture for other ORMs
- **Command/Query Pattern**: Separate command objects for operations that don't return values
- **Repository Pattern**: Abstract data access through repository interfaces
- **Unit of Work Pattern**: Manage transaction boundaries and change tracking
- **Oracle Support**: First-class support for Oracle databases with proper case sensitivity handling

## Quick Start

```python
from datajam import IRepository, IDataContext
from datajam_sqlalchemy import DataContext, Domain

# Define your domain
class MyDomain(Domain):
    def configure_mappings(self, metadata):
        # Configure your entity mappings
        pass

# Use the repository
domain = MyDomain(connection_string="your_connection_string")
with DataContext(domain) as context:
    repository = context.repository

    # Execute commands and queries
    result = repository.find(MyQuery())
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

- **Core abstractions** (`datajam` package): Interfaces and base classes
- **SQLAlchemy implementation** (`datajam_sqlalchemy` package): SQLAlchemy-specific implementations
- **Testing utilities** (`datajam_testing` package): Test helpers and patterns

## Testing

Integration tests use TestContainers for database provisioning, ensuring tests run against real database instances:

```bash
# Run all tests
nox -s test-all

# Run only Oracle integration tests
nox -s test-integration -- tests/integration/oracle/
```

## License

MIT License - see LICENSE file for details.
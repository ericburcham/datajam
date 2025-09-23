# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build and Development Commands

The project uses NUKE for build automation. The primary build commands are:

- `./build.cmd` or `./build.sh` - Main build script (automatically installs required .NET SDK)
- `./build.cmd Compile` - Compile the solution
- `./build.cmd Test` - Run all tests
- `./build.cmd Pack` - Create NuGet packages (runs tests first)
- `./build.cmd Clean` - Clean build artifacts
- `./build.cmd Restore` - Restore NuGet packages and tools

Standard .NET CLI commands work within the src/ directory:
- `dotnet build src/datajam.sln` - Build the solution
- `dotnet test src/datajam.sln` - Run all tests
- `dotnet test src/tests/DataJam.Testing.UnitTests/` - Run specific test project

## Code Architecture

DataJam is a .NET library providing abstraction patterns for data access across different ORM technologies. The architecture follows domain-driven design principles with clear separation of concerns.

### Core Concepts

**Domain-Driven Architecture**: The library is organized around domains that encapsulate related entities and their data access configuration. Each domain provides:
- Entity mappings and configuration
- Data context abstractions
- Command/query pattern implementations

**Multi-ORM Support**: The project supports multiple data access technologies:
- Entity Framework Core (`DataJam.EntityFrameworkCore`)
- Entity Framework 6 (`DataJam.EntityFramework`)
- Core abstractions (`DataJam`)
- Testing utilities (`DataJam.Testing`)

**Command/Query Pattern**: Uses command objects that implement `ICommand` interface for operations that don't return values, with execution through `IUnitOfWork`.

### Project Structure

```
src/
├── code/                           # Core library projects
│   ├── DataJam/                    # Core abstractions and patterns
│   ├── DataJam.EntityFrameworkCore/# EF Core implementations
│   ├── DataJam.EntityFramework/    # EF6 implementations
│   └── DataJam.Testing/           # Testing utilities
├── test-support/                  # Test infrastructure libraries
└── tests/                         # Test projects (unit and integration)
```

**Key Interfaces**:
- `IDataContext` - Represents a disposable unit of work for read/write operations
- `IDomain<TConfigurationBinder, TConfigurationOptions>` - Domain configuration and mapping
- `ICommand` - Command pattern for operations without return values
- `IUnitOfWork` - Transaction boundary abstraction

### Configuration and Standards

- **Target Frameworks**: net8.0 and net9.0
- **Nullable Reference Types**: Enabled
- **StyleCop**: Enforced code style with custom ruleset
- **Central Package Management**: Versions managed in `Directory.Packages.props`
- **Documentation**: XML documentation required for public APIs

## Testing

The project includes comprehensive test coverage:

**Unit Tests**: Located in `src/tests/DataJam.Testing.UnitTests/`
**Integration Tests**: Database-specific integration tests for different providers:
- SQL Server: `DataJam.EntityFrameworkCore.MsSql.IntegrationTests`
- MySQL: `DataJam.EntityFrameworkCore.MySql.IntegrationTests`
- SQLite: `DataJam.EntityFrameworkCore.Sqlite.IntegrationTests`

Integration tests use Testcontainers for database provisioning.

## Branching and CI/CD

- **Main Branch**: `main`
- **Development Branch**: `develop`
- **Feature Branches**: `feature/*`
- **CI**: GitHub Actions workflow runs on all pushes
- **Package Publishing**: Automated NuGet publishing from main branch when `NuGetApiKey` is available

The build system uses GitVersion for semantic versioning and automatically creates pre-release packages for non-main branches.
"""DataJam - Abstraction patterns for data access across different ORM technologies.

This package provides domain-driven design patterns and abstractions for data access
that work across different ORM technologies, similar to the .NET DataJam library.
"""

from .__about__ import __version__
from .exceptions import (
    CommandException,
    ConcurrencyException,
    ConfigurationException,
    DataJamException,
    DomainException,
    EntityNotFoundException,
    QueryException,
    RepositoryException,
    TransactionException,
    UnitOfWorkException,
)
from .interfaces import (
    Command,
    Domain,
    ICommand,
    IConfigureDomainMappings,
    IDataContext,
    IDataSource,
    IDomain,
    IDomainRepositoryFactory,
    IQuery,
    IQueryable,
    IQueryWithProjection,
    IRepository,
    IScalar,
    IUnitOfWork,
    Query,
    Scalar,
)

__all__ = [
    "__version__",
    # Core interfaces
    "ICommand",
    "IQuery",
    "IScalar",
    "IQueryWithProjection",
    "IDataSource",
    "IQueryable",
    "IUnitOfWork",
    "IDataContext",
    "IRepository",
    "IConfigureDomainMappings",
    "IDomain",
    "IDomainRepositoryFactory",
    # Base classes
    "Command",
    "Query",
    "Scalar",
    "Domain",
    # Exceptions
    "DataJamException",
    "ConfigurationException",
    "RepositoryException",
    "QueryException",
    "CommandException",
    "DomainException",
    "UnitOfWorkException",
    "EntityNotFoundException",
    "ConcurrencyException",
    "TransactionException",
]

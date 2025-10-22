"""Core interfaces for DataJam - Python port of .NET DataJam abstractions."""

from __future__ import annotations

from abc import ABC, abstractmethod
from typing import TYPE_CHECKING, Any, Generic, Protocol, TypeVar, runtime_checkable

if TYPE_CHECKING:
    from collections.abc import AsyncIterator

# Type variables for generic interfaces
T = TypeVar("T")
TEntity = TypeVar("TEntity")
TResult = TypeVar("TResult")
TSelection = TypeVar("TSelection")
TProjection = TypeVar("TProjection")
TConfigurationBinder = TypeVar("TConfigurationBinder")
TConfigurationOptions = TypeVar("TConfigurationOptions")


@runtime_checkable
class ICommand(Protocol):
    """Command pattern interface for operations that don't return values."""

    @abstractmethod
    async def execute(self, unit_of_work: IUnitOfWork) -> None:
        """Execute the command with the given unit of work."""
        ...


@runtime_checkable
class IQuery(Protocol, Generic[T]):
    """Query pattern interface for operations that return collections."""

    @abstractmethod
    async def execute(self, data_source: IDataSource) -> list[T]:
        """Execute the query against the data source."""
        ...


@runtime_checkable
class IScalar(Protocol, Generic[T]):
    """Scalar query interface for operations that return single values."""

    @abstractmethod
    async def execute(self, data_source: IDataSource) -> T | None:
        """Execute the scalar query against the data source."""
        ...


@runtime_checkable
class IQueryWithProjection(Protocol, Generic[TSelection, TProjection]):
    """Query interface supporting projection/transformation."""

    @abstractmethod
    async def execute(self, data_source: IDataSource) -> list[TProjection]:
        """Execute the query with projection."""
        ...


@runtime_checkable
class IDataSource(Protocol):
    """Provides query capabilities for data access."""

    @abstractmethod
    def create_query(self, entity_type: type[T]) -> IQueryable[T]:
        """Create a queryable for the specified entity type."""
        ...


@runtime_checkable
class IQueryable(Protocol, Generic[T]):
    """Queryable interface providing fluent query API."""

    @abstractmethod
    def filter(self, predicate: Any) -> IQueryable[T]:
        """Add a filter predicate to the query."""
        ...

    @abstractmethod
    def order_by(self, *columns: Any) -> IQueryable[T]:
        """Add ordering to the query."""
        ...

    @abstractmethod
    def limit(self, count: int) -> IQueryable[T]:
        """Limit the number of results."""
        ...

    @abstractmethod
    def offset(self, count: int) -> IQueryable[T]:
        """Skip a number of results."""
        ...

    @abstractmethod
    async def to_list(self) -> list[T]:
        """Execute the query and return results as a list."""
        ...

    @abstractmethod
    async def first_or_none(self) -> T | None:
        """Execute the query and return the first result or None."""
        ...

    @abstractmethod
    async def count(self) -> int:
        """Execute the query and return the count of results."""
        ...

    @abstractmethod
    def __aiter__(self) -> AsyncIterator[T]:
        """Support async iteration over query results."""
        ...


@runtime_checkable
class IUnitOfWork(Protocol):
    """Transaction boundary abstraction for managing entity changes."""

    @abstractmethod
    def add(self, entity: Any) -> None:
        """Add an entity to be inserted."""
        ...

    @abstractmethod
    def update(self, entity: Any) -> None:
        """Mark an entity for update."""
        ...

    @abstractmethod
    def remove(self, entity: Any) -> None:
        """Mark an entity for deletion."""
        ...

    @abstractmethod
    async def commit(self) -> None:
        """Commit all pending changes."""
        ...

    @abstractmethod
    async def rollback(self) -> None:
        """Rollback all pending changes."""
        ...

    @abstractmethod
    async def reload(self, entity: TEntity) -> TEntity:
        """Reload an entity from the data store."""
        ...


@runtime_checkable
class IDataContext(IDataSource, IUnitOfWork, Protocol):
    """Primary abstraction combining data source, unit of work, and context management."""

    async def __aenter__(self) -> IDataContext:
        """Enter the async context manager."""
        ...

    async def __aexit__(
        self,
        exc_type: type[BaseException] | None,
        exc_val: BaseException | None,
        exc_tb: Any,
    ) -> None:
        """Exit the async context manager."""
        ...


@runtime_checkable
class IRepository(Protocol):
    """Repository pattern abstraction for command and query execution."""

    @property
    @abstractmethod
    def context(self) -> IDataContext:
        """Get the underlying data context."""
        ...

    @abstractmethod
    async def execute(self, command: ICommand) -> None:
        """Execute a command."""
        ...

    @abstractmethod
    async def find(self, query: IQuery[T]) -> list[T]:
        """Execute a query and return results."""
        ...

    @abstractmethod
    async def find_scalar(self, scalar: IScalar[T]) -> T | None:
        """Execute a scalar query and return the result."""
        ...


@runtime_checkable
class IConfigureDomainMappings(Protocol, Generic[TConfigurationBinder]):
    """Interface for configuring domain entity mappings."""

    @abstractmethod
    def configure(self, configuration_binder: TConfigurationBinder) -> None:
        """Configure entity mappings using the configuration binder."""
        ...


@runtime_checkable
class IDomain(Protocol, Generic[TConfigurationBinder, TConfigurationOptions]):
    """Domain configuration and mapping interface."""

    @property
    @abstractmethod
    def configuration_options(self) -> TConfigurationOptions:
        """Get the configuration options for this domain."""
        ...

    @property
    @abstractmethod
    def mapping_configurator(self) -> IConfigureDomainMappings[TConfigurationBinder] | None:
        """Get the mapping configurator for this domain."""
        ...


@runtime_checkable
class IDomainRepositoryFactory(Protocol, Generic[TConfigurationBinder, TConfigurationOptions]):
    """Factory for creating domain-specific repositories."""

    @abstractmethod
    async def create_repository(self, domain: IDomain[TConfigurationBinder, TConfigurationOptions]) -> IRepository:
        """Create a repository for the specified domain."""
        ...


# Base classes providing common functionality


class Command(ABC):
    """Base class for command implementations."""

    @abstractmethod
    async def execute(self, unit_of_work: IUnitOfWork) -> None:
        """Execute the command with the given unit of work."""
        pass


class Query(ABC, Generic[T]):
    """Base class for query implementations with fluent API support."""

    def __init__(self) -> None:
        self._predicates: list[Any] = []
        self._ordering: list[Any] = []
        self._limit_count: int | None = None
        self._offset_count: int | None = None

    def add_predicate(self, predicate: Any) -> Query[T]:
        """Add a predicate to the query."""
        self._predicates.append(predicate)
        return self

    def add_ordering(self, *columns: Any) -> Query[T]:
        """Add ordering to the query."""
        self._ordering.extend(columns)
        return self

    def set_limit(self, count: int) -> Query[T]:
        """Set the limit for the query."""
        self._limit_count = count
        return self

    def set_offset(self, count: int) -> Query[T]:
        """Set the offset for the query."""
        self._offset_count = count
        return self

    @abstractmethod
    async def execute(self, data_source: IDataSource) -> list[T]:
        """Execute the query against the data source."""
        pass


class Scalar(ABC, Generic[T]):
    """Base class for scalar query implementations."""

    def __init__(self) -> None:
        self._predicates: list[Any] = []

    def add_predicate(self, predicate: Any) -> Scalar[T]:
        """Add a predicate to the scalar query."""
        self._predicates.append(predicate)
        return self

    @abstractmethod
    async def execute(self, data_source: IDataSource) -> T | None:
        """Execute the scalar query against the data source."""
        pass


class Domain(ABC, Generic[TConfigurationBinder, TConfigurationOptions]):
    """Base class for domain implementations."""

    def __init__(
        self,
        configuration_options: TConfigurationOptions,
        mapping_configurator: IConfigureDomainMappings[TConfigurationBinder] | None = None,
    ) -> None:
        self._configuration_options = configuration_options
        self._mapping_configurator = mapping_configurator

    @property
    def configuration_options(self) -> TConfigurationOptions:
        """Get the configuration options for this domain."""
        return self._configuration_options

    @property
    def mapping_configurator(self) -> IConfigureDomainMappings[TConfigurationBinder] | None:
        """Get the mapping configurator for this domain."""
        return self._mapping_configurator

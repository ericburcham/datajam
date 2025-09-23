"""DataJam exceptions and error types."""


class DataJamException(Exception):
    """Base exception for all DataJam errors."""

    pass


class ConfigurationException(DataJamException):
    """Exception raised for configuration-related errors."""

    pass


class RepositoryException(DataJamException):
    """Exception raised for repository operation errors."""

    pass


class QueryException(DataJamException):
    """Exception raised for query execution errors."""

    pass


class CommandException(DataJamException):
    """Exception raised for command execution errors."""

    pass


class DomainException(DataJamException):
    """Exception raised for domain-related errors."""

    pass


class UnitOfWorkException(DataJamException):
    """Exception raised for unit of work operation errors."""

    pass


class EntityNotFoundException(RepositoryException):
    """Exception raised when an entity is not found."""

    def __init__(self, entity_type: type, identifier: str | int | None = None) -> None:
        if identifier is not None:
            message = f"Entity of type {entity_type.__name__} with identifier '{identifier}' was not found."
        else:
            message = f"Entity of type {entity_type.__name__} was not found."
        super().__init__(message)
        self.entity_type = entity_type
        self.identifier = identifier


class ConcurrencyException(UnitOfWorkException):
    """Exception raised when a concurrency conflict occurs."""

    pass


class TransactionException(UnitOfWorkException):
    """Exception raised for transaction-related errors."""

    pass

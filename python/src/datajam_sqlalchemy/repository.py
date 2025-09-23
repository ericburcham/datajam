"""SQLAlchemy implementation of IRepository."""

from __future__ import annotations

from typing import TypeVar

from datajam import ICommand, IDataContext, IQuery, IRepository, IScalar

T = TypeVar("T")


class Repository(IRepository):
    """SQLAlchemy implementation of IRepository for command and query execution."""

    def __init__(self, context: IDataContext) -> None:
        """Initialize the repository with a data context."""
        self._context = context

    @property
    def context(self) -> IDataContext:
        """Get the underlying data context."""
        return self._context

    async def execute(self, command: ICommand) -> None:
        """Execute a command."""
        await command.execute(self._context)

    async def find(self, query: IQuery[T]) -> list[T]:
        """Execute a query and return results."""
        return await query.execute(self._context)

    async def find_scalar(self, scalar: IScalar[T]) -> T | None:
        """Execute a scalar query and return the result."""
        return await scalar.execute(self._context)

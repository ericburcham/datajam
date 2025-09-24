"""SQLAlchemy implementation of IDataContext."""

from __future__ import annotations

from typing import Any, TypeVar

from sqlalchemy import MetaData
from sqlalchemy.ext.asyncio import AsyncSession

from datajam import IDataContext, IQueryable

T = TypeVar("T")


class DataContext(IDataContext):
    """SQLAlchemy implementation of IDataContext combining data source, unit of work, and context management."""

    def __init__(self, session: AsyncSession, metadata: MetaData | None = None) -> None:
        """Initialize the data context with an async session."""
        self._session = session
        self._metadata = metadata or MetaData()

    @property
    def session(self) -> AsyncSession:
        """Get the underlying SQLAlchemy session."""
        return self._session

    @property
    def metadata(self) -> MetaData:
        """Get the SQLAlchemy metadata."""
        return self._metadata

    def create_query(self, entity_type: type[T]) -> IQueryable[T]:
        """Create a queryable for the specified entity type."""
        from .queryable import QueryableImpl

        return QueryableImpl(entity_type, self._session)

    def add(self, entity: Any) -> None:
        """Add an entity to be inserted."""
        self._session.add(entity)

    def update(self, entity: Any) -> None:
        """Mark an entity for update.

        In SQLAlchemy, entities are automatically tracked for changes
        when they are attached to a session, so this is typically a no-op.
        """
        # SQLAlchemy automatically tracks changes for attached entities
        if entity not in self._session:
            self._session.add(entity)

    def remove(self, entity: Any) -> None:
        """Mark an entity for deletion."""
        # Simply call delete - SQLAlchemy handles whether entity is attached
        self._session.delete(entity)

    async def commit(self) -> None:
        """Commit all pending changes."""
        await self._session.commit()

    async def rollback(self) -> None:
        """Rollback all pending changes."""
        await self._session.rollback()

    async def reload(self, entity: T) -> T:
        """Reload an entity from the data store."""
        await self._session.refresh(entity)
        return entity

    async def flush(self) -> None:
        """Flush pending changes to the database without committing."""
        await self._session.flush()

    async def __aenter__(self) -> IDataContext:
        """Enter the async context manager."""
        return self

    async def __aexit__(self, exc_type: type[BaseException] | None, exc_val: BaseException | None, exc_tb: Any) -> None:
        """Exit the async context manager."""
        if exc_type is not None:
            await self.rollback()
        else:
            await self.commit()
        await self._session.close()

    def __enter__(self) -> DataContext:
        """Enter the sync context manager (not recommended - use async version)."""
        return self

    def __exit__(self, exc_type: type[BaseException] | None, exc_val: BaseException | None, exc_tb: Any) -> None:
        """Exit the sync context manager (not recommended - use async version)."""
        # For sync context manager, we'll use the sync methods
        # This is not ideal but provides compatibility
        if exc_type is not None:
            # Note: This would require sync SQLAlchemy session for proper implementation
            pass
        else:
            # Note: This would require sync SQLAlchemy session for proper implementation
            pass
        # Note: Session closing would also need to be sync

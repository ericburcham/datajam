"""SQLAlchemy implementation of IQueryable."""

from __future__ import annotations

from typing import TYPE_CHECKING, Any, TypeVar

from sqlalchemy import func, select
from sqlalchemy.ext.asyncio import AsyncSession

from datajam import IQueryable

if TYPE_CHECKING:
    from collections.abc import AsyncIterator

T = TypeVar("T")


class QueryableImpl(IQueryable[T]):
    """SQLAlchemy implementation of IQueryable providing fluent query API."""

    def __init__(self, entity_type: type[T], session: AsyncSession) -> None:
        """Initialize the queryable with entity type and session."""
        self._entity_type = entity_type
        self._session = session
        self._query = select(entity_type)

    def filter(self, predicate: Any) -> IQueryable[T]:
        """Add a filter predicate to the query."""
        new_query = QueryableImpl(self._entity_type, self._session)
        new_query._query = self._query.where(predicate)
        return new_query

    def order_by(self, *columns: Any) -> IQueryable[T]:
        """Add ordering to the query."""
        new_query = QueryableImpl(self._entity_type, self._session)
        new_query._query = self._query.order_by(*columns)
        return new_query

    def limit(self, count: int) -> IQueryable[T]:
        """Limit the number of results."""
        new_query = QueryableImpl(self._entity_type, self._session)
        new_query._query = self._query.limit(count)
        return new_query

    def offset(self, count: int) -> IQueryable[T]:
        """Skip a number of results."""
        new_query = QueryableImpl(self._entity_type, self._session)
        new_query._query = self._query.offset(count)
        return new_query

    async def to_list(self) -> list[T]:
        """Execute the query and return results as a list."""
        result = await self._session.execute(self._query)
        return list(result.scalars().all())

    async def first_or_none(self) -> T | None:
        """Execute the query and return the first result or None."""
        result = await self._session.execute(self._query.limit(1))
        return result.scalars().first()  # type: ignore[no-any-return]

    async def count(self) -> int:
        """Execute the query and return the count of results."""
        count_query = select(func.count()).select_from(self._query.subquery())
        result = await self._session.execute(count_query)
        return result.scalar() or 0

    def __aiter__(self) -> AsyncIterator[T]:
        """Support async iteration over query results."""
        return self._async_iterate()

    async def _async_iterate(self) -> AsyncIterator[T]:
        """Async iterator implementation."""
        result = await self._session.execute(self._query)
        for row in result.scalars():
            yield row

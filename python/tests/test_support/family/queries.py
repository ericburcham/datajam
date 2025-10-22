"""Family domain queries for testing DataJam patterns."""

from __future__ import annotations

from datajam import IDataSource, Scalar

from .entities import Child


class GetChildren(Scalar[Child]):
    """Scalar query to get a single child from the database."""

    async def execute(self, data_source: IDataSource) -> Child | None:
        """Execute the query to get a single child."""
        # Create a queryable and get first result
        queryable = data_source.create_query(Child)
        return await queryable.first_or_none()

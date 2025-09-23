"""Oracle integration test for persisting and retrieving a child - Python port of .NET test."""

from __future__ import annotations

from typing import TYPE_CHECKING

import pytest
from sqlalchemy.ext.asyncio import AsyncEngine

from tests.integration.oracle.family_domain import OracleFamilyDomain
from tests.test_support.family import Child, Father, GetChildren, Mother

if TYPE_CHECKING:
    from tests.integration.oracle.oracle_container_manager import OracleContainerManager


class TestWhenPersistingAndRetrievingAChild:
    """Test class for persisting and retrieving a child entity - matches .NET test pattern."""

    @pytest.fixture(autouse=True)
    async def setup_and_teardown(
        self,
        oracle_container: OracleContainerManager,
        oracle_engine: AsyncEngine,
    ) -> None:
        """Setup and teardown for each test method."""
        # Create domain
        self.domain = OracleFamilyDomain(
            engine=oracle_engine,
            connection_string=oracle_container.async_connection_string,
        )

        # Create schema
        await self.domain.create_schema()

        # Create repository and execute the test scenario
        repository = await self.domain.create_repository()

        try:
            # Arrange - Create family entities
            father = Father(name="Dad")
            mother = Mother(name="Mom")
            child = Child(name="Kid")
            child.add_parents(father, mother)

            # Add child to repository (which should also add parents due to relationships)
            repository.context.add(child)
            await repository.context.commit()

            # Act - Retrieve the child
            scalar_query = GetChildren()
            self.result = await repository.find_scalar(scalar_query)

        finally:
            # Cleanup
            await repository.context.__aexit__(None, None, None)

        yield

        # Teardown - Drop schema
        await self.domain.drop_schema()

    async def test_it_should_have_the_correct_name(self) -> None:
        """Test that the retrieved child has the correct name."""
        assert self.result is not None
        assert self.result.name == "Kid"

    async def test_it_should_have_the_correct_father(self) -> None:
        """Test that the retrieved child has the correct father."""
        assert self.result is not None
        assert self.result.father is not None
        assert self.result.father.name == "Dad"

    async def test_it_should_have_the_correct_mother(self) -> None:
        """Test that the retrieved child has the correct mother."""
        assert self.result is not None
        assert self.result.mother is not None
        assert self.result.mother.name == "Mom"

"""Pytest configuration and fixtures for DataJam tests."""

import asyncio
from collections.abc import AsyncGenerator

import pytest
import pytest_asyncio
from sqlalchemy.ext.asyncio import AsyncEngine

from tests.integration.oracle.oracle_container_manager import OracleContainerManager, cleanup_oracle_container


@pytest_asyncio.fixture(scope="function")
async def oracle_container() -> AsyncGenerator[OracleContainerManager, None]:
    """Function-scoped Oracle container fixture."""
    async with OracleContainerManager() as container:
        yield container


@pytest_asyncio.fixture(scope="function")
async def oracle_engine(oracle_container: OracleContainerManager) -> AsyncEngine:
    """Function-scoped Oracle engine fixture."""
    return oracle_container.engine


@pytest.fixture(scope="session", autouse=True)
def cleanup_containers():
    """Ensure containers are cleaned up after all tests."""
    yield
    # This will run after all tests are complete
    asyncio.run(cleanup_oracle_container())

"""Pytest configuration and fixtures for DataJam tests."""

import asyncio
from collections.abc import AsyncGenerator

import pytest
import pytest_asyncio
from sqlalchemy.ext.asyncio import AsyncEngine

from tests.integration.oracle.oracle_container_manager import (
    OracleContainerManager,
    cleanup_oracle_container,
    oracle_container_fixture,
)


@pytest.fixture(scope="session")
def event_loop():
    """Create an instance of the default event loop for the test session."""
    loop = asyncio.get_event_loop_policy().new_event_loop()
    yield loop
    loop.close()


@pytest_asyncio.fixture(scope="session")
async def oracle_container() -> AsyncGenerator[OracleContainerManager, None]:
    """Session-scoped Oracle container fixture."""
    async for container in oracle_container_fixture():
        yield container


@pytest_asyncio.fixture(scope="session")
async def oracle_engine(oracle_container: OracleContainerManager) -> AsyncEngine:
    """Session-scoped Oracle engine fixture."""
    return oracle_container.engine


@pytest.fixture(scope="session", autouse=True)
def cleanup_containers():
    """Ensure containers are cleaned up after all tests."""
    yield
    # This will run after all tests are complete
    asyncio.run(cleanup_oracle_container())

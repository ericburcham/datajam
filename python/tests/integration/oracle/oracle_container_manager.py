"""Oracle TestContainer manager for integration tests."""

from __future__ import annotations

import logging
from typing import TYPE_CHECKING

from sqlalchemy.ext.asyncio import AsyncEngine, create_async_engine
from testcontainers.oracle import OracleDbContainer

if TYPE_CHECKING:
    from collections.abc import AsyncGenerator

from datajam_sqlalchemy import create_oracle_connection_string

from .container_constants import (
    ORACLE_IMAGE,
    ORACLE_PASSWORD,
    ORACLE_PORT,
    ORACLE_SERVICE_NAME,
    ORACLE_USERNAME,
)

logger = logging.getLogger(__name__)


class OracleContainerManager:
    """Manages Oracle TestContainer lifecycle for integration tests."""

    def __init__(self) -> None:
        """Initialize the Oracle container manager."""
        self._container: OracleDbContainer | None = None
        self._engine: AsyncEngine | None = None
        self._connection_string: str | None = None

    async def start_container(self) -> None:
        """Start the Oracle container and wait for it to be ready."""
        if self._container is not None:
            logger.warning("Oracle container already started")
            return

        logger.info("Starting Oracle container...")

        # Create and start the Oracle container using the free image
        self._container = OracleDbContainer(
            image="gvenzl/oracle-free:slim"  # Use the official free image
        )

        # Start the container (this will block until ready)
        self._container.start()

        # Get connection URL directly from the container
        self._connection_string = self._container.get_connection_url()

        # For async support, convert the connection string to use async driver
        if self._connection_string.startswith("oracle+cx_oracle://"):
            async_connection_string = self._connection_string.replace("oracle+cx_oracle://", "oracle+oracledb_async://")
        elif self._connection_string.startswith("oracle+oracledb://"):
            async_connection_string = self._connection_string.replace("oracle+oracledb://", "oracle+oracledb_async://")
        else:
            async_connection_string = self._connection_string

        self._engine = create_async_engine(
            async_connection_string,
            echo=True,  # Enable SQL logging for debugging
            future=True,
        )

        logger.info(f"Oracle container started with connection: {self._connection_string}")

        # Test the connection
        await self._test_connection()

    async def _test_connection(self) -> None:
        """Test the Oracle connection."""
        if not self._engine:
            raise RuntimeError("Engine not initialized")

        try:
            from sqlalchemy import text
            async with self._engine.begin() as conn:
                result = await conn.execute(text("SELECT 1 FROM DUAL"))
                row = result.fetchone()
                if row and row[0] == 1:
                    logger.info("Oracle connection test successful")
                else:
                    raise RuntimeError("Oracle connection test failed")
        except Exception as e:
            logger.error(f"Oracle connection test failed: {e}")
            raise

    async def stop_container(self) -> None:
        """Stop the Oracle container and clean up resources."""
        if self._engine:
            await self._engine.dispose()
            self._engine = None

        if self._container:
            logger.info("Stopping Oracle container...")
            self._container.stop()
            self._container = None

        self._connection_string = None
        logger.info("Oracle container stopped")

    @property
    def connection_string(self) -> str:
        """Get the Oracle connection string."""
        if not self._connection_string:
            raise RuntimeError("Oracle container not started")
        return self._connection_string

    @property
    def async_connection_string(self) -> str:
        """Get the Oracle async connection string."""
        connection_string = self.connection_string
        return connection_string.replace("oracle+oracledb://", "oracle+oracledb_async://")

    @property
    def engine(self) -> AsyncEngine:
        """Get the SQLAlchemy async engine."""
        if not self._engine:
            raise RuntimeError("Oracle container not started")
        return self._engine

    async def __aenter__(self) -> OracleContainerManager:
        """Enter async context manager."""
        await self.start_container()
        return self

    async def __aexit__(self, exc_type, exc_val, exc_tb) -> None:
        """Exit async context manager."""
        await self.stop_container()


# Global container manager instance
_oracle_manager: OracleContainerManager | None = None


async def get_oracle_container() -> OracleContainerManager:
    """Get or create the global Oracle container manager."""
    global _oracle_manager

    if _oracle_manager is None:
        _oracle_manager = OracleContainerManager()
        await _oracle_manager.start_container()

    return _oracle_manager


async def cleanup_oracle_container() -> None:
    """Clean up the global Oracle container manager."""
    global _oracle_manager

    if _oracle_manager is not None:
        await _oracle_manager.stop_container()
        _oracle_manager = None


# Pytest fixture for Oracle container
async def oracle_container_fixture() -> AsyncGenerator[OracleContainerManager, None]:
    """Pytest fixture that provides an Oracle container for tests."""
    async with OracleContainerManager() as container:
        yield container

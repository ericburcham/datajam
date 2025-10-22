"""SQLAlchemy implementation of IDomain."""

from __future__ import annotations

from sqlalchemy import MetaData
from sqlalchemy.ext.asyncio import AsyncEngine, AsyncSession, async_sessionmaker

from datajam import IConfigureDomainMappings, IDomain

from .data_context import DataContext
from .repository import Repository


class Domain(IDomain[MetaData, str]):
    """SQLAlchemy implementation of IDomain using MetaData as configuration binder and connection string as options."""

    def __init__(
        self,
        connection_string: str,
        engine: AsyncEngine,
        mapping_configurator: IConfigureDomainMappings[MetaData] | None = None,
        metadata: MetaData | None = None,
    ) -> None:
        """Initialize the domain with connection string, engine, and optional mapping configurator."""
        self._connection_string = connection_string
        self._engine = engine
        self._mapping_configurator = mapping_configurator
        self._metadata = metadata or MetaData()
        self._session_factory = async_sessionmaker(engine, class_=AsyncSession, expire_on_commit=False)

        # Configure mappings if provided
        if self._mapping_configurator:
            self._mapping_configurator.configure(self._metadata)

    @property
    def configuration_options(self) -> str:
        """Get the configuration options (connection string) for this domain."""
        return self._connection_string

    @property
    def mapping_configurator(self) -> IConfigureDomainMappings[MetaData] | None:
        """Get the mapping configurator for this domain."""
        return self._mapping_configurator

    @property
    def metadata(self) -> MetaData:
        """Get the SQLAlchemy metadata."""
        return self._metadata

    @property
    def engine(self) -> AsyncEngine:
        """Get the SQLAlchemy async engine."""
        return self._engine

    async def create_session(self) -> AsyncSession:
        """Create a new async session."""
        return self._session_factory()

    async def create_data_context(self) -> DataContext:
        """Create a new data context with a fresh session."""
        session = await self.create_session()
        return DataContext(session, self._metadata)

    async def create_repository(self) -> Repository:
        """Create a new repository with a fresh data context."""
        data_context = await self.create_data_context()
        return Repository(data_context)

    async def create_tables(self) -> None:
        """Create all tables defined in the metadata."""
        async with self._engine.begin() as conn:
            await conn.run_sync(self._metadata.create_all, checkfirst=False)

    async def drop_tables(self) -> None:
        """Drop all tables defined in the metadata."""
        async with self._engine.begin() as conn:
            await conn.run_sync(self._metadata.drop_all)

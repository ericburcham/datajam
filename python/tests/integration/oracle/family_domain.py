"""Oracle-specific Family domain implementation for integration tests."""

from __future__ import annotations

from sqlalchemy import MetaData
from sqlalchemy.ext.asyncio import AsyncEngine

from datajam import IConfigureDomainMappings
from datajam_sqlalchemy import Domain, OracleNamingConvention
from tests.test_support.family.entities import Base


class FamilyMappingConfigurator(IConfigureDomainMappings[MetaData]):
    """Mapping configurator for the Family domain with Oracle-specific settings."""

    def configure(self, metadata: MetaData) -> None:
        """Configure entity mappings for Oracle."""
        # Apply Oracle naming conventions to all tables
        OracleNamingConvention.configure_metadata_for_oracle(metadata)


class OracleFamilyDomain(Domain):
    """Oracle-specific Family domain implementation."""

    def __init__(self, engine: AsyncEngine, connection_string: str) -> None:
        """Initialize the Oracle Family domain."""
        # Create metadata from our entities
        metadata = Base.metadata

        # Create mapping configurator
        mapping_configurator = FamilyMappingConfigurator()

        # Initialize the domain
        super().__init__(
            connection_string=connection_string,
            engine=engine,
            mapping_configurator=mapping_configurator,
            metadata=metadata,
        )

    async def create_schema(self) -> None:
        """Create the database schema for the Family domain."""
        await self.create_tables()

    async def drop_schema(self) -> None:
        """Drop the database schema for the Family domain."""
        await self.drop_tables()

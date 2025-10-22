"""Unit tests for Domain implementation."""

from unittest.mock import AsyncMock, Mock

import pytest
from sqlalchemy import MetaData
from sqlalchemy.ext.asyncio import AsyncEngine

from datajam import IConfigureDomainMappings
from datajam_sqlalchemy import DataContext, Domain, Repository


class MockMappingConfigurator(IConfigureDomainMappings[MetaData]):
    """Mock mapping configurator for testing."""

    def __init__(self):
        self.configured = False

    def configure(self, metadata: MetaData) -> None:
        self.configured = True


class TestDomain:
    """Test Domain implementation."""

    @pytest.fixture
    def mock_engine(self):
        """Create a mock async engine."""
        engine = Mock(spec=AsyncEngine)

        # Create a mock connection
        mock_conn = Mock()
        mock_conn.run_sync = AsyncMock()

        # Create an async context manager that returns the connection
        async_context = AsyncMock()
        async_context.__aenter__ = AsyncMock(return_value=mock_conn)
        async_context.__aexit__ = AsyncMock(return_value=None)

        # Make engine.begin() return the async context manager
        engine.begin.return_value = async_context

        # Store the connection for test assertions
        engine._test_connection = mock_conn

        return engine

    @pytest.fixture
    def metadata(self):
        """Create a metadata instance."""
        return MetaData()

    @pytest.fixture
    def mapping_configurator(self):
        """Create a mock mapping configurator."""
        return MockMappingConfigurator()

    @pytest.fixture
    def domain(self, mock_engine, metadata, mapping_configurator):
        """Create a Domain instance."""
        return Domain(
            connection_string="test://connection",
            engine=mock_engine,
            mapping_configurator=mapping_configurator,
            metadata=metadata,
        )

    def test_configuration_options_returns_connection_string(self, domain):
        """Test that configuration_options returns the connection string."""
        assert domain.configuration_options == "test://connection"

    def test_mapping_configurator_is_called_during_init(self, domain, mapping_configurator):
        """Test that mapping configurator is called during initialization."""
        assert mapping_configurator.configured

    def test_metadata_property_returns_metadata(self, domain, metadata):
        """Test that metadata property returns the metadata."""
        assert domain.metadata == metadata

    def test_engine_property_returns_engine(self, domain, mock_engine):
        """Test that engine property returns the engine."""
        assert domain.engine == mock_engine

    async def test_create_data_context_returns_data_context(self, domain):
        """Test that create_data_context returns a DataContext instance."""
        data_context = await domain.create_data_context()
        assert isinstance(data_context, DataContext)

    async def test_create_repository_returns_repository(self, domain):
        """Test that create_repository returns a Repository instance."""
        repository = await domain.create_repository()
        assert isinstance(repository, Repository)

    async def test_create_tables_calls_metadata_create_all(self, domain, mock_engine):
        """Test that create_tables calls metadata.create_all."""
        await domain.create_tables()

        # Verify that run_sync was called on the connection
        mock_engine._test_connection.run_sync.assert_called_once()

    async def test_drop_tables_calls_metadata_drop_all(self, domain, mock_engine):
        """Test that drop_tables calls metadata.drop_all."""
        await domain.drop_tables()

        # Verify that run_sync was called on the connection
        mock_engine._test_connection.run_sync.assert_called_once()

    def test_domain_without_mapping_configurator(self, mock_engine, metadata):
        """Test that Domain can be created without mapping configurator."""
        domain = Domain(connection_string="test://connection", engine=mock_engine, metadata=metadata)
        assert domain.mapping_configurator is None

    def test_domain_without_metadata_creates_default(self, mock_engine):
        """Test that Domain creates default metadata if none provided."""
        domain = Domain(connection_string="test://connection", engine=mock_engine)
        assert isinstance(domain.metadata, MetaData)

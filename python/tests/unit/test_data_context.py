"""Unit tests for DataContext implementation."""

from unittest.mock import AsyncMock, Mock

import pytest
from sqlalchemy import MetaData
from sqlalchemy.ext.asyncio import AsyncSession

from datajam_sqlalchemy import DataContext


class TestDataContext:
    """Test DataContext implementation."""

    @pytest.fixture
    def mock_session(self):
        """Create a mock async session."""
        session = Mock(spec=AsyncSession)
        session.commit = AsyncMock()
        session.rollback = AsyncMock()
        session.close = AsyncMock()
        session.add = Mock()
        session.delete = Mock()
        session.execute = AsyncMock()
        session.__aenter__ = AsyncMock(return_value=session)
        session.__aexit__ = AsyncMock()
        return session

    @pytest.fixture
    def metadata(self):
        """Create a metadata instance."""
        return MetaData()

    @pytest.fixture
    def data_context(self, mock_session, metadata):
        """Create a DataContext with mock session."""
        return DataContext(mock_session, metadata)

    async def test_commit_calls_session_commit(self, data_context, mock_session):
        """Test that commit calls session.commit()."""
        await data_context.commit()
        mock_session.commit.assert_called_once()

    async def test_rollback_calls_session_rollback(self, data_context, mock_session):
        """Test that rollback calls session.rollback()."""
        await data_context.rollback()
        mock_session.rollback.assert_called_once()

    def test_add_calls_session_add(self, data_context, mock_session):
        """Test that add calls session.add()."""
        entity = Mock()
        data_context.add(entity)
        mock_session.add.assert_called_once_with(entity)

    def test_remove_calls_session_delete(self, data_context, mock_session):
        """Test that remove calls session.delete()."""
        entity = Mock()
        # Mock the entity being in the session
        mock_session.__contains__ = Mock(return_value=True)
        data_context.remove(entity)
        mock_session.delete.assert_called_once_with(entity)

    async def test_context_manager_enters_and_exits(self, data_context, mock_session):
        """Test that DataContext works as async context manager."""
        async with data_context as ctx:
            assert ctx == data_context

        # DataContext doesn't delegate to session's context manager, it manages commit/rollback itself
        mock_session.commit.assert_called_once()
        mock_session.close.assert_called_once()

    def test_session_property_returns_session(self, data_context, mock_session):
        """Test that session property returns the underlying session."""
        assert data_context.session == mock_session

    def test_metadata_property_returns_metadata(self, data_context, metadata):
        """Test that metadata property returns the metadata."""
        assert data_context.metadata == metadata

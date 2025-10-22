"""Unit tests for Repository implementation."""

from unittest.mock import AsyncMock, Mock

import pytest

from datajam import ICommand, IQuery, IScalar
from datajam_sqlalchemy import Repository


class MockCommand(ICommand):
    """Mock command for testing."""

    def __init__(self):
        self.executed = False

    async def execute(self, unit_of_work):
        self.executed = True


class MockQuery(IQuery[str]):
    """Mock query for testing."""

    def __init__(self, result: list[str]):
        self.result = result

    async def execute(self, data_source):
        return self.result


class MockScalar(IScalar[str]):
    """Mock scalar query for testing."""

    def __init__(self, result: str):
        self.result = result

    async def execute(self, data_source):
        return self.result


class TestRepository:
    """Test Repository implementation."""

    @pytest.fixture
    def mock_context(self):
        """Create a mock data context."""
        context = Mock()
        context.__aenter__ = AsyncMock(return_value=context)
        context.__aexit__ = AsyncMock(return_value=None)
        return context

    @pytest.fixture
    def repository(self, mock_context):
        """Create a repository with mock context."""
        return Repository(mock_context)

    async def test_execute_command_calls_command_execute(self, repository, mock_context):
        """Test that execute calls command.execute with unit of work."""
        command = MockCommand()

        await repository.execute(command)

        assert command.executed

    async def test_find_calls_query_execute(self, repository, mock_context):
        """Test that find calls query.execute with data source."""
        expected_result = ["item1", "item2"]
        query = MockQuery(expected_result)

        result = await repository.find(query)

        assert result == expected_result

    async def test_find_scalar_calls_scalar_execute(self, repository, mock_context):
        """Test that find_scalar calls scalar.execute with data source."""
        expected_result = "single_item"
        scalar = MockScalar(expected_result)

        result = await repository.find_scalar(scalar)

        assert result == expected_result

    def test_context_property_returns_context(self, repository, mock_context):
        """Test that context property returns the underlying context."""
        assert repository.context == mock_context

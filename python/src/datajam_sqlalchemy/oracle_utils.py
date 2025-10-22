"""Oracle-specific utilities for DataJam SQLAlchemy implementation."""

from __future__ import annotations

from sqlalchemy import MetaData, Table


class OracleNamingConvention:
    """Oracle naming convention handler for case sensitivity."""

    @staticmethod
    def to_oracle_identifier(name: str) -> str:
        """Convert identifier to Oracle format (uppercase)."""
        return name.upper()

    @staticmethod
    def configure_table_for_oracle(table: Table) -> None:
        """Configure a table for Oracle naming conventions."""
        # Update table name to uppercase
        if table.name:
            table.name = OracleNamingConvention.to_oracle_identifier(table.name)

        # Update column names to uppercase
        for column in table.columns:
            if column.name:
                column.name = OracleNamingConvention.to_oracle_identifier(column.name)

        # Update index names
        for index in table.indexes:
            if index.name and isinstance(index.name, str):
                index.name = OracleNamingConvention.to_oracle_identifier(index.name)

        # Update constraint names
        for constraint in table.constraints:
            if constraint.name and isinstance(constraint.name, str):
                constraint.name = OracleNamingConvention.to_oracle_identifier(constraint.name)

    @staticmethod
    def configure_metadata_for_oracle(metadata: MetaData) -> None:
        """Configure all tables in metadata for Oracle naming conventions."""
        for table in metadata.tables.values():
            OracleNamingConvention.configure_table_for_oracle(table)


class OracleSequenceHelper:
    """Helper for working with Oracle sequences."""

    @staticmethod
    def create_sequence_ddl(table_name: str, column_name: str = "ID") -> str:
        """Generate DDL for creating an Oracle sequence."""
        sequence_name = f"SEQ_{table_name}_{column_name}"
        return f"CREATE SEQUENCE {sequence_name} START WITH 1 INCREMENT BY 1"

    @staticmethod
    def drop_sequence_ddl(table_name: str, column_name: str = "ID") -> str:
        """Generate DDL for dropping an Oracle sequence."""
        sequence_name = f"SEQ_{table_name}_{column_name}"
        return f"DROP SEQUENCE {sequence_name}"

    @staticmethod
    def get_sequence_name(table_name: str, column_name: str = "ID") -> str:
        """Get the sequence name for a table and column."""
        return f"SEQ_{table_name}_{column_name}"


def create_oracle_connection_string(
    host: str = "localhost",
    port: int = 1521,
    service_name: str = "XE",
    username: str = "system",
    password: str = "oracle",
) -> str:
    """Create an Oracle connection string for SQLAlchemy using python-oracledb."""
    return f"oracle+oracledb://{username}:{password}@{host}:{port}/?service_name={service_name}"

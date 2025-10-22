"""Unit tests for Oracle utilities."""

from sqlalchemy import Column, Integer, MetaData, String, Table

from datajam_sqlalchemy.oracle_utils import (
    OracleNamingConvention,
    OracleSequenceHelper,
    create_oracle_connection_string,
)


class TestOracleNamingConvention:
    """Test OracleNamingConvention utility."""

    def test_to_oracle_identifier_converts_to_uppercase(self):
        """Test that to_oracle_identifier converts names to uppercase."""
        assert OracleNamingConvention.to_oracle_identifier("test_table") == "TEST_TABLE"
        assert OracleNamingConvention.to_oracle_identifier("user_name") == "USER_NAME"
        assert OracleNamingConvention.to_oracle_identifier("lowercase") == "LOWERCASE"

    def test_configure_table_for_oracle_updates_names(self):
        """Test that configure_table_for_oracle updates table and column names."""
        metadata = MetaData()
        table = Table(
            "test_table",
            metadata,
            Column("test_column", String(50)),
            Column("another_column", Integer),
        )

        OracleNamingConvention.configure_table_for_oracle(table)

        assert table.name == "TEST_TABLE"
        assert "TEST_COLUMN" in [col.name for col in table.columns]
        assert "ANOTHER_COLUMN" in [col.name for col in table.columns]

    def test_configure_metadata_for_oracle_updates_all_tables(self):
        """Test that configure_metadata_for_oracle updates all tables."""
        metadata = MetaData()
        table1 = Table("table_one", metadata, Column("col_one", String(50)))
        table2 = Table("table_two", metadata, Column("col_two", Integer))

        OracleNamingConvention.configure_metadata_for_oracle(metadata)

        assert table1.name == "TABLE_ONE"
        assert table2.name == "TABLE_TWO"
        assert "COL_ONE" in [col.name for col in table1.columns]
        assert "COL_TWO" in [col.name for col in table2.columns]


class TestOracleSequenceHelper:
    """Test OracleSequenceHelper utility."""

    def test_create_sequence_ddl_generates_correct_sql(self):
        """Test that create_sequence_ddl generates correct DDL."""
        ddl = OracleSequenceHelper.create_sequence_ddl("PERSON")
        expected = "CREATE SEQUENCE SEQ_PERSON_ID START WITH 1 INCREMENT BY 1"
        assert ddl == expected

    def test_create_sequence_ddl_with_custom_column(self):
        """Test create_sequence_ddl with custom column name."""
        ddl = OracleSequenceHelper.create_sequence_ddl("USER", "USER_ID")
        expected = "CREATE SEQUENCE SEQ_USER_USER_ID START WITH 1 INCREMENT BY 1"
        assert ddl == expected

    def test_drop_sequence_ddl_generates_correct_sql(self):
        """Test that drop_sequence_ddl generates correct DDL."""
        ddl = OracleSequenceHelper.drop_sequence_ddl("PERSON")
        expected = "DROP SEQUENCE SEQ_PERSON_ID"
        assert ddl == expected

    def test_drop_sequence_ddl_with_custom_column(self):
        """Test drop_sequence_ddl with custom column name."""
        ddl = OracleSequenceHelper.drop_sequence_ddl("USER", "USER_ID")
        expected = "DROP SEQUENCE SEQ_USER_USER_ID"
        assert ddl == expected

    def test_get_sequence_name_returns_correct_name(self):
        """Test that get_sequence_name returns correct sequence name."""
        name = OracleSequenceHelper.get_sequence_name("PERSON")
        assert name == "SEQ_PERSON_ID"

    def test_get_sequence_name_with_custom_column(self):
        """Test get_sequence_name with custom column name."""
        name = OracleSequenceHelper.get_sequence_name("USER", "USER_ID")
        assert name == "SEQ_USER_USER_ID"


class TestCreateOracleConnectionString:
    """Test create_oracle_connection_string function."""

    def test_default_parameters(self):
        """Test connection string with default parameters."""
        conn_str = create_oracle_connection_string()
        expected = "oracle+oracledb://system:oracle@localhost:1521/?service_name=XE"
        assert conn_str == expected

    def test_custom_parameters(self):
        """Test connection string with custom parameters."""
        conn_str = create_oracle_connection_string(
            host="oracle.example.com", port=1234, service_name="PROD", username="myuser", password="mypass"
        )
        expected = "oracle+oracledb://myuser:mypass@oracle.example.com:1234/?service_name=PROD"
        assert conn_str == expected

    def test_special_characters_in_password(self):
        """Test connection string with special characters in password."""
        conn_str = create_oracle_connection_string(username="user", password="p@ssw0rd!")
        expected = "oracle+oracledb://user:p@ssw0rd!@localhost:1521/?service_name=XE"
        assert conn_str == expected

"""Family domain entities for testing DataJam patterns."""

from __future__ import annotations

from sqlalchemy import Column, ForeignKey, Integer, String
from sqlalchemy.orm import DeclarativeBase, relationship


# Create base class for our entities
class Base(DeclarativeBase):
    """Base class for all entities."""

    pass


class Person(Base):
    """Base person entity."""

    __tablename__ = "PERSON"

    id = Column("ID", Integer, primary_key=True, autoincrement=True)
    name = Column("NAME", String(100), nullable=False)
    person_type = Column("PERSON_TYPE", String(20), nullable=False)

    # Polymorphic configuration
    __mapper_args__ = {
        "polymorphic_identity": "person",
        "polymorphic_on": person_type,
        "with_polymorphic": "*",
    }

    def __repr__(self) -> str:
        return f"<{self.__class__.__name__}(id={self.id}, name='{self.name}')>"


class Father(Person):
    """Father entity inheriting from Person."""

    __tablename__ = "FATHER"

    id = Column("ID", Integer, ForeignKey("PERSON.ID"), primary_key=True)

    # Relationship to children
    children = relationship("Child", back_populates="father", foreign_keys="Child.father_id")

    __mapper_args__ = {
        "polymorphic_identity": "father",
    }


class Mother(Person):
    """Mother entity inheriting from Person."""

    __tablename__ = "MOTHER"

    id = Column("ID", Integer, ForeignKey("PERSON.ID"), primary_key=True)

    # Relationship to children
    children = relationship("Child", back_populates="mother", foreign_keys="Child.mother_id")

    __mapper_args__ = {
        "polymorphic_identity": "mother",
    }


class Child(Person):
    """Child entity inheriting from Person."""

    __tablename__ = "CHILD"

    id = Column("ID", Integer, ForeignKey("PERSON.ID"), primary_key=True)
    father_id = Column("FATHER_ID", Integer, ForeignKey("FATHER.ID"), nullable=True)
    mother_id = Column("MOTHER_ID", Integer, ForeignKey("MOTHER.ID"), nullable=True)

    # Relationships to parents
    father = relationship("Father", back_populates="children", foreign_keys=[father_id])
    mother = relationship("Mother", back_populates="children", foreign_keys=[mother_id])

    __mapper_args__ = {
        "polymorphic_identity": "child",
    }

    def add_parents(self, father: Father, mother: Mother) -> None:
        """Add parents to this child."""
        self.father = father
        self.mother = mother
        self.father_id = father.id
        self.mother_id = mother.id

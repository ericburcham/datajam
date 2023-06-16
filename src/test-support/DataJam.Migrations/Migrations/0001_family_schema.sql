-- Create the family schema
CREATE SCHEMA Family
    GO

-- Create family tables for child, father, and mother
CREATE TABLE Family.Mother
(
    Id bigint NOT NULL IDENTITY (1, 1),
    Name varchar(100) NOT NULL
)  ON [PRIMARY]

ALTER TABLE Family.Mother ADD CONSTRAINT
    PK_Mother PRIMARY KEY CLUSTERED
        (
         Id
            ) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

ALTER TABLE Family.Mother SET (LOCK_ESCALATION = TABLE)

CREATE TABLE Family.Father
(
    Id bigint NOT NULL IDENTITY (1, 1),
    Name varchar(100) NOT NULL
)  ON [PRIMARY]

ALTER TABLE Family.Father ADD CONSTRAINT
    PK_Father PRIMARY KEY CLUSTERED
        (
         Id
            ) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]


ALTER TABLE Family.Father SET (LOCK_ESCALATION = TABLE)

CREATE TABLE Family.Child
(
    Id bigint NOT NULL IDENTITY (1, 1),
    FatherId bigint NOT NULL,
    MotherId bigint NOT NULL,
    Name varchar(100) NOT NULL
)  ON [PRIMARY]

ALTER TABLE Family.Child ADD CONSTRAINT
    PK_Child PRIMARY KEY CLUSTERED
        (
         Id
            ) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]


ALTER TABLE Family.Child ADD CONSTRAINT
    FK_Child_Father FOREIGN KEY
        (
         FatherId
            ) REFERENCES Family.Father
            (
             Id
                ) ON UPDATE  NO ACTION
        ON DELETE  NO ACTION

ALTER TABLE Family.Child ADD CONSTRAINT
    FK_Child_Mother FOREIGN KEY
        (
         MotherId
            ) REFERENCES Family.Mother
            (
             Id
                ) ON UPDATE  NO ACTION
        ON DELETE  NO ACTION

ALTER TABLE Family.Child SET (LOCK_ESCALATION = TABLE)
GO
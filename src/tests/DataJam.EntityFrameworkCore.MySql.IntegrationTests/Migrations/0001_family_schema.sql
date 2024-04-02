-- Use the correct database.
USE test;

-- Create family tables for child, father, and mother
CREATE TABLE Mother
(
    Id   bigint       NOT NULL AUTO_INCREMENT,
    Name varchar(100) NOT NULL,
    CONSTRAINT PK_Mother PRIMARY KEY (ID)
);

 CREATE TABLE Father
 (
     Id   bigint       NOT NULL AUTO_INCREMENT,
     Name varchar(100) NOT NULL,
     CONSTRAINT  PK_Father PRIMARY KEY (ID)
 );
 
CREATE TABLE Child
(
    Id       bigint       NOT NULL AUTO_INCREMENT,
    FatherId bigint       NOT NULL,
    MotherId bigint       NOT NULL,
    Name     varchar(100) NOT NULL,
    CONSTRAINT PK_Child PRIMARY KEY (ID)
);

ALTER TABLE Child
    ADD CONSTRAINT
        FK_Child_Father FOREIGN KEY
            (
             FatherId
                ) REFERENCES Father
                (
                 Id
                    );

ALTER TABLE Child
    ADD CONSTRAINT
        FK_Child_Mother FOREIGN KEY
            (
             MotherId
                ) REFERENCES Mother
                (
                 Id
                    );

-- Create family tables for child, father, and mother
CREATE TABLE Mother
(
    Id   INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    Name TEXT    NOT NULL
);

CREATE TABLE Father
(
    Id   INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    Name TEXT    NOT NULL
);

CREATE TABLE Child
(
    Id       INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    FatherId INTEGER NOT NULL,
    MotherId INTEGER NOT NULL,
    Name     TEXT    NOT NULL,
    FOREIGN KEY (FatherId) REFERENCES Father (Id),
    FOREIGN KEY (MotherId) REFERENCES Mother (Id)
);

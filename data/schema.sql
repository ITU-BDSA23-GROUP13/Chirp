DROP TABLE IF EXISTS Author;
CREATE TABLE Author (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name STRING NOT NULL,
    Email STRING NOT NULL,
    PwHash STRING NOT NULL
);

DROP TABLE IF EXISTS Cheep;
CREATE TABLE Cheep (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    AuthorId INTEGER NOT NULL,
    Text STRING NOT NULL,
    Timestamp INTEGER
);

CREATE DATABASE petteFilmLibrary;
GO

USE petteFilmLibrary;
GO

CREATE TABLE Genres (
    Id        INT IDENTITY(1,1) PRIMARY KEY,
    GenreName NVARCHAR(100) NOT NULL
);

CREATE TABLE Movies (
    Id                INT IDENTITY(1,1) PRIMARY KEY,
    MovieName         NVARCHAR(200) NOT NULL,
    Rating            INT           NOT NULL,
    GenreId           INT           NOT NULL FOREIGN KEY REFERENCES Genres(Id),
    YearOfPublication INT           NOT NULL
);
GO

-- Genres (insert order defines the Ids used below)
INSERT INTO Genres (GenreName) VALUES
    'Action', 'Comedy', 'Drama', 'Horror', 'Sci-Fi', 'Fantasy', 'Thriller', 'Animation', 'Romance', 'Documentary';
GO

INSERT INTO Movies (MovieName, Rating, GenreId, YearOfPublication) VALUES
    ('Die Hard',                8,  1, 1988),
    ('Mad Max: Fury Road',      8,  1, 2015),
    ('The Big Lebowski',        8,  2, 1998),
    ('Superbad',                7,  2, 2007),
    ('The Godfather',           9,  3, 1972),
    ('Whiplash',                8,  3, 2014),
    ('The Thing',               8,  4, 1982),
    ('Hereditary',              7,  4, 2018),
    ('Blade Runner',            9,  5, 1982),
    ('Arrival',                 8,  5, 2016),
    ('The Lord of the Rings',   9,  6, 2001),
    ('Pan''s Labyrinth',        8,  6, 2006),
    ('Se7en',                   8,  7, 1995),
    ('The Silence of the Lambs',9,  7, 1991),
    ('Spirited Away',           9,  8, 2001),
    ('Toy Story',               8,  8, 1995),
    ('Before Sunrise',          8,  9, 1995),
    ('Pride & Prejudice',       7,  9, 2005),
    ('Planet Earth',            9, 10, 2006),
    ('Free Solo',               8, 10, 2018);
GO
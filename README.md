# Petters ADO Movie Library Database Manager

A C# .NET console application for managing a movie library movies and their
genres stored in a SQL Server database. All database access is done with raw
**ADO.NET** (`SqlConnection`, `SqlCommand`, `SqlParameter`, `SqlDataReader`).

Built as an individual course assignment in *Programmering med C# .NET*.

---

## Features

The app is driven by an interactive, arrow-key menu:

- **List all movies** shows name, publication year, genre and rating, using an
  `INNER JOIN` between the `Movies` and `Genres` tables.
- **Search movies by genre** pick a genre and see only the movies in it
  (parameterized query with `@GenreId`).
- **Add a movie** enter name, rating, publication year and pick an existing genre.
- **Edit a movie** optionally change name, rating, genre and/or publication year.
- **Remove a movie** pick a movie and delete it.
- **Exit** close the application.

All values coming from the user are sent to SQL Server with
`SqlParameter`/`AddWithValue`, so the queries are not vulnerable to SQL injection.

---

## Tech stack

| Component | Version / Detail |
| --- | --- |
| Framework | .NET 10.0 (console application) |
| Language | C# (nullable + implicit usings enabled) |
| Database | SQL Server (LocalDB, Express or full) |
| Data access | `Microsoft.Data.SqlClient` 7.1.1 |
| Console UI | `Spectre.Console` 0.57.2 |
| Menu engine | Custom `StateMachine` library (State pattern) |

---

## Architecture

The project uses a **State pattern** for navigation: each screen is a state, and
the context flows from one screen to the next based on the user's selection.

```
Program
  └── MenuStateContext (StateMachine.Context)
        └── MenuScreen (abstract state base)
              ├── StartMenuScreen        — main menu
              ├── ListAllMoviesScreen    — view all movies (JOIN)
              ├── SearchByGenreScreen    — filter movies by genre
              ├── AddMovieScreen         — insert a movie
              ├── EditMovieScreen        — update a movie
              ├── RemoveMovieScreen      — delete a movie
              └── ExitAppScreen          — quit
```

### Project structure

```
AdoFilmLibrary/
├── AdoFilmLibrary.sln
├── Tasks.md                              # Assignment checklist (Swedish)
└── AdoFilmLibrary/
    ├── Program.cs                        # Entry point
    ├── Core/
    │   ├── SqlConnect.cs                 # Factory for SqlConnection
    │   └── ProgramConstants.cs           # SQL queries, messages, connection string
    ├── Models/
    │   ├── Movie.cs
    │   └── Genre.cs
    ├── Services/
    │   ├── MovieService.cs               # Read queries for movies
    │   └── GenreService.cs               # Read queries for genres
    ├── UI/
    │   └── UIHelpers.cs                  # Renders movies as a Spectre table
    └── MenuStateMachine/
        ├── StatePattern/
        │   ├── MenuStateContext.cs       # State machine context
        │   └── MenuScreen.cs             # Abstract screen / state base
        └── States/
            ├── StartMenuScreen.cs
            ├── ListAllMoviesScreen.cs
            ├── SearchByGenreScreen.cs
            ├── ExitAppScreen.cs
            ├── MovieCRUD/
            │   ├── AddMovieScreen.cs
            │   ├── EditMovieScreen.cs
            │   └── RemoveMovieScreen.cs
            └── GenreCRUD/                # Not wired into the menu (see Notes)
                ├── AddGenreScreen.cs
                ├── EditGenreScreen.cs
                └── RemoveGenreScreen.cs
```

**Data flow:** a screen calls a method on `MovieService`/`GenreService`, which
opens a `SqlConnection` from `SqlConnect`, runs a query defined in
`ProgramConstants.SqlQueries`, and returns a `List<Movie>`/`List<Genre>` that
`UiHelpers` renders as a Spectre table.

---

## Database schema

The app expects a database named `petteFilmLibrary` with two tables.

```sql
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
```

### Sample data

```sql
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
```

---

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (LocalDB, Express or full) reachable via Windows/trusted connection
- The custom `StateMachine` project (see the caveat below)

---

## Getting started

1. **Create the database and tables** using the SQL in
   [Database schema](#database-schema), optionally with the sample data.

2. **Check the connection string.** It is hardcoded in
   `AdoFilmLibrary/AdoFilmLibrary/Core/ProgramConstants.cs`:

   ```csharp
   public const string ConnectionString =
       "Server=localhost;Database=petteFilmLibrary;Trusted_Connection=True;TrustServerCertificate=True;";
   ```

   Adjust `Server` (and credentials, if not using a trusted connection) to match
   your local SQL Server instance.

3. **Restore the `StateMachine` dependency.** `AdoFilmLibrary.csproj` references
   a project that lives *outside* this repository:

   ```xml
   <ProjectReference Include="..\..\..\..\..\RiderProjects\StateMachine\StateMachine\StateMachine.csproj" />
   ```

   A fresh clone will not build until this reference exists at that path, or is
   replaced with your own copy of the `StateMachine` library.

4. **Build and run:**

   ```bash
   dotnet run --project AdoFilmLibrary/AdoFilmLibrary
   ```

---

## Usage

After launch, press any key to enter the main menu and use the **arrow keys**
(and Enter) to navigate:

```
Start Menu
├── All Movies          → lists every moviie with genre, year and rating
├── Search By Genre     → pick a genre, then see its movies
├── Add Movie           → enter name, rating, genre and year
├── Remove Movie        → pick a movie to delete
├── Edit Movie Info     → optionally edit name, rating, genre and year
└── Exit App            → quit
```

---

## Notes & limitations

- **Hardcoded connection string.** There is no `appsettings.json` or environment
  configuration; the connection string lives in `ProgramConstants.cs` and must be
  edited per machine.
- **External project reference.** The `StateMachine` library is referenced from a
  path outside the repository, so the project does not build from a clean clone
  without it.
- **Unwired Genre screens.** `GenreCRUD/AddGenreScreen`, `EditGenreScreen` and
  `RemoveGenreScreen` exist but are not connected to any menu, so they are
  currently dead code.
- **Placeholder file.** `DataInitializer.cs` is empty.
- **Data access spread out.** `MovieService`/`GenreService` handle reads, but some
  `INSERT`/`UPDATE`/`DELETE` statements still live directly in the screen classes
  rather than in the service layer.
- **`Tasks.md`** is the original (Swedish) assignment checklist and is partly out
  of date — several items it marks as open are already implemented.

---

## Author

- **Author:** PHallin
- **Course:** Programmering med C# .NET
- **Repository:** https://github.com/PHallin99/DotNetAdoAssignment

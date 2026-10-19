namespace AdoFilmLibrary.Core;

public class ProgramConstants {
    public struct SqlQueries {
        public const string SelectAllMovies = "SELECT Id, MovieName, Rating, GenreId FROM Movies";
        public const string SelectAllGenres = "SELECT Id, GenreName FROM Genres";

        public const string SelectMoviesJoinedGenres = @"
            SELECT Movies.MovieName, Movies.Rating, Genres.GenreName, Movies.YearOfPublication
            FROM Movies
            INNER JOIN Genres On Movies.GenreId = Genres.Id";

        public const string SelectAllMoviesInGenreJoined = @"
            SELECT Movies.MovieName, Movies.Rating, Genres.GenreName, Movies.YearOfPublication
            FROM Movies
            INNER JOIN Genres On Movies.GenreId = Genres.Id
            WHERE Movies.GenreId = @GenreId";

        public const string AddMovie = "INSERT INTO Movies (MovieName, Rating, GenreId, YearOfPublication) Values (@Name, @Rating, @GenreId, @YearOfPublication)";
        public const string UpdateMovieName = "UPDATE Movies SET MovieName = @Name WHERE Id = @Id";
        public const string UpdateMovieRating = "UPDATE Movies SET Rating = @Rating WHERE Id = @Id";
        public const string UpdateMovieGenre = "UPDATE Movies SET GenreId = @GenreId WHERE Id = @Id";
        public const string UpdateMovieYearOfPublication = "UPDATE Movies SET YearOfPublication = @YearOfPublishing WHERE Id = @Id";
        public const string DeleteMovieById = "DELETE FROM Movies WHERE Id = @Id";
    }

    public struct ConsoleMessages {
        public const string PressAnyKey = "-- Press ANY KEY to run program --";
        public const string PressAnyKeyMarkup = "[yellow]-- Press ANY KEY to run program --[/]";
        public const string NoMovies = "[yellow]No movies in the library! Please add movies first![/]";
        public const string NoMoviesInGenre = "[yellow]No movies in this genre! Please add some first![/]";

        public const string HeaderAscii = """
                                          --- Petters ADO Movie Library Database Manager ---
                                          """;

    }

    public const string ConnectionString =
        "Server=localhost;Database=petteFilmLibrary;Trusted_Connection=True;TrustServerCertificate=True;";

    public struct ConsoleErrors {
        public const string NoGenres = "[red]No genres found in database! Please execute the setup SQL Query again![/]";
    }
}
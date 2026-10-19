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
            WHERE GenreId = @GenreId";
    }

    public struct ConsoleMessages {
        public const string PressAnyKey = "-- Press ANY KEY to run program --";
        public const string PressAnyKeyMarkup = "[yellow]-- Press ANY KEY to run program --[/]";
        public const string NoMovies = "[yellow]No movies in the library! Please add movies first![/]";

        public const string HeaderAscii = """
                                          --- Petters ADO Movie Library Database Manager ---
                                          """;
    }

    public const string ConnectionString =
        "Server=localhost;Database=petteFilmLibrary;Trusted_Connection=True;TrustServerCertificate=True;";
}
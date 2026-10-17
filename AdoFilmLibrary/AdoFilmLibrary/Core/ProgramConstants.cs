namespace AdoFilmLibrary.Core;

public class ProgramConstants {
    public struct SqlQueries {
        public const string SelectAllMovies = "SELECT Id, MovieName, Rating, GenreId FROM Movies";
        public static string SelectAllGenres = "SELECT Id, GenreName FROM Genres";
    }
    
    public struct ConsoleMessages {
        public const string PressAnyKey = "-- Press ANY KEY to run program --";
        public const string PressAnyKeyMarkup = "[yellow]-- Press ANY KEY to run program --[/]";
    }

    public const string HeaderAscii = """
                                      --- Petters ADO Movie Library Database Manager ---
                                      """;

    public const string ConnectionString = "Server=localhost;Database=petteFilmLibrary;Trusted_Connection=True;TrustServerCertificate=True;";
}
using AdoFilmLibrary.Core;
using AdoFilmLibrary.Models;
using Spectre.Console;

namespace AdoFilmLibrary.UI;

public static class UiHelpers {
    public static void DisplayMoviesOrMessage(List<Movie> movies) {
        if (movies.Count == 0) {
            AnsiConsole.MarkupLine(ProgramConstants.ConsoleMessages.NoMovies);
            return;
        }
        
        DisplayMoviesTable(movies);
    }
    
    public static void DisplayMoviesTable(List<Movie> movies) {
        var table = new Table();
        table.AddColumn("Name");
        table.AddColumn("Publication Year");
        table.AddColumn("Genre");
        table.AddColumn("Rating");

        foreach (var movie in movies) {
            table.AddRow(movie.Name, $"{movie.YearOfPublication}", movie.GenreName, $"{movie.Rating}");
        }

        AnsiConsole.Write(table);
    }
}
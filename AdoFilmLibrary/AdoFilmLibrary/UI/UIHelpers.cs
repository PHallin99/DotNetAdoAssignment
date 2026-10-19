using AdoFilmLibrary.Core;
using AdoFilmLibrary.Models;
using Spectre.Console;

namespace AdoFilmLibrary.UI;

public static class UiHelpers {
    public static void DisplayMoviesOrMessage(List<Movie> movies, string message) {
        if (movies.Count == 0) {
            AnsiConsole.MarkupLine(message);
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
            table.AddRow(
                Markup.Escape(movie.Name),
                movie.YearOfPublication.ToString(),
                Markup.Escape(movie.GenreName),
                movie.Rating.ToString()
            );
        }

        AnsiConsole.Write(table);
    }
}
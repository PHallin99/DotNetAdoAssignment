using AdoFilmLibrary.Core;
using AdoFilmLibrary.MenuStateMachine.StatePattern;
using AdoFilmLibrary.Models;
using AdoFilmLibrary.Services;
using Microsoft.Data.SqlClient;
using Spectre.Console;
using StateMachine;

namespace AdoFilmLibrary.MenuStateMachine.States;

public class EditMovieScreen(Context context) : MenuScreen("Edit Movie Info", context), IState {
    public override void EnterState() {
        StateTransitions = [
            new StartMenuScreen(Context),
        ];

        AnsiConsole.MarkupLine(ProgramConstants.HeaderAscii);
        AnsiConsole.MarkupLineInterpolated($"[green]<State> {MenuName} Entered![/]");
    }

    public override void RunState() {
        var allMoviesFromDb = MovieService.GetAllMoviesFromDb();
        var selectedMovie = AnsiConsole.Prompt(new SelectionPrompt<Movie>()
            .Title("Select Movie To Remove From Library")
            .PageSize(5)
            .MoreChoicesText("[grey](Use arrow keys to see more movies)[/]")
            .AddChoices(allMoviesFromDb)
            .UseConverter(movie => movie.Name));

        AnsiConsole.Clear();

        using var connection = new SqlConnection(ProgramConstants.ConnectionString);
        connection.Open();
        if (AnsiConsole.Confirm("Edit Movie Name?")) {
            selectedMovie.Name = AnsiConsole.Ask<string>("New Movie [green]name[/]?");
            using var command = new SqlCommand("UPDATE Movies SET Name = @Name WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Name", selectedMovie.Name);
            command.Parameters.AddWithValue("@Id", selectedMovie.Id);
            var renameRowsAffected = command.ExecuteNonQuery();
            AnsiConsole.MarkupLineInterpolated($"[green]{renameRowsAffected} movie(s) renamed![/]");
        }

        if (AnsiConsole.Confirm("Edit Movie Rating?")) {
            selectedMovie.Rating = AnsiConsole.Ask<int>("New Movie [green]rating[/]?");
            using var command = new SqlCommand("UPDATE Movies SET Rating = @Rating WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Rating", selectedMovie.Rating);
            command.Parameters.AddWithValue("@Id", selectedMovie.Id);
            var renameRowsAffected = command.ExecuteNonQuery();
            AnsiConsole.MarkupLineInterpolated($"[green]{renameRowsAffected} movie(s) had their rating changed![/]");
        }

        if (AnsiConsole.Confirm("Edit Movie Genre?")) {
            selectedMovie.GenreId = AnsiConsole.Prompt(new SelectionPrompt<Genre>()
                .Title("What [green]Genre[/] is the movie?")
                .AddChoices(GenreService.GetAllGenresFromDb())
                .UseConverter(g => g.Name)).Id;

            using var command = new SqlCommand("UPDATE Movies SET GenreId = @GenreId WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@GenreId", selectedMovie.GenreId);
            command.Parameters.AddWithValue("@Id", selectedMovie.Id);
            var renameRowsAffected = command.ExecuteNonQuery();
            AnsiConsole.MarkupLineInterpolated($"[green]{renameRowsAffected} movie(s) updated Genre![/]");
        }

        connection.Close();
        StateSelection = StateTransitions[0];
        PauseForInput();
        base.RunState();
    }

    public string GetName() {
        return MenuName;
    }
}
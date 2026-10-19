using AdoFilmLibrary.Core;
using AdoFilmLibrary.MenuStateMachine.StatePattern;
using AdoFilmLibrary.Models;
using AdoFilmLibrary.Services;
using Microsoft.Data.SqlClient;
using Spectre.Console;
using StateMachine;

namespace AdoFilmLibrary.MenuStateMachine.States.MovieCRUD;

public class RemoveMovieScreen(Context context) : MenuScreen("Remove Movie", context), IState {
    public override void EnterState() {
        StateTransitions = [
            new StartMenuScreen(Context),
        ];

        AnsiConsole.MarkupLine(ProgramConstants.ConsoleMessages.HeaderAscii);
        AnsiConsole.MarkupLineInterpolated($"[green]<State> {MenuName} Entered![/]");
    }

    public override void RunState() {
        var allMovies = MovieService.GetAllMoviesFromDatabase();
        if (allMovies.Count <= 0) {
            AnsiConsole.MarkupLine("[yellow]No movies in the library! Please add movies first![/]");
        } else {
            using var connection = SqlConnect.GetSqlConnection();
            connection.Open();
            AnsiConsole.MarkupLine("[green]SQL Connection Open[/]");
            using var command = new SqlCommand("DELETE FROM Movies WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id",
                AnsiConsole.Prompt(new SelectionPrompt<Movie>()
                    .Title("Which Movie To Remove From Library?")
                    .PageSize(5)
                    .MoreChoicesText("[grey](Use arrow keys to see more movies)[/]")
                    .AddChoices(allMovies)
                    .UseConverter(movie => movie.Name)).Id);

            var rowsAffected = command.ExecuteNonQuery();
            connection.Close();
            AnsiConsole.MarkupLine("[red]SQL Connection Closed[/]");
            AnsiConsole.MarkupLineInterpolated($"[green]{rowsAffected} movie(s) removed![/]");
        }

        StateSelection = StateTransitions[0];
        PauseForInput();
        base.RunState();
    }
}
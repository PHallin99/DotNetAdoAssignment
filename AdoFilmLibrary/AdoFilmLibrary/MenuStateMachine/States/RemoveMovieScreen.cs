using AdoFilmLibrary.Core;
using AdoFilmLibrary.MenuStateMachine.StatePattern;
using AdoFilmLibrary.Models;
using AdoFilmLibrary.Services;
using Microsoft.Data.SqlClient;
using Spectre.Console;
using StateMachine;

namespace AdoFilmLibrary.MenuStateMachine.States;

public class RemoveMovieScreen(Context context) : MenuScreen("Remove Movie", context), IState {
    public override void EnterState() {
        StateTransitions = [
            new StartMenuScreen(Context),
        ];

        AnsiConsole.MarkupLine(ProgramConstants.HeaderAscii);
        AnsiConsole.MarkupLineInterpolated($"[green]<State> {MenuName} Entered![/]");
    }

    public override void RunState() {
        using var connection = SqlConnect.GetSqlConnection();
        connection.Open();

        using var command = new SqlCommand("DELETE FROM Movies WHERE Id = @Id", connection);
        command.Parameters.AddWithValue("@Id", 
            AnsiConsole.Prompt(new SelectionPrompt<Movie>()
                .Title("Select Movie To Remove From Library")
                .PageSize(5)
                .MoreChoicesText("[grey](Use arrow keys to see more movies)[/]")
                .AddChoices(MovieService.GetAllMoviesFromDb())
                .UseConverter(movie => movie.Name)).Id);
        var rowsAffected = command.ExecuteNonQuery();
        connection.Close();
        AnsiConsole.MarkupLineInterpolated($"[green]{rowsAffected} movie(s) removed![/]");

        StateSelection = StateTransitions[0];
        PauseForInput();
        base.RunState();
    }

    public string GetName() {
        return MenuName;
    }
}
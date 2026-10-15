using AdoFilmLibrary.Core;
using AdoFilmLibrary.MenuStateMachine.StatePattern;
using AdoFilmLibrary.Models;
using AdoFilmLibrary.Services;
using Microsoft.Data.SqlClient;
using Spectre.Console;
using StateMachine;

namespace AdoFilmLibrary.MenuStateMachine.States;

public class RemoveGenreScreen(Context context) : MenuScreen("Remove Genre", context), IState {
    public override void EnterState() {
        StateTransitions = [
            new StartMenuScreen(Context),
        ];

        AnsiConsole.MarkupLine(ProgramConstants.HeaderAscii);
        AnsiConsole.MarkupLineInterpolated($"[green]<State> {MenuName} Entered![/]");
    }

    public override void RunState() {
        var connection = SqlConnect.GetSqlConnection();
        connection.Open();
        AnsiConsole.MarkupLine("[green]SQL Connection Open[/]");

        var movie = AnsiConsole.Prompt(new SelectionPrompt<Movie>()
            .Title("Which movie?")
            .AddChoices(MovieService.GetAllMoviesFromDb())
            .UseConverter(m => m.Name));

        using var command = new SqlCommand("DELETE FROM Movies WHERE Id = @Id", connection);
        command.Parameters.AddWithValue("@Id", movie.Id);
        var deletedRowsAffected = command.ExecuteNonQuery();
        AnsiConsole.MarkupLineInterpolated($"[green]{deletedRowsAffected} movie(s) removed from library![/]");

        connection.Close();
        AnsiConsole.MarkupLine("[red]SQL Connection Closed[/]");
        StateSelection = StateTransitions[0];
        PauseForInput();
        base.RunState();
    }

    public string GetName() {
        return MenuName;
    }
}
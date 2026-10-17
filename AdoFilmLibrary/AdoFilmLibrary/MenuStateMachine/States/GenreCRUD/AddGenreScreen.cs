using AdoFilmLibrary.Core;
using AdoFilmLibrary.MenuStateMachine.StatePattern;
using Microsoft.Data.SqlClient;
using Spectre.Console;
using StateMachine;

namespace AdoFilmLibrary.MenuStateMachine.States.GenreCRUD;

public class AddGenreScreen(Context context) : MenuScreen("Add Genre", context), IState {
    public override void EnterState() {
        StateTransitions = [
            new StartMenuScreen(Context),
        ];

        AnsiConsole.MarkupLine(ProgramConstants.HeaderAscii);
        AnsiConsole.MarkupLineInterpolated($"[green]<State> {MenuName} Entered![/]");
    }

    public override void RunState() {
        using var connection = new SqlConnection(ProgramConstants.ConnectionString);
        connection.Open();
        AnsiConsole.MarkupLine("[green]SQL Connection Open[/]");

        using var command = new SqlCommand("INSERT INTO Genres (GenreName) Values (@Name)", connection);
        command.Parameters.AddWithValue("@Name", AnsiConsole.Ask<string>("Genre [green]name[/]?"));

        var rowsAffected = command.ExecuteNonQuery();
        AnsiConsole.MarkupLineInterpolated($"[green]{rowsAffected} genre(s) added![/]");

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
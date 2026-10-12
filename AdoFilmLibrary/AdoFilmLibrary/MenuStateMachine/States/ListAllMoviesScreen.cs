using AdoFilmLibrary.Core;
using AdoFilmLibrary.MenuStateMachine.StatePattern;
using Microsoft.Data.SqlClient;
using Spectre.Console;
using StateMachine;

namespace AdoFilmLibrary.MenuStateMachine.States;

public class ListAllMoviesScreen(Context context) : MenuScreen("All Movies", context), IState {
    public override void EnterState() {
        StateTransitions = [
            new StartMenuScreen(Context),
        ];
        
        AnsiConsole.MarkupLine(ProgramConstants.HeaderAscii);
        base.EnterState();
    }

    public override void RunState() {
        // Get all movies from Movies Table
        // Display them
        using var connection = new SqlConnection(ProgramConstants.ConnectionString);
        connection.Open();
        AnsiConsole.MarkupLine("[green]SQL Connection Open[/]");
        
        using var command = new SqlCommand(ProgramConstants.SqlQueries.SelectAllMovies, connection);
        using var reader = command.ExecuteReader();
        while (reader.Read()) {
            var id = reader.GetInt32(0);
            var name = reader.GetString(1);
            var rating = reader.GetInt32(2);
            AnsiConsole.MarkupLineInterpolated($"{id} Name: {name} Rating: {rating}");
        }

        Console.ReadKey(false);
        base.RunState();
    }

    public string GetName() {
        return MenuName;
    }
}
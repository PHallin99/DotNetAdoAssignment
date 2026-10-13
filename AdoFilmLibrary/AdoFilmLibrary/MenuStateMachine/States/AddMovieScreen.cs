using AdoFilmLibrary.Core;
using AdoFilmLibrary.MenuStateMachine.StatePattern;
using AdoFilmLibrary.Models;
using AdoFilmLibrary.Services;
using Microsoft.Data.SqlClient;
using Spectre.Console;
using StateMachine;

namespace AdoFilmLibrary.MenuStateMachine.States;

public class AddMovieScreen(Context context) : MenuScreen("Add Movie", context), IState {

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

        using var command = new SqlCommand("INSERT INTO Movies (Name, Rating, GenreId) Values (@Name, @Rating, @GenreId)", connection);
        command.Parameters.AddWithValue("@Name", AnsiConsole.Ask<string>("Movie [green]name[/]?"));
        command.Parameters.AddWithValue("@Rating", AnsiConsole.Ask<int>("What [green]rating[/] does the movie have?"));
        command.Parameters.AddWithValue("@GenreId", 
            AnsiConsole.Prompt(new SelectionPrompt<Genre>()
            .Title("What [green]Genre[/] is the movie?")
            .AddChoices(GenreService.GetAllGenresFromDb())
            .UseConverter(g => g.Name)).Id);

        var rowsAffected = command.ExecuteNonQuery();
        AnsiConsole.MarkupLineInterpolated($"[green]{rowsAffected} movie(s) added![/]");

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
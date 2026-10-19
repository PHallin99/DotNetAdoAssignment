using AdoFilmLibrary.Core;
using AdoFilmLibrary.MenuStateMachine.StatePattern;
using AdoFilmLibrary.Models;
using AdoFilmLibrary.Services;
using Microsoft.Data.SqlClient;
using Spectre.Console;
using StateMachine;

namespace AdoFilmLibrary.MenuStateMachine.States.GenreCRUD;

public class EditGenreScreen(Context context) : MenuScreen("Edit Genre", context), IState {
    public override void EnterState() {

        StateTransitions = [
            new StartMenuScreen(Context),
        ];

        AnsiConsole.MarkupLine(ProgramConstants.ConsoleMessages.HeaderAscii);
        AnsiConsole.MarkupLineInterpolated($"[green]<State> {MenuName} Entered![/]");
    }

    public override void RunState() {
        var allGenresFromDb = GenreService.GetAllGenresFromDb();
        var selectedGenre = AnsiConsole.Prompt(new SelectionPrompt<Genre>()
            .Title("Genre To [green]Edit[/] From Library")
            .PageSize(5)
            .MoreChoicesText("[grey](Use arrow keys to see more movies)[/]")
            .AddChoices(allGenresFromDb)
            .UseConverter(g => g.Name));

        AnsiConsole.Clear();

        var connection = SqlConnect.GetSqlConnection();
        connection.Open();
        AnsiConsole.MarkupLine("[green]SQL Connection Open[/]");

        if (AnsiConsole.Confirm($"Edit [green]Name[/] of \"{selectedGenre.Name}\"")) {
            AnsiConsole.Clear();
            selectedGenre.Name = AnsiConsole.Ask<string>("New [green]Name[/]");
            using var command = new SqlCommand("UPDATE Genres SET GenreName = @Name WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Name", selectedGenre.Name);
            command.Parameters.AddWithValue("@Id", selectedGenre.Id);
            var renameRowsAffected = command.ExecuteNonQuery();
            AnsiConsole.MarkupLineInterpolated($"[green]{renameRowsAffected} genres(s) renamed![/]");
        }

        connection.Close();
        AnsiConsole.MarkupLine("[red]SQL Connection Closed[/]");
        StateSelection = StateTransitions[0];
        PauseForInput();
        base.RunState();
    }
}
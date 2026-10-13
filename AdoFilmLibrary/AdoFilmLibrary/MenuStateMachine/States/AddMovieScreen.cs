using AdoFilmLibrary.Core;
using AdoFilmLibrary.MenuStateMachine.StatePattern;
using Microsoft.Data.SqlClient;
using Spectre.Console;
using StateMachine;

namespace AdoFilmLibrary.MenuStateMachine.States;

public class AddMovieScreen(Context context) : MenuScreen("Add Movie", context), IState {
    private record Genre(int Id, string Name);
    
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
        // var sql = "INSERT INTO Products (Name, Price) VALUES (@Name, @Price)";
        var name = AnsiConsole.Ask<string>("Movie [green]name[/]?");
        var rating = AnsiConsole.Ask<int>("What [green]rating[/] does the movie have?");
        var genres = new List<Genre>() {
            new Genre(0, "Cartoon"),
            new Genre(1, "Sci-Fi"),
            new Genre(2, "Fantasy"),
        };
        
        var genre = AnsiConsole.Prompt(new SelectionPrompt<Genre>()
            .AddChoices(genres)
            .Title("What [green]Genre[/] is the movie?"));

        var genreId = genre.Id;
        var sql = "INSERT INTO Movies (Name, Rating, GenreId) Values (@Name, @Rating, @GenreId)";
        
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Name", name);
        command.Parameters.AddWithValue("@Rating", rating);
        command.Parameters.AddWithValue("@GenreId", genreId);
 
        var rowsAffected = command.ExecuteNonQuery();
        AnsiConsole.MarkupLineInterpolated($"[green]{rowsAffected} movie(s) added![/]");
        
        StateSelection = StateTransitions[0];
        PauseForInput();
        base.RunState();
    }

    public string GetName() {
        return MenuName;
    }
}
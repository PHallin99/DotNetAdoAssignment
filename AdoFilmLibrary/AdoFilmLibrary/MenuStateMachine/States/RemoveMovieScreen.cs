using AdoFilmLibrary.Core;
using AdoFilmLibrary.MenuStateMachine.StatePattern;
using AdoFilmLibrary.Models;
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

        var movies = new List<Movie>();
        using var allMoviesCommand = new SqlCommand(ProgramConstants.SqlQueries.SelectAllMovies, connection);
        using (var reader = allMoviesCommand.ExecuteReader()) {
            while (reader.Read()) {
                movies.Add(new Movie {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Rating = reader.GetInt32(2),
                });
            }
        }

        var selectedMovie = AnsiConsole.Prompt(new SelectionPrompt<Movie>()
            .Title("Select Movie To Remove From Library")
            .PageSize(5)
            .MoreChoicesText("[grey](Use arrow keys to see more movies)[/]")
            .AddChoices(movies)
            .UseConverter(movie => movie.Name));

        var sql = "DELETE FROM Movies WHERE Id = @Id";
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", selectedMovie.Id);
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
using AdoFilmLibrary.Core;
using AdoFilmLibrary.MenuStateMachine.StatePattern;
using AdoFilmLibrary.Models;
using AdoFilmLibrary.Services;
using Microsoft.Data.SqlClient;
using Spectre.Console;
using StateMachine;

namespace AdoFilmLibrary.MenuStateMachine.States.MovieCRUD;

public class EditMovieScreen(Context context) : MenuScreen("Edit Movie Info", context), IState {
    public override void EnterState() {
        StateTransitions = [
            new StartMenuScreen(Context),
        ];

        AnsiConsole.MarkupLine(ProgramConstants.HeaderAscii);
        AnsiConsole.MarkupLineInterpolated($"[green]<State> {MenuName} Entered![/]");
    }

    public override void RunState() {
        var allMovies = MovieService.GetAllMoviesFromDatabase();

        if (allMovies.Count <= 0) {
            AnsiConsole.MarkupLine("[yellow]No movies in the library! Please add movies first![/]");
        } else {
            var selectedMovie = AnsiConsole.Prompt(new SelectionPrompt<Movie>()
                .Title("Movie To [green]Update[/] In Library")
                .PageSize(5)
                .MoreChoicesText("[grey](Use arrow keys to see more movies)[/]")
                .AddChoices(allMovies)
                .UseConverter(movie => movie.Name));

            AnsiConsole.Clear();

            using var connection = new SqlConnection(ProgramConstants.ConnectionString);
            connection.Open();
            if (AnsiConsole.Confirm("Edit Movie Name?")) {
                selectedMovie.Name = AnsiConsole.Ask<string>("New Movie [green]name[/]?");
                using var command = new SqlCommand("UPDATE Movies SET MovieName = @Name WHERE Id = @Id", connection);
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
                AnsiConsole.MarkupLineInterpolated(
                    $"[green]{renameRowsAffected} movie(s) had their rating changed![/]");
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

            if (AnsiConsole.Confirm("Edit Movie Publishing Year?")) {
                selectedMovie.YearOfPublication = AnsiConsole.Ask<int>("Which year?");
                using var command = new SqlCommand("UPDATE Movies SET YearOfPublication = @YearOfPublishing WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@YearOfPublishing", selectedMovie.YearOfPublication);
                command.Parameters.AddWithValue("@Id", selectedMovie.Id);
                var renameRowsAffected = command.ExecuteNonQuery();
                AnsiConsole.MarkupLineInterpolated($"[green]{renameRowsAffected} movie(s) had their publishing year changed![/]");
            }

            connection.Close();
        }

        StateSelection = StateTransitions[0];
        PauseForInput();
        base.RunState();
    }

    public string GetName() {
        return MenuName;
    }
}
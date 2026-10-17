using AdoFilmLibrary.Core;
using AdoFilmLibrary.MenuStateMachine.StatePattern;
using AdoFilmLibrary.Services;
using Spectre.Console;
using StateMachine;

namespace AdoFilmLibrary.MenuStateMachine.States;

public class ListAllMoviesScreen(Context context) : MenuScreen("All Movies", context), IState {
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
            using var connection = SqlConnect.GetSqlConnection();
            connection.Open();
            AnsiConsole.MarkupLine("[green]SQL Connection Open[/]");
            foreach (var movie in MovieService.GetAllMoviesWithGenresFromDatabase()) {
                AnsiConsole.MarkupLineInterpolated($"{movie.Name} ({movie.YearOfPublication}) - {movie.GenreName} - {movie.Rating}/10");
            }

            connection.Close();
            AnsiConsole.MarkupLine("[red]SQL Connection Closed[/]");
        }

        StateSelection = StateTransitions[0];
        PauseForInput();
        base.RunState();
    }

    public string GetName() {
        return MenuName;
    }
}
using AdoFilmLibrary.Core;
using AdoFilmLibrary.MenuStateMachine.StatePattern;
using AdoFilmLibrary.Services;
using AdoFilmLibrary.UI;
using Spectre.Console;
using StateMachine;

namespace AdoFilmLibrary.MenuStateMachine.States;

public class ListAllMoviesScreen(Context context) : MenuScreen("All Movies", context), IState {
    public override void EnterState() {
        StateTransitions = [
            new StartMenuScreen(Context),
        ];

        AnsiConsole.MarkupLine(ProgramConstants.ConsoleMessages.HeaderAscii);
        AnsiConsole.MarkupLineInterpolated($"[green]<State> {MenuName} Entered![/]");
    }

    public override void RunState() {
        var allMovies = MovieService.GetAllMoviesWithGenresFromDatabase();
        UiHelpers.DisplayMoviesOrMessage(allMovies);
        StateSelection = StateTransitions[0];
        PauseForInput();
        base.RunState();
    }
}
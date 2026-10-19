using AdoFilmLibrary.Core;
using AdoFilmLibrary.MenuStateMachine.StatePattern;
using AdoFilmLibrary.MenuStateMachine.States.MovieCRUD;
using Spectre.Console;
using StateMachine;

namespace AdoFilmLibrary.MenuStateMachine.States;

public class StartMenuScreen(Context context) : MenuScreen("Start Menu", context), IState {
    public override void EnterState() {
        AnsiConsole.MarkupLine(ProgramConstants.ConsoleMessages.HeaderAscii);
        StateTransitions = [
            new ListAllMoviesScreen(Context),
            new SearchByGenreScreen(Context),
            new AddMovieScreen(Context),
            new RemoveMovieScreen(Context),
            new EditMovieScreen(Context),
            new ExitAppScreen(Context),
        ];

        base.EnterState();
    }
}
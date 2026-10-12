using AdoFilmLibrary.Core;
using AdoFilmLibrary.MenuStateMachine.StatePattern;
using Spectre.Console;
using StateMachine;

namespace AdoFilmLibrary.MenuStateMachine.States;

public class StartMenuScreen(Context context) : MenuScreen("List Movies", context), IState {
    public override void EnterState() {
        AnsiConsole.MarkupLine(ProgramConstants.HeaderAscii);
        StateTransitions = [
            new ListAllMoviesScreen(Context),
            new AddMovieScreen(Context),
            new RemoveMovieScreen(Context),
        ];

        base.EnterState();
    }

    public string GetName() {
        return MenuName;
    }
}
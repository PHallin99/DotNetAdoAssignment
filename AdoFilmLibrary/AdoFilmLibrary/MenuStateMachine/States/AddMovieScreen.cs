using AdoFilmLibrary.Core;
using AdoFilmLibrary.MenuStateMachine.StatePattern;
using Spectre.Console;
using StateMachine;

namespace AdoFilmLibrary.MenuStateMachine.States;

public class AddMovieScreen(Context context) : MenuScreen("Add Movie", context), IState {
    public override void EnterState() {
        StateTransitions = [
            new StartMenuScreen(Context),
        ];

        AnsiConsole.MarkupLine(ProgramConstants.HeaderAscii);
        base.EnterState();
    }

    public override void RunState() {
        
        base.RunState();
    }

    public string GetName() {
        return MenuName;
    }
}
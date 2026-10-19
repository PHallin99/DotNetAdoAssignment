using AdoFilmLibrary.Core;
using AdoFilmLibrary.MenuStateMachine.StatePattern;
using Spectre.Console;
using StateMachine;

namespace AdoFilmLibrary.MenuStateMachine.States;

public class ExitAppScreen(Context context) : MenuScreen("Exit App", context), IState {
    public override void EnterState() {
        AnsiConsole.MarkupLine(ProgramConstants.ConsoleMessages.HeaderAscii);
        AnsiConsole.MarkupLine("[green]Exiting app...[/]");
        Environment.Exit(0);
    }

    public override void RunState() {
    }

    public override void ExitState() {
    }

    public string GetName() {
        return MenuName;
    }
}
using AdoFilmLibrary.Core;
using Spectre.Console;
using StateMachine;

namespace AdoFilmLibrary.MenuStateMachine.StatePattern;

public abstract class MenuScreen(string menuName, Context context) {
    protected string MenuName { get; } = menuName;
    protected IState[] StateTransitions { get; set; } = null!;
    protected Context Context { get; } = context;
    protected IState? StateSelection { get; set; }

    public virtual void EnterState() {
        AnsiConsole.MarkupLineInterpolated($"[green]{MenuName} Entered![/]");
        StateSelection = AnsiConsole.Prompt(new SelectionPrompt<IState>()
            .Title(MenuName)
            .UseConverter(menu => menu.GetName())
            .WrapAround()
            .AddChoices(StateTransitions));
    }

    public virtual void RunState() {
        AnsiConsole.MarkupLineInterpolated($"[green]{MenuName} Ran![/]");
        Context.SetState(StateSelection!);
    }

    public virtual void ExitState() {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLineInterpolated($"[green]{MenuName} Exited![/]");
    }

    protected static void PauseForInput() {
        AnsiConsole.MarkupLine(ProgramConstants.ConsoleMessages.PressAnyKeyMarkup);
        Console.ReadKey(false);
        AnsiConsole.Clear();
    }

    protected static void WriteNotFoundMarkup(string entityName) {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine(ProgramConstants.HeaderAscii);
        AnsiConsole.MarkupLineInterpolated($"[yellow]No {entityName}s found in database. Please [/][green]ADD[/] [yellow]one first![/]");
    }
}
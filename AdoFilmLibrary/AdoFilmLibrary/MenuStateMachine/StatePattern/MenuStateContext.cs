using AdoFilmLibrary.Core;
using AdoFilmLibrary.MenuStateMachine.States;
using Spectre.Console;
using StateMachine;

namespace AdoFilmLibrary.MenuStateMachine.StatePattern;

public class MenuStateContext : Context {
    public static MenuStateContext CreateInstance() {
        var context = new MenuStateContext();
        context.SetState(new StartMenuScreen(context));
        return context;
    }
    
    public override void StartContext() {
        AnsiConsole.MarkupLine("[green]App Started Successfully![/]");
        AnsiConsole.MarkupLine($"[yellow]{ProgramConstants.ConsoleMessages.PressAnyKey}[/]");
        Console.ReadKey(false);
        AnsiConsole.Clear();
        State?.EnterState();
        State?.RunState();
    }
}
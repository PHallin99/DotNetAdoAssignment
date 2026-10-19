using AdoFilmLibrary.Core;
using AdoFilmLibrary.MenuStateMachine.StatePattern;
using AdoFilmLibrary.Models;
using AdoFilmLibrary.Services;
using AdoFilmLibrary.UI;
using Spectre.Console;
using StateMachine;

namespace AdoFilmLibrary.MenuStateMachine.States;

public class SearchByGenreScreen(Context context) : MenuScreen("Search By Genre", context), IState {
    public override void EnterState() {
        StateTransitions = [
            new StartMenuScreen(Context),
        ];

        AnsiConsole.MarkupLine(ProgramConstants.ConsoleMessages.HeaderAscii);
        AnsiConsole.MarkupLineInterpolated($"[green]<State> {MenuName} Entered![/]");
    }

    public override void RunState() {
        var allGenres = GenreService.GetAllGenresFromDb();
        if (allGenres.Count == 0) {
            AnsiConsole.MarkupLine(ProgramConstants.ConsoleErrors.NoGenres);
        } else {
            var selectedGenre = AnsiConsole.Prompt(
                new SelectionPrompt<Genre>()
                    .Title("Please select genre to search with!")
                    .AddChoices(allGenres)
                    .UseConverter(g => g.Name)
            );

            UiHelpers.DisplayMoviesInGenreOrMessage(MovieService.GetAllMoviesFromGenre(selectedGenre));
        }

        StateSelection = StateTransitions[0];
        PauseForInput();
        base.RunState();
    }
}
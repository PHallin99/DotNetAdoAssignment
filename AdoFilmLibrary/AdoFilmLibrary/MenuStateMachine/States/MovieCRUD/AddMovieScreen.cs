using AdoFilmLibrary.Core;
using AdoFilmLibrary.MenuStateMachine.StatePattern;
using AdoFilmLibrary.Models;
using AdoFilmLibrary.Services;
using Spectre.Console;
using StateMachine;

namespace AdoFilmLibrary.MenuStateMachine.States.MovieCRUD;

public class AddMovieScreen(Context context) : MenuScreen("Add Movie", context), IState {

    public override void EnterState() {
        StateTransitions = [
            new StartMenuScreen(Context),
        ];

        AnsiConsole.MarkupLine(ProgramConstants.ConsoleMessages.HeaderAscii);
        AnsiConsole.MarkupLineInterpolated($"[green]<State> {MenuName} Entered![/]");
    }

    public override void RunState() {
        var name = AnsiConsole.Ask<string>("Movie [green]name[/]?");
        var rating = AnsiConsole.Ask<int>("What [green]rating[/] does the movie have?");
        var genre = AnsiConsole.Prompt(new SelectionPrompt<Genre>()
            .Title("What [green]Genre[/] is the movie?")
            .AddChoices(GenreService.GetAllGenresFromDb())
            .UseConverter(g => g.Name)).Id;

        var yearOfPublication = AnsiConsole.Ask<int>("What year did the movie publish?");
        var rowsAffected = MovieService.AddMovie(name, rating, genre, yearOfPublication);
        AnsiConsole.MarkupLineInterpolated($"[green]{rowsAffected} movie(s) added![/]");
        StateSelection = StateTransitions[0];
        PauseForInput();
        base.RunState();
    }
}
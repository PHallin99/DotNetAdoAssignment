using AdoFilmLibrary.Core;
using AdoFilmLibrary.MenuStateMachine.StatePattern;
using AdoFilmLibrary.Models;
using AdoFilmLibrary.Services;
using Spectre.Console;
using StateMachine;

namespace AdoFilmLibrary.MenuStateMachine.States.MovieCRUD;

public class EditMovieScreen(Context context) : MenuScreen("Edit Movie Info", context), IState {
    public override void EnterState() {
        StateTransitions = [
            new StartMenuScreen(Context),
        ];

        AnsiConsole.MarkupLine(ProgramConstants.ConsoleMessages.HeaderAscii);
        AnsiConsole.MarkupLineInterpolated($"[green]<State> {MenuName} Entered![/]");
    }

    public override void RunState() {
        var allMovies = MovieService.GetAllMoviesFromDatabase();
        if (allMovies.Count <= 0) {
            AnsiConsole.MarkupLine("[yellow]No movies in the library! Please add movies first![/]");
        } else {
            var selectedMovie = AnsiConsole.Prompt(new SelectionPrompt<Movie>()
                .Title("Movie To [green]Update[/] In Library")
                .PageSize(5)
                .MoreChoicesText("[grey](Use arrow keys to see more movies)[/]")
                .AddChoices(allMovies)
                .UseConverter(movie => movie.Name));

            AnsiConsole.Clear();

            var name = AnsiConsole.Confirm("Edit Movie Name?")
                ? AnsiConsole.Ask<string>("New Movie [green]name[/]?")
                : selectedMovie.Name;

            var rating = AnsiConsole.Confirm("Edit Movie Rating?")
                ? AnsiConsole.Ask<int>("New Movie [green]rating[/]?")
                : selectedMovie.Rating;

            var genre = AnsiConsole.Confirm("Edit Movie Genre?")
                ? selectedMovie.GenreId = AnsiConsole.Prompt(new SelectionPrompt<Genre>()
                    .Title("What [green]Genre[/] is the movie?")
                    .AddChoices(GenreService.GetAllGenresFromDb())
                    .UseConverter(g => g.Name)).Id
                : selectedMovie.GenreId;

            var yearOfPublishing = AnsiConsole.Confirm("Edit Movie Publishing Year?")
                ? AnsiConsole.Ask<int>("Which year?")
                : selectedMovie.YearOfPublication;

            var rowsAffected = MovieService.EditMovie(selectedMovie.Id, name, rating, genre, yearOfPublishing);
            AnsiConsole.MarkupLineInterpolated($"[green]{rowsAffected} movie(s) edited![/]");
        }

        StateSelection = StateTransitions[0];
        PauseForInput();
        base.RunState();
    }
}
using AdoFilmLibrary.Core;
using AdoFilmLibrary.Models;
using Microsoft.Data.SqlClient;

namespace AdoFilmLibrary.Services;

public static class MovieService {
    public static List<Movie> GetAllMoviesFromDatabase() {
        var movies = new List<Movie>();
        var sqlConnection = SqlConnect.GetSqlConnection();
        sqlConnection.Open();
        using var allMoviesCommand = new SqlCommand(ProgramConstants.SqlQueries.SelectAllMovies, sqlConnection);
        using var reader = allMoviesCommand.ExecuteReader();
        while (reader.Read())
            movies.Add(new Movie {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Rating = reader.GetInt32(2),
            });

        sqlConnection.Close();
        return movies;
    }

    public static List<Movie> GetAllMoviesWithGenresFromDatabase() {
        var sqlConnection = SqlConnect.GetSqlConnection();
        sqlConnection.Open();
        using var allJoinedMovieGenre = new SqlCommand(ProgramConstants.SqlQueries.SelectMoviesJoinedGenres, sqlConnection);
        using var reader = allJoinedMovieGenre.ExecuteReader();
        var genres = new List<Movie>();
        while (reader.Read()) {
            genres.Add(new Movie {
                Id = 0,
                Name = reader["MovieName"].ToString(),
                Rating = Convert.ToInt32(reader["Rating"]),
                GenreId = 0,
                GenreName = reader["GenreName"].ToString(),
                YearOfPublication = Convert.ToInt32(reader["YearOfPublication"]),
            });
        }

        sqlConnection.Close();
        return genres;
    }

    public static bool HasAnyMovies() {
        return GetAllMoviesFromDatabase().Count != 0;
    }

    public static List<Movie> GetAllMoviesFromGenre(Genre selectedGenre) {
        var moviesByGenre = new List<Movie>();
        var sqlConnection = SqlConnect.GetSqlConnection();
        sqlConnection.Open();
        using var moviesInGenreCommand = new SqlCommand(ProgramConstants.SqlQueries.SelectAllMoviesInGenreJoined, sqlConnection);
        moviesInGenreCommand.Parameters.AddWithValue("@GenreId", selectedGenre.Id);
        using var reader = moviesInGenreCommand.ExecuteReader();
        while (reader.Read()) {
            moviesByGenre.Add(
                new Movie {
                    Id = 0,
                    Name = reader["MovieName"].ToString(),
                    Rating = Convert.ToInt32(reader["Rating"]),
                    GenreId = 0,
                    GenreName = reader["GenreName"].ToString(),
                    YearOfPublication = Convert.ToInt32(reader["YearOfPublication"]),
                });
        }

        sqlConnection.Close();
        return moviesByGenre;
    }
}
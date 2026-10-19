using AdoFilmLibrary.Core;
using AdoFilmLibrary.Models;
using Microsoft.Data.SqlClient;

namespace AdoFilmLibrary.Services;

public static class MovieService {
    public static int AddMovie(string name, int rating, int genreId, int yearOfPublication) {
        using var sqlConnection = SqlConnect.GetSqlConnection();
        sqlConnection.Open();
        using var command = new SqlCommand(ProgramConstants.SqlQueries.AddMovie, sqlConnection);
        command.Parameters.AddWithValue("@Name", name);
        command.Parameters.AddWithValue("@Rating", rating);
        command.Parameters.AddWithValue("@GenreId", genreId);
        command.Parameters.AddWithValue("@YearOfPublication", yearOfPublication);
        return command.ExecuteNonQuery();
    }

    public static int RemoveMovieById(int id) {
        using var sqlConnection = SqlConnect.GetSqlConnection();
        sqlConnection.Open();
        using var command = new SqlCommand(ProgramConstants.SqlQueries.DeleteMovieById, sqlConnection);
        command.Parameters.AddWithValue("@Id", id);
        return command.ExecuteNonQuery();
    }

    public static int EditMovie(int movieId, string name, int rating, int genreId, int yearOfPublishing) {
        using var sqlConnection = SqlConnect.GetSqlConnection();
        sqlConnection.Open();
        using var command = new SqlCommand(ProgramConstants.SqlQueries.UpdateMovieQuery, sqlConnection);
        command.Parameters.AddWithValue("@Name", name);
        command.Parameters.AddWithValue("@Rating", rating);
        command.Parameters.AddWithValue("@GenreId", genreId);
        command.Parameters.AddWithValue("@YearOfPublishing", yearOfPublishing);
        command.Parameters.AddWithValue("@Id", movieId);
        return command.ExecuteNonQuery();
    }
    
    public static List<Movie> GetAllMoviesFromDatabase() {
        var movies = new List<Movie>();
        using var sqlConnection = SqlConnect.GetSqlConnection();
        sqlConnection.Open();
        using var allMoviesCommand = new SqlCommand(ProgramConstants.SqlQueries.SelectAllMovies, sqlConnection);
        using var reader = allMoviesCommand.ExecuteReader();
        while (reader.Read())
            movies.Add(new Movie {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Rating = reader.GetInt32(2),
            });

        return movies;
    }

    public static List<Movie> GetAllMoviesWithGenresFromDatabase() {
        using var sqlConnection = SqlConnect.GetSqlConnection();
        sqlConnection.Open();
        using var allJoinedMovieGenre = new SqlCommand(ProgramConstants.SqlQueries.SelectMoviesJoinedGenres, sqlConnection);
        using var reader = allJoinedMovieGenre.ExecuteReader();
        var genres = new List<Movie>();
        while (reader.Read()) {
            genres.Add(new Movie {
                Id = 0, // Data not available from query - Query for display only
                Name = reader["MovieName"].ToString() ?? string.Empty,
                Rating = Convert.ToInt32(reader["Rating"]),
                GenreId = 0, // Data not available from query - Query for display only
                GenreName = reader["GenreName"].ToString() ?? string.Empty,
                YearOfPublication = Convert.ToInt32(reader["YearOfPublication"]),
            });
        }

        return genres;
    }

    public static List<Movie> GetAllMoviesFromGenre(Genre selectedGenre) {
        var moviesByGenre = new List<Movie>();
        using var sqlConnection = SqlConnect.GetSqlConnection();
        sqlConnection.Open();
        using var moviesInGenreCommand = new SqlCommand(ProgramConstants.SqlQueries.SelectAllMoviesInGenreJoined, sqlConnection);
        moviesInGenreCommand.Parameters.AddWithValue("@GenreId", selectedGenre.Id);
        using var reader = moviesInGenreCommand.ExecuteReader();
        while (reader.Read()) {
            moviesByGenre.Add(
                new Movie {
                    Id = 0, // Data not available from query - Query for display only
                    Name = reader["MovieName"].ToString() ?? string.Empty,
                    Rating = Convert.ToInt32(reader["Rating"]),
                    GenreId = 0, // Data not available from query - Query for display only
                    GenreName = reader["GenreName"].ToString() ?? string.Empty,
                    YearOfPublication = Convert.ToInt32(reader["YearOfPublication"]),
                });
        }

        return moviesByGenre;
    }
}
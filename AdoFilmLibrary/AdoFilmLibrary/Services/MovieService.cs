using AdoFilmLibrary.Core;
using AdoFilmLibrary.Models;
using Microsoft.Data.SqlClient;

namespace AdoFilmLibrary.Services;

public static class MovieService {
    public static List<Movie> GetAllMoviesFromDb() {
        var movies = new List<Movie>();
        var sqlConnection = SqlConnect.GetSqlConnection();
        sqlConnection.Open();
        using var allMoviesCommand = new SqlCommand(ProgramConstants.SqlQueries.SelectAllMovies, sqlConnection);
        using var reader = allMoviesCommand.ExecuteReader();
        while (reader.Read()) {
            movies.Add(new Movie {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Rating = reader.GetInt32(2),
            });
        }

        sqlConnection.Close();
        return movies;
    }
}
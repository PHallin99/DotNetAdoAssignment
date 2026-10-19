using AdoFilmLibrary.Core;
using AdoFilmLibrary.Models;
using Microsoft.Data.SqlClient;

namespace AdoFilmLibrary.Services;

public static class GenreService {
    public static List<Genre> GetAllGenresFromDb() {
        var genres = new List<Genre>();
        using var sqlConnection = SqlConnect.GetSqlConnection();
        sqlConnection.Open();
        using var allGenresCommand = new SqlCommand(ProgramConstants.SqlQueries.SelectAllGenres, sqlConnection);
        using var reader = allGenresCommand.ExecuteReader();
        while (reader.Read()) {
            genres.Add(new Genre {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
            });
        }

        return genres;
    }
}
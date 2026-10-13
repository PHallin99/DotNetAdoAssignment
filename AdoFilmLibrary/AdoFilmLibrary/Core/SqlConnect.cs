using Microsoft.Data.SqlClient;

namespace AdoFilmLibrary.Core;

public static class SqlConnect {
    public static SqlConnection GetSqlConnection() {
        return new SqlConnection(ProgramConstants.ConnectionString);
    }
}
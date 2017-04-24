using System.Data;
using System.Data.SqlClient;
using DAL_Database.Constants;
using Microsoft.Extensions.Configuration;

namespace DAL_Database.Dapper.Repositories
{
    public class BaseRepository
    {
        private readonly string _connectionString;
        protected SqlConnection Connection => new SqlConnection(_connectionString);

        public BaseRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString(ConnectionStringContants.AnimeConnectionString);
        }

    }
    
}

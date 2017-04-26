using System.Data.SqlClient;
using System.Threading.Tasks;
using DAL_Database.Constants;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace DAL_Database.Dapper.QueryHandlers
{
    public abstract class BaseQueryHandler<TQuery, TDto> : IAsyncRequestHandler<TQuery, TDto> where TQuery : IRequest<TDto>
    {
        #region Sql Connection
        private readonly string _connectionString;
        protected SqlConnection Connection => new SqlConnection(_connectionString);
        #endregion
        protected BaseQueryHandler(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString(ConnectionStringContants.AnimeConnectionString);
        }

        public abstract Task<TDto> Handle(TQuery message);
    }
}

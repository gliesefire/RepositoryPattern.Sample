using Dapper;
using Microsoft.Extensions.Logging;
using RepositoryPattern.Abstractions;
using System.Data.Common;
using System.Threading.Tasks;

namespace RepositoryPattern.Sample.Persistence
{
    public interface IErrorRepository
    {
        Task<bool> AddAsync(string errorMessage);
    }

    public class ErrorRepository : IErrorRepository
    {
        private readonly IConnectionFactory _factory;
        private readonly ILogger<ErrorRepository> _logger;

        public ErrorRepository(IConnectionFactory factory, ILoggerFactory loggerFactory)
        {
            _factory = factory;
            _logger = loggerFactory.CreateLogger<ErrorRepository>();
        }

        public async Task<bool> AddAsync(string errorMessage)
        {
            try
            {
                var connection = await _factory.CreateOpenConnectionAsync();
                var rowsAffected = connection.Execute($"insert into errors(_message) values(@{nameof(errorMessage)})", new { errorMessage }, _factory.CurrentTransaction);
                if (rowsAffected == 1)
                    return true;
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Something messed up");
            }
            return false;
            
        }
    }
}

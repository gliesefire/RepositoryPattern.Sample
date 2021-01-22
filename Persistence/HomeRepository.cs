using Dapper;
using Microsoft.Extensions.Logging;
using RepositoryPattern.Abstractions;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace RepositoryPattern.Sample.Persistence
{
    public interface IHomeRepository
    {
        Task<bool> AddAsync(DateTime visitedAt);
    }

    public class HomeRepository : IHomeRepository
    {
        private readonly IConnectionFactory _factory;
        private readonly ILogger<HomeRepository> _logger;

        public HomeRepository(IConnectionFactory factory, ILoggerFactory loggerFactory)
        {
            _factory = factory;
            _logger = loggerFactory.CreateLogger<HomeRepository>();
        }

        public async Task<bool> AddAsync(DateTime visitedAt)
        {
            try
            {
                var connection = await _factory.CreateOpenConnectionAsync();
                var rowsAffected = connection.Execute($"insert into visitors(visited_at) values(@{nameof(visitedAt)})", new { visitedAt }, _factory.CurrentTransaction);
                if (rowsAffected == 1)
                    return true;                
            }
            catch(DbException ex)
            {
                _logger.LogError(ex, "Something messed up");
            }
            return false;
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using RepositoryPattern.Abstractions;
using RepositoryPattern.Sample.Persistence;
using System;
using System.Threading.Tasks;

namespace RepositoryPattern.Sample
{
    public class DbContext : IUnitOfWork
    {
        private readonly IServiceProvider _provider;
        private readonly IConnectionFactory _factory;

        public DbContext(IServiceProvider provider, IConnectionFactory factory)
        {
            _provider = provider;
            _factory = factory;

            // This can be called from a separate method too, if you wish to manually initiate a transaction instead of this implicit transaction.
            factory.BeginTransaction().GetAwaiter().GetResult();
        }

        private IErrorRepository _errors;
        public IErrorRepository Errors
        {
            get
            {
                if (_errors is null)
                    _errors = _provider.GetRequiredService<IErrorRepository>();
                return _errors;
            }
        }

        private IHomeRepository _home;
        public IHomeRepository Homes
        {
            get
            {
                if (_home is null)
                    _home = _provider.GetRequiredService<IHomeRepository>();
                return _home;
            }
        }

        public Task SaveChanges()
        {
            return _factory.SaveChanges();
        }
    }
}

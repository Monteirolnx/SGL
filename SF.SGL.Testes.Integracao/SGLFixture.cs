using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using SF.SGL.Infra.Data.Contextos;
using Xunit;

namespace SF.SGL.Testes.Integracao
{
    
    [CollectionDefinition(nameof(SGLFixture))]
    public class SliceFixtureCollection : ICollectionFixture<SGLFixture> { }

    public class SGLFixture : IAsyncLifetime
    {
        private readonly WebApplicationFactory<API.Startup> _factory;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly Checkpoint _checkpoint;
        private readonly IConfiguration _configuration;

        public SGLFixture()
        {
            _factory = new SGLTesteAplicacaoFactory();
            _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
            
            _configuration = _factory.Services.GetRequiredService<IConfiguration>();
            _checkpoint = new Checkpoint();
        }
        
        internal class SGLTesteAplicacaoFactory : WebApplicationFactory<API.Startup>
        {
            protected override void ConfigureWebHost(IWebHostBuilder builder)
            {
                builder.ConfigureAppConfiguration((_, configBuilder) =>
                {
                    configBuilder.AddInMemoryCollection(new Dictionary<string, string>
                    {
                        {"ConnectionStrings:DefaultConnection", _connectionString}
                    });
                });
            }

            private readonly string _connectionString = "Data Source=COMPUTADOR01\\SQLEXPRESS;Initial Catalog=SGL-Testes;Integrated Security=True";
        }

        public Task<T> ExecuteDbContextAsync<T>(Func<SGLContexto, Task<T>> action)
        {
            return ExecuteScopeAsync(sp => action(sp.GetService<SGLContexto>()));
        }

        public async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
        {
            using IServiceScope scope = _scopeFactory.CreateScope();
            SGLContexto sglContexto = scope.ServiceProvider.GetRequiredService<SGLContexto>();

            try
            {
                await sglContexto.BeginTransactionAsync();

                T result = await action(scope.ServiceProvider);

                await sglContexto.CommitTransactionAsync();

                return result;
            }
            catch (Exception)
            {
                sglContexto.RollbackTransaction();
                throw;
            }
        }

        public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            return ExecuteScopeAsync(sp =>
            {
                var mediator = sp.GetRequiredService<IMediator>();

                return mediator.Send(request);
            });
        }

        public Task<T> FindAsync<T>(int id) where T : class
        {
            return ExecuteDbContextAsync(db => db.Set<T>().FindAsync(id).AsTask());
        }

        public Task InitializeAsync()
        {
            return _checkpoint.Reset(_configuration.GetConnectionString("DefaultConnection"));
        }

        public Task DisposeAsync()
        {
            _factory?.Dispose();
            return Task.CompletedTask;
        }
    }
}

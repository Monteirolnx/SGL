using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using SF_SGL.Data;
using SF_SGL.Entidades;
using Xunit;

namespace SF_SGL.IntegrationTests
{
    [CollectionDefinition(nameof(SliceFixture))]
    public class SliceFixtureCollection : ICollectionFixture<SliceFixture> { }

    public class SliceFixture : IAsyncLifetime
    {
        private readonly Checkpoint _checkpoint;
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly WebApplicationFactory<Startup> _factory;

        public SliceFixture()
        {
            _factory = new SGLTestApplicationFactory();

            _configuration = _factory.Services.GetRequiredService<IConfiguration>();
            _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();

            _checkpoint = new Checkpoint();
        }

        public class SGLTestApplicationFactory : WebApplicationFactory<Startup>
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

            private readonly string _connectionString = "Data Source=COMPUTADOR01\\SQLEXPRESS;Initial Catalog=SGL;Integrated Security=True";
        }

        internal Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            return ExecuteScopeAsync(sp =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                return mediator.Send(request);
            });
        }

        internal Task<T> FindAsync<T>(int id) where T : class, IEntity
        {
            return ExecuteDbContextAsync(db => db.Set<T>().FindAsync(id).AsTask());
        }
        public Task<T> ExecuteDbContextAsync<T>(Func<SGLContext, Task<T>> action)
        {
            return ExecuteScopeAsync(sp => action(sp.GetService<SGLContext>()));
        }

        public async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
        {
            using var scope = _scopeFactory.CreateScope();
            SGLContext dbContext = scope.ServiceProvider.GetRequiredService<SGLContext>();

            try
            {
                await dbContext.BeginTransactionAsync();

                T result = await action(scope.ServiceProvider);

                await dbContext.CommitTransactionAsync();

                return result;
            }
            catch (Exception)
            {
                dbContext.RollbackTransaction();
                throw;

            }
        }

        public Task InitializeAsync()
        {
            _checkpoint.TablesToIgnore = new[] { "sistema" };
            return _checkpoint.Reset(_configuration.GetConnectionString("DefaultConnection"));
        }

        public Task DisposeAsync()
        {
            _factory?.Dispose();
            return Task.CompletedTask;
        }
    }
}

using System.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SF.SGL.Dominio.Entidades;

namespace SF.SGL.Infra.Data.Contextos
{
    public class SGLContexto : DbContext
    {
        private IDbContextTransaction _currentTransaction;

        public DbSet<EntidadeSistema> EntidadeSistema { get; set; }

        public DbSet<EntidadeParametroExpurgo> EntidadeExpurgo {  get; set; }

        public SGLContexto(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            _ = modelBuilder.ApplyConfigurationsFromAssembly(typeof(SGLContexto).Assembly);
        }

        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                return;
            }
            // Instalar pacote microsoft.entityframeworkcore.relational.
            _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync();

                await (_currentTransaction?.CommitAsync() ?? Task.CompletedTask);
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
    }
}

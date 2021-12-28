namespace SF.SGL.API.Infra.Contexto;

public class SGLContexto : DbContext
{
    private IDbContextTransaction _currentTransaction;

    public DbSet<EntidadeSistema> EntidadeSistema { get; set; }

    public DbSet<EntidadeParametroExpurgo> EntidadeExpurgo { get; set; }

    public DbSet<EntidadeMonitoramento> EntidadeMonitoramento { get; set; }

    public DbSet<EntidadeExecucaoMonitoramento> EntidadeExecucaoMonitoramento { get; set; }

    public SGLContexto(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SGLContexto).Assembly);
        modelBuilder.HasDefaultSchema("dbo");
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

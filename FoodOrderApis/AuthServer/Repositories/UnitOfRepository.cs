using AuthServer.Data.Context;
using AuthServer.Repositories.Implements;
using AuthServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace AuthServer.Repositories;

public class UnitOfRepository : IUnitOfRepository, IDisposable
{
    private readonly AuthDbContext _context;
    private IDbContextTransaction _transaction;
    public IApiResourceRepository ApiResource { get; }
    public IApiResourceScopeRepository ApiResourceScope { get; }
    public IClientGrantTypeRepository ClientGrantType { get; }
    public IClientRepository Client { get; }
    public IClientScopeRepository ClientScope { get; }
    public IClientSecretRepository ClientSecret { get; }
    public IUserRepository User { get; }
    public IRolePermissionRepository RolePermission { get; }

    public UnitOfRepository(AuthDbContext context)
    {
        _context = context;
        ApiResource = new ApiResourceRepository(_context);
        ApiResourceScope = new ApiResourceScopeRepository(_context);
        ClientGrantType = new ClientGrantTypeRepository(_context);
        Client = new ClientRepository(_context);
        ClientScope = new ClientScopeRepository(_context);
        ClientSecret = new ClientSecretRepository(_context);
        User = new UserRepository(_context);
        RolePermission = new RolePermissionRepository(_context);
    }
    
    
    public async Task CompleteAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<IDbContextTransaction> OpenTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
        return _transaction;
    }

    public async Task CommitAsync()
    {
        await _transaction.CommitAsync();
    }

    public async Task RollbackAsync()
    {
        await _transaction.RollbackAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}

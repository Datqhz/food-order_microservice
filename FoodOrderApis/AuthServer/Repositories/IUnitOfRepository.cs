using AuthServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace AuthServer.Repositories;

public interface IUnitOfRepository
{
    public IApiResourceRepository ApiResource { get; }
    public IApiResourceScopeRepository ApiResourceScope { get; }
    public IClientGrantTypeRepository ClientGrantType { get; }
    public IClientRepository Client { get; }
    public IClientScopeRepository ClientScope { get; }
    public IClientSecretRepository ClientSecret { get; }
    public IUserRepository User { get; }

    Task CompleteAsync();
    Task<IDbContextTransaction> OpenTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}

using AuthServer.Data.Context;
using AuthServer.Data.Models;
using AuthServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Repositories.Implements;

public class ClientScopeRepository : GenericRepository<ClientScope>, IClientScopeRepository
{
    public ClientScopeRepository(AuthDbContext context) : base(context){}
}
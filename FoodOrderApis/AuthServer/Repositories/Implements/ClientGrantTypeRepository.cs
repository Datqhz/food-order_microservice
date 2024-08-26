using AuthServer.Data.Context;
using AuthServer.Data.Models;
using AuthServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Repositories.Implements;

public class ClientGrantTypeRepository : GenericRepository<ClientGrantType>, IClientGrantTypeRepository
{
    public ClientGrantTypeRepository(AuthDbContext context) : base(context){}
}
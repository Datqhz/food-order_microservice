using AuthServer.Data.Context;
using AuthServer.Data.Models;
using AuthServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Repositories.Implements;

public class ClientRepository : GenericRepository<Client>, IClientRepository
{
    public ClientRepository(AuthDbContext context) : base(context){}
}
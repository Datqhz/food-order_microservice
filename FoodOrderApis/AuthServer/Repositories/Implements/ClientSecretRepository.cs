using AuthServer.Data.Context;
using AuthServer.Data.Models;
using AuthServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Repositories.Implements;

public class ClientSecretRepository : GenericRepository<ClientSecret>, IClientSecretRepository
{
    public ClientSecretRepository(AuthDbContext context) : base(context){}
}
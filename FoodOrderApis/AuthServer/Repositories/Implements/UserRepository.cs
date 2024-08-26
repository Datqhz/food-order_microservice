using AuthServer.Data.Context;
using AuthServer.Data.Models;
using AuthServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Repositories.Implements;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(AuthDbContext context) : base(context){}
}
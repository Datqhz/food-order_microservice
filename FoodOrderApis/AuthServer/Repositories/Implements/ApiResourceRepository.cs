using AuthServer.Data.Context;
using AuthServer.Data.Models;
using AuthServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Repositories.Implements;

public class ApiResourceRepository : GenericRepository<ApiResource>, IApiResourceRepository
{
    public ApiResourceRepository(AuthDbContext context) : base(context){}
}
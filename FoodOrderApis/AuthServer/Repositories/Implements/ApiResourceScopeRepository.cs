using AuthServer.Data.Context;
using AuthServer.Data.Models;
using AuthServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Repositories.Implements;

public class ApiResourceScopeRepository : GenericRepository<ApiResourceScope>, IApiResourceScopeRepository
{
    public ApiResourceScopeRepository(AuthDbContext context) : base(context){}
}
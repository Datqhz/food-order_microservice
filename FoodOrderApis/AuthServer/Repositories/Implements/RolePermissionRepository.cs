using AuthServer.Data.Context;
using AuthServer.Data.Models;
using AuthServer.Repositories.Interfaces;

namespace AuthServer.Repositories.Implements;

public class RolePermissionRepository : GenericRepository<RolePermission>,IRolePermissionRepository
{
    public RolePermissionRepository(AuthDbContext context) : base(context)
    {
    }
}

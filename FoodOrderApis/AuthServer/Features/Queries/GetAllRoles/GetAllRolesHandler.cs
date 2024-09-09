using AuthServer.Data.Dtos;
using AuthServer.Data.Responses;
using AuthServer.Repositories;
using FoodOrderApis.Common.Helpers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Features.Queries.GetAllRoles;

public class GetAllRolesHandler : IRequestHandler<GetAllRolesQuery, GetAllRolesResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly ILogger<GetAllRolesHandler> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;

    public GetAllRolesHandler(IUnitOfRepository unitOfRepository, ILogger<GetAllRolesHandler> logger, RoleManager<IdentityRole> roleManager)
    {
        _unitOfRepository = unitOfRepository;
        _logger = logger;
        _roleManager = roleManager;
    }
    public async Task<GetAllRolesResponse> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var functionName = nameof(GetAllRolesHandler);
        _logger.LogInformation($"{functionName} - Start");
        var response = new GetAllRolesResponse() {StatusCode = (int)ResponseStatusCode.BadRequest};
        try
        {
            var roles = await _roleManager.Roles
                .AsNoTracking()
                .Select(r => new RoleDto
                    {
                        Id = r.Id,
                        RoleName = r.Name
                    }
                ).ToListAsync(cancellationToken);
            response.StatusCode = (int)ResponseStatusCode.OK;
            response.StatusText = "Get all roles successful";
            response.Data = roles;
            _logger.LogInformation($"{functionName} - End");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{functionName} has error: Message = {ex.Message}");
            response.StatusCode = (int)ResponseStatusCode.InternalServerError;
            response.StatusText = "Internal Server Error";
            response.ErrorMessage = ex.Message;
            return response;
        }
    }
}
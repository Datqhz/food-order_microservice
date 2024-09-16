using AuthServer.Data.Models;
using AuthServer.Data.Responses;
using AuthServer.Repositories;
using FoodOrderApis.Common.Helpers;
using FoodOrderApis.Common.HttpContextCustom;
using IdentityModel.Client;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Features.Queries.RevokeToken;

public class RevokeTokenHandler : IRequestHandler<RevokeTokenQuery, RevokeTokenResponse>
{
    private readonly ILogger<RevokeTokenHandler> _logger;
    private readonly HttpClient _httpClient;
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly ICustomHttpContextAccessor _httpContext;

    public RevokeTokenHandler(ILogger<RevokeTokenHandler> logger, HttpClient httpClient, IUnitOfRepository unitOfRepository, ICustomHttpContextAccessor httpContext)
    {
        _logger = logger;
        _httpClient = httpClient;
        _unitOfRepository = unitOfRepository;
        _httpContext = httpContext;
    }
    public async Task<RevokeTokenResponse> Handle(RevokeTokenQuery request, CancellationToken cancellationToken)
    {
        var functionName = nameof(RevokeTokenHandler);
        _logger.LogInformation($"{functionName} - Start");
        var payload = request.Payload;
        var response = new RevokeTokenResponse(){StatusCode = (int)ResponseStatusCode.OK};
        try
        {
            var clientId = _httpContext.GetClientId();
            var clientSecret = await
                (
                    from client in _unitOfRepository.Client.GetAll()
                    join cs in _unitOfRepository.ClientSecret.GetAll()
                        on client.Id equals cs.Id
                    select new ClientSecret
                    {
                        Id = cs.Id,
                        SecretName = cs.SecretName
                    }
                )
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var host = "authserver";
            if (environment == "Development")
            {
                host = "localhost";
            }
            var revokeTokenRequest = new TokenRevocationRequest
            {
                Address = $"http://{host}:5092/connect/revocation",
                ClientId = clientId,
                ClientSecret = clientSecret.SecretName,
                Token = payload.AccessToken
            };
            var revokeTokenResponse = await _httpClient.RevokeTokenAsync(revokeTokenRequest, cancellationToken);
            revokeTokenResponse.HttpResponse.EnsureSuccessStatusCode();
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{functionName} => has error: Message = {ex.Message}");
            response.StatusCode = (int)ResponseStatusCode.InternalServerError;
            response.StatusText = "Internal Server Error";
            response.ErrorMessage = ex.Message;
            return response;
        }
    }
}

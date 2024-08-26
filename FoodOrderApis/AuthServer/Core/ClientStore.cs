
using AuthServer.Repositories;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Core;

public class ClientStore : IClientStore
{
    private readonly IUnitOfRepository _unitOfRepository;

    public ClientStore(IUnitOfRepository unitOfRepository)
    {
        _unitOfRepository = unitOfRepository;
    }
    public async Task<Client> FindClientByIdAsync(string clientId)
    {
        try
        {
            var clients = await (
                from cl in _unitOfRepository.Client.GetAll() 
                where cl.ClientId == clientId 
                select new
                {
                    ClientId = cl.ClientId,
                    GrantTypes = 
                    (
                        from gt in _unitOfRepository.ClientGrantType.GetAll() 
                        where gt.ClientId == cl.Id
                        select gt.GrantType
                    )
                    .ToList(),
                    Secrets = 
                        (
                            from sc in _unitOfRepository.ClientSecret.GetAll() 
                            where sc.ClientId == cl.Id
                            select sc.SecretName
                        )
                        .ToList(),
                    Scopes = 
                        (
                            from sc in _unitOfRepository.ClientScope.GetAll() 
                            where sc.ClientId == cl.Id
                            select sc.Scope
                            )
                        .ToList(),
                }
                    ).AsNoTracking().ToListAsync();
           
            if (!clients.Any())
            {
                return new Client();
            }

            var client = clients
                .Select(s => new Client
                {
                    ClientId = s.ClientId,
                    AllowedGrantTypes = s.GrantTypes?.ToList() ?? new List<string>(),
                    ClientSecrets = s.Secrets?.Select(cs => new Secret(cs.Sha256())).ToList(),
                    AllowedScopes = s.Scopes?.ToList() ?? new List<string>(),
                }).FirstOrDefault();

            return client;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new Client();
        }
    }
}

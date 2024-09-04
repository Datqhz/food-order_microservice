using CustomerService.Data.Responses;
using CustomerService.Repositories;
using FoodOrderApis.Common.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Features.Queries.UserInfoQueries.GetAllMerchant;

public class GetAllMerchantHandler : IRequestHandler<GetAllMerchantQuery, GetAllMerchantByIdResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;

    public GetAllMerchantHandler(IUnitOfRepository unitOfRepository)
    {
        _unitOfRepository = unitOfRepository;
    }
    public async Task<GetAllMerchantByIdResponse> Handle(GetAllMerchantQuery request, CancellationToken cancellationToken)
    {
        var response = new GetAllMerchantByIdResponse(){StatusCode = (int)ResponseStatusCode.InternalServerError};

        try
        {
            var merchants = await _unitOfRepository.User.Where(u => u.ClientId == "Merchant").AsNoTracking()
                .ToListAsync(cancellationToken);
            response.StatusCode = (int)ResponseStatusCode.OK;
            response.Data = merchants;
            response.StatusText = $"Successfully retrieved all merchants";
            return response;
        }
        catch (Exception ex)
        {
            response.StatusText = "Internal Server Error";
            response.ErrorMessage = ex.Message;
            return response;
        }
    }
}

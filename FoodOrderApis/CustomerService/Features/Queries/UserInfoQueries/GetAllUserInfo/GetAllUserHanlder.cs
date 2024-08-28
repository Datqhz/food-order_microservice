using CustomerService.Data.Responses;
using CustomerService.Repositories;
using FoodOrderApis.Common.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Features.Queries.UserInfoQueries.GetAllUserInfo;

public class GetAllUserHanlder : IRequestHandler<GetAllUserQuery, GetAllUserInfoResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;

    public GetAllUserHanlder(IUnitOfRepository unitOfRepository)
    {
        _unitOfRepository = unitOfRepository;
    }
    public async Task<GetAllUserInfoResponse> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
    {
        var response = new GetAllUserInfoResponse(){StatusCode = (int)ResponseStatusCode.BadRequest};
        try
        {
            var users = await _unitOfRepository.User.GetAll().AsNoTracking().ToListAsync();
            response.Data = users;
            response.StatusCode = (int)ResponseStatusCode.OK;
            response.StatusText = "Get all user successfully";
            return response;
        }
        catch (Exception ex)
        {
            response.StatusCode = (int)ResponseStatusCode.InternalServerError;
            response.StatusText = "Internal Server Error";
            response.ErrorMessage = ex.Message;
            return response;
        }
    }
}

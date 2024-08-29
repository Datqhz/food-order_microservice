using CustomerService.Data.Responses;
using CustomerService.Repositories;
using FoodOrderApis.Common.Helpers;
using MediatR;

namespace CustomerService.Features.Queries.UserInfoQueries.GetUserInfoById;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, GetUserInfoByIdResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;

    public GetUserByIdHandler(IUnitOfRepository unitOfRepository)
    {
        _unitOfRepository = unitOfRepository;
    }
    public async Task<GetUserInfoByIdResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new GetUserInfoByIdResponse(){StatusCode = (int)ResponseStatusCode.BadRequest};
        try
        {
            GetUserByIdValidator validator = new GetUserByIdValidator();
            var validateResult = validator.Validate(request);
            if (!validateResult.IsValid)
            {
                response.StatusText = validateResult.ToString("~");
                return response;
            }

            var user = await _unitOfRepository.User.GetById(request.UserId);
            if (user == null)
            {
                response.StatusCode = (int)ResponseStatusCode.NotFound;
                response.StatusText = "User does not exist";
            }
            else
            {
                response.StatusCode = (int)ResponseStatusCode.OK;
                response.StatusText = "User retrieved";
                response.Data = user;
            }
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

using CustomerService.Data.Responses;
using MediatR;

namespace CustomerService.Features.Queries.UserInfoQueries.GetUserInfoById;

public class GetUserByIdQuery : IRequest<GetUserInfoByIdResponse>
{
    public string UserId { get; set; }
}

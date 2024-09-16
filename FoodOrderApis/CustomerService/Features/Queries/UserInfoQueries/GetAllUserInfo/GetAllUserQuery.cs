using CustomerService.Data.Requests;
using CustomerService.Data.Responses;
using FoodOrderApis.Common.Enums;
using MediatR;

namespace CustomerService.Features.Queries.UserInfoQueries.GetAllUserInfo;

public class GetAllUserQuery : IRequest<GetAllUserInfoResponse>
{
    public GetAllUserInfoRequest Payload { get; set; }
}

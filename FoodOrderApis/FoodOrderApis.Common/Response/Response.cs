using FoodOrderApis.Common.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderApis.Common.Helpers;

public class BaseResponse
{
    public int StatusCode { get; set; }
    public string StatusText { get; set; }
    public string ErrorMessage { get; set; }
}
public static class ResponseHelper
{
    public static ObjectResult ToResponse(int httpStatusCode, string statusText, string errorMessage = "",
        object data = null)
    {
        return new ObjectResult(new
        {
            StatusCode = httpStatusCode,
            StatusText = statusText,
            ErrorMessage = errorMessage,
            Data = data
        }){StatusCode = httpStatusCode};
    }
    public static ObjectResult ToPaginationResponse(int httpStatusCode, string statusText, string errorMessage,  object data = null, PagingDto paging = null)
    {
        return new ObjectResult(new
            {
                StatusCode = httpStatusCode,
                StatusText = statusText,
                ErrorMessage = errorMessage,
                Data = data,
                Paging = paging
            })
            { StatusCode = httpStatusCode };
    }
}
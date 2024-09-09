using FoodOrderApis.Common.Helpers;

namespace FoodOrderApis.Common.Validation;

public class ValidationException : Exception
{
    public int StatusCode { get; } = (int)ResponseStatusCode.BadRequest;
    public string Message { get; } = "Validation Failed";
    public List<ValidationError> Errors { get; set; }
}

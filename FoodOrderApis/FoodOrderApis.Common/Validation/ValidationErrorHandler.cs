using Newtonsoft.Json;

namespace FoodOrderApis.Common.Validation;

public class ValidationErrorHandler
{
    public int statusCode { get; set; }
    public string statusText { get; set; }
    public string errorMessage { get; set; }
    public List<ValidationError> errors { get; set; }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}

using System.Text.Json.Serialization;

namespace FoodService.Data.InternalResponse;

public class CheckFoodIsUsedResponse
{ 
    [JsonPropertyName("statusCode")]
    public int StatusCode { get; set; }
    [JsonPropertyName("statusText")]
    public string StatusText { get; set; }
    [JsonPropertyName("data")]
    public bool Data { get; set; }
    [JsonPropertyName("errorMessage")]
    public string ErrorMessage { get; set; }
}


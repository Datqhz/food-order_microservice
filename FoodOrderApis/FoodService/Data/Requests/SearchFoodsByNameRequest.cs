namespace FoodService.Data.Requests;

public class SearchFoodsByNameRequest
{
    public string Keyword { get; set; }
    public int? Page { get; set; }
    public int? MaxPerPage { get; set; }
}

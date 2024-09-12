namespace CustomerService.Data.Requests;

public class SearchMerchantByNameRequest
{
    public string Keyword { get; set; }
    public int? Page { get; set; }
    public int? MaxPerPage { get; set; }
}

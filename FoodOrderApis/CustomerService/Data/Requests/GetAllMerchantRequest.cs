namespace CustomerService.Data.Requests;

public class GetAllMerchantRequest
{
    public int PageNumber { get; set; } = 1;
    public int MaxPerPage { get; set; } = 10;
}

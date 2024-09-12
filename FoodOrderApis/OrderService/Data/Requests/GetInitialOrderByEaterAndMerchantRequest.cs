namespace OrderService.Data.Requests;

public class GetInitialOrderByEaterAndMerchantRequest
{
    public string EaterId { get; set; }
    public string MerchantId { get; set; }
}

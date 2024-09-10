namespace OrderService.Data.Requests;

public class GetInitialOrderByEaterAndMerchantInput
{
    public string EaterId { get; set; }
    public string MerchantId { get; set; }
}

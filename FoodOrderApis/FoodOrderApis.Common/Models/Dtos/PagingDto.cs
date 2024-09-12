namespace FoodOrderApis.Common.Models.Dtos;

public class PagingDto
{
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public int PageNumber { get; set; }
    public int MaxPerPage { get; set; }
}

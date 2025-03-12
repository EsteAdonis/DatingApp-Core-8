namespace API.Helpers;

public class PaginationHeader(int currentePage, int itemsPerPage, int totalItems, int totalPages)
{
  public int CurrentPage { get; set; } = currentePage;
  public int ItemsPerPage { get; set; } = itemsPerPage;
  public int TotalItems { get; set; } = totalItems;
  public int TotalPages { get; set; } = totalPages;
}

namespace API.Helpers;

public class UserParams
{
  public const int MaxpageSize = 50;
  public int PageNumber { get; set; } = 1;
  public int _pageSize = 10;

  public int PageSize
  {
    get => _pageSize; 
    set =>_pageSize = (value > MaxpageSize) ? MaxpageSize : value;
  }
   
  public string? Gender { get; set; }
  public string? CurrentUsername { get; set; }

  public int MinAge { get; set; } = 18;
  public int MaxAge { get; set; } = 100;
  public string OrderBy { get; set; } = "lastActive";
}

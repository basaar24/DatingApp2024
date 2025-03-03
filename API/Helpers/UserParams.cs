namespace API.Helpers;

public class UserParams
{
    private const int MAX_PAGE_SIZE = 50;

    private int pageNumber = 1;
    public int PageNumber
    {
        get => pageNumber;
        set => pageNumber = (value is < 1) ? 1 : value;
    }

    private int pageSize = 12;
    public int PageSize
    {
        get => pageSize;
        set => pageSize = (value is < 1 or > MAX_PAGE_SIZE) ? MAX_PAGE_SIZE : value;
    }

    public string? CurrentUsername { get; set; }
    public string? Gender { get; set; }
    public int MinAge { get; set; } = 18;
    public int MaxAge { get; set; } = 100;
    public string OrderBy { get; set; } = "lastActive";
}
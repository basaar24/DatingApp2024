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
}
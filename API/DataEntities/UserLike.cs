namespace API.DataEntities;

public class UserLike
{
    // EF Navigation properties
    public AppUser SourceUser { get; set; } = null!;
    public int SourceUserId { get; set; }
    public AppUser TargetUser { get; set; } = null!;
    public int TargetUserId { get; set; }

}
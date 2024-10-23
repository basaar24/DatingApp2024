namespace API.Data;
using API.DataEntities;
using Microsoft.EntityFrameworkCore;

public class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<AppUser> Users { get; set; }
}

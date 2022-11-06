using Microsoft.EntityFrameworkCore;
using TomKasCoursesAPI.Models;

namespace TomKasCoursesAPI.Data;

public class TomKasCoursesAPIContext : DbContext
{
    public TomKasCoursesAPIContext (DbContextOptions<TomKasCoursesAPIContext> options)
        : base(options)
    {
    }

    public DbSet<Course> Course { get; set; } = default!;
}

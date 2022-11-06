using Microsoft.EntityFrameworkCore;
using TomKasStudentsAPI.Models;

namespace TomKasStudentsAPI.Data;

public class TomKasStudentsAPIContext : DbContext
{
    public TomKasStudentsAPIContext (DbContextOptions<TomKasStudentsAPIContext> options)
        : base(options)
    {
    }

    public DbSet<Student> Student { get; set; } = default!;
    public DbSet<Enrollment> Enrollment { get; set; } = default!;
}

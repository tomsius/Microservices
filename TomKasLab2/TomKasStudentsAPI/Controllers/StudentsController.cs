using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TomKasStudentsAPI.Data;
using TomKasStudentsAPI.Models;

namespace TomKasStudentsAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentsController : ControllerBase
{
    private readonly TomKasStudentsAPIContext _context;
    private readonly ILogger<StudentsController> _logger;

    public StudentsController(TomKasStudentsAPIContext context, ILogger<StudentsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: api/Students
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Student>>> GetStudent()
    {
        DateTimeOffset start = DateTimeOffset.UtcNow;
        var data = await _context.Student.ToListAsync();
        DateTimeOffset end = DateTimeOffset.UtcNow;

        _logger.LogInformation("Processed {Route} {Method} method in {Elapsed} ms.", "api/Students", "GET", end - start);

        return data;
    }

    // GET: api/Students/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Student>> GetStudent(int id)
    {
        DateTimeOffset start = DateTimeOffset.UtcNow;
        var student = await _context.Student.Include(s => s.Enrollments).AsNoTracking().FirstOrDefaultAsync(s => s.ID == id);
        DateTimeOffset end = DateTimeOffset.UtcNow;

        if (student == null)
        {
            _logger.LogWarning("Processed {Route}{Id} {Method} method in {Elapsed}ms but no student was found.", "api/Students/", id, "GET", end - start);
            return NotFound();
        }

        _logger.LogInformation("Processed {Route}{Id} {Method} method in {Elapsed}ms. Returned {@Student}.", "api/Students/", id, "GET", end - start, student);
        return student;
    }

    // PUT: api/Students/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutStudent(int id, Student student)
    {
        if (id != student.ID)
        {
            return BadRequest();
        }

        _context.Entry(student).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!StudentExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Students
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Student>> PostStudent(Student student)
    {
        _context.Student.Add(student);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetStudent", new { id = student.ID }, student);
    }

    // DELETE: api/Students/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        var student = await _context.Student.FindAsync(id);
        if (student == null)
        {
            return NotFound();
        }

        _context.Student.Remove(student);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool StudentExists(int id)
    {
        return _context.Student.Any(e => e.ID == id);
    }
}

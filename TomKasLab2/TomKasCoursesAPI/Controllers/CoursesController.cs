using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TomKasCoursesAPI.Data;
using TomKasCoursesAPI.Models;

namespace TomKasCoursesAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoursesController : ControllerBase
{
    private readonly TomKasCoursesAPIContext _context;
    private readonly ILogger<CoursesController> _logger;

    public CoursesController(TomKasCoursesAPIContext context, ILogger<CoursesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: api/Courses
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Course>>> GetCourse()
    {
        DateTimeOffset start = DateTimeOffset.UtcNow;
        var data = await _context.Course.ToListAsync();
        DateTimeOffset end = DateTimeOffset.UtcNow;

        _logger.LogInformation("Processed {Route} {Method} method in {Elapsed} ms.", "api/Courses", "GET", end - start);

        return data;
    }

    // GET: api/Courses/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Course>> GetCourse(int id)
    {
        DateTimeOffset start = DateTimeOffset.UtcNow;
        var course = await _context.Course.FindAsync(id);
        DateTimeOffset end = DateTimeOffset.UtcNow;
        

        if (course == null)
        {
            _logger.LogWarning("Processed {Route}{Id} {Method} method in {Elapsed}ms but no student was found.", "api/Courses/", id, "GET", end - start);
            return NotFound();
        }

        _logger.LogInformation("Processed {Route}{Id} {Method} method in {Elapsed}ms. Returned {@Student}.", "api/Courses/", id, "GET", end - start, course);
        return course;
    }

    // PUT: api/Courses/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCourse(int id, Course course)
    {
        if (id != course.CourseID)
        {
            return BadRequest();
        }

        _context.Entry(course).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CourseExists(id))
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

    // POST: api/Courses
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Course>> PostCourse(Course course)
    {
        _context.Course.Add(course);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            if (CourseExists(course.CourseID))
            {
                return Conflict();
            }
            else
            {
                throw;
            }
        }

        return CreatedAtAction("GetCourse", new { id = course.CourseID }, course);
    }

    // DELETE: api/Courses/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        var course = await _context.Course.FindAsync(id);
        if (course == null)
        {
            return NotFound();
        }

        _context.Course.Remove(course);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CourseExists(int id)
    {
        return _context.Course.Any(e => e.CourseID == id);
    }
}

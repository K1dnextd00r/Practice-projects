using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GradeTracker.Data;
using GradeTracker.Models;

namespace GradeTracker.Controller;

[ApiController]
[Route("api/[controller]")]
public class GradesController : ControllerBase
{
    private readonly AppDbContext _context;

    public GradesController(AppDbContext context)
    {
        _context = context; 
    }

    // GET /api/grades/student/:studentId
    [HttpGet("student/{studentId}")]
    public async Task<IActionResult> GetByStudent(int studentId)
    {
        var grades = await _context.Grades
            .Where(x => x.StudentId == studentId)
            .Include(x => x.Subject)
            .ToListAsync();

        return Ok(grades);
    }

    // POST /api/grades
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Grade grade)
    {
        //Verify that the student exists
        var student = await _context.Students.FindAsync(grade.StudentId);
        if (student == null)
            return NotFound(new { message = "Student not found." });

        // Verify the subject exists
        var subject = await _context.Subjects.FindAsync(grade.SubjectId);
        if (subject == null)
            return NotFound(new { message = "Subject not found." });

        grade.Date = DateTime.UtcNow;
        _context.Grades.Add(grade);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetByStudent), new { studentId = grade.StudentId }, grade);
    }

    // DELETE /api/grades/:id
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var grade = await _context.Grades.FindAsync(id);

        if (grade == null)
            return NotFound(new { message = $"Grade with ID {id} not found." });

        _context.Grades.Remove(grade);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
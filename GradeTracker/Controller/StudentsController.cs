using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GradeTracker.Data;
using GradeTracker.Models;

namespace GradeTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly AppDbContext _context;

    public StudentController(AppDbContext context)
    {
        _context = context; 
    }

    // Get /api/students
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var students = await _context.Students.ToListAsync();
        return Ok(students);
    }

    // Get /api/students/:id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var students = await _context.Students.FindAsync(id);

        if (students == null)
            return NotFound(new { message = $"Student with ID {id} not found" });

        return Ok(students);
    }

    // POST /api/students
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Student student)
    {
        _context.Students.Add(student);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new {id = student.Id}, student);
    }

    // DELETE /api/students/:id
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var student = await _context.Students.FindAsync(id);

        if (student == null)
            return NotFound(new { message = $"Student with ID {id} not found." });

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
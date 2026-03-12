using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GradeTracker.Data;
using GradeTracker.Models;

namespace GradeTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubjectsController : ControllerBase
{
    private readonly AppDbContext _context;

    public SubjectsController(AppDbContext context)
    {
        _context = context;
    }

    // GET /api/subjects
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var subjects = await _context.Subjects.ToListAsync();
        return Ok(subjects);
    }

    // POST /api/subjects
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Subject subject)
    {
        _context.Subjects.Add(subject);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAll), new { id = subject.Id }, subject);
    }
}
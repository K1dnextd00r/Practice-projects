using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using HabitTracker.Data;
using HabitTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace HabitTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogEntriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LogEntriesController(AppDbContext context)
        {
            _context = context;
        }

        //GET /api/habits/{habitId}/logs
        [HttpGet("/api/habits/{habitId}/logs")]
        public async Task<IActionResult> GetAllLogs(int id)
        {
            var habit = await _context.LogEntries.FindAsync(id);
            if (habit == null) 
                return NotFound(new {message = $"Habit with Id {id} not found"});

            var logs = await _context.LogEntries
                .Where(l => l.Id == id)
                .OrderByDescending(l => l.Completed)
                .ToListAsync();

            return Ok(logs);
        }

        //Post - /api/habits/{habitId}/logs
        [HttpPost("/api/habits/{habitId}/logs")]
        public async Task<IActionResult> LogCompletion(int id)
        {
            var habit = await _context.LogEntries.FindAsync(id);
            if (habit == null)
                return NotFound(new { message = $"Habit with id {id} not found" });

            var today = DateTime.Now;
            var alreadyLog = await _context.LogEntries.AnyAsync(l => l.HabitId == id && l.Completed == today);

            if (alreadyLog)
                return BadRequest(new { message = " This habit has already been logged today." });

            var logEntry = new LogEntry
            {
                HabitId = id,
                Completed = today,
            };

            _context.LogEntries.Add(logEntry);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAllLogs), new { id }, logEntry);
        }
    }
}

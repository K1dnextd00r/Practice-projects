using Elfie.Serialization;
using HabitTracker.Data;
using HabitTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/*
Before writing any endpoint, answer these:

[ ] What HTTP method? (GET / POST / PUT / DELETE)
[ ] What is the route ? (/ api / resource or / api / resource /{ id})
[ ] Where is my data coming from? (URL / query string / body)
[ ] What can go wrong? (404 / 400 / 401)
[ ] What do I return on success ? (200 / 201 / 204)
*/

namespace HabitTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HabitsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HabitsController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/habit
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var habits = await _context.Habits
                .Where(h => !h.isArchived)
                .ToListAsync();

            return Ok(habits);
        }

        // GET: api/habit/{Id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var habit = await _context.Habits.FindAsync(id);
            if (habit == null)
            {
                return NotFound(new { message = $"Habit with Id {id} not found." });
            }

            return Ok(habit);
        }

        // POST /api/habit
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Habit habit)
        {
            if(string.IsNullOrEmpty(habit.Name))
                return BadRequest(new {message = "Habit name is required"});

            habit.CreatedAt = DateTime.Now;
            habit.isArchived = false;
            _context.Habits.Add(habit);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = habit.Id }, habit);
        }

        // PUT /api/habits/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Habit updated)
        {
            var habit = await _context.Habits.FindAsync();

            if (habit == null)
                return NotFound(new { message = $"Habit with ID {id} not found" });

            habit.Name = updated.Name;
            habit.Description = updated.Description;
            habit.Frequency = updated.Frequency;
            await _context.SaveChangesAsync();

            return NoContent();

        }

        // DELETE: /api/habit/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var habit = await _context.Habits.FindAsync(id);
            if (habit == null)
                return NotFound(new { message = $"Habit with ID {id} not found" });

            habit.isArchived = true;
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}

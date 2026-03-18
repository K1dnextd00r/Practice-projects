using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HabitTracker.Data;
using HabitTracker.Models;

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

        // GET: Habits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var habit = await _context.Habits.FindAsync(id);
            if (habit == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", habit.UserId);
            return View(habit);
        }

        // POST: Habits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Name,Description,Frequency,CreatedAt,isArchived")] Habit habit)
        {
            if (id != habit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(habit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HabitExists(habit.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", habit.UserId);
            return View(habit);
        }

        // GET: Habits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var habit = await _context.Habits
                .Include(h => h.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (habit == null)
            {
                return NotFound();
            }

            return View(habit);
        }

        // POST: Habits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var habit = await _context.Habits.FindAsync(id);
            if (habit != null)
            {
                _context.Habits.Remove(habit);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HabitExists(int id)
        {
            return _context.Habits.Any(e => e.Id == id);
        }
    }
}

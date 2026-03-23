using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using HabitTracker.Data;
using HabitTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

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
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        //Post /api/auth/register
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (string.IsNullOrEmpty(user.Username) ||
                string.IsNullOrEmpty(user.Email) ||
                string.IsNullOrEmpty(user.PasswordHash))
                return BadRequest(new { message = "Username, email and Password are all required to be entered" });

            var emailTaken = await _context.Users.AnyAsync(u => u.Email == user.Email);
            if (emailTaken)
                return BadRequest(new { message = "An account with this email already exists" });

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            user.CreatedAt = DateTime.Now;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            user.PasswordHash = null;
            return CreatedAtAction(nameof(Login), new { id = user.Id }, user);
        }
    }
}

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
            if (string.IsNullOrEmpty(user.Username))
                return BadRequest(new { message = "User must have a name and email must be provided" });

            user.CreatedAt = DateTime.Now;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Login), new { Id = user.Id }, user);
        }
    }
}

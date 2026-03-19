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
    }
}

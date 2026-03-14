namespace HabitTracker.Models
{
    public class LogEntry
    {
        public int Id { get; set; }
        public int HabitId { get; set; }
        public DateTime Completed { get; set; }
        public string Notes { get; set; } = string.Empty;
        public Habit Habit { get; set; } = null!;
    }
}

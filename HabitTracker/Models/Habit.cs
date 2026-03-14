namespace HabitTracker.Models
{
    public class Habit
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty; // e.g., "Daily", "Weekly"
        public DateTime CreatedAt { get; set; }
        public bool isArchived { get; set; }

        public User User { get; set; } = null!;
    }
}

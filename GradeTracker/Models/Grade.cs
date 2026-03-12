namespace GradeTracker.Models;

public class Grade
{
    public int Id { get; set; }
    public int Score { get; set; }
    public DateTime Date { get; set; }

    //Foreign Keys
    public int StudentId { get; set; }
    public int SubjectId { get; set; }

    public Student Student { get; set; } = null!; 
    public Subject Subject { get; set; } = null!;
}
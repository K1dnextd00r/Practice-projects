namespace GradeTracker.Models;

public class Subject
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<Grade> Grades { get; set; } = new();
}
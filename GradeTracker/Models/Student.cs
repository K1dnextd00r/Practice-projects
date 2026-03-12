namespace GradeTracker.Models;

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    //Navigation property - Ef core uses this to know that a student has many grades
    public List<Grade> Grades { get; set; } = new();
}
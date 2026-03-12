import { useState, useEffect } from "react";

export default function GradeList({ student }) {
  const [grades, setGrades] = useState([]);
  const [subjects, setSubjects] = useState([]);
  const [subjectId, setSubjectId] = useState("");
  const [score, setScore] = useState("");
  const [loading, setLoading] = useState(false);
  const [refresh, setRefresh] = useState(false);

  // Fetch grades for this student
  useEffect(() => {
    fetch(`http://localhost:5181/api/Grades/student/${student.id}`)
      .then(res => res.json())
      .then(data => setGrades(data));
  }, [student, refresh]);

  // Fetch all subjects for the dropdown
  useEffect(() => {
    fetch("http://localhost:5181/api/Subjects")
      .then(res => res.json())
      .then(data => {
        setSubjects(data);
        if (data.length > 0) setSubjectId(data[0].id); // default to first subject
      });
  }, []);

  const handleAddGrade = async () => {
    if (!subjectId || !score) return alert("Please select a subject and enter a score.");
    if (score < 0 || score > 100) return alert("Score must be between 0 and 100.");

    setLoading(true);

    await fetch("http://localhost:5181/api/Grades", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        studentId: student.id,
        subjectId: parseInt(subjectId),
        score: parseInt(score),
      }),
    });

    setScore("");
    setLoading(false);
    setRefresh(!refresh); // trigger grade list to reload
  };

    return (
        <div style={{ padding: 16, border: "1px solid #ddd", borderRadius: 8 }}>
            <h2>Grades for {student.name}</h2>

            {/* Grade Table */}
            {grades.length === 0 ? (
            <p>No grades recorded yet.</p>
            ) : (
            <table style={{ width: "100%", borderCollapse: "collapse", marginBottom: 24 }}>
                <thead>
                    <tr style={{ borderBottom: "2px solid #ddd" }}>
                        <th style={{ textAlign: "left", padding: 8 }}>Subject</th>
                        <th style={{ textAlign: "left", padding: 8 }}>Score</th>
                        <th style={{ textAlign: "left", padding: 8 }}>Date</th>
                    </tr>
                </thead>
                <tbody>
                    {grades.map(grade => (
                    <tr key={grade.id} style={{ borderBottom: "1px solid #eee" }}>
                        <td style={{ padding: 8 }}>{grade.subject?.name ?? "—"}</td>
                        <td style={{ padding: 8 }}>{grade.score}%</td>
                        <td style={{ padding: 8 }}>{new Date(grade.date).toLocaleDateString()}</td>
                    </tr>
                    ))}
                </tbody>
            </table>
        )}

        {/* Add Grade Form */}
        <div style={{ borderTop: "1px solid #eee", paddingTop: 16 }}>
            <h3>Add Grade</h3>
            {subjects.length === 0 ? (
            <p style={{ color: "#999" }}>No subjects found. Add subjects to the database first.</p>
            ) : (
            <div style={{ display: "flex", gap: 8, alignItems: "center", flexWrap: "wrap" }}>
                <select
                value={subjectId}
                onChange={e => setSubjectId(e.target.value)}
                style={{ padding: 8 }}
                >
                {subjects.map(subject => (
                    <option key={subject.id} value={subject.id}>
                    {subject.name}
                    </option>
                ))}
                </select>
                <input
                    type="number"
                    placeholder="Score (0-100)"
                    value={score}
                    onChange={e => setScore(e.target.value)}
                    min={0}
                    max={100}
                    style={{ padding: 8, width: 140 }}
                />
                <button onClick={handleAddGrade} disabled={loading}>
                    {loading ? "Adding..." : "Add Grade"}
                </button>
            </div>
            )}
        </div>
    </div>
    );
}
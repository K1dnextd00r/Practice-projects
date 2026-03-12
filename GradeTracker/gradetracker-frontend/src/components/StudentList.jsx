import { useState, useEffect } from "react";

export default function StudentList({ refresh, onSelectStudent, selectedStudent }) {
    const [students, setStudents] = useState([]);

    useEffect(() => {
    fetch("http://localhost:5181/api/Student")
        .then(res => res.json())
        .then(data => setStudents(data));
    }, [refresh]);

    if (students.length === 0) return <p>No students yet. Add one above!</p>;

    return (
    <div style={{ marginBottom: 32 }}>
        <h2>Students</h2>
        {students.map(student => (
        <div
            key={student.id}
            onClick={() => onSelectStudent(student)}
            style={{
            padding: 12,
            marginBottom: 8,
            border: "1px solid #ddd",
            borderRadius: 8,
            cursor: "pointer",
            background: selectedStudent?.id === student.id ? "#e8f4ff" : "white",
            }}
        >
            <strong>{student.name}</strong> — {student.email}
        </div>
        ))}
    </div>
    );
}
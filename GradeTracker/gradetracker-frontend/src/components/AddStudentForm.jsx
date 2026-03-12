import { useState } from "react";

export default function AddStudentForm({ onStudentAdded }) {
    const [name, setName] = useState("");
    const [email, setEmail] = useState("");
    const [loading, setLoading] = useState(false);

    const handleSubmit = async () => {
        if (!name || !email) return alert("Please fill in both fields.");
        setLoading(true);

        await fetch("http://localhost:5181/api/Student", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ name, email }),
        });

        setName("");
        setEmail("");
        setLoading(false);
        onStudentAdded(); // tell App to refresh the student list
    };

    return (
        <div style={{ marginBottom: 32, padding: 16, border: "1px solid #ddd", borderRadius: 8 }}>
            <h2>Add Student</h2>
            <input
                placeholder="Name"
                value={name}
                onChange={e => setName(e.target.value)}
                style={{ marginRight: 8, padding: 8 }}
            />
            <input
                placeholder="Email"
                value={email}
                onChange={e => setEmail(e.target.value)}
                style={{ marginRight: 8, padding: 8 }}
            />
            <button onClick={handleSubmit} disabled={loading}>
                {loading ? "Adding..." : "Add Student"}
            </button>
        </div>
    );
}
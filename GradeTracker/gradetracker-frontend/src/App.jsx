import { useState } from "react";
import StudentList from "./components/StudentList";
import AddStudentForm from "./components/AddStudentForm";
import GradeList from "./components/GradeList";

export default function App() {
  const [selectedStudent, setSelectedStudent] = useState(null);
  const [refresh, setRefresh] = useState(false);

  const handleStudentAdded = () => setRefresh(!refresh);

  return (
    <div style={{ maxWidth: 800, margin: "0 auto", padding: "32px 16px", fontFamily: "sans-serif" }}>
      <h1>🎓 GradeTracker</h1>

      <AddStudentForm onStudentAdded={handleStudentAdded} />

      <StudentList
        refresh={refresh}
        onSelectStudent={setSelectedStudent}
        selectedStudent={selectedStudent}
      />

      {selectedStudent && (
        <GradeList student={selectedStudent} />
      )}
    </div>
  );
}
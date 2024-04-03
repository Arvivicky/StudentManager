using Entity;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class StudentsRepo : IStudentsRepo
    {
        private readonly ContextDb _contextDb;

        public StudentsRepo(ContextDb contextDb)
        {
            _contextDb = contextDb;
        }

        public async Task<List<Student>> GetAllStudents()
        {
            List<Student> students = await _contextDb.Student.ToListAsync();
            return students;
        }
        public async Task<Student> GetById(int Id)
        {
            Student student = await _contextDb.Student.FindAsync(Id);
            if (student == null)
            {
                throw new Exception("Student not found");
            }
            return student;
        }

        public async Task<Student> InsertStudent(Student student)
        {
            var entry = await _contextDb.Student.AddAsync(student);
            await _contextDb.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<Student> UpdateStudent(Student student, int Id)
        {
            Student studentEdit = await _contextDb.Student.FindAsync(Id);
            if (studentEdit == null)
            {
                throw new Exception("Student not found");
            }
            if (student.Name != null)
                studentEdit.Name = student.Name;
            if (student.RollNumber != null)
                studentEdit.RollNumber = student.RollNumber;
            if (student.Age != 0)
                studentEdit.Age = student.Age;
            if (student.Cgpa != 0)
                studentEdit.Cgpa = student.Cgpa;
            await _contextDb.SaveChangesAsync();
            return studentEdit;
        }
        public async Task DeleteStudent(int Id)
        {
            Student student = await _contextDb.Student.FindAsync(Id);
            if (student == null)
            {
                throw new Exception("Student not found");
            }
            _contextDb.Remove(student);
            await _contextDb.SaveChangesAsync();
        }
    }
}

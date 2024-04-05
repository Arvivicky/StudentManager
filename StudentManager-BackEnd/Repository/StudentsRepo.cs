using Entity;
using Microsoft.EntityFrameworkCore;
using StudentManager_BackEnd.Entity;
namespace Repository
{
    public class StudentsRepo : IStudentsRepo
    {
        private readonly ContextDb contextDb;

        public StudentsRepo(ContextDb contextDb)
        {
            this.contextDb = contextDb;
        }

        public async Task<List<Student>> GetAllStudents()
        {
            List<Student> students = await contextDb.Student.ToListAsync();
            return students;
        }
        public async Task<Student> GetById(int Id)
        {
            Student student = await contextDb.Student.FindAsync(Id);
            if (student == null)
            {
                throw new Exception("Student not found");
            }
            return student;
        }

        public async Task<Student> InsertStudent(Student student)
        {
            var entry = await contextDb.Student.AddAsync(student);
            await contextDb.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<Student> UpdateStudent(Student student, int Id)
        {
            Student studentEdit = await contextDb.Student.FindAsync(Id);
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
            await contextDb.SaveChangesAsync();
            return studentEdit;
        }
        public async Task DeleteStudent(int Id)
        {
            Student student = await contextDb.Student.FindAsync(Id);
            if (student == null)
            {
                throw new Exception("Student not found");
            }
            contextDb.Remove(student);
            await contextDb.SaveChangesAsync();
        }
    }
}

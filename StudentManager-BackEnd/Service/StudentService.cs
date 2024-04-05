using Entity;
using Microsoft.EntityFrameworkCore;
using Repository;
using StudentManager_BackEnd.Entity;
namespace Service
{
    public class StudentService : IStudentService
    {
        private readonly IStudentsRepo studentsRepo;

        public StudentService(IStudentsRepo studentsRepo)
        {
            this.studentsRepo = studentsRepo;
        }

        public async Task<List<Student>> GetAllStudents()
        {
            try
            {
                List<Student> students = await studentsRepo.GetAllStudents();
                return students;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public async Task<Student> GetById(int Id)
        {
            try
            {
                Student student = await studentsRepo.GetById(Id);
                return student;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<Student> InsertStudent(Student student)
        {
            try
            {
                Student studentSave = await studentsRepo.InsertStudent(student);
                return studentSave;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<Student> UpdateStudent(Student student, int Id)
        {
            try
            {
                Student studentUpdate = await studentsRepo.UpdateStudent(student, Id);
                return studentUpdate;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task DeleteStudent(int Id)
        {
            await studentsRepo.DeleteStudent(Id);
        }
    }
}

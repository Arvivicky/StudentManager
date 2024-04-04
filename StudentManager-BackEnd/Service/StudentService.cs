using Entity;
using Microsoft.EntityFrameworkCore;
using Repository;
using StudentManager_BackEnd.Entity;
namespace Service
{
    public class StudentService : IStudentService
    {
        private readonly IStudentsRepo _studentsRepo;

        public StudentService(IStudentsRepo studentsRepo)
        {
            try
            {
                _studentsRepo = studentsRepo;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<Student>> GetAllStudents()
        {
            try
            {
                List<Student> students = await _studentsRepo.GetAllStudents();
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
                Student student = await _studentsRepo.GetById(Id);
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
                Student studentSave = await _studentsRepo.InsertStudent(student);
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
                Student studentUpdate = await _studentsRepo.UpdateStudent(student, Id);
                return studentUpdate;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task DeleteStudent(int Id)
        {
            await _studentsRepo.DeleteStudent(Id);
        }
    }
}

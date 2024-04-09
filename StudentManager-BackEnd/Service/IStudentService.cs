using Entity;
using StudentManager_BackEnd.Entity;
namespace Service
{
    public interface IStudentService
    {
        Task DeleteStudent(int id);
        Task<List<Student>> GetAllStudents();
        Task<Student> GetByName(string name);
        Task<Student> InsertStudent(Student student);
        Task<Student> UpdateStudent(Student student, int Id);
    }
}

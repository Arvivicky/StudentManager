using Entity;

namespace Repository
{
    public interface IStudentsRepo
    {
        Task DeleteStudent(int id);
        Task<List<Student>> GetAllStudents();
        Task<Student> GetById(int Id);
        Task<Student> InsertStudent(Student student);
        Task<Student> UpdateStudent(Student student, int Id);
    }
}

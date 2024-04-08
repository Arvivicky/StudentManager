using Entity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using StudentManager_BackEnd.Entity;

namespace Controllers
{
    [Authorize(Roles ="Admin")]
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService studentService;


        public StudentController(IStudentService studentService)
        {
            this.studentService = studentService;
        }

        [HttpGet]
        public string Welcome()
        {
            return "Hello!";
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<List<Student>>> GetAll()
        {
            List<Student> students = await studentService.GetAllStudents();
            return Ok(students);
        }

        [HttpGet("getById/{int Id}")]
        public async Task<ActionResult<List<Student>>> GetById(int Id)
        {
            Student student = await studentService.GetById(Id);
            return Ok(student);
        }

        [HttpPost("/addStudent")]
        public async Task<ActionResult<Student>> Insert(Student student)
        {
            Student students = await studentService.InsertStudent(student);
            return Ok(students);
        }

        [HttpPut("/updateStudent/{int Id}")]
        public async Task<ActionResult<Student>> Update(Student student, int Id)
        {
            Student students = await studentService.UpdateStudent(student, Id);
            return Ok(students);
        }

        [HttpDelete("/deleteStudent/{int Id}")]
        public async Task<ActionResult<string>> Delete(int Id)
        {
            await studentService.DeleteStudent(Id);
            return Ok("Deleted Successfully");
        }


    }
}

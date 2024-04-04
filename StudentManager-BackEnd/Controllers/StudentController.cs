using Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using StudentManager_BackEnd.Entity;

namespace Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        private readonly IStudentService _studentService;


        public StudentController(ILogger<StudentController> logger, IStudentService studentService)
        {
            _logger = logger;
            _studentService = studentService;
        }

        [HttpGet]
        public string Welcome()
        {
            return "Hello!";
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<List<Student>>> GetAll()
        {
            List<Student> students = await _studentService.GetAllStudents();
            return Ok(students);
        }

        //READ
        [HttpGet("getById/{int Id}")]
        public async Task<ActionResult<List<Student>>> GetById(int Id)
        {
            Student student = await _studentService.GetById(Id);
            return Ok(student);
        }

        //CREATE
        [HttpPost("/addStudent")]
        public async Task<ActionResult<Student>> Insert(Student student)
        {
            Student students = await _studentService.InsertStudent(student);
            return Ok(students);
        }

        //UPDATE
        [HttpPut("/updateStudent/{int Id}")]
        public async Task<ActionResult<Student>> Update(Student student, int Id)
        {
            Student students = await _studentService.UpdateStudent(student, Id);
            return Ok(students);
        }

        //DELETE
        [HttpDelete("/deleteStudent/{int Id}")]
        public async Task<ActionResult<string>> Delete(int Id)
        {
            await _studentService.DeleteStudent(Id);
            return Ok("Deleted Successfully");
        }


    }
}

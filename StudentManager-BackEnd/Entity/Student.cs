using System.ComponentModel.DataAnnotations;

namespace StudentManager_BackEnd.Entity
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string RollNumber { get; set; }
        public int Age { get; set; }
        public float Cgpa { get; set; }
        public Student(int id, string name, string rollNumber, int age, float cgpa)
        {
            Id = id;
            Name = name;
            RollNumber = rollNumber;
            Age = age;
            Cgpa = cgpa;
        }
        public Student()
        {
        }
    }
}

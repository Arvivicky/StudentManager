using System.ComponentModel.DataAnnotations;

namespace StudentManager_BackEnd.Entity
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        public string RollNumber { get; set; }
        public string Name { get; set; }
        public string dept { get; set; }
        public int Age { get; set; }
        public ICollection<SemesterDetails> SemesterDetails { get; set; } = new List<SemesterDetails>();

    }
    
}

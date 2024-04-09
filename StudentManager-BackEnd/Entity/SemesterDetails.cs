using System.ComponentModel.DataAnnotations;

namespace StudentManager_BackEnd.Entity
{
    public class SemesterDetails
    {
        [Key]
        public int Id { get; set; }
        public int SemesterNumber { get; set; }
        public double GPA { get; set; }
        public double CGPA { get; set; }

        // Foreign key to Student
        public int StudentId { get; set; }
        public Student Student { get; set; }
    }
}

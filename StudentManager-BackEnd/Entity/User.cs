using System.ComponentModel.DataAnnotations;

namespace StudentManager_BackEnd.Entity
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string? RefreshToken { get; set; }
    }
}

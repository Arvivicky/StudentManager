using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace StudentManager_BackEnd.Entity
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
        public ICollection<Role> Roles { get; set; }=new List<Role>(); //initialize to avoid nullExp :)
    }
}

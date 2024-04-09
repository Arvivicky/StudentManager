using System.Data;
using System.Text.Json.Serialization;

namespace StudentManager_BackEnd.Entity
{
    
    public class UserRole
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}

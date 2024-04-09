using System.ComponentModel.DataAnnotations;

namespace StudentManager_BackEnd.Entity
{
    public class RoleMenu
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Method { get; set; }
        [Required]
        public string Url { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}

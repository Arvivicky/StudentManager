using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StudentManager_BackEnd.Entity
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore] //to avoid infinite loooop (thanks to many-to-many)
        public ICollection<User> Users { get; set; } =new List<User>(); //initialize to avoid nullExp :)
    }
}

namespace Dto
{
    public class AddUserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } = null;
        public string? RefreshToken { get; set; }
        public DateTime TokenExpires { get; set; }
        public ICollection<RoleDto> Roles { get; set; } = new List<RoleDto>();
    }
    public class RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<RoleMenuDto> RoleMenus { get; set; } = new List<RoleMenuDto>();
    }

    public class RoleMenuDto
    {
        public int Id { get; set; }
        public string Method { get; set; }
        public string Url { get; set; }
    }
}

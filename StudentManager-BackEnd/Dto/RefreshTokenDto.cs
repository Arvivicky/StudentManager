namespace Dto
{
    public class RefreshTokenDto
    {
        public required string? Token { get; set; }
        public DateTime TokenCreated { get; set; }= DateTime.Now;
        public DateTime Expires {  get; set; }
    }
}

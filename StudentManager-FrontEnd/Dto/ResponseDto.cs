namespace StudentManager_FrontEnd.Dto
{
    public class ResponseDto
    {
        public string Token { get; set; }
        public RefreshTokenDto RefreshToken { get; set; }
    }

    public class RefreshTokenDto
    {
        public string Token { get; set; }
        public DateTime TokenCreated { get; set; }
        public DateTime Expires { get; set; }
    }

}

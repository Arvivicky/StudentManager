namespace StudentManager_FrontEnd.Service
{
    public interface ISetCookies
    {
        public Task SetCookies(IEnumerable<string> cookies, IHttpContextAccessor httpContextAccessor);
    }
}

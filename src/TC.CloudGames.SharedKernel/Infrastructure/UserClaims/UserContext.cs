namespace TC.CloudGames.SharedKernel.Infrastructure.UserClaims
{
    public sealed class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid Id =>
            _httpContextAccessor
                .HttpContext?
                .User
                .GetUserId() ??
            throw new InvalidOperationException("User context is unavailable");

        public string Name =>
            _httpContextAccessor
                .HttpContext?
                .User
                .GetUserName() ??
            throw new InvalidOperationException("User context is unavailable");

        public string Email =>
            _httpContextAccessor
                .HttpContext?
                .User
                .GetUserEmail() ??
            throw new InvalidOperationException("User context is unavailable");

        public string Username =>
            _httpContextAccessor
                .HttpContext?
                .User
                .GetUserUsername() ??
            throw new InvalidOperationException("User context is unavailable");

        public string Role =>
            _httpContextAccessor
                .HttpContext?
                .User
                .GetUserRole() ??
            throw new InvalidOperationException("User context is unavailable");
    }
}

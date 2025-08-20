namespace TC.CloudGames.SharedKernel.Infrastructure.UserClaims
{
    public sealed class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICorrelationIdGenerator _correlationId;

        public UserContext(IHttpContextAccessor httpContextAccessor, ICorrelationIdGenerator correlationId)
        {
            _httpContextAccessor = httpContextAccessor;
            _correlationId = correlationId;
        }

        public Guid Id =>
            IsAuthenticated
                ? _httpContextAccessor.HttpContext?.User.GetUserId() ?? Guid.Empty
                : Guid.Empty;

        public string Name =>
            IsAuthenticated
                ? _httpContextAccessor.HttpContext?.User.GetUserName() ?? "Anonymous"
                : "Anonymous";

        public string Email =>
            IsAuthenticated
                ? _httpContextAccessor.HttpContext?.User.GetUserEmail() ?? "anonymous@cloudgames.local"
                : "anonymous@cloudgames.local";

        public string Username =>
            IsAuthenticated
                ? _httpContextAccessor.HttpContext?.User.GetUserUsername() ?? "anonymous"
                : "anonymous";

        public string Role =>
            IsAuthenticated
                ? _httpContextAccessor.HttpContext?.User.GetUserRole() ?? "Guest"
                : "Guest";

        public string? CorrelationId =>
            _correlationId.CorrelationId;

        public Guid? TenantId =>
            Guid.NewGuid(); // Implementação futura para multi-tenant

        public bool IsAuthenticated =>
            _httpContextAccessor
                .HttpContext?
                .User
                .Identity?
                .IsAuthenticated ?? false;
    }
}

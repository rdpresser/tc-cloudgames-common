namespace TC.CloudGames.SharedKernel.Infrastructure.Authentication
{
    public sealed class TokenProvider(IOptions<JwtSettings> jwtSettings) : ITokenProvider
    {
        private readonly JwtSettings _jwtSettings = jwtSettings.Value;

        public string Create(UserTokenProvider user)
        {
            var token = JwtBearer.CreateToken(options =>
            {
                options.SigningKey = _jwtSettings.SecretKey;
                options.User.Claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
                options.User.Claims.Add(new Claim(JwtRegisteredClaimNames.Name, user.Name));
                options.User.Claims.Add(new Claim(JwtRegisteredClaimNames.UniqueName, user.Username));
                options.User.Claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
                options.User.Roles.Add(user.Role);
                options.User.Claims.AddRange(_jwtSettings.Audience.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
                options.ExpireAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes);
                options.Issuer = _jwtSettings.Issuer;
            });

            return token;
        }
    }
}

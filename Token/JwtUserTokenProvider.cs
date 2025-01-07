using System.IdentityModel.Tokens.Jwt;
using Catedra3.Model;

namespace Catedra3.Token;

using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public class JwtUserTokenProvider
{
     private readonly string _jwtSecret;
        private readonly string _validIssuer;
        private readonly string _validAudience;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        /// <summary>
        /// Initializes the JWT token provider with the required configuration.
        /// </summary>
        /// <param name="configuration">The application configuration containing the JWT keys.</param>
        /// <param name="roleRepository">The role repository to retrieve the user's role information.</param>
        public JwtUserTokenProvider(IConfiguration configuration)
        {
            _jwtSecret = configuration["JWT:Secret"];
            _validIssuer = configuration["JWT:ValidIssuer"];
            _validAudience = configuration["JWT:ValidAudience"];
        }

        /// <summary>
        /// Generates a JWT token for the specified user.
        /// The token includes the user's name, email, and role.
        /// </summary>
        /// <param name="user">The user for whom the token is generated.</param>
        public string Token(User user)
        {
            
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            DateTime? expiration = DateTime.UtcNow.AddHours(1);
            var credentials = new SigningCredentials(
                authSigningKey,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            return _jwtSecurityTokenHandler
                .WriteToken(token);
        }
}
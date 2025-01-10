using System.IdentityModel.Tokens.Jwt;
using Catedra3.Model;

namespace Catedra3.Token;

using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public class JwtUserTokenProvider : IUserTokenProvider
{
     private readonly string _jwtSecret;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler ;

        /// <summary>
        /// Initializes the JWT token provider with the required configuration.
        /// </summary>
        /// <param name="configuration">The application configuration containing the JWT keys.</param>
        public JwtUserTokenProvider(IConfiguration configuration)
        {
            _jwtSecret = configuration["JWT:Secret"];
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            Console.WriteLine("S " + _jwtSecret);
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
            
            Console.WriteLine("HOLII: " + _jwtSecret);

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            DateTime? expiration = DateTime.UtcNow.AddHours(1);
            var credentials = new SigningCredentials(
                authSigningKey,
                SecurityAlgorithms.HmacSha256
            );
            
            Console.WriteLine("AAAA");

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );
            
            Console.WriteLine("AAAA");

            var tokenS = _jwtSecurityTokenHandler
                .WriteToken(token);
            
            Console.WriteLine("U: " + tokenS);

            return tokenS;
        }
}
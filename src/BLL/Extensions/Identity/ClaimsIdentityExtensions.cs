using BLL.Authentication.Options;
using BLL.Extensions.Date;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BLL.Extensions.Identity
{
    public static class ClaimsIdentityExtensions
    {
        public static async Task<string> GenerateJwtToken(this ClaimsIdentity identity, JwtIssuerOptions jwtOptions = null)
        {
            if(jwtOptions == null)

                jwtOptions = new JwtIssuerOptions();
            // Create list of claims that need to be added in the JWT token
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, identity.Name),
                new Claim(JwtRegisteredClaimNames.Jti, await jwtOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, jwtOptions.IssuedAt.ToEpochTime().ToString(), ClaimValueTypes.Integer64),
            };

            // Create the JWT security token and encode it.
            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: claims,
                notBefore: jwtOptions.NotBefore,
                expires: jwtOptions.Expiration,
                signingCredentials: jwtOptions.SigningCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}

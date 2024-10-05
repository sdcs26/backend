using Microsoft.IdentityModel.Tokens;
using Sowing_O2.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Sowing_O2.Utilities
{
    public static class JwtUtility
    {
        public static LoginResponseDto GenTokenkey(UsuarioDto usuario, JwtSettingsDto jwtSettings)
        {
            var key = Encoding.UTF8.GetBytes(jwtSettings.IssuerSigningKey);
            DateTime expireTime;

            if (jwtSettings.FlagExpirationTimeHours)
            {
                expireTime = DateTime.UtcNow.AddHours(jwtSettings.ExpirationTimeHours);
            }
            else if (jwtSettings.FlagExpirationTimeMinutes)
            {
                expireTime = DateTime.UtcNow.AddMinutes(jwtSettings.ExpirationTimeMinutes);
            }
            else
            {
                throw new Exception("Tiempo de expiración no especificado.");
            }

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, usuario.Nombre),
            new Claim(ClaimTypes.Email, usuario.Correo)
        };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expireTime,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSettings.ValidIssuer,
                Audience = jwtSettings.ValidAudience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new LoginResponseDto
            {
                Nombre = usuario.Nombre,
                Correo = usuario.Correo,
                Token = tokenHandler.WriteToken(token),
                TiempoExpiracion = expireTime
            };
        }
    }
}

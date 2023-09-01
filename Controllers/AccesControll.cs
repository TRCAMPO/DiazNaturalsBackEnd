using BACK_END_DIAZNATURALS.DTO;
using BACK_END_DIAZNATURALS.Encrypt;
using BACK_END_DIAZNATURALS.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Net;

using Microsoft.AspNetCore.Http;

namespace BACK_END_DIAZNATURALS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccesControll : ControllerBase
    {
        private readonly DiazNaturalsContext _context;

        public AccesControll(DiazNaturalsContext context)
        {
            _context = context;
        }


        [HttpPost]
        [Route("Validar")]
        public IActionResult Validar([FromBody] InputCredentialDTO request)
        {
            if (_context.Administrators == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { token = "" });
            }
            User user = _context.Users.FirstOrDefault(i => i.UserName == name);

            if (user == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { token = "" });
            }
            var credential = _context.Credentials.FirstOrDefault(i => i.UserId == user.UserId);

            if (credential == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { token = "" });
            }

            var hash = HashEncryption.CheckHash(passwordUser, credential.UserPassword, credential.CredentialSalt);
            if (!hash)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { token = "" });
            }
            if (user.UserState.Equals(0))
            {
                return StatusCode(StatusCodes.S, new { token = "" });
            }

            var jwt = new Jw
            {
                Key = "LabDistriinscUni.@-l",
                Issuer = "http://www.DistriInscriptions.somee.com/",
                Audience = "http://www.DistriInscriptions.somee.com",
                Subject = "basewebInscriptions"
            };
            var claims = new[]
            {
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, jwt.Subject),
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("id", user.UserId.ToString()),
                new Claim("Correo", user.UserEmail),
                new Claim("Nombre", user.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes((jwt.Key)));
            var singIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                jwt.Issuer,
                jwt.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: singIn
                );



            if (request.email == _context.Administrators.Find(1).EmailAdministrator && request.password == _context.Administrators.Find(1).contrasena)
            {
                var KeyBytes = Encoding.ASCII.GetBytes(secretKey);
                var claims = new ClaimsIdentity();

                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, request.correo));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(KeyBytes), SecurityAlgorithms.HmacSha256Signature),
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

                string tokencreado = tokenHandler.WriteToken(tokenConfig);

                return StatusCode(StatusCodes.Status200OK, new { token = tokencreado });

            }
          
            else
            {
            
                return StatusCode(StatusCodes.Status401Unauthorized, new { token = "" });

            }
        }

        private 
    }
}

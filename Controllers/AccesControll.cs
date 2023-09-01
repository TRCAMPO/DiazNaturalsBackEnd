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
using BACK_END_DIAZNATURALS.Jwt;

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
            Administrator user = _context.Administrators.FirstOrDefault(i => i.EmailAdministrator == request.email);

            if (user == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { token = "" });
            }
            var credential = _context.Credentials.FirstOrDefault(i => i.IdAdministrator == user.IdAdministrator);

            if (credential == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { token = "" });
            }

            var hash = HashEncryption.CheckHash(request.password, credential.Password, credential.SaltCredential);
            if (!hash)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { token = "" });
            }
          
            var jwt = new JwtData
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
                new Claim("id", user.IdAdministrator.ToString()),
                new Claim("Correo", user.EmailAdministrator),
                new Claim("Nombre", user.NameAdministrator)
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
                return StatusCode(StatusCodes.Status200OK, new { token = token });
            }
          
      
    }
}

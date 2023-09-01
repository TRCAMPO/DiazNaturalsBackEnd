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
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System;
using Credential = BACK_END_DIAZNATURALS.Model.Credential;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BACK_END_DIAZNATURALS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccesControll : ControllerBase
    {
        private readonly DiazNaturalsContext _context;
        private readonly Random _random = new Random();
        private readonly IMemoryCache _cache;
        private const string CacheKey = "RandomCode";
        private String code;

        public AccesControll(DiazNaturalsContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
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
                Key = "TrabajoCampoDe.@-l",
                Issuer = "https://localhost:7200/",
                Audience = "https://localhost:7200/",
                Subject = "basewebDiazNaturals"
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
                return StatusCode(StatusCodes.Status200OK, new { token = new JwtSecurityTokenHandler().WriteToken(token) });
            }


        [HttpPost]
        [Route("SendEmail")]
        public async Task<IActionResult> SendEmail([FromBody] EmailDTO email)
        {
            if (_context.Administrators == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { token = "" });
            }
            GenerateRandomCode();
            try
            {
               
                EmailService emailService = new EmailService();
                await emailService.SendEmail(email.Email, "Recuperación de contraseña DiazNaturals", code);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("ValidarCode")]
        public IActionResult ValidarCode([FromBody] CodeValidator codeValidator)
        {
           if (codeValidator == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { token = "" });
            }
            
            bool emailExists = _context.Administrators.Any(a => a.EmailAdministrator == codeValidator.Email);
            string cachedCode;
            var ca = _cache.TryGetValue(CacheKey, out  cachedCode);
           
            if (emailExists && codeValidator.Code == cachedCode)
            {
                return Ok(new { exists = emailExists });
            }
            return StatusCode(StatusCodes.Status404NotFound, new { token = cachedCode });
        }

        [HttpPut("EditarContraseña")]
        // [Authorize]
        public async Task<IActionResult> PutAdminsitratorPassword(InputCredentialDTO newCredential)
        {
            Administrator administrator = _context.Administrators.FirstOrDefault(a => a.EmailAdministrator == newCredential.email);
            if (administrator == null)
            {
                return NotFound();
            }
            var credential = _context.Credentials.FirstOrDefault(i => i.IdAdministrator == administrator.IdAdministrator);
            HashedFormat hash = HashEncryption.Hash( newCredential.password);
            credential.Password = hash.Password;
            credential.SaltCredential = hash.HashAlgorithm;
             await _context.SaveChangesAsync();
            return Ok();
        }



            private  void GenerateRandomCode(int length = 8)
        {
             const string AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
           
            char[] result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = AllowedChars[_random.Next(0, AllowedChars.Length)];
            }
            string randomCode = new string(result);
            code = randomCode;
            _cache.Set(CacheKey, randomCode);

        }
    }
}

﻿using Azure.Core;
using BACK_END_DIAZNATURALS.DTO;
using BACK_END_DIAZNATURALS.Encrypt;
using BACK_END_DIAZNATURALS.Jwt;
using BACK_END_DIAZNATURALS.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
            Administrator administrator = _context.Administrators.FirstOrDefault(i => i.EmailAdministrator == request.email);
            Client client = _context.Clients.FirstOrDefault(i => i.EmailClient == request.email);

            if (administrator != null)
            {
                var credential = _context.Credentials.FirstOrDefault(i => i.IdCredential == administrator.IdCredential);

                if (credential == null)
                {
                    Log.Warning($"Intento de acceso con credenciales nulas para {request.email}");
                    return StatusCode(StatusCodes.Status404NotFound, new { token = "" });
                }

                var hash = HashEncryption.CheckHash(request.password, credential.PasswordCredential, credential.SaltCredential);
                if (!hash)
                {
                    Log.Warning($"Intento de acceso con credenciales invalidas para {request.email}");
                    return StatusCode(StatusCodes.Status401Unauthorized, new { token = "" });
                }

                // Crear el token JWT para el administrador
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
                    new Claim("id", administrator.IdAdministrator.ToString()),
                    new Claim("Correo", administrator.EmailAdministrator),
                    new Claim("Nombre", administrator.NameAdministrator)
                };

                var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes((jwt.Key)));
                var singIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    jwt.Issuer,
                    jwt.Audience,
                    claims,
                    expires: DateTime.Now.AddMinutes(60),
                    signingCredentials: singIn
                );
                Log.Information($"Acceso desde el usuario {request.email}");
                return StatusCode(StatusCodes.Status200OK, new { token = new JwtSecurityTokenHandler().WriteToken(token), typeUser = "admin" });
            }
            else if (client != null)
            {
                var credential = _context.Credentials.FirstOrDefault(i => i.IdCredential == client.IdCredential);

                if (credential == null)
                {
                    Log.Warning($"Intento de acceso con credenciales nulas para {request.email}");
                    return StatusCode(StatusCodes.Status404NotFound, new { token = "" });
                }

                var hash = HashEncryption.CheckHash(request.password, credential.PasswordCredential, credential.SaltCredential);
                if (!hash)
                {
                    Log.Warning($"Intento de acceso con credenciales invalidas para {request.email}");
                    return StatusCode(StatusCodes.Status401Unauthorized, new { token = "" });
                }

                // Crear el token JWT para el administrador
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
                    new Claim("id", client.IdClient.ToString()),
                    new Claim("Correo", client.EmailClient),
                    new Claim("Nombre", client.NameClient)
                };

                var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes((jwt.Key)));
                var singIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    jwt.Issuer,
                    jwt.Audience,
                    claims,
                    expires: DateTime.Now.AddMinutes(60),
                    signingCredentials: singIn
                );
                Log.Information($"Acceso desde el usuario {request.email}");
                return StatusCode(StatusCodes.Status200OK, new { token = new JwtSecurityTokenHandler().WriteToken(token), typeUser = "client" });
            }
            Log.Information($"Intento de acceso con el correo no registrado {request.email}");
            return StatusCode(StatusCodes.Status404NotFound, new { token = "" });
        }




        [HttpPost]
        [Route("SendEmail")]
        public async Task<IActionResult> SendEmail([FromBody] EmailDTO email)
        {
            if (_context.Administrators == null)
            {
                Log.Error($"Error en el acceso al servidor al intentar extraer informacion de administradores y usuarios, cod error 500, Internal Server error");
                return StatusCode(StatusCodes.Status404NotFound, new { token = "" });
            }
            try
            {
                var mailAddress = new System.Net.Mail.MailAddress(email.Email);
            }
            catch {
                Log.Warning($"Intento de acceso con formato de correo invalido {email}"); 
                return BadRequest("Email no valido"); }

            if (_context.Administrators.Any(i => i.EmailAdministrator == email.Email) || _context.Clients.Any(i => i.EmailClient == email.Email))
            {
                GenerateRandomCode();
                try
                {
                    EmailService emailService = new EmailService();
                    await emailService.SendEmail(email.Email, "Recuperación de contraseña DiazNaturals", code);
                    Log.Warning($"Solicitud de recuperacion de contraseña para el correo {email}");
                    return Ok();
                }
                catch { return BadRequest(); }
            }
            else {
                Log.Warning($"Solicitud de recuperacion de contraseña para el correo NO REGISTRADO {email}"); 
                return NotFound("Email no encontrado"); }
        }



        [HttpPost]
        [Route("ValidarCode")]
        public IActionResult ValidarCode([FromBody] CodeValidator codeValidator)
        {
            if (codeValidator == null)
            {
                Log.Error($"Error en el contenido de la peticion para validar el codigo de recuperacion, {codeValidator.Email}, "+ $"cod error {NotFound().StatusCode}");
                return StatusCode(StatusCodes.Status404NotFound, new { token = "" });
            }
            bool emailExists = _context.Administrators.Any(a => a.EmailAdministrator == codeValidator.Email);
            bool emailExistsClient = _context.Clients.Any(a => a.EmailClient == codeValidator.Email);
            string cachedCode;
            var ca = _cache.TryGetValue(CacheKey, out cachedCode);

            if ((emailExists && codeValidator.Code == cachedCode))
            {
                Log.Warning($"Validacion exitosa de codigo de recuperacion para el correo  {codeValidator.Email}");
                return Ok(new { exists = emailExists });
            } else if((emailExistsClient && codeValidator.Code == cachedCode))
            {
                Log.Warning($"Validacion exitosa de codigo de recuperacion para el correo  {codeValidator.Email}");
                return Ok(new { exists = emailExistsClient });
            }
            Log.Warning($"Validacion INCORRECTA de codigo de recuperacion para el correo  {codeValidator.Email}, cod {NotFound().StatusCode}");
            return StatusCode(StatusCodes.Status404NotFound, new { token = cachedCode });
        }



        [HttpPut("EditarContrasena")]
        public async Task<IActionResult> PutAdminsitratorPassword(InputCredentialDTO newCredential)
        {
            Administrator administrator = _context.Administrators.FirstOrDefault(a => a.EmailAdministrator == newCredential.email);
            Client client = _context.Clients.FirstOrDefault(a => a.EmailClient == newCredential.email);

            if (administrator == null && client == null)
            {
                Log.Error($"No se encontro el usuario con correo , {newCredential.email}, " + $"cod error {NotFound().StatusCode}");
                return NotFound();
            }

            Credential credential = null;

            if (administrator != null)
            {
                credential = _context.Credentials.FirstOrDefault(i => i.IdCredential == administrator.IdCredential);
            }
            else if (client != null)
            {
                credential = _context.Credentials.FirstOrDefault(i => i.IdCredential == client.IdCredential);
            }

            if (credential == null)
            {
                Log.Warning($"Intento de cambio de contraseña para el correo no registrado {newCredential.email}");
                return NotFound();
            }

            HashedFormat hash = HashEncryption.Hash(newCredential.password);
            credential.PasswordCredential = hash.Password;
            credential.SaltCredential = hash.HashAlgorithm;

            await _context.SaveChangesAsync();
            Log.Warning($"Cambio de contraseña para el correo  {newCredential.email}");
            return Ok();
        }




        private void GenerateRandomCode(int length = 8)
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

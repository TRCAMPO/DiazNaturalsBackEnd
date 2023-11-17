using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BACK_END_DIAZNATURALS.Model;
using BACK_END_DIAZNATURALS.DTO;
using BACK_END_DIAZNATURALS.Encrypt;
using Microsoft.AspNetCore.Authorization;
using Serilog;

namespace BACK_END_DIAZNATURALS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly DiazNaturalsContext _context;
        private readonly Random _random = new Random();



        public ClientsController(DiazNaturalsContext context)
        {
            _context = context;
        }



        [HttpGet]
        [Route("all")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ClientsDTO>>> GetClients()
        {
            if (_context.Clients == null)
            {
                return NotFound();
            }

            var clientDTOs = await _context.Clients
          .Select(client => new ClientsDTO
          {
              idClient = client.IdClient,
              nameClient = client.NameClient,
              addressClient = client.AddressClient,
              cityClient = client.CityClient,
              emailClient = client.EmailClient,
              nameContactClient = client.NameContactClient,
              nitClient = client.NitClient,
              phoneClient = client.PhoneClient,
              stateClient = client.StateClient,
          })
          .ToListAsync();
            return clientDTOs;
        }



        [HttpGet]
        [Route("active")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ClientsDTO>>> GetActiveClients()
        {
            if (_context.Clients == null)
            {
                return NotFound();
            }

            var clientDTOs = await _context.Clients
                .Where(p => p.IsActiveClient == true)
          .Select(client => new ClientsDTO
          {
              idClient = client.IdClient,
              nameClient = client.NameClient,
              addressClient = client.AddressClient,
              cityClient = client.CityClient,
              emailClient = client.EmailClient,
              nameContactClient = client.NameContactClient,
              nitClient = client.NitClient,
              phoneClient = client.PhoneClient,
              stateClient = client.StateClient,
          })
          .ToListAsync();
            return Ok(clientDTOs);
        }



        [HttpGet]
        [Route("inactive")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ClientsDTO>>> GetInactiveClients()
        {
            if (_context.Clients == null)
            {
                return NotFound();
            }

            var clientDTOs = await _context.Clients
                .Where(p => p.IsActiveClient == false)
          .Select(client => new ClientsDTO
          {
              idClient = client.IdClient,
              nameClient = client.NameClient,
              addressClient = client.AddressClient,
              cityClient = client.CityClient,
              emailClient = client.EmailClient,
              nameContactClient = client.NameContactClient,
              nitClient = client.NitClient,
              phoneClient = client.PhoneClient,
              stateClient = client.StateClient,
          })
          .ToListAsync();
            return Ok(clientDTOs);
        }



        [HttpGet("{nit}")]
        [Authorize]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            if (_context.Clients == null)
            {
                return NotFound();
            }
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
            {
                return NotFound();
            }
            return client;
        }



        [HttpGet]
        [Route("search")]
        [Authorize]
        public async Task<ActionResult<ClientsDTO>> GetSearchClients(string search)
        {
            Client client = SearchClient(search);
            if (client == null||!client.IsActiveClient)
            {
                return NotFound();
            }
            var clientDTO = new ClientsDTO
            {
                idClient = client.IdClient,
                nameClient = client.NameClient,
                addressClient = client.AddressClient,
                cityClient = client.CityClient,
                emailClient = client.EmailClient,
                nameContactClient = client.NameContactClient,
                nitClient = client.NitClient,
                phoneClient = client.PhoneClient,
                stateClient = client.StateClient,
            };
            return Ok(clientDTO);
        }

        [HttpGet]
        [Route("searchEmail")]
        [Authorize]
        public async Task<ActionResult<ClientsDTO>> GetSearchClientsEmail(string search)
        {
            Client client = SearchClientEmail(search);
            if (client == null || !client.IsActiveClient)
            {
                return NotFound();
            }
            var clientDTO = new ClientsDTO
            {
                idClient = client.IdClient,
                nameClient = client.NameClient,
                addressClient = client.AddressClient,
                cityClient = client.CityClient,
                emailClient = client.EmailClient,
                nameContactClient = client.NameContactClient,
                nitClient = client.NitClient,
                phoneClient = client.PhoneClient,
                stateClient = client.StateClient,
            };
            return Ok(clientDTO);
        }


        private Client SearchClient(string name)
        {
            var client = _context.Clients.FirstOrDefault(i => i.NameClient == name);
            if (client == null)
            {
                client = _context.Clients.FirstOrDefault(c => c.NitClient == name);
            }
            return client;
        }

        private Client SearchClientEmail(string email)
        {
            var client = _context.Clients.FirstOrDefault(i => i.EmailClient == email);
            if (client == null)
            {
                client = _context.Clients.FirstOrDefault(c => c.NitClient == email);
            }
            return client;
        }



        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutClient(int id, ClientsDTO clientDTO)
        {
            if (clientDTO == null)
            {
                return BadRequest();
            }
            if(clientDTO.nameClient==null || clientDTO.emailClient==null|| clientDTO.addressClient==null || clientDTO.nitClient==null || clientDTO.cityClient==null || clientDTO.stateClient==null
                || clientDTO.phoneClient==null || clientDTO.nameContactClient==null) return BadRequest();
            if (ClientNameExistsEdit(clientDTO.nameClient, clientDTO.idClient)) return Conflict("El nombre de cliente ya existe");
            if (ClientNitExistsEdit(clientDTO.nitClient, clientDTO.idClient)) return Conflict("El Nit de cliente ya existe");
            if (ClientEmailExistsEdit(clientDTO.emailClient, clientDTO.idClient)) return Conflict("El email de cliente ya existe");

            var client = _context.Clients.FirstOrDefault(i => i.IdClient == id);
            if (client == null || !client.IsActiveClient)
            {
                return NotFound();
            }
            client.NitClient = clientDTO.nitClient;
            client.NameClient = clientDTO.nameClient;
            client.EmailClient = clientDTO.emailClient;
            client.PhoneClient = clientDTO.phoneClient;
            client.StateClient = clientDTO.stateClient;
            client.AddressClient = clientDTO.addressClient;
            client.CityClient = clientDTO.cityClient;
            client.NameContactClient = clientDTO.nameContactClient;

            _context.Entry(client).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                Log.Information("Información del cliente actualizado: {@Client}", clientDTO);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(client.IdClient))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }



        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Client>> PostClient(ClientsDTO clientDTO)
        {
            if (_context.Clients == null || clientDTO == null)
            {
                return Problem("Entity set 'DiazNaturalsContext.Clients'  is null.");
            }
            if (ClientNameExists(clientDTO.nameClient)) {
                Log.Warning($"Se intento agregar un cliente cuyo nombre ya existe en la base de datos: {clientDTO.nameClient}");
                return Conflict("El nombre de cliente ya existe"); 
            }
            if (ClientNitExists(clientDTO.nitClient)) {
                Log.Warning($"Se intento agregar un cliente cuyo nit ya existe en la base de datos: {clientDTO.nitClient}");
                return Conflict("El Nit de cliente ya existe"); 
            }
            if (ClientEmailExists(clientDTO.emailClient)) {
                Log.Warning($"Se intento agregar un cliente cuyo correo ya existe en la base de datos: {clientDTO.emailClient}");
                return Conflict("El email de cliente ya existe"); 
            }

            string password = GenerateRandomCode();
            HashedFormat hash = HashEncryption.Hash(password);
            var credential = new Credential()
            {
                PasswordCredential = hash.Password,
                SaltCredential = hash.HashAlgorithm
            };
            if (credential == null)
            {
                Log.Error($"Error al generar credenciales para: {clientDTO.emailClient}");
                return NotFound();
            }

            _context.Credentials.Add(credential);
            await _context.SaveChangesAsync();
            int id = credential.IdCredential;
            var client = new Client
            {
                IdCredential = id,
                NitClient = clientDTO.nitClient,
                NameClient = clientDTO.nameClient,
                EmailClient = clientDTO.emailClient,
                IsActiveClient = true,
                AddressClient = clientDTO.addressClient,
                PhoneClient = clientDTO.phoneClient,
                CityClient = clientDTO.cityClient,
                StateClient = clientDTO.stateClient,
                NameContactClient = clientDTO.nameContactClient,
            };
            if (client == null)
            {
                Log.Error($"Error al crear el nuevo usuario: {clientDTO.emailClient}");
                return NotFound();
            }
            try
            {
                _context.Clients.Add(client);
                await _context.SaveChangesAsync();
                Log.Information($"Se agrego el usuario: {clientDTO.nameClient} con correo: {clientDTO.emailClient}");

            }
            catch { return BadRequest(); }

            try
            {
                EmailService emailService = new EmailService();
                await emailService.SendEmail(clientDTO.emailClient, "Crendecial de acceso a la pagina DiazNaturals", "Su constraseña predeterminada es: " + password + "\nPor favor actualizar la contraseña lo mas pronto posible");
                return Ok();
            }
            catch {
                Log.Error($"Error al enviar correo con credenciales para: {clientDTO.emailClient}");
                return BadRequest(); 
            }
        }



        private string GenerateRandomCode(int length = 10)
        {
            const string AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            char[] result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = AllowedChars[_random.Next(0, AllowedChars.Length)];
            }
            string randomCode = new string(result);
            return randomCode;
        }



        [HttpPatch]
        [Route("EditState")]
        [Authorize]
        public async Task<ActionResult> PatchClient(ClientDeleteDTO clientDTO)
        {
            var client = _context.Clients.FirstOrDefault(i => i.NitClient == clientDTO.nitClient);

            if (clientDTO == null || client == null)
            {
                Log.Error("Error en el cambio de estatus del cliente nit: {@Client}", clientDTO);
                return NotFound();
            }

            client.IsActiveClient = clientDTO.isActive;
            try
            {
                await _context.SaveChangesAsync();
                Log.Warning($"Cambio de estado realizado al cliente {client.NameClient}");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(client.IdClient))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }



        [HttpDelete("{nit}")]
        [Authorize]
        public async Task<IActionResult> DeleteClient(int id)
        {
            if (_context.Clients == null)
            {
                return NotFound();
            }
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return NoContent();
        }



        private bool ClientExists(int id)
        {
            return (_context.Clients?.Any(e => e.IdClient == id)).GetValueOrDefault();
        }
        private bool ClientNitExists(string nit)
        {
            return (_context.Clients?.Any(e => e.NitClient == nit)).GetValueOrDefault();
        }
        private bool ClientNameExists(string name)
        {
            return (_context.Clients?.Any(e => e.NameClient == name)).GetValueOrDefault();
        }
        private bool ClientEmailExists(string email)
        {
            return (_context.Clients?.Any(e => e.EmailClient == email) ?? false) ||
                   (_context.Administrators?.Any(e => e.EmailAdministrator == email) ?? false);
        }

        private bool ClientNitExistsEdit(string nit, int id)
        {
            return (_context.Clients?
                .Where(i=> i.IdClient != id)
                .Any(e => e.NitClient == nit)).GetValueOrDefault();
        }
        private bool ClientNameExistsEdit(string name, int id)
        {
            return (_context.Clients?
                .Where(i=> i.IdClient != id)
                .Any(e => e.NameClient == name)).GetValueOrDefault();
        }
        private bool ClientEmailExistsEdit(string email, int id)
        {
            return (_context.Clients?
                .Where(i=>i.IdClient!=id)
                .Any(e => e.EmailClient == email)).GetValueOrDefault();
        }

    }
}

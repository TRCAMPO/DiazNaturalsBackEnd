﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BACK_END_DIAZNATURALS.Model;
using BACK_END_DIAZNATURALS.DTO;
using BACK_END_DIAZNATURALS.Encrypt;

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
        public async Task<ActionResult<IEnumerable<ClientsDTO>>> GetClients()
        {
          if (_context.Clients == null)
          {
              return NotFound();
          }

            var clientDTOs = await _context.Clients
          .Select(client => new ClientsDTO
          {
             idClient= client.IdClient,
             nameClient= client.NameClient,
             addressClient= client.AddressClient,
             cityClient= client.CityClient,
             emailClient = client.EmailClient,
             nameContactClient= client.NameContactClient,
             nitClient= client.NitClient,
             phoneClient= client.PhoneClient,
             stateClient = client.StateClient,

          })
          .ToListAsync();

            return clientDTOs;

            
        }

        [HttpGet("{id}")]
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


        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient(int id, Client client)
        {
            if (id != client.IdClient)
            {
                return BadRequest();
            }

            _context.Entry(client).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
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
        public async Task<ActionResult<Client>> PostClient(ClientsDTO clientDTO)
        {
          if (_context.Clients == null || clientDTO== null)
          {
              return Problem("Entity set 'DiazNaturalsContext.Clients'  is null.");
          }

            string password= GenerateRandomCode();

            HashedFormat hash = HashEncryption.Hash(password);

            var credential = new Credential()
            {
                PasswordCredential = hash.Password,
                SaltCredential = hash.HashAlgorithm
            };
            if (credential == null)
            {
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
                return NotFound();
            }
            try
            {
                _context.Clients.Add(client);
                await _context.SaveChangesAsync();

            }catch
            {
                return BadRequest();
            }
            
            try
            {
                EmailService emailService = new EmailService();
                await emailService.SendEmail(clientDTO.emailClient, "Crendecial de acceso a la pagina DiazNaturals", "Su constraseña predeterminada es: " + password + "\nPor favor actualizar la contraseña lo mas pronto posible");
                return Ok();
            }
            catch
            {
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

        [HttpDelete("{id}")]
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BACK_END_DIAZNATURALS.Model;
using BACK_END_DIAZNATURALS.DTO;
using Microsoft.AspNetCore.Authorization;
using Google.Apis.Util;
using Serilog;

namespace BACK_END_DIAZNATURALS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly DiazNaturalsContext _context;
        public StatusController(DiazNaturalsContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetStatusDTO>>> GetStatus()
        {
            if (_context.Statuses == null) return NotFound();

            var status = await _context.Statuses
                .Select(o => new GetStatusDTO
                {
                   IdStatus = o.IdStatus,
                   NameStatus = o.NameStatus,
                }).
                ToListAsync();
            if (status == null) return NotFound();
            return Ok(status);
        }



        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<GetStatusDTO>> GetStatus(int id)
        {
            if (_context.Statuses == null) return NotFound();
            var status = await _context.Statuses
                .Where(p => p.IdStatus == id)
                .Select(p => new GetStatusDTO
                {
                    IdStatus = p.IdStatus,
                    NameStatus = p.NameStatus,
                }).FirstOrDefaultAsync();

            if (status == null) return NotFound();
            return status;
        }



        [HttpGet]
        [Route("Search/{nameStatus}")]
       // [Authorize]
        public async Task<ActionResult<IEnumerable<GetStatusDTO>>> GetStatusNext(string nameStatus)
        {
            if (_context.Statuses == null) return NotFound();

            var status = _context.Statuses
                .Where(p => p.NameStatus == nameStatus).FirstOrDefault();
            if(status == null) return NotFound();   
            int id = status.IdStatus;
            List<GetStatusDTO> getStatusDTO = new List<GetStatusDTO>();
            {

            };
            switch (id)
            {
                case 1:
                    getStatusDTO = getStatusNext(1,2, 5);
                    break;
                case 2:
                    getStatusDTO = getStatusNext(2, 3);
                    break;
                case 3:
                    getStatusDTO = getStatusNext(3, 4);
                    break;
                case 4:
                    getStatusDTO = getStatusNext(4);
                    break;
                case 5:
                    getStatusDTO = getStatusNext(5);
                    break;
                case 6:
                    getStatusDTO = getStatusNext(6);
                    break;
            }
            if (getStatusDTO == null) return NotFound();
            return Ok(getStatusDTO);
        }


        private List<GetStatusDTO> getStatusNext(int actual, int idStatus, int idStatus2)
        {
            var getStatusDTO = _context.Statuses
                        .Where(c => c.IdStatus== actual || c.IdStatus == idStatus || c.IdStatus == idStatus2 )
                        .Select(p => new GetStatusDTO
                        {
                            IdStatus = p.IdStatus,
                            NameStatus = p.NameStatus,
                        }).ToList();

            return getStatusDTO;
        }


        private List<GetStatusDTO> getStatusNext(int actual, int idStatus)
        {
            var getStatusDTO = _context.Statuses
                        .Where(c => c.IdStatus == actual || c.IdStatus == idStatus)
                        .Select(p => new GetStatusDTO
                        {
                            IdStatus = p.IdStatus,
                            NameStatus = p.NameStatus,
                        }).ToList();

            return getStatusDTO;
        }



        private List<GetStatusDTO> getStatusNext(int actual)
        {
            var getStatusDTO = _context.Statuses
                        .Where(c => c.IdStatus == actual)
                        .Select(p => new GetStatusDTO
                        {
                            IdStatus = p.IdStatus,
                            NameStatus = p.NameStatus,
                        }).ToList();

            return getStatusDTO;
        }




        [HttpPost]
        public async Task<ActionResult<GetStatusDTO>> PostStatus(PostStatusDTO status)
        {
            if (_context.Statuses == null)
            {
                Log.Information($"Error en el servidor, cod error 500");
                return Problem("Entity set 'DiazNaturalsContext.Entries'  is null.");
            }
            Status statusAux = new Status
            {
                NameStatus = status.NameStatus ,
            };

            _context.Statuses.Add(statusAux);
            await _context.SaveChangesAsync();
            Log.Information($"Se agregaron la categoria {statusAux.IdStatus}");
            return Ok(statusAux);

        }




        [HttpPut]
        public async Task<IActionResult> PutStatus(int id, GetStatusDTO status)
        {
            if (id != status.IdStatus)
            {
                Log.Error($"Error en los parametros de la aplicacion para editar el estado: {id}");
                return BadRequest();
            }
            var statusAux = _context.Statuses.Find(id);
            if (statusAux == null)
            {
                Log.Error($"No se encontro el estado: {id}");
                return NotFound();
            }
            statusAux.NameStatus = status.NameStatus;
            _context.Entry(statusAux).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                Log.Information("Información del estado actualizada: {@Status}", status);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StatusExists(id))
                {
                    return NotFound();
                }
                else { throw; }
            }

            return NoContent();
        }

        private bool StatusExists(int id)
        {
            return (_context.Statuses?.Any(e => e.IdStatus == id)).GetValueOrDefault();
        }
    }
}

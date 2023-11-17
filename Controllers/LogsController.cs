using BACK_END_DIAZNATURALS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.IO;

namespace BACK_END_DIAZNATURALS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly FileListService _fileListService;

        public LogsController(FileListService fileListService)
        {
            _fileListService = fileListService;
        }

        [HttpGet("DownloadLogs/{name}")]
        public IActionResult DownloadLogs(string name)
        {
           
            var filePath = "logs/"+name; 
            string rutaCopiaLogs = "logs/log_copia.txt";
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("El archivo de logs no se encontró.");
            }
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Copy(filePath, rutaCopiaLogs, true);
            }
            
            var fileBytes = System.IO.File.ReadAllBytes(rutaCopiaLogs);
            return File(fileBytes, "text/plain", "log.txt");

        }
        [HttpGet]
        public IActionResult GetFileList()
        {
            var folderPath = "logs"; 
            var fileList = _fileListService.GetFileList(folderPath);
            return Ok(fileList);
        }

    }
}

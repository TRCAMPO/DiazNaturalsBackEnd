using BACK_END_DIAZNATURALS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.IO;

namespace BACK_END_DIAZNATURALS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LogsController : ControllerBase
    {
        private readonly FileListService _fileListService;

        public LogsController(FileListService fileListService)
        {
            _fileListService = fileListService;
        }

        [HttpGet("descargarLogs/{name}")]
        public IActionResult DescargarLogs(string name)
        {
            // Ruta al archivo de logs
            var filePath = "logs/"+name; // Asegúrate de proporcionar la ruta correcta
            string rutaCopiaLogs = "logs/log_copia.txt";
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("El archivo de logs no se encontró.");
            }
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Copy(filePath, rutaCopiaLogs, true);
            }
            // Lee el archivo y prepara la descarga como un FileResult
            var fileBytes = System.IO.File.ReadAllBytes(rutaCopiaLogs);
            return File(fileBytes, "text/plain", "log.txt");

        }
        [HttpGet]
        public IActionResult GetFileList()
        {
            var folderPath = "logs"; // Ruta de la carpeta en tu proyecto
            var fileList = _fileListService.GetFileList(folderPath);
            return Ok(fileList);
        }

    }
}

using BACK_END_DIAZNATURALS.DTO;
using BACK_END_DIAZNATURALS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Serilog;
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
            string nameAux= name.Remove(0, "yyyy-MM-dd  HH-mm-ss  ".Length);
            var filePath = "logs/"+ nameAux; 
            string rutaCopiaLogs = "logs/log_copia.txt";
            if (!System.IO.File.Exists(filePath))
            {
                Log.Error($"No se encontro el archivo de logs {nameAux}, cod error {NotFound().StatusCode}");
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
            ManageCSV c = new ManageCSV();
            List<string> x = c.ReadFile();
            
            List<DateTime> dates = new List<DateTime>();
            x.ForEach(v => dates.Add(DateTime.Parse(v)));
            //string fechaFormateada = fecha.ToString("yyyy-MM-dd  HH-mm-ss  ");
            List<string> dateFormat = new List<string>();
            dates.ForEach(v => dateFormat.Add(v.ToString("yyyy-MM-dd  HH-mm-ss  ")));
           
           
              var folderPath = "logs"; 
            List<String> fileList = _fileListService.GetFileList(folderPath);
        
           
            for (int i = 0; i < fileList.Count; i++)
            {

                if (i!=fileList.Count-1)
                {

                    fileList[i] = dateFormat[i]+fileList[i];

                }
            }
            return Ok(fileList);
           
        }

    }
}

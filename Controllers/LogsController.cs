using BACK_END_DIAZNATURALS.DTO;
using BACK_END_DIAZNATURALS.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.VisualBasic;
using Serilog;
using System;
using System.Drawing;
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
      //  [EnableCors]
        public IActionResult GetFileList()
        {
            //ManageCSV c = new ManageCSV();
            //List<string> x = c.ReadFile();
            
           // List<DateTime> dates = new List<DateTime>();
           // x.ForEach(v => dates.Add(DateTime.Parse(v)));
           // List<string> dateFormat = new List<string>();
            //dates.ForEach(v => dateFormat.Add(v.ToString("yyyy-MM-dd  HH-mm-ss  ")));
           
           
              var folderPath = "logs"; 
            List<String> fileList = _fileListService.GetFileList(folderPath);
            //string year = "";
           // string month = "";
          //  string nameAux = "";
            for (int i = 0; i < fileList.Count; i++)
            {
                /* nameAux = fileList[0].Remove(0, 2);
                // nameAux =  nameAux.Substring(0, "202311".Length);
                // string nameAu2x =  nameAux.Substring(0, "202311".Length);
                year =  nameAux.Substring(0, "2023".Length);
                month =  nameAux.Substring("2023".Length, "11".Length);

                if (fileList[i]!= "log_copia.txt")
                {

                    fileList[i] = dateFormat[i]+fileList[i];

                }*/
                if (fileList[i] != "log_copia.txt")
                {
                    string name = "2023-11-19  10-30-31  ";
                    fileList[i] = name + fileList[i];
                }
            }
            return Ok(fileList);
           
        }

    }
}

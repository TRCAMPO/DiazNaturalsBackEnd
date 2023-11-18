using System;
using System.IO;
using System.Linq;
namespace BACK_END_DIAZNATURALS.Services
{
    public class FileListService
    {
        public string GetFileList(string folderPath)
        {
            try
            {
                if (Directory.Exists(folderPath))
                {
                    var fileNames = Directory.GetFiles(folderPath)
                        .Select(Path.GetFileName)
                        .ToList();

                    return Newtonsoft.Json.JsonConvert.SerializeObject(fileNames);
                }
                else
                {
                    return "La carpeta especificada no existe.";
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
    }
}

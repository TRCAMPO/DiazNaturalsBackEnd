using System;
using System.IO;
using System.Linq;
namespace BACK_END_DIAZNATURALS.Services
{
    public class FileListService
    {
        public List<string> GetFileList(string folderPath)
        {
            List<string> fileNames = new List<string>();

            try
            {
                if (Directory.Exists(folderPath))
                {
                    fileNames = Directory.GetFiles(folderPath)
                                        .Select(Path.GetFileName)
                                        .ToList();
                }
                else
                {
                    // Aquí podrías lanzar una excepción o manejarlo de otra manera si prefieres.
                    // En este ejemplo, se retorna una lista vacía si la carpeta no existe.
                    Console.WriteLine("La carpeta especificada no existe.");
                }
            }
            catch (Exception ex)
            {
                // Aquí podrías lanzar una excepción o manejar el error de otra manera si prefieres.
                // En este ejemplo, se imprime el mensaje de error y se retorna una lista vacía.
                Console.WriteLine("Error: " + ex.Message);
            }

            return fileNames;
        }

    }
}

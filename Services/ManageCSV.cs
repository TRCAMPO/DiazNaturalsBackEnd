using NuGet.Packaging;

namespace BACK_END_DIAZNATURALS.Services
{
    public class ManageCSV
    {
        public void addFile()
        {
            DateTime fecha = DateTime.Now;
            // Ruta donde se guardará el archivo CSV
            string rutaArchivoCSV = @"Services/DateLog.csv"; // Reemplaza con la ruta deseada

            // Datos a guardar en el CSV (en este caso, solo se guarda la fecha formateada)
            string[] datos = { fecha.ToString() };

            try
            {
                // Verifica si el archivo ya existe
                using (StreamWriter writer = File.AppendText(rutaArchivoCSV))
                {
                    writer.WriteLine(string.Join(",", datos)); // Escribe los datos separados por comas
                }
                Console.WriteLine("Datos agregados al archivo CSV.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al acceder al archivo CSV: {ex.Message}");
            }
        }

        public List<string> ReadFile()
        {
            string rutaArchivoCSV = @"Services/DateLog.csv";
            List<string> lineasCSV = new List<string>();

            try
            {
                lineasCSV = File.ReadAllLines(rutaArchivoCSV).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al leer el archivo CSV: {ex.Message}");
            }

            return lineasCSV;
        }

    }
}

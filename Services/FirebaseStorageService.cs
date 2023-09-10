using Google.Cloud.Storage.V1;

namespace BACK_END_DIAZNATURALS.Services
{
    public class FirebaseStorageService
    {
        private readonly StorageClient _storageClient;
        private readonly string BucketName = "diaznaturals-e056b.appspot.com";

        public FirebaseStorageService(IConfiguration configuration)
        {
            string projectId = configuration["Firebase:ProjectId"];
            _storageClient = StorageClient.Create();
        }



        public async Task<string> ImageUploadAsync(IFormFile file, string fileName)
        {
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Position = 0;
                string filePath = $"Productos/{fileName}";
                await _storageClient.UploadObjectAsync(BucketName, filePath, null, stream);
                string url = $"https://storage.googleapis.com/{BucketName}/{filePath}";
                return url;
            }
        }



        public async Task<Stream> GetImageAsync(string fileName)
        {
            string filePath = $"Productos/{fileName}";
            try
            {
                var stream = new MemoryStream();
                await _storageClient.DownloadObjectAsync(BucketName, filePath, stream);
                stream.Position = 0;
                return stream;
            }
            catch (Google.GoogleApiException ex)
            {
                Console.WriteLine($"Error al obtener la imagen: {ex.Message}");
                return null;
            }
        }
    }
}

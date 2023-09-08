using BACK_END_DIAZNATURALS.Services;
using Microsoft.AspNetCore.Mvc;

namespace BACK_END_DIAZNATURALS.Controllers
{
    public class BlobController : ControllerBase
    {
        private readonly FirebaseStorageService _firebaseStorageService;

        public BlobController(FirebaseStorageService firebaseStorageService)
        {
            _firebaseStorageService = firebaseStorageService;
        }

        [HttpPost]
        [Route("load")]
        public async Task<IActionResult> LoadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Archivo no válido");

            string fileName = file.FileName; 
            string url = await _firebaseStorageService.ImageUploadAsync(file, fileName);
            string[] urlParts = url.Split('/');
            string imageName = urlParts[urlParts.Length - 1]; 
            return Ok(new { FileName = imageName });
        }

        [HttpGet("{imageName}")]
        public async Task<IActionResult> GetImage(string imageName)
        {
            var imageStream = await _firebaseStorageService.GetImageAsync(imageName);

            if (imageStream == null)
            {
                return NotFound(); 
            }

            return File(imageStream, "image/png"); 
        }
    }
}

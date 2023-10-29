using BACK_END_DIAZNATURALS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BACK_END_DIAZNATURALS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlobController : ControllerBase
    {
        private readonly FirebaseStorageService _firebaseStorageService;

        

        public BlobController(FirebaseStorageService firebaseStorageService)
        {
            _firebaseStorageService = firebaseStorageService;
        }



        [HttpPost]
        [Route("load")]
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> GetImage(string imageName)
        {
            var imageStream = await _firebaseStorageService.GetImageAsync(imageName);

            if (imageStream == null) { return NotFound(); }
            return File(imageStream, "image/png");
        }


        [HttpPost]
        [Route("loadProof")]
        [Authorize]
        public async Task<IActionResult> LoadImageProofPaymet(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Archivo no válido");
            string fileName = file.FileName;
            string url = await _firebaseStorageService.ImageUploadProofAsync(file, fileName);
            string[] urlParts = url.Split('/');
            string imageName = urlParts[urlParts.Length - 1];
            return Ok(new { FileName = imageName });
        }




        [HttpGet]
        [Route("proof")]
        [Authorize]
        public async Task<IActionResult> GetImageProofPaymet(string imageName)
        {
            var imageStream = await _firebaseStorageService.GetImageProofAsync(imageName);

            if (imageStream == null) { return NotFound(); }
            return File(imageStream, "image/png");
        }
    }
    }

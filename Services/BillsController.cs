
using Microsoft.AspNetCore.Mvc;

namespace BACK_END_DIAZNATURALS.Services;
[ApiController]
[Route("api/[controller]")]
public class BillsController : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> Get(int number)
    {
        try
        {
            EmailService emailService = new EmailService();
            await emailService.SendEmail("julian.ramirez04@uptc.edu.co", "Prueba 1", "Tu código de recuperación es 12345");

            return Ok(); // Retorna un resultado exitoso (código 200)
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}"); // Retorna un código 500 en caso de error
        }
    }


}


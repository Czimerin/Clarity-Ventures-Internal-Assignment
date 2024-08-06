using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using Core.Models;


namespace Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        // POST api/email/send
        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] EmailModel email)
        {
            if (email == null)
            {
                return BadRequest("Email data is null.");
            }

            try
            {
                await _emailService.SendEmailAsync(email);
                return Ok("Email sent successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}

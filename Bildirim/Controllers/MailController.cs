using Bildirim.Models;
using Bildirim.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Bildirim.Controllers
{	
	
	[Route("sendmail")]
        [ApiController]
	public class MailController: ControllerBase
    {

        private readonly IMailService _MailService;

		public MailController (IMailService mailService)
		{
			_MailService = mailService;
		}

        [HttpPost]
		public async Task<IActionResult> Send( MailModel model)
		{
			var result = _MailService.SendAsync(model);
			return Ok(result);
		}

	}
}

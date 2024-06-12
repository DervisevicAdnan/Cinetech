using CineTech.Models;
using CineTech.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CineTech.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailService _mailService;
        //injecting the IMailService into the constructor
        public MailController(IMailService _MailService)
        {
            _mailService = _MailService;
        }

        [HttpPost]
        [Route("SendMail")]
        public bool SendMail(MailData mailData)
        {
            return _mailService.SendMail(mailData);
        }

        [HttpPost]
        [Route("SendHTMLMail")]
        public bool SendHTMLMail(HTMLMailData htmlMailData)
        {
            return _mailService.SendHTMLMail(htmlMailData);
        }
    }
}

using aspDotNetCore.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace aspDotNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IOptions<AttachmentOptions> _attachmentOptions;

        public ConfigController(IConfiguration configuration, IOptions<AttachmentOptions> attachmentOptions)
        {
            this._configuration = configuration;
            this._attachmentOptions = attachmentOptions;
        }

        [HttpGet]
        [Route("/")]
        public ActionResult GetConfig()
        {
            var config = new
            {
                AllowedHosts = _configuration["AllowedHosts"],
                ConnectionStrings = _configuration.GetConnectionString("DefaultConnection"),
                DefaultLogLevel = _configuration["Logging:LogLevel:Default"],
                TestKey = _configuration["TestKey"],
                SigningKey = _configuration["SigningKey"],
                AttachmentOptions = _attachmentOptions.Value,
            };
            return Ok(config);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace student_resource_hub.Controllers
{
    [Authorize]
    public class AiAssistantController : Controller
    {
        private readonly ILogger<AiAssistantController> _logger;

        public AiAssistantController(ILogger<AiAssistantController> logger)
        {
            _logger = logger;
        }

        // GET: /AiAssistant
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}

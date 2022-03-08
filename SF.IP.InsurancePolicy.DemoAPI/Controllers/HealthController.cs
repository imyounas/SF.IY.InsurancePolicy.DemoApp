using Microsoft.AspNetCore.Mvc;
using System;

namespace SF.IY.InsurancePolicy.DemoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Health()
        {  
            return Ok($"Pong! {DateTime.UtcNow.ToString("dddd, dd MMMM yyyy HH:mm:ss")}");
        }
    }
}

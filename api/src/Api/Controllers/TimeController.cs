using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TimeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok(DateTimeOffset.Now);
    }
}

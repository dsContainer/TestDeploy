using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalSignature.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class SignController : ControllerBase
    {
        //private readonly ISignService _service;

    }
}

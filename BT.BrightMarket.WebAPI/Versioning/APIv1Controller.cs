using Microsoft.AspNetCore.Mvc;

namespace BT.BrightMarket.WebAPI.Versioning
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public abstract class APIv1Controller : ControllerBase { }
}

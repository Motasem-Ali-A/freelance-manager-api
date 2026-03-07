 using Microsoft.AspNetCore.Mvc;

 namespace FreelanceManager.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ClientsController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return ["client"];
        }
        [HttpGet ("{id}")]
        public string Get(int id)
        {
            return $"client {id}";
        }
    };

}
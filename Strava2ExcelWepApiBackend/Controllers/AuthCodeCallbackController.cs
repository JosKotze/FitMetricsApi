using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Strava2ExcelWebApiBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthCodeCallbackController : ControllerBase
    {
        // GET: api/<AuthCodeCallbackController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Server is", "working!" };
        }

        // GET api/<AuthCodeCallbackController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AuthCodeCallbackController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AuthCodeCallbackController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AuthCodeCallbackController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

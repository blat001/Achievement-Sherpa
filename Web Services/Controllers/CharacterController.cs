using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Web_Services.Controllers
{
    public class CharacterController : ApiController
    {
        // GET api/character
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/character/5
        public string Get(string server, string name)
        {
            return "value";
        }

        // POST api/character
        public void Post([FromBody]string value)
        {
        }

        // PUT api/character/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/character/5
        public void Delete(int id)
        {
        }
    }
}

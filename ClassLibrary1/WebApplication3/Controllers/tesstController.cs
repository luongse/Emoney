using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication3.Controllers
{
    public class tesstController : ApiController
    {
        // GET: api/tesst
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/tesst/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/tesst
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/tesst/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/tesst/5
        public void Delete(int id)
        {
        }
    }
}

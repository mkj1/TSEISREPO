using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using OwnerControl.Interface;
using System.Web.Http;
using Requester.Interface;

namespace Web1.Controllers
{
    [Route("api/")]
    public class ValuesController : Controller
    {
        // GET api/
        [HttpGet]
        public async Task<string> Get()
        {
            IBuy mybuy =
    ServiceProxy.Create<IBuy>(new Uri("fabric:/TSEIS/Requester"));

            bool ok = await mybuy.BuyStockAsync();


            return ok.ToString();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        //[Route("provider")]
        //[HttpPost]
        //public Stock Post([FromUri]Stock stck)
        //{
        //    var x = stck;
        //    x.name = "hehehe";

        //    return x;
        //}

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

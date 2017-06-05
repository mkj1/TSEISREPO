using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using OwnerControl.Interface;
using Models;
using System.Web.Http;

namespace Web1.Controllers
{
    [Route("api/")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public string Get()
        {
            //IAddStock stock =
            //    ServiceProxy.Create<IAddStock>(new Uri("fabric:/TSEIS/OwnerControl"), new ServicePartitionKey(0));

            //var x = await stock.AddStockAsync();

            return "test";
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [Route("provider")]
        [HttpPost]
        public Stock Post([FromUri]Stock stck)
        {
            var x = stck;
            x.name = "hehehe";

            return x;
        }

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

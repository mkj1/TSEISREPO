using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Stateless1.Interface;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;

namespace Stateless2
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    class Stateless2 : StatelessService, IHello
    {
        public Stateless2(StatelessServiceContext context)
            : base(context)
        { }

        public Task<List<Stock>> HelloWorldAsync(Stock stck)
        {

            var stocklist = new List<Stock>();

            for(int i = 0; i < 5; i++)
            {
                stocklist.Add(stck);
            }

            return Task.FromResult(stocklist);
        }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new[] { new ServiceInstanceListener(context => this.CreateServiceRemotingListener(context)) };
        }


    }
}

using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using TobinTaxer.Interface;

namespace TobinTaxer
{

    internal sealed class TobinTaxer : StatelessService, ICalcTax
    {
        public TobinTaxer(StatelessServiceContext context)
            : base(context)
        { }


        public Task<float> CalcTaxAsync(float amount)
        {
            float newamount = (amount /10)*9;

            return Task.FromResult(newamount);
        }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new[] { new ServiceInstanceListener(context => this.CreateServiceRemotingListener(context)) };
        }
    }
}

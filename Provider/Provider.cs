using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using TobinTaxer.Interface;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using OwnerControl.Interface;
using Microsoft.ServiceFabric.Services.Client;

namespace Provider
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class Provider : StatelessService
    {
        public Provider(StatelessServiceContext context)
            : base(context)
        { }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[0];
        }


        protected override async Task RunAsync(CancellationToken cancellationToken)
        {


            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();





                ICalcTax calcTaxClient = ServiceProxy.Create<ICalcTax>(new Uri("fabric:/TSEIS/TobinTaxer"));

                var restamount = await calcTaxClient.CalcTaxAsync(600);

                ServiceEventSource.Current.ServiceMessage(this.Context, "restamount-{0}", restamount.ToString());

                await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken);

                IAddStock stock =
ServiceProxy.Create<IAddStock>(new Uri("fabric:/TSEIS/OwnerControl"), new ServicePartitionKey(0));

                var x = await stock.AddStockAsync();
            }
        }
    }
}

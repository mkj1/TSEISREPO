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
using Provider.Interface;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;

namespace Provider
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class Provider : StatelessService, IProvider
    {
        public Provider(StatelessServiceContext context)
            : base(context)
        { }

        public async Task<bool> AddStocksAsync()
        {
            IAddStock stock =
ServiceProxy.Create<IAddStock>(new Uri("fabric:/TSEIS/OwnerControl"), new ServicePartitionKey(0));

            var x = await stock.GetAllAsync();

            return true;
        }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new List<ServiceInstanceListener>()
    {
        new ServiceInstanceListener(
            (context) =>
                this.CreateServiceRemotingListener(context))
    };
        }


        protected override async Task RunAsync(CancellationToken cancellationToken)
        {


            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();


                await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken);


            }
        }
    }
}

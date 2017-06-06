using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using OwnerControl.Interface;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Client;
using Requester.Interface;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using TobinTaxer.Interface;
using Models;
using Broker.Interface;

namespace Requester
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class Requester : StatelessService, IBuy
    {
        public Requester(StatelessServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new List<ServiceInstanceListener>()
            {
                new ServiceInstanceListener(
                    (context) =>
                        this.CreateServiceRemotingListener(context))
            };
        }


        public async Task<bool> BuyStockAsync()
        {
            IAddStock stock =
                ServiceProxy.Create<IAddStock>(new Uri("fabric:/TSEIS/OwnerControl"), new ServicePartitionKey(0));

            var x = await stock.GetAllAsync();
            ServiceEventSource.Current.ServiceMessage(this.Context, "From Requester: List length: {0}", x.Count.ToString());

            var tempstock = new Stock() { };


            Random r = new Random();
            var rand = x[r.Next(x.Count)];

            tempstock.name = rand.name;
            tempstock.value = rand.value;
            tempstock.owner = rand.owner;



            IBuyExactStock buyxstock = ServiceProxy.Create<IBuyExactStock>(new Uri("fabric:/TSEIS/Broker"));

            var success = await buyxstock.BuyExactStockAsync(tempstock);

            ServiceEventSource.Current.ServiceMessage(this.Context, "From Requester - Succes buying this stock: {0}", tempstock.name);




            ICalcTax calcTaxClient = ServiceProxy.Create<ICalcTax>(new Uri("fabric:/TSEIS/TobinTaxer"));

            var requesterPrice = await calcTaxClient.CalcTaxAsync(tempstock.value);

            ServiceEventSource.Current.ServiceMessage(this.Context, "From Requester - Price paid: {0}", requesterPrice.ToString());


            return true;
        }

    }
}

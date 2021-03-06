﻿using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Models;
using Broker.Interface;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using TobinTaxer.Interface;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using OwnerControl.Interface;
using Microsoft.ServiceFabric.Services.Client;

namespace Broker
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class Broker : StatelessService, IBuyExactStock
    {
        public Broker(StatelessServiceContext context)
            : base(context)
        { }



        public async Task<bool> BuyExactStockAsync(Stock stock)
        {
            var sst = stock;

            IAddStock scproxy =
                ServiceProxy.Create<IAddStock>(new Uri("fabric:/TSEIS/OwnerControl"), new ServicePartitionKey(0));

            var success = await scproxy.UpdateStockAsync(stock);



            return await Task.FromResult(success);
        }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new[] { new ServiceInstanceListener(context => this.CreateServiceRemotingListener(context)) };
        }

        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
    }
}

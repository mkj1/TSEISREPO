using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Models;
using OwnerControl.Interface;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Data;

namespace OwnerControl
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class OwnerControl : StatefulService, IAddStock
    {
        public OwnerControl(StatefulServiceContext context)
            : base(context)
        { }

        public async Task<List<Stock>> AddStockAsync() //returns all
        {
            var myDictionary =
                 await this.StateManager.GetOrAddAsync<IReliableDictionary<string, List<Stock>>>("myDictionary");

            using (var tx = this.StateManager.CreateTransaction())
            {
                var result = await myDictionary.TryGetValueAsync(tx, "TSEISlist");
                return result.Value;
            }

        }

        public async Task<List<Stock>> GetAllAsync() //adds stocks
        {
            var myDictionary =
                 await this.StateManager.GetOrAddAsync<IReliableDictionary<string, List<Stock>>>("myDictionary");

            using (ITransaction tx = StateManager.CreateTransaction())
            {
                ConditionalValue<List<Stock>> currentStock =
                   await myDictionary.TryGetValueAsync(tx, "TSEISlist");


                if (currentStock.HasValue)
                {
                    var updatedStock = new List<Stock>();
                    updatedStock = currentStock.Value;

                    updatedStock.Add(new Stock() { value = 25, name = "BABA", owner = "John", id = ++idcounter });
                    updatedStock.Add(new Stock() { value = 35, name = "DB", owner = "John", id = ++idcounter });
                    updatedStock.Add(new Stock() { value = 45, name = "CARLSBERG", owner = "John", id = ++idcounter });

                    ServiceEventSource.Current.ServiceMessage(this.Context, "OwnerControl: Stock added. Now contains {0} stocks.", updatedStock.Count.ToString());

                    await myDictionary.SetAsync(tx, "TSEISlist", updatedStock);

                    await tx.CommitAsync();

                }



            }

            using (var tx = this.StateManager.CreateTransaction())
            {
                var result = await myDictionary.TryGetValueAsync(tx, "TSEISlist");
                return result.Value;
            }

        }



        public async Task<bool> UpdateStockAsync(Stock stck) //updates stock
        {

            var myDictionary =
                 await this.StateManager.GetOrAddAsync<IReliableDictionary<string, List<Stock>>>("myDictionary");

            using (ITransaction tx = StateManager.CreateTransaction())
            {
                ConditionalValue<List<Stock>> currentStockList =
                   await myDictionary.TryGetValueAsync(tx, "TSEISlist");



                string newstock = "N/A";
                if (currentStockList.HasValue)
                {
                    var updatedStockList = new List<Stock>();
                    updatedStockList = currentStockList.Value;

                    foreach(var curstck in updatedStockList)
                    {
                        if(curstck.id == stck.id)
                        {
                            curstck.owner = stck.owner;
                            newstock = stck.name;
                        }
                    }


                    ServiceEventSource.Current.ServiceMessage(this.Context, "OwnerControl: Stock" + " {0} " + " changed owner.", newstock);

                    await myDictionary.SetAsync(tx, "TSEISlist", updatedStockList);

                    await tx.CommitAsync();

                }



            }

            return true;

        }


        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new List<ServiceReplicaListener>()
            {
                new ServiceReplicaListener(
                    (context) =>
                        this.CreateServiceRemotingListener(context))
            };
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.
            var myDictionary =
            await this.StateManager.GetOrAddAsync<IReliableDictionary<string, List<Stock>>>("myDictionary");

            using (ITransaction tx = StateManager.CreateTransaction())
            {
                ConditionalValue<List<Stock>> currentStock =
                   await myDictionary.TryGetValueAsync(tx, "TSEISlist");


                if (!currentStock.HasValue)
                {
                    await myDictionary.TryAddAsync(tx, "TSEISlist", new List<Stock>() { });
                    await tx.CommitAsync();

                }

            }


        }

        private int idcounter { get; set; } = 0;
    }
}

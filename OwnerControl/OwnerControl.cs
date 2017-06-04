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

        public async Task<Stock> AddStockAsync()
        {
            var myDictionary =
                 await this.StateManager.GetOrAddAsync<IReliableDictionary<string, Stock>>("myDictionary");

            using (var tx = this.StateManager.CreateTransaction())
            {
                var result = await myDictionary.TryGetValueAsync(tx, "Hey");
                return result.Value;
            }
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

            var myDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, Stock>>("myDictionary");

            while (true)
            {

                using (ITransaction tx = StateManager.CreateTransaction())
                {
                    // Use the user’s name to look up their data
                    ConditionalValue<Stock> currentStock =
                       await myDictionary.TryGetValueAsync(tx, "Hey");

                    // The user exists in the dictionary, update one of their properties.
                    if (currentStock.HasValue)
                    {
                        // Create new user object with the same state as the current user object.
                        // NOTE: This must be a deep copy; not a shallow copy. Specifically, only
                        // immutable state can be shared by currentUser & updatedUser object graphs.
                        Stock updatedStock = new Stock() { name = currentStock.Value.name, value = currentStock.Value.value, owner = currentStock.Value.owner };

                        // In the new object, modify any properties you desire
                        updatedStock.value++;

                        // Update the key’s value to the updateUser info
                        await myDictionary.SetAsync(tx, "Hey", updatedStock);

                        await tx.CommitAsync();
                    }

                    else
                    {

                            // AddAsync takes key's write lock; if >4 secs, TimeoutException
                            // Key & value put in temp dictionary (read your own writes),
                            // serialized, redo/undo record is logged & sent to
                            // secondary replicas
                            await myDictionary.TryAddAsync(tx, "Hey", new Stock() {value= 25, owner ="John", name="BABA" });

                            // CommitAsync sends Commit record to log & secondary replicas
                            // After quorum responds, all locks released
                            await tx.CommitAsync();

                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }
    }
}

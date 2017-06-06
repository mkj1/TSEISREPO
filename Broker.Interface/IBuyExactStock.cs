using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Interface
{
    using Microsoft.ServiceFabric.Services.Remoting;
    using Models;

    public interface IBuyExactStock : IService
    {
        Task<bool> BuyExactStockAsync(Stock stck);
    }
}

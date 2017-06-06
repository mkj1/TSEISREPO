using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Requester.Interface
{
    using Microsoft.ServiceFabric.Services.Remoting;

    public interface IBuy : IService
    {
        Task<bool> BuyStockAsync();
    }
}

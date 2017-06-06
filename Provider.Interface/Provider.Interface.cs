using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Provider.Interface
{

        using Microsoft.ServiceFabric.Services.Remoting;

        public interface IProvider : IService
        {
            Task<bool> AddStocksAsync();
        }

}

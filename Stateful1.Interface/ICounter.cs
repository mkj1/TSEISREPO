using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stateful1.Interface
{
    using Microsoft.ServiceFabric.Services.Remoting;

    public interface ICounter : IService
    {
        Task<long> GetCountAsync();
    }
}

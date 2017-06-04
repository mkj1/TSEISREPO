using System.Threading.Tasks;

namespace Stateless1.Interface
{
    using Microsoft.ServiceFabric.Services.Remoting;
    using System.Collections.Generic;

    public interface IHello : IService
    {
        Task<List<Stock>> HelloWorldAsync(Stock stck);
    }
}

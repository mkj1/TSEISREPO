using System.Threading.Tasks;

namespace OwnerControl.Interface
{
    using Microsoft.ServiceFabric.Services.Remoting;
    using Models;

    public interface IAddStock : IService
    {
        Task<Stock> AddStockAsync();
    }
}

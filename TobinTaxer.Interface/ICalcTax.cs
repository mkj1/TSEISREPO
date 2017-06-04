
using System.Threading.Tasks;

namespace TobinTaxer.Interface
{
    using Microsoft.ServiceFabric.Services.Remoting;

    public interface ICalcTax : IService
    {
        Task<float> CalcTaxAsync(float amount);
    }
}

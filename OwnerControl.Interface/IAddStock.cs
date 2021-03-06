﻿using System.Threading.Tasks;

namespace OwnerControl.Interface
{
    using Microsoft.ServiceFabric.Services.Remoting;
    using Models;
    using System.Collections.Generic;

    public interface IAddStock : IService
    {
        Task<List<Stock>> AddStockAsync(); //returns all
        Task<List<Stock>> GetAllAsync(); //addstock

        Task<bool> UpdateStockAsync(Stock stck);
    }
}

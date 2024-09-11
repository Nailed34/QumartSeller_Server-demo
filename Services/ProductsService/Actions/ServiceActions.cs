/**
 * @QumartSeller_Client
 * https://github.com/Nailed34/QumartSeller_Server-demo.git
 *
 * Copyright (c) 2024 https://github.com/Nailed34
 * Released under the MIT license
 */

using ProductsServiceNamespace.DataBase;

namespace ProductsServiceNamespace.Actions
{
    public class ServiceActions
    {
        // Main service link
        private ProductsService Service { get; init; }
        internal ServiceActions(ProductsService service) => Service = service;

        /// <summary>
        /// Return number of products in DB or -1 if db error
        /// </summary>
        public async Task<int> GetProductsCount()
        {
            var count = await Service.DataBase.GetProductsCount();
            return count ?? -1;
        }

        /// <summary>
        /// Return products list by range
        /// </summary>
        public async Task<List<ProductOnion>> GetProductsInRange(int firstIndex, int lastIndex)
        {
            var onionsList = await Service.DataBase.GetProductsInRange(firstIndex, lastIndex);
            return onionsList ?? new List<ProductOnion>();
        }
    }
}

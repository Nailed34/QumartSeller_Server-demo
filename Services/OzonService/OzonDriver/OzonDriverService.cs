/**
 * @QumartSeller_Client
 * https://github.com/Nailed34/QumartSeller_Server-demo.git
 *
 * Copyright (c) 2024 https://github.com/Nailed34
 * Released under the MIT license
 */

using OzonServiceNamespace.OzonDriver.Actions;
using OzonServiceNamespace.OzonDriver.Tree;

namespace OzonServiceNamespace.OzonDriver
{
    /// <summary>
    /// Class contains actions for ozon API interaction
    /// </summary>
    internal sealed class OzonDriverService
    {
        // Main service link
        private OzonService Service { get; init; }
        public OzonDriverService(OzonService service) => Service = service;

        /// <summary>
        /// Get products offer_id, product_id and total cards by filter
        /// </summary>
        public async Task<OutOzonActionGetProductList> GetProductList(InOzonAction_filter? filter, string last_id = "", int limit = 1000)
        {
            var request = new InOzonActionGetProductList(Service, filter, last_id, limit);
            var request_result = await request.PostRequest();
            if (request_result == null)
                return new OutOzonActionGetProductList();
            else
                return new OutOzonActionGetProductList(request_result);
        }

        /// <summary>
        /// Get detailed info about product
        /// </summary>
        public async Task<OutOzonActionGetProductInfo> GetProductInfo(string offer_id = "", int product_id = 0, int sku = 0)
        {
            var request = new InOzonActionGetProductInfo(Service, offer_id, product_id, sku);
            var request_result = await request.PostRequest();
            if (request_result == null)
                return new OutOzonActionGetProductInfo();
            else
                return new OutOzonActionGetProductInfo(request_result);
        }

        /// <summary>
        /// Get detailed info about product list
        /// </summary>
        public async Task<OutOzonActionGetProductInfoList> GetProductInfoList(string[]? offer_id, int[]? product_id, int[]? sku)
        {
            var request = new InOzonActionGetProductInfoList(Service, offer_id, product_id, sku);
            var request_result = await request.PostRequest();
            if (request_result == null)
                return new OutOzonActionGetProductInfoList();
            else
                return new OutOzonActionGetProductInfoList(request_result);
        }

        /// <summary>
        /// Get detailed info about stocks by filter
        /// </summary>
        public async Task<OutOzonActionGetProductInfoStocks> GetProductInfoStocks(InOzonAction_filter? filter, string last_id = "", int limit = 1000)
        {
            var request = new InOzonActionGetProductInfoStocks(Service, filter, last_id, limit);
            var request_result = await request.PostRequest();
            if (request_result == null)
                return new OutOzonActionGetProductInfoStocks();
            else
                return new OutOzonActionGetProductInfoStocks(request_result);
        }
    }
}

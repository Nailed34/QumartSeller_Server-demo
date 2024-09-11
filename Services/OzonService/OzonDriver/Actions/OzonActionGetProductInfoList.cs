/**
 * @QumartSeller_Client
 * https://github.com/Nailed34/QumartSeller_Server-demo.git
 *
 * Copyright (c) 2024 https://github.com/Nailed34
 * Released under the MIT license
 */

using OzonServiceNamespace.DataBase;
using OzonServiceNamespace.OzonDriver.Tree;
using System.Net.Http.Json;

namespace OzonServiceNamespace.OzonDriver.Actions
{
    /// <summary>
    /// Use for create GetProductInfoList request
    /// </summary>
    public sealed class InOzonActionGetProductInfoList : InOzonActionBase
    {
        public InOzonActionGetProductInfoList(OzonService ozon_service, string[]? offer_id, int[]? product_id, int[]? sku) : base(ozon_service)
        {
            _requestLink = "https://api-seller.ozon.ru/v2/product/info/list";

            this.offer_id = offer_id;
            this.product_id = product_id;
            this.sku = sku;

            _content = JsonContent.Create(new { offer_id, product_id, sku });
        }


        /// <summary>
        /// Идентификатор товара в системе продавца — артикул.
        /// </summary>
        string[]? offer_id { get; set; }

        /// <summary>
        /// Идентификатор товара.
        /// </summary>
        int[]? product_id { get; set; }

        /// <summary>
        /// Идентификатор товара в системе Ozon — SKU.
        /// </summary>
        int[]? sku { get; set; }
    }

    /// <summary>
    /// Use for create GetProductInfoList response
    /// </summary>
    public sealed class OutOzonActionGetProductInfoList : OutOzonActionBase<OutOzonActionGetProductInfoList_200>
    {
        public OutOzonActionGetProductInfoList() : base() { }
        public OutOzonActionGetProductInfoList(HttpResponseMessage http_response_message) : base(http_response_message) { }
    }

    public sealed class OutOzonActionGetProductInfoList_200
    {
        public OutOzonActionGetProductInfoList_result result { get; set; } = new();
    }

    public sealed class OutOzonActionGetProductInfoList_result
    {
        // Array of product_info request object
        public OutOzonActionGetProductInfo_result[] items { get; set; } = Array.Empty<OutOzonActionGetProductInfo_result>();

        /// <summary>
        /// Convert this response to ProductCard list presentation
        /// </summary>
        public List<OzonCard> ToProductCards()
        {
            try
            {
                return (from p in items select p.ToOzonCard()).ToList();
            }
            catch
            {
                throw new Exception();
            }
        }
    }
}

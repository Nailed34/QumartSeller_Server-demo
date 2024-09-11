/**
 * @QumartSeller_Client
 * https://github.com/Nailed34/QumartSeller_Server-demo.git
 *
 * Copyright (c) 2024 https://github.com/Nailed34
 * Released under the MIT license
 */

using System.Net.Http.Json;
using OzonServiceNamespace.OzonDriver.Tree;

namespace OzonServiceNamespace.OzonDriver.Actions
{
    /// <summary>
    /// Use for create GetProductList request
    /// </summary>
    public sealed class InOzonActionGetProductList : InOzonActionBase
    {
        public InOzonActionGetProductList(OzonService ozon_service, InOzonAction_filter? filter, string last_id = "", int limit = 1000) : base(ozon_service)
        {
            _requestLink = "https://api-seller.ozon.ru/v2/product/list";

            this.filter = filter;
            this.last_id = last_id;
            this.limit = limit;

            if (filter != null)
                _content = JsonContent.Create(new { filter = filter.GetObject(), last_id, limit });
            else
                _content = JsonContent.Create(new { last_id, limit });
        }

        /// <summary>
        /// Фильтр по товарам
        /// </summary>
        InOzonAction_filter? filter { get; set; }

        /// <summary>
        /// Идентификатор последнего значения на странице. Оставьте это поле пустым при выполнении первого запроса
        /// </summary>
        string last_id { get; set; } = "";

        /// <summary>
        /// Количество значений на странице. Минимум — 1, максимум — 1000
        /// </summary>
        int limit { get; set; } = 0;
    }

    /// <summary>
    /// Use for create GetProductList response
    /// </summary>
    public sealed class OutOzonActionGetProductList : OutOzonActionBase<OutOzonActionGetProductList_200>
    {
        public OutOzonActionGetProductList() : base() { }
        public OutOzonActionGetProductList(HttpResponseMessage http_response_message) : base(http_response_message) { }
    }

    public sealed class OutOzonActionGetProductList_200
    {
        public OutOzonActionGetProductList_result result { get; set; } = new();
    }

    public sealed class OutOzonActionGetProductList_result
    {
        /// <summary>
        /// Идентификатор последнего значения на странице, подставить его в следующем запросе для получения следующих значений
        /// </summary>
        public string last_id { get; set; } = "";
        /// <summary>
        /// Количество уникальных товаров, для которых выводится информация об остатках
        /// </summary>
        public int total { get; set; } = 0;
        /// <summary>
        /// Список товаров
        /// </summary>
        public OutOzonActionGetProductList_items[] items { get; set; } = Array.Empty<OutOzonActionGetProductList_items>();
    }
    // Level 3
    public sealed class OutOzonActionGetProductList_items
    {
        /// <summary>
        /// Идентификатор товара в системе продавца - артикул
        /// </summary>
        public string offer_id { get; set; } = "";
        /// <summary>
        /// Идентификатор товара
        /// </summary>
        public int product_id { get; set; } = 0;
    }
}

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
    /// Use for create GetProductInfoStocks request
    /// </summary>
    public sealed class InOzonActionGetProductInfoStocks : InOzonActionBase
    {
        public InOzonActionGetProductInfoStocks(OzonService ozon_service, InOzonAction_filter? filter, string last_id = "", int limit = 1000) : base(ozon_service)
        {
            _requestLink = "https://api-seller.ozon.ru/v3/product/info/stocks";

            this.filter = filter;
            this.last_id = last_id;
            this.limit = limit;

            if (filter == null)
            {
                filter = new InOzonAction_filter(Array.Empty<string>(), Array.Empty<string>(), InOzonAction_visibility.VISIBLE);
            }
            this.filter = filter;

            _content = JsonContent.Create(new { filter = filter.GetObject(), last_id, limit });
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
    /// Use for create GetProductInfoStocks response
    /// </summary>
    public sealed class OutOzonActionGetProductInfoStocks : OutOzonActionBase<OutOzonActionGetProductInfoStocks_200>
    {
        public OutOzonActionGetProductInfoStocks() : base() { }
        public OutOzonActionGetProductInfoStocks(HttpResponseMessage http_response_message) : base(http_response_message) { }
    }

    public sealed class OutOzonActionGetProductInfoStocks_200
    {
        public OutOzonActionGetProductInfoStocks_result result { get; set; } = new();
    }

    public sealed class OutOzonActionGetProductInfoStocks_result
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
        public OutOzonActionGetProductInfoStocks_items[] items { get; set; } = Array.Empty<OutOzonActionGetProductInfoStocks_items>();
    }

    public sealed class OutOzonActionGetProductInfoStocks_items
    {
        /// <summary>
        /// Идентификатор товара в системе продавца - артикул
        /// </summary>
        public string offer_id { get; set; } = "";
        /// <summary>
        /// Идентификатор товара
        /// </summary>
        public int product_id { get; set; } = 0;
        /// <summary>
        /// Информация об остатках
        /// </summary>
        public OutOzonActionGetProductInfoStocks_items_stocks[] stocks { get; set; } = Array.Empty<OutOzonActionGetProductInfoStocks_items_stocks>();
    }

    public sealed class OutOzonActionGetProductInfoStocks_items_stocks
    {
        /// <summary>
        /// Сейчас на складе
        /// </summary>
        public int present { get; set; } = 0;
        /// <summary>
        /// Зарезервировано
        /// </summary>
        public int reserved { get; set; } = 0;
        /// <summary>
        /// Тип склада
        /// </summary>
        public string type { get; set; } = "";
    }
}

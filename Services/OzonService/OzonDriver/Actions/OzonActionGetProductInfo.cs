/**
 * @QumartSeller_Client
 * https://github.com/Nailed34/QumartSeller_Server-demo.git
 *
 * Copyright (c) 2024 https://github.com/Nailed34
 * Released under the MIT license
 */

using OzonServiceNamespace.DataBase;
using OzonServiceNamespace.OzonDriver.Tree;
using ServicesDomain.Enums;
using System.Net.Http.Json;

namespace OzonServiceNamespace.OzonDriver.Actions
{
    /// <summary>
    /// Use for create GetProductInfo request
    /// </summary>
    public sealed class InOzonActionGetProductInfo : InOzonActionBase
    {
        public InOzonActionGetProductInfo(OzonService ozon_service, string offer_id = "", int product_id = 0, int sku = 0) : base(ozon_service)
        {
            _requestLink = "https://api-seller.ozon.ru/v2/product/info";

            this.offer_id = offer_id;
            this.product_id = product_id;
            this.sku = sku;

            _content = JsonContent.Create(new { offer_id, product_id, sku });
        }

        /// <summary>
        /// Идентификатор товара в системе продавца — артикул.
        /// </summary>
        string offer_id { get; set; } = "";

        /// <summary>
        /// Идентификатор товара.
        /// </summary>
        int product_id { get; set; } = 0;

        /// <summary>
        /// Идентификатор товара в системе Ozon — SKU.
        /// </summary>
        int sku { get; set; } = 0;
    }

    /// <summary>
    /// Use for create GetProductInfo response
    /// </summary>
    public sealed class OutOzonActionGetProductInfo : OutOzonActionBase<OutOzonActionGetProductInfo_200>
    {
        public OutOzonActionGetProductInfo() : base() { }
        public OutOzonActionGetProductInfo(HttpResponseMessage http_response_message) : base(http_response_message) { }
    }

    public sealed class OutOzonActionGetProductInfo_200
    {
        public OutOzonActionGetProductInfo_result result { get; set; } = new();
    }

    public sealed class OutOzonActionGetProductInfo_result
    {
        /// <summary>
        /// Идентификатор товара (product_id)
        /// </summary>
        public int id { get; set; } = 0;
        /// <summary>
        /// Название товара
        /// </summary>
        public string name { get; set; } = "";
        /// <summary>
        /// Идентификатор товара в системе продавца - артикул
        /// </summary>
        public string offer_id { get; set; } = "";
        /// <summary>
        /// Штрих код
        /// </summary>
        public string barcode { get; set; } = "";
        /// <summary>
        /// Дата создания карточки
        /// </summary>
        public DateTime created_at { get; set; } = new DateTime();
        /// <summary>
        /// Информация об остатках
        /// </summary>
        public OutOzonActionGetProductInfo_stocks stocks { get; set; } = new();
        /// <summary>
        /// Главное фото товара
        /// </summary>
        public string primary_image { get; set; } = "";
        /// <summary>
        /// Информация о статусах товара
        /// </summary>
        public OutOzonActionGetProductInfo_status status { get; set; } = new();
        /// <summary>
        /// Уцененный ли товар
        /// </summary>
        public bool is_discounted { get; set; } = false;
        /// <summary>
        /// Массив штрихкодов
        /// </summary>
        public string[]? barcodes { get; set; }

        /// <summary>
        /// Convert this response to OzonCard presentation
        /// </summary>
        public OzonCard ToOzonCard()
        {
            try
            {
                OzonCard productCard = new()
                {
                    articul = offer_id,
                    marketplace_articul = id.ToString(),
                    name = name,
                    creation_date = created_at,
                    stocks = stocks.present,
                    photo = primary_image,
                    marketplace = EMarketplaces.Ozon
                };
                productCard.barcodes = new();
                if (barcode != "")
                    productCard.barcodes.Add(barcode);
                if (barcodes != null && barcodes.Length > 0)
                {
                    foreach (var barcode in barcodes)
                        if (!productCard.barcodes.Contains(barcode))
                            productCard.barcodes.Add(barcode);
                }

                return productCard;
            }
            catch
            {
                throw new Exception();
            }
        }
    }

    public sealed class OutOzonActionGetProductInfo_stocks
    {
        /// <summary>
        /// Ожидаемая поставка
        /// </summary>
        public int coming { get; set; } = 0;
        /// <summary>
        /// Сейчас на складе
        /// </summary>
        public int present { get; set; } = 0;
        /// <summary>
        /// Зарезервировано
        /// </summary>
        public int reserved { get; set; } = 0;
    }

    public sealed class OutOzonActionGetProductInfo_status
    {
        /// <summary>
        /// Состояние товара
        /// </summary>
        public string state { get; set; } = "";
        /// <summary>
        /// Состояние товара, на переходе в которое произошла ошибка
        /// </summary>
        public string state_failed { get; set; } = "";
        /// <summary>
        /// Статус модерации
        /// </summary>
        public string moderate_status { get; set; } = "";
        /// <summary>
        /// Статус валидации
        /// </summary>
        public string validation_state { get; set; } = "";
        /// <summary>
        /// Название состояния товара
        /// </summary>
        public string state_name { get; set; } = "";
        /// <summary>
        /// Описание состояния товара
        /// </summary>
        public string state_description { get; set; } = "";
        /// <summary>
        /// Признак, что при создании товара возникли ошибки
        /// </summary>
        public bool is_failed { get; set; } = true;
        /// <summary>
        /// Признак, что товар создан
        /// </summary>
        public bool is_created { get; set; } = false;
        /// <summary>
        /// Подсказки для текущего состояния товара
        /// </summary>
        public string state_tooltip { get; set; } = "";
    }
}

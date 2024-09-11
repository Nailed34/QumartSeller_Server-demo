/**
 * @QumartSeller_Client
 * https://github.com/Nailed34/QumartSeller_Server-demo.git
 *
 * Copyright (c) 2024 https://github.com/Nailed34
 * Released under the MIT license
 */

using System.Net.Http.Json;

namespace OzonServiceNamespace.OzonDriver.Tree
{
    /// <summary>
    /// Base class for all input ozon actions (requests to marketplace)
    /// </summary>
    public abstract class InOzonActionBase : InMarketplaceActionBase
    {
        // Main service link
        OzonService OzonService { get; set; }

        public InOzonActionBase(OzonService ozon_service)
        {
            OzonService = ozon_service;
        }

        public override async Task<HttpResponseMessage?> PostRequest()
        {
            if (Content != null)
                return await OzonService.AddMarketplaceRequest(RequestLink, Content);
            return null;
        }
    }

    /// <summary>
    /// Filter ozon presentation
    /// </summary>
    public class InOzonAction_filter
    {
        public InOzonAction_filter(string[] offer_id, string[] product_id, InOzonAction_visibility visibility = InOzonAction_visibility.ALL)
        {
            this.offer_id = offer_id;
            this.product_id = product_id;
            this.visibility = visibility;
        }

        public object GetObject()
        {
            return new { offer_id, product_id, visibility = visibility.ToString() };
        }

        // Фильтр по параметру offer_id. Можно передавать список значений

        string[] offer_id;
        // Фильтр по параметру product_id. Можно передавать список значений
        string[] product_id;
        // Фильтр по видимости товара
        InOzonAction_visibility visibility = InOzonAction_visibility.ALL;
    }

    /// <summary>
    /// Visibility enum ozon presentation (used by filter)
    /// </summary>
    public enum InOzonAction_visibility
    {
        ALL, VISIBLE, INVISIBLE, EMPTY_STOCK, NOT_MODERATED, MODERATED, DISABLED, STATE_FAILED,
        READY_TO_SUPPLY, VALIDATION_STATE_PENDING, VALIDATION_STATE_FAIL, VALIDATION_STATE_SUCCESS,
        TO_SUPPLY, IN_SALE, REMOVED_FROM_SALE, BANNED, OVERPRICED, CRITICALLY_OVERPRICED, EMPTY_BARCODE,
        BARCODE_EXISTS, QUARANTINE, ARCHIVED, OVERPRICED_WITH_STOCK, PARTIAL_APPROVED, IMAGE_ABSENT,
        MODERATION_BLOCK
    }

    /// <summary>
    /// Base class for all output ozon actions (response from marketplace). Specify result class for deserialization
    /// </summary>
    public abstract class OutOzonActionBase<TSuccessResult> : OutMarketplaceActionBase where TSuccessResult : new()
    {
        /// <summary>
        /// Success result of request
        /// </summary>
        public TSuccessResult SuccessResult { get => _successResult; }
        private TSuccessResult _successResult = new();

        /// <summary>
        /// Error result of request
        /// </summary>
        public OutOzonActionError OzonErrorMessage { get => _ozonErrorMessage; }
        protected OutOzonActionError _ozonErrorMessage = new();

        public OutOzonActionBase(HttpResponseMessage httpResponseMessage) : base(httpResponseMessage) { }
        public OutOzonActionBase() { }

        protected override bool DeserializeSuccessMessage()
        {
            if (HttpResponseMessage == null)
                return false;

            var json_decode = HttpResponseMessage.Content.ReadFromJsonAsync<TSuccessResult>(JsonSerializerOptions);

            if (json_decode.Result != null)
            {
                _successResult = json_decode.Result;
                return true;
            }

            return false;
        }

        protected override bool DeserializeErrorMessage()
        {
            if (HttpResponseMessage == null)
                return false;

            var json_decode = HttpResponseMessage.Content.ReadFromJsonAsync<OutOzonActionError>(JsonSerializerOptions);

            if (json_decode.Result != null)
            {
                _ozonErrorMessage = json_decode.Result;
                return true;
            }

            return false;
        }
    }

    /// <summary>
    /// Default error result of ozon request presentation
    /// </summary>
    public class OutOzonActionError
    {
        // Код ошибки
        public int code { get; set; } = 0;
        // Описание ошибки
        public string message { get; set; } = "";
        // Дополнительная информация об ошибке
        public OutOzonActionErrorDetails[]? details { get; set; }
    }

    /// <summary>
    /// Ozon error details
    /// </summary>
    public class OutOzonActionErrorDetails
    {
        // Тип протокола передачи данных
        public string typeUrl { get; set; } = "";
        // Значение ошибки
        public string value { get; set; } = "";
    }
}

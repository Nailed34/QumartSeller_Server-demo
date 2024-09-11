/**
 * @QumartSeller_Client
 * https://github.com/Nailed34/QumartSeller_Server-demo.git
 *
 * Copyright (c) 2024 https://github.com/Nailed34
 * Released under the MIT license
 */

using System.Text.Json;
using System.Text.Json.Serialization;

namespace OzonServiceNamespace.OzonDriver.Tree
{
    /// <summary>
    /// Base class for all marketplace input actions (requests to marketplace)
    /// </summary>
    public abstract class InMarketplaceActionBase
    {
        /// <summary>
        /// Final content for sending
        /// </summary>
        public HttpContent? Content { get => _content; }
        protected HttpContent? _content;

        /// <summary>
        /// Link for request
        /// </summary>
        public string RequestLink { get => _requestLink; }
        protected string _requestLink = "";

        /// <summary>
        /// Do post request. Use this method after init link and content for sending
        /// </summary>
        public abstract Task<HttpResponseMessage?> PostRequest();
    }

    /// <summary>
    /// Base class for all marketplace output actions (response from marketplace)
    /// </summary>
    public abstract class OutMarketplaceActionBase
    {
        /// <summary>
        /// Settings for success deserialization response message
        /// </summary>
        public static JsonSerializerOptions JsonSerializerOptions { get => _jsonSerializerOptions; }
        protected static JsonSerializerOptions _jsonSerializerOptions;

        // Setup json options
        static OutMarketplaceActionBase()
        {
            _jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerOptions.Default);
            _jsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
            _jsonSerializerOptions.AllowTrailingCommas = true;
            _jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        }

        /// <summary>
        /// Response message for deserialize it
        /// </summary>
        public HttpResponseMessage? HttpResponseMessage { get => _httpResponseMessage; }
        protected HttpResponseMessage? _httpResponseMessage;

        /// <summary>
        /// Status of response message
        /// </summary>
        public EMarketplaceActionStatus ActionStatus { get => _actionStatus; }
        protected EMarketplaceActionStatus _actionStatus = EMarketplaceActionStatus.MarketplaceError;

        /// <summary>
        /// Create class with response message and determine status of request
        /// </summary>
        public OutMarketplaceActionBase(HttpResponseMessage http_response_message) : this()
        {
            _httpResponseMessage = http_response_message;

            if (_httpResponseMessage.IsSuccessStatusCode)
                _actionStatus = DeserializeSuccessMessage() ? EMarketplaceActionStatus.SuccessResult : EMarketplaceActionStatus.SerializationError;
            else
                _actionStatus = DeserializeErrorMessage() ? EMarketplaceActionStatus.ErrorResult : EMarketplaceActionStatus.SerializationError;
        }

        /// <summary>
        /// Create class without response message, has marketplace error status by default
        /// </summary>
        public OutMarketplaceActionBase() { }

        /// <summary>
        /// Method for deserialize response message with success status code
        /// </summary>
        protected abstract bool DeserializeSuccessMessage();

        /// <summary>
        /// Method for deserialize response message with error status code
        /// </summary>
        protected abstract bool DeserializeErrorMessage();
    }

    /// <summary>
    /// Types of action results after deserialization
    /// </summary>
    public enum EMarketplaceActionStatus
    {
        SuccessResult, ErrorResult, SerializationError, MarketplaceError
    }
}

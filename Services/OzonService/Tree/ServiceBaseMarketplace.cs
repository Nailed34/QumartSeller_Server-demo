/**
 * @QumartSeller_Client
 * https://github.com/Nailed34/QumartSeller_Server-demo.git
 *
 * Copyright (c) 2024 https://github.com/Nailed34
 * Released under the MIT license
 */

namespace OzonServiceNamespace.Tree
{
    /// <summary>
    /// Base class with http client for requests
    /// </summary>
    public abstract class ServiceBaseMarketplace : ServiceBaseQueue
    {
        /// <summary>
        /// Client for send requests to marketplace
        /// </summary>
        protected static HttpClient HttpClient { get => _httpClient; }
        private static HttpClient _httpClient = new();

        public ServiceBaseMarketplace(ServiceQueueSettings queueSettings) : base(queueSettings) { }

        /// <summary>
        /// Use this method when request must be safe by time
        /// </summary>
        internal async Task<HttpResponseMessage> AddMarketplaceRequest(string Uri, HttpContent Content)
        {
            TaskCompletionSource newRequest = new TaskCompletionSource();
            AddNewRequest(newRequest);
            await newRequest.Task;
            return await HttpClient.PostAsync(Uri, Content);
        }
    }
}

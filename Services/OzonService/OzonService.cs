/**
 * @QumartSeller_Client
 * https://github.com/Nailed34/QumartSeller_Server-demo.git
 *
 * Copyright (c) 2024 https://github.com/Nailed34
 * Released under the MIT license
 */

using OzonServiceNamespace.Actions;
using OzonServiceNamespace.DataBase;
using OzonServiceNamespace.OzonDriver;
using OzonServiceNamespace.Events;
using OzonServiceNamespace.Tree;

namespace OzonServiceNamespace
{
    /// <summary>
    /// Settings for create ozon service. Contains marketplace data for requests
    /// </summary>
    public struct OzonServiceSettings
    {
        public string ClientId { get; set; }
        public string ApiKey { get; set; }
    }

    /// <summary>
    /// Service for ozon
    /// </summary>
    public sealed class OzonService : ServiceBaseMarketplace
    {
        internal DataBaseService DataBase { get; init; }
        internal OzonDriverService OzonDriverService { get; init; }
        public ServiceActions Actions { get; init; }
        public ServiceEvents Events { get; init; }

        public OzonService(string dataBaseUrl, ServiceQueueSettings queueSettings, OzonServiceSettings ozonSettings) : base (queueSettings)
        {


            HttpClient.DefaultRequestHeaders.Add("Client-Id", ozonSettings.ClientId);
            HttpClient.DefaultRequestHeaders.Add("Api-Key", ozonSettings.ApiKey);

            DataBase = new(dataBaseUrl, "ozon", "ozon");
            OzonDriverService = new(this);
            Actions = new(this);
            Events = new(this);
        }
    }
}

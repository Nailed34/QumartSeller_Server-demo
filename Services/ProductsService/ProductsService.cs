/**
 * @QumartSeller_Client
 * https://github.com/Nailed34/QumartSeller_Server-demo.git
 *
 * Copyright (c) 2024 https://github.com/Nailed34
 * Released under the MIT license
 */

using ProductsServiceNamespace.Actions;
using ProductsServiceNamespace.DataBase;
using ProductsServiceNamespace.Events;
using ServicesDomain;
using ServicesDomain.Enums;

namespace ProductsServiceNamespace
{
    /// <summary>
    /// Service for onions
    /// </summary>
    public sealed class ProductsService : ServiceBase
    {
        /// <summary>
        /// Actions for users from server
        /// </summary>
        public ServiceActions Actions { get; init; }

        public ServiceEvents Events { get; init; }     

        /// <summary>
        /// Which marketplace use for onion name and photo
        /// </summary>
        internal EMarketplaces MarketplacePriority { get; init; }

        internal DataBaseService DataBase { get; init; }

        public ProductsService(string dataBaseUrl, EMarketplaces marketplacePriority) : base()
        {
            DataBase = new(dataBaseUrl, "products", "products");
            MarketplacePriority = marketplacePriority;
            Actions = new(this);
            Events = new(this);        
        }
    }
}

/**
 * @QumartSeller_Client
 * https://github.com/Nailed34/QumartSeller_Server-demo.git
 *
 * Copyright (c) 2024 https://github.com/Nailed34
 * Released under the MIT license
 */

using ASPNetServer.ServerBus.ServerActions;
using ASPNetServer.ServerSettingsNamespace;
using DemoServiceNamespace;
using OzonServiceNamespace;
using OzonServiceNamespace.Tree;
using ProductsServiceNamespace;
using ServicesDomain.Enums;
using UsersServiceNamespace;

namespace ASPNetServer.ServerBus
{
    /// <summary>
    /// Main class for services interacting
    /// </summary>
    public class MainServerBus
    {
        // Created on IsDemo setting. This service will be used instead other services on true
        internal DemoService? DemoService { get; init; }

        // Services
        internal UsersService UsersService { get; init; }
        internal OzonService OzonService { get; init; }
        internal ProductsService ProductsService { get; init; }

        // Actions
        internal UsersActions UsersActions { get; init; }
        internal OzonActions OzonActions { get; init; }
        internal ProductsActions ProductsActions { get; init; }

        // Services binding
        internal ServiceBinding ServiceBinding { get; init; }

        public MainServerBus()
        {
            // Create demo service
            if (ServerSettings.Options.IsDemo)
                DemoService = new();

            // Create services
            UsersService = new(ServerSettings.Options.DataBaseUrl);
            OzonService = new(ServerSettings.Options.DataBaseUrl, new ServiceQueueSettings(), new OzonServiceSettings
            {
                ClientId = ServerSettings.Options.OzonClientId,
                ApiKey = ServerSettings.Options.OzonApiKey
            });
            
            ProductsService = new(ServerSettings.Options.DataBaseUrl, EMarketplaces.Ozon);

            // Create actions
            UsersActions = new(this);
            OzonActions = new(this);
            ProductsActions = new(this);

            // Create binding class
            ServiceBinding = new(this);
        }
    }
}

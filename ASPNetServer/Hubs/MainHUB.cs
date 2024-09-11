/**
 * @QumartSeller_Client
 * https://github.com/Nailed34/QumartSeller_Server-demo.git
 *
 * Copyright (c) 2024 https://github.com/Nailed34
 * Released under the MIT license
 */

using ClientServerConnection;
using ClientServerConnection.Actions;
using ASPNetServer.ServerBus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using ASPNetServer.ServerSettingsNamespace;

namespace ASPNetServer.Hubs
{
    [Authorize]
    internal class MainHUB : Hub
    {
        private MainServerBus MainServerBus;
        public MainHUB(MainServerBus mainServerBus) => MainServerBus = mainServerBus;

        public async Task InGetProductsCount()
        {
            if (!ServerSettings.Options.IsDemo)
            {
                int count = await MainServerBus.ProductsActions.GetProductsCount();
                await Clients.Caller.SendAsync("OutGetProductsCount", count);
            }
            else
            {
#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
                await Clients.Caller.SendAsync("OutGetProductsCount", MainServerBus.DemoService.GetProductsCount());
#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.
            }
        }
        public async Task InGetProductsInfo(InGetProductsInfo options)
        {
            if (!ServerSettings.Options.IsDemo)
            {
                var products = await MainServerBus.ProductsActions.GetProductsInfo(options.FirstIndex, options.LastIndex);
                var productsCount = await MainServerBus.ProductsActions.GetProductsCount();
                await Clients.Caller.SendAsync("OutGetProductsInfo", new OutGetProductsInfo()
                {
                    ProductsInfo = ProductInfo.SerializeProductInfo(products),
                    ProductsCount = productsCount
                });
            }
            else
            {
#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
                await Clients.Caller.SendAsync("OutGetProductsInfo", new OutGetProductsInfo()
                {
                    ProductsInfo = ProductInfo.SerializeProductInfo(MainServerBus.DemoService.GetProductsInfo(options.FirstIndex, options.LastIndex)),
                    ProductsCount = MainServerBus.DemoService.GetProductsCount()
                });
#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.
            }
        }
    }
}

/**
 * @QumartSeller_Client
 * https://github.com/Nailed34/QumartSeller_Server-demo.git
 *
 * Copyright (c) 2024 https://github.com/Nailed34
 * Released under the MIT license
 */

using ServicesDomain.Database.Data;

namespace ASPNetServer.ServerBus
{
    /// <summary>
    /// This class contains services events realizations
    /// </summary>
    internal class ServiceBinding
    {
        // Main bus link
        private MainServerBus MainServerBus { get; init; }

        public ServiceBinding(MainServerBus mainServerBus)
        {
            MainServerBus = mainServerBus;

            //MainServerBus.OzonService.Events.NewCardsEvent += OnOzonServiceNewCards;
            //MainServerBus.OzonService.Events.RemoveCardsEvent += OnOzonServiceRemoveCards;
            //MainServerBus.ProductsService.Events.GetOzonProductCardsEvent += OnProductsServiceGetOzonProductCards;
        }

        // Bind ozon -> products: NewCardsEvent
        //private async Task<Dictionary<string, string>> OnOzonServiceNewCards(List<ProductCard> newProductCards)
        //{
        //    return await MainServerBus.ProductsService.Events.OnAddNewProductCards(newProductCards);
        //}

        // Bind ozon -> products: RemoveCardsEvent
        //private async Task OnOzonServiceRemoveCards(List<ProductCard> removedProductCards)
        //{
        //    await MainServerBus.ProductsService.Events.OnRemoveProductCards(removedProductCards);
        //}

        // Bind products -> ozon: OzonProductCardsGetRequestEvent
        //private async Task<List<ProductCard>> OnProductsServiceGetOzonProductCards(string field, string value)
        //{
        //    return await MainServerBus.OzonService.Events.OnGetOzonProductCards(field, value);
        //}
    }
}

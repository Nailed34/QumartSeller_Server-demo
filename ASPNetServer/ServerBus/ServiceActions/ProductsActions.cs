/**
 * @QumartSeller_Client
 * https://github.com/Nailed34/QumartSeller_Server-demo.git
 *
 * Copyright (c) 2024 https://github.com/Nailed34
 * Released under the MIT license
 */

using ClientServerConnection;
using ProductsServiceNamespace.DataBase;
using ServicesDomain.Database.Data;

namespace ASPNetServer.ServerBus.ServerActions
{
    /// <summary>
    /// Class with products service actions realization
    /// </summary>
    internal class ProductsActions
    {
        // Service link
        private MainServerBus MainServerBus { get; init; }
        public ProductsActions(MainServerBus mainServerBus) => MainServerBus = mainServerBus;

        // Convert data base presentation to domain
        private EMarketplaces ConvertEMarketplaces(ServicesDomain.Enums.EMarketplaces marketplace)
        {
            switch (marketplace)
            {
                case ServicesDomain.Enums.EMarketplaces.Ozon:
                    return EMarketplaces.Ozon;
                case ServicesDomain.Enums.EMarketplaces.Wildberries:
                    return EMarketplaces.Wildberries;
                case ServicesDomain.Enums.EMarketplaces.YandexMarket:
                    return EMarketplaces.YandexMarket;
                default:
                    return EMarketplaces.None;
            }
        }

        // Convert ProductCard to ProductInfoCard
        private ProductInfoCard FromProductCardToInfo(ProductCard productCard)
        {
            var result = new ProductInfoCard()
            {
                articul = productCard.articul,
                name = productCard.name,
                stocks = productCard.stocks,
                creation_date = productCard.creation_date,
                is_synch = productCard.is_synch,
                multiplicity = productCard.multiplicity,
                marketplace = ConvertEMarketplaces(productCard.marketplace),
                barcodes = productCard.barcodes
            };
            return result;
        }

        // Convert ProductOnion to ProductInfo
        private ProductInfo FromProductOnionToInfo(ProductOnion productOnion, List<ProductCard> productCards)
        {
            var result = new ProductInfo()
            {
                id = productOnion._id,
                name = productOnion.name,
                photo = productOnion.photo,
                stocks = productOnion.stocks,
                articuls = productOnion.articuls,
                barcodes = productOnion.barcodes,
                marketplaces = productOnion.marketplaces.Select(ConvertEMarketplaces).ToList(),
                cards = productCards.Select(FromProductCardToInfo).ToList()
            };
            return result;
        }

        public async Task<List<ProductInfo>> GetProductsInfo(int firstIndex, int lastIndex)
        {
            // Get onions from db
            var onions = await MainServerBus.ProductsService.Actions.GetProductsInRange(firstIndex, lastIndex);

            var result = new List<ProductInfo>();

            // Convert onions to product info
            foreach (var onion in onions)
            {
                // Get attached cards
                var productCards = new List<ProductCard>();
                foreach (var card in onion.cards)
                {
                    // Get full info about card by marketplace
                    switch (card.marketplace)
                    {
                        case ServicesDomain.Enums.EMarketplaces.Ozon:
                            productCards.Add(await MainServerBus.OzonService.Actions.GetOzonCard("_id", card._id));
                            break;
                    }
                }
                // Add converted onion to result list
                result.Add(FromProductOnionToInfo(onion, productCards));
            }

            return result;
        }

        public async Task<int> GetProductsCount()
        {
            return await MainServerBus.ProductsService.Actions.GetProductsCount();
        }
    }
}

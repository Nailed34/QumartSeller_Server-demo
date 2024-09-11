/**
 * @QumartSeller_Client
 * https://github.com/Nailed34/QumartSeller_Server-demo.git
 *
 * Copyright (c) 2024 https://github.com/Nailed34
 * Released under the MIT license
 */

using ServicesDomain.Database.Data;

namespace ProductsServiceNamespace.Events
{
    public class ServiceEvents
    {
        private ProductsService Service { get; init; }
        //private NewProductCardsEvent NewProductCardsEvent { get; init; }
        //private RemovedProductCardsEvent RemovedProductCardsEvent { get; init; }
        internal ServiceEvents(ProductsService service)
        {
            Service = service;
            //NewProductCardsEvent = new NewProductCardsEvent(service);
            //RemovedProductCardsEvent = new RemovedProductCardsEvent(service);
        }

        /// <summary>
        /// Create new onions or unite created with new product cards. Return dictionary with onion _id and card _id
        /// </summary>
        //public async Task<Dictionary<string, string>> OnAddNewProductCards(List<ProductCard> newProductCards)
        //{
        //    return await NewProductCardsEvent.OnAddNewProductCards(newProductCards);
        //}

        /// <summary>
        /// Remove cards from onions or remove onions if it has only this card
        /// </summary>
        //public async Task OnRemoveProductCards(List<ProductCard> removedProductCards)
        //{
        //    await RemovedProductCardsEvent.OnRemoveProductCards(removedProductCards);
        //}

        public event Func<string, string, Task<List<ProductCard>>>? GetOzonProductCardsEvent;

        internal async Task<List<ProductCard>?> CallGetOzonProductCards(string field, string value)
        {
            return GetOzonProductCardsEvent != null ? await GetOzonProductCardsEvent.Invoke(field, value) : null;
        }
    }
}

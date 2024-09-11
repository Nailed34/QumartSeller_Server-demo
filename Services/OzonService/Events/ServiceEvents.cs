/**
 * @QumartSeller_Client
 * https://github.com/Nailed34/QumartSeller_Server-demo.git
 *
 * Copyright (c) 2024 https://github.com/Nailed34
 * Released under the MIT license
 */

//using OzonServiceNamespace.Actions.Tree;
using OzonServiceNamespace.DataBase;
using ServicesDomain.Database.Data;

namespace OzonServiceNamespace.Events
{
    public class ServiceEvents
    {
        private OzonService Service { get; init; }
        internal ServiceEvents(OzonService service) => Service = service;

        public async Task<List<ProductCard>> OnGetOzonProductCards(string field, string value)
        {
            var db_result = await Service.DataBase.GetOzonCards(field, value);
            return db_result != null ? db_result.ToProductCardList() : new();
        }

        public event Func<List<ProductCard>, Task<Dictionary<string, string>>>? NewCardsEvent;
        public event Func<List<ProductCard>, Task>? RemoveCardsEvent;
        public event Action<int>? ImportProductsStartedEvent;
        //public event Action<EImportProductsEndResult>? ImportProductsEndedEvent;
        public event Action<int, int>? ImportProductsUpdatedEvent;
        public event Action<int>? ImportStocksStartedEvent;
        //public event Action<EImportStocksEndResult>? ImportStocksEndedEvent;
        public event Action<int, int>? ImportStocksUpdatedEvent;

        internal async Task<Dictionary<string, string>> CallNewCardsEvent(List<ProductCard> newCards)
        {
            return NewCardsEvent != null ? await NewCardsEvent.Invoke(newCards) : new Dictionary<string, string>();
        }

        internal async Task CallRemoveCardsEvent(List<ProductCard> removedCards)
        {
            if (RemoveCardsEvent != null) await RemoveCardsEvent.Invoke(removedCards);
        }

        internal void CallImportProductsStartedEvent(int total)
        {
            ImportProductsStartedEvent?.Invoke(total);
        }

        //internal void CallImportProductsEndedEvent(EImportProductsEndResult endResult)
        //{
        //    ImportProductsEndedEvent?.Invoke(endResult);
        //}

        internal void CallImportProductsUpdatedEvent(int imported, int total)
        {
            ImportProductsUpdatedEvent?.Invoke(imported, total);
        }

        internal void CallImportStocksStartedEvent(int total)
        {
            ImportStocksStartedEvent?.Invoke(total);
        }

        //internal void CallImportStocksEndedEvent(EImportStocksEndResult endResult)
        //{
        //    ImportStocksEndedEvent?.Invoke(endResult);
        //}

        internal void CallImportStocksUpdatedEvent(int imported, int total)
        {
            ImportStocksUpdatedEvent?.Invoke(imported, total);
        }
    }
}

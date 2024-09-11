/**
 * @QumartSeller_Client
 * https://github.com/Nailed34/QumartSeller_Server-demo.git
 *
 * Copyright (c) 2024 https://github.com/Nailed34
 * Released under the MIT license
 */

//using OzonServiceNamespace.Actions.Tree;
using OzonServiceNamespace.DataBase;

namespace OzonServiceNamespace.Actions
{
    /// <summary>
    /// Class contains actions for server interaction
    /// </summary>
    public sealed class ServiceActions
    {
        private OzonService Service { get; init; }

        /// <summary>
        /// Object for import products actions
        /// </summary>
        //internal OzonImportProducts ImportProductsAction { get; init; }

        /// <summary>
        /// Object for import stocks actions
        /// </summary>
        //internal OzonImportStocks ImportStocksAction { get; init; }

        internal ServiceActions(OzonService service)
        {
            Service = service;
            //ImportProductsAction = new(service);
            //ImportStocksAction = new(service);
        }

        //public async Task<EImportProductsStartResult> StartImportProducts(EImportProductsMethod importType)
        //{
        //    return await ImportProductsAction.StartImport(importType);
        //}

        //public void CancelImportProducts()
        //{
        //    ImportProductsAction.CancelImport();
        //}

        //public async Task<EImportStocksStartResult> StartImportStocks()
        //{
        //    return await ImportStocksAction.StartImport();
        //}

        //public void CancelImportStocks()
        //{
        //    ImportStocksAction.CancelImport();
        //}

        public async Task<OzonCard> GetOzonCard(string field, string value)
        {
            var productCard = await Service.DataBase.GetOzonCards(field, value);
            return productCard != null ? productCard.Count > 0 ? productCard.First() : new() : new();
        }
    }
}

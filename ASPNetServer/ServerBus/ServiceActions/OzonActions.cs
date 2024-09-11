/**
 * @QumartSeller_Client
 * https://github.com/Nailed34/QumartSeller_Server-demo.git
 *
 * Copyright (c) 2024 https://github.com/Nailed34
 * Released under the MIT license
 */

//using OzonServiceNamespace.Actions.Tree;

namespace ASPNetServer.ServerBus.ServerActions
{
    /// <summary>
    /// Class with ozon service actions realization
    /// </summary>
    internal class OzonActions
    {
        // Service link
        private MainServerBus MainServerBus { get; init; }
        public OzonActions(MainServerBus mainBus) => MainServerBus = mainBus;

        /// <summary>
        /// Try to start import ozon products cards by selected method
        /// </summary>
        //public async Task<EImportProductsStartResult> StartImportProducts(EImportProductsMethod importType)
        //{
        //    return await MainServerBus.OzonService.Actions.StartImportProducts(importType);
        //}

        /// <summary>
        /// Stop import ozon products cards
        /// </summary>
        //public void CancelImportProducts()
        //{
        //    MainServerBus.OzonService.Actions.CancelImportProducts();
        //}
    }
}

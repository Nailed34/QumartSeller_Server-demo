/**
 * @QumartSeller_Client
 * https://github.com/Nailed34/QumartSeller_Server-demo.git
 *
 * Copyright (c) 2024 https://github.com/Nailed34
 * Released under the MIT license
 */

using MongoDB.Bson;
using ServicesDomain.Database.Data;
using ServicesDomain.Enums;

namespace ProductsServiceNamespace.DataBase
{
    /// <summary>
    /// Onion class saved in data base and contains convertation between this class and BsonDocument
    /// </summary>
    public class ProductOnion
    {
        /// <summary>
        /// Return new object of ProductOnion from BsonDocument presentation
        /// </summary>
        public static ProductOnion FromBsonDocument(BsonDocument document)
        {
            ProductOnion newOnion = new ProductOnion();
            try
            {              
                // Import main info
                newOnion._id = document["_id"].ToString() ?? "";
                newOnion.name = document["name"].ToString() ?? "";
                newOnion.custom_name = document["custom_name"].ToBoolean();
                newOnion.photo = document["photo"].ToString() ?? "";
                newOnion.stocks = document["stocks"].ToInt32();

                // Import articuls
                var articuls = document["articuls"].AsBsonArray;
                newOnion.articuls = new List<string>();
                foreach (var item in articuls)
                    newOnion.articuls.Add(item.ToString() ?? "");

                // Import barcodes
                var barcodes = document["barcodes"].AsBsonArray;
                newOnion.barcodes = new List<string>();
                foreach (var item in barcodes)
                    newOnion.barcodes.Add(item.ToString() ?? "");

                // Import marketplaces
                var marketplaces = document["marketplaces"].AsBsonArray;
                newOnion.marketplaces = new List<EMarketplaces>();
                foreach (var item in marketplaces)
                    newOnion.marketplaces.Add((EMarketplaces)item.ToInt32());

                // Import onion cards
                var cards = document["cards"].AsBsonArray;
                newOnion.cards = new List<ProductOnionCard>();
                foreach (var item in cards)
                    newOnion.cards.Add(new() { _id = item["_id"].ToString() ?? "", marketplace = (EMarketplaces)item["marketplace"].ToInt32() });
            }
            catch
            {
               // Log 
            }
            return newOnion;
        }

        /// <summary>
        /// Return list of ProductOnion from BsonDocument list presentation
        /// </summary>
        public static List<ProductOnion> FromBsonDocument(List<BsonDocument> documents)
        {
            List<ProductOnion> result = new();

            foreach (var item in documents)
                result.Add(FromBsonDocument(item)); 

            return result;
        }

        /// <summary>
        /// Return true if card doesn't have _id from data base
        /// </summary>
        public bool IsEmpty()
        {
            return _id == "";
        }

        /// <summary>
        /// Data base index
        /// </summary>
        public string _id { get; set; } = "";
        /// <summary>
        /// Name of onion determines by priority marketplace
        /// </summary>
        public string name { get; set; } = "";
        /// <summary>
        /// Name of onion determines by priority marketplace
        /// </summary>
        public bool custom_name { get; set; } = false;
        /// <summary>
        /// Photo of product determines by prority marketplace
        /// </summary>
        public string photo { get; set; } = "";
        /// <summary>
        /// General stocks for child cards
        /// </summary>
        public int stocks { get; set; } = 0;
        /// <summary>
        /// List of child cards articuls
        /// </summary>
        public List<string> articuls { get; set; } = new();
        /// <summary>
        /// List of child cards barcodes
        /// </summary>
        public List<string> barcodes { get; set; } = new();
        /// <summary>
        /// List of child cards marketplaces
        /// </summary>
        public List<EMarketplaces> marketplaces { get; set; } = new();
        /// <summary>
        /// List of child cards db indexes with marketplaces
        /// </summary>
        public List<ProductOnionCard> cards { get; set; } = new();
    }

    /// <summary>
    /// Class for attach onion with card. Contains db index and card marketplace
    /// </summary>
    public class ProductOnionCard
    {
        /// <summary>
        /// Card DB index
        /// </summary>
        public string _id { get; set; } = "";
        /// <summary>
        /// Which marketplace use for find card
        /// </summary>
        public EMarketplaces marketplace { get; set; } = EMarketplaces.None;
    }

    public static class ProductCardExtension
    {
        /// <summary>
        /// Extension for create new onion by product card. Onion id not generated!
        /// </summary>
        public static ProductOnion ToProductOnion(this ProductCard productCard)
        {
            var newOnion = new ProductOnion();

            newOnion.custom_name = false;
            newOnion.stocks = productCard.stocks;
            newOnion.photo = productCard.photo;
            newOnion.barcodes = productCard.barcodes;          
            newOnion.name = productCard.name;
            newOnion.marketplaces = new() { productCard.marketplace };
            newOnion.articuls = new() { productCard.articul };
            
            newOnion.cards = new()
            {
                new ProductOnionCard()
                {
                    _id = productCard._id,
                    marketplace = productCard.marketplace
                }
            };

            return newOnion;
        }
    }
}

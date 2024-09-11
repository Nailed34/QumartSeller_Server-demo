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

namespace OzonServiceNamespace.DataBase
{
    /// <summary>
    /// This class contains main info from ozon, contains convertation between this class and BsonDocument
    /// </summary>
    public class OzonCard : ProductCard
    {
        /// <summary>
        /// Return new object of OzonCard from BsonDocument presentation
        /// </summary>
        public static OzonCard FromBsonDocument(BsonDocument document)
        {
            OzonCard newCard = new OzonCard();
            try
            {              
                // Import main info
                newCard._id = document["_id"].ToString() ?? "";
                newCard.parent_onion_id = document["parent_onion_id"].ToString() ?? "";
                newCard.articul = document["articul"].ToString() ?? "";
                newCard.marketplace_articul = document["marketplace_articul"].ToString() ?? "";
                newCard.name = document["name"].ToString() ?? "";
                newCard.stocks = document["stocks"].ToInt32();
                newCard.creation_date = document["creation_date"].ToUniversalTime();
                newCard.photo = document["photo"].ToString() ?? "";
                newCard.is_synch = document["is_synch"].ToBoolean();
                newCard.multiplicity = document["multiplicity"].ToInt32();
                newCard.marketplace = EMarketplaces.Ozon;

                // Import barcodes
                var barcodes = document["barcodes"].AsBsonArray;
                newCard.barcodes = new List<string>();
                foreach (var item in barcodes)
                    newCard.barcodes.Add(item.ToString() ?? "");
                
            }
            catch
            {
                // Convertation error
            }
            return newCard;
        }

        /// <summary>
        /// Return list of OzonCard from BsonDocument list presentation
        /// </summary>
        public static List<OzonCard> FromBsonDocument(List<BsonDocument> documents)
        {
            List<OzonCard> result = new();

            foreach (var item in documents)
                result.Add(FromBsonDocument(item));

            return result;
        }
    }

    /// <summary>
    /// Extension for convert list of ozon cards to product cards
    /// </summary>
    public static class OzonCardExtension
    {
        public static List<ProductCard> ToProductCardList(this List<OzonCard> ozonCards)
        {
            return ozonCards.Select(oc => oc as ProductCard).ToList();
        }
    }
}

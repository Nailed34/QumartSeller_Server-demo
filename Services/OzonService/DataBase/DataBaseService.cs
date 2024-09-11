/**
 * @QumartSeller_Client
 * https://github.com/Nailed34/QumartSeller_Server-demo.git
 *
 * Copyright (c) 2024 https://github.com/Nailed34
 * Released under the MIT license
 */

using MongoDB.Bson;
using MongoDB.Driver;
using ServicesDomain.Database;

namespace OzonServiceNamespace.DataBase
{
    /// <summary>
    /// Data base service for ozon
    /// </summary>
    internal sealed class DataBaseService : MongoDatabaseService
    {
        public DataBaseService(string dataBaseUrl, string dataBaseName, string collectionName) : base(dataBaseUrl, dataBaseName, collectionName) { }

        /// <summary>
        /// Return number of all ozon cards
        /// </summary>
        public async Task<int?> GetOzonCardsCount()
        {
            if (Status != EDatabaseServiceStatus.Connected) return null;

            try
            {
                return (int)await Collection.CountDocumentsAsync("{}");
            }
            catch
            {
                SetupDisconnected();
                return null;
            }
        }

        /// <summary>
        /// Return ozon card by field name (_id, artucul ...)
        /// </summary>
        public async Task<List<OzonCard>?> GetOzonCards(string fieldName, string fieldValue)
        {
            if (Status != EDatabaseServiceStatus.Connected) return null;

            try
            {
                var queryFilter = new BsonDocument { { fieldName, fieldValue } };
                var queryResult = await Collection.FindAsync(queryFilter);
                var result = queryResult.ToList();
                if (result.Count > 0)
                    return OzonCard.FromBsonDocument(result);
                else
                    return new();
            }
            catch
            {
                SetupDisconnected();
                return null;
            }
        }

        /// <summary>
        /// Return ozon card list by field name (_id, artucul ...)
        /// </summary>
        public async Task<List<OzonCard>?> GetOzonCards(string fieldName, List<string> fieldValues)
        {
            if (Status != EDatabaseServiceStatus.Connected) return null;

            try
            {
                var queryFilter = new BsonDocument(fieldName, new BsonDocument("$in", new BsonArray(fieldValues)));
                var queryResult = await Collection.FindAsync(queryFilter);
                var cards = queryResult.ToList();
                if (cards.Count > 0)
                    return OzonCard.FromBsonDocument(cards);
                else
                    return new();
            }
            catch
            {
                SetupDisconnected();
                return null;
            }
        }

        /// <summary>
        /// Return all ozon cards from DB
        /// </summary>
        public async Task<List<OzonCard>?> GetAllOzonCards()
        {
            if (Status != EDatabaseServiceStatus.Connected) return null;

            try
            {
                var queryResult = await Collection.FindAsync("{}");
                if (queryResult.ToList().Count > 0)
                    return OzonCard.FromBsonDocument(queryResult.ToList());
                else
                    return new();
            }
            catch
            {
                SetupDisconnected();
                return null;
            }
        }

        /// <summary>
        /// Find card in BD
        /// </summary>
        public async Task<bool?> IsOzonCardExist(string articul)
        {
            if (Status != EDatabaseServiceStatus.Connected) return null;

            try
            {
                var queryFilter = new BsonDocument { { "articul", articul } };
                return await Collection.CountDocumentsAsync(queryFilter) > 0;
            }
            catch
            {
                SetupDisconnected();
                return null;
            }
        }

        /// <summary>
        /// Return list of uncreated articuls
        /// </summary>
        public async Task<List<string>?> GetNewOzonCards(List<string> articuls)
        {
            if (Status != EDatabaseServiceStatus.Connected) return null;

            try
            {
                var existedCards = await GetOzonCards("articul", articuls);
                if (existedCards != null)
                {
                    if (existedCards.Count > 0)
                        return articuls.Except(existedCards.Select(p => p.articul)).ToList();
                    else
                        return articuls;
                }
                else
                    return null;
            }
            catch
            {
                SetupDisconnected();
                return null;
            }
        }

        /// <summary>
        /// Insert new ozon card in DB
        /// </summary>
        public async Task<bool> AddNewOzonCards(OzonCard card)
        {
            if (Status != EDatabaseServiceStatus.Connected) return false;

            try
            {
                await Collection.InsertOneAsync(card.ToBsonDocument());
                return true;
            }
            catch
            {
                SetupDisconnected();
                return false;
            }
        }

        /// <summary>
        /// Insert new ozon card list in DB
        /// </summary>
        public async Task<bool> AddNewOzonCards(List<OzonCard> cards)
        {
            if (Status != EDatabaseServiceStatus.Connected) return false;

            try
            {
                await Collection.InsertManyAsync(from p in cards select p.ToBsonDocument());
                return true;
            }
            catch
            {
                SetupDisconnected();
                return false;
            }
        }

        /// <summary>
        /// Remove ozon cards by articul
        /// </summary>
        public async Task<bool?> RemoveOzonCards(string articul)
        {
            if (Status != EDatabaseServiceStatus.Connected) return null;

            try
            {
                var queryFilter = new BsonDocument { { "articul", articul } };
                var queryResult = await Collection.DeleteOneAsync(queryFilter);
                return queryResult.DeletedCount > 0;
            }
            catch
            {
                SetupDisconnected();
                return null;
            }
        }

        /// <summary>
        /// Remove list of ozon cards by articuls
        /// </summary>
        public async Task<bool?> RemoveOzonCards(List<string> articuls)
        {
            if (Status != EDatabaseServiceStatus.Connected) return null;

            try
            {
                var queryFilter = new BsonDocument("articul", new BsonDocument("$in", new BsonArray(articuls)));
                var queryResult = await Collection.DeleteManyAsync(queryFilter);
                return queryResult.DeletedCount == articuls.Count;
            }
            catch
            {
                SetupDisconnected();
                return null;
            }
        }

        /// <summary>
        /// Update stocks in ozon card
        /// </summary>
        public async Task<bool?> UpdateOzonCardsStocks(OzonCard productCardToUpdate)
        {
            if (Status != EDatabaseServiceStatus.Connected) return null;

            try
            {
                var queryFilter = new BsonDocument("_id", productCardToUpdate._id);
                var queryUpdateDefinition = new BsonDocument("$set", new BsonDocument("stocks", productCardToUpdate.stocks));

                var query_result = await Collection.UpdateOneAsync(queryFilter, queryUpdateDefinition);

                return query_result.ModifiedCount > 0;
            }
            catch
            {
                SetupDisconnected();
                return null;
            }
        }

        /// <summary>
        /// Update stocks in ozon cards list
        /// </summary>
        public async Task<bool?> UpdateOzonCardsStocks(List<OzonCard> ozonCardsToUpdate)
        {
            if (Status != EDatabaseServiceStatus.Connected) return null;

            try
            {
                var bulkRequestModel = new List<WriteModel<BsonDocument>>();
                foreach (var productCard in ozonCardsToUpdate)
                {
                    var queryFilter = new BsonDocument("_id", productCard._id);
                    var queryUpdateDefinition = new BsonDocument("$set", new BsonDocument("stocks", productCard.stocks));

                    bulkRequestModel.Add(new UpdateOneModel<BsonDocument>(queryFilter, queryUpdateDefinition));
                }

                var result = await Collection.BulkWriteAsync(bulkRequestModel);
                return result.ModifiedCount > 0;
            }
            catch
            {
                SetupDisconnected();
                return null;
            }
        }

        /// <summary>
        /// Update parent id in ozon card
        /// </summary>
        public async Task<bool?> UpdateOzonCardsParentOnionId(OzonCard ozonCardToUpdate)
        {
            if (Status != EDatabaseServiceStatus.Connected) return null;

            try
            {
                var queryFilter = new BsonDocument("_id", ozonCardToUpdate._id);
                var queryUpdateDefinition = new BsonDocument("$set", new BsonDocument("parent_onion_id", ozonCardToUpdate.parent_onion_id));

                var queryResult = await Collection.UpdateOneAsync(queryFilter, queryUpdateDefinition);

                return queryResult.ModifiedCount > 0;
            }
            catch
            {
                SetupDisconnected();
                return null;
            }
        }

        /// <summary>
        /// Update parent id in ozon cards list
        /// </summary>
        public async Task<bool?> UpdateOzonCardsParentOnionId(List<OzonCard> ozonCardsToUpdate)
        {
            if (Status != EDatabaseServiceStatus.Connected) return null;

            try
            {
                var bulkRequestModel = new List<WriteModel<BsonDocument>>();
                foreach (var productCard in ozonCardsToUpdate)
                {
                    var queryFilter = new BsonDocument("_id", productCard._id);
                    var queryUpdateDefinition = new BsonDocument("$set", new BsonDocument("parent_onion_id", productCard.parent_onion_id));

                    bulkRequestModel.Add(new UpdateOneModel<BsonDocument>(queryFilter, queryUpdateDefinition));
                }

                var result = await Collection.BulkWriteAsync(bulkRequestModel);
                return result.ModifiedCount > 0;
            }
            catch
            {
                SetupDisconnected();
                return null;
            }
        }
    }
}

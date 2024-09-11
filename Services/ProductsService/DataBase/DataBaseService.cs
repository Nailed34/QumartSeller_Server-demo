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

namespace ProductsServiceNamespace.DataBase
{
    /// <summary>
    /// Data base service for onions
    /// </summary>
    internal sealed class DataBaseService : MongoDatabaseService
    {
        public DataBaseService(string dataBaseUrl, string dataBaseName, string collectionName) : base(dataBaseUrl, dataBaseName, collectionName) { }

        /// <summary>
        /// Return number of all products in DB
        /// </summary>
        public async Task<int?> GetProductsCount()
        {
            if (Status != EDatabaseServiceStatus.Connected) return null;

            try
            {
                var count = await Collection.CountDocumentsAsync("{}");
                return (int)count;
            }
            catch
            {
                SetupDisconnected();
                return null;
            }
        }

        /// <summary>
        /// Return ordered list of products in range
        /// </summary>
        public async Task<List<ProductOnion>?> GetProductsInRange(int firstIndex, int lastIndex)
        {
            if (Status != EDatabaseServiceStatus.Connected) return null;

            try
            {
                // Get info from DB
                var queryTaskResult = Collection.Find("{}");
                var queryResult = await queryTaskResult
                    .Sort("{_id:-1}")
                    .Skip(firstIndex - 1)
                    .Limit(lastIndex - firstIndex + 1)
                    .ToListAsync();

                if (queryResult.Count > 0)
                    return ProductOnion.FromBsonDocument(queryResult);
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
        /// Find and return products by articul
        /// </summary>
        public async Task<ProductOnion?> GetProducts(string articul)
        {
            if (Status != EDatabaseServiceStatus.Connected) return null;

            try
            {
                var queryFilter = new BsonDocument { { "articuls", articul } };
                var queryResult = await Collection.FindAsync(queryFilter);
                var onions = queryResult.ToList();
                return onions.Count > 0 ? ProductOnion.FromBsonDocument(queryResult.First()) : new ProductOnion();
            }
            catch
            {
                SetupDisconnected();
                return null;
            }
        }

        /// <summary>
        /// Find existed products by articuls in DB and return list with them
        /// </summary>
        public async Task<List<ProductOnion>?> GetProducts(List<string> articuls)
        {
            if (Status != EDatabaseServiceStatus.Connected) return null;

            try
            {
                var queryFilter = new BsonDocument("articul", new BsonDocument("$in", new BsonArray(articuls)));
                var queryTaskResult = await Collection.FindAsync(queryFilter);
                var queryResult = queryTaskResult.ToList();

                if (queryResult.Count > 0)
                    return ProductOnion.FromBsonDocument(queryResult);
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
        /// Find existed products by index in DB
        /// </summary>
        public async Task<ProductOnion?> GetCreatedProducts(string _id)
        {
            if (Status != EDatabaseServiceStatus.Connected) return null;

            try
            {
                var queryFilter = new BsonDocument("_id", _id);
                var queryResult = await Collection.FindAsync(queryFilter);
                var onions = queryResult.ToList();
                return onions.Count > 0 ? ProductOnion.FromBsonDocument(queryResult.First()) : new ProductOnion();
            }
            catch
            {
                SetupDisconnected();
                return null;
            }
        }

        /// <summary>
        /// Find existed products by index in DB and return list with them
        /// </summary>
        public async Task<List<ProductOnion>?> GetCreatedProducts(List<string> _id)
        {
            if (Status != EDatabaseServiceStatus.Connected) return null;

            var queryFilter = new BsonDocument("_id", new BsonDocument("$in", new BsonArray(_id)));
            var queryTaskResult = await Collection.FindAsync(queryFilter);
            var queryResult = queryTaskResult.ToList();

            if (queryResult.Count > 0)
                return ProductOnion.FromBsonDocument(queryResult);
            else
                return new();
        }

        /// <summary>
        /// Insert new product in DB
        /// </summary>
        public async Task<bool> AddNewProducts(ProductOnion product)
        {
            if (Status != EDatabaseServiceStatus.Connected) return false;

            try
            {
                await Collection.InsertOneAsync(product.ToBsonDocument());
                return true;
            }
            catch
            {
                SetupDisconnected();
                return false;
            }
        }

        /// <summary>
        /// Insert new products list in DB
        /// </summary>
        public async Task<bool> AddNewProducts(List<ProductOnion> products)
        {
            if (Status != EDatabaseServiceStatus.Connected) return false;

            try
            {
                await Collection.InsertManyAsync(from p in products select p.ToBsonDocument());
                return true;
            }
            catch
            {
                SetupDisconnected();
                return false;
            }
        }

        /// <summary>
        /// Remove product by id
        /// </summary>
        public async Task<bool?> RemoveProducts(string _id)
        {
            if (Status != EDatabaseServiceStatus.Connected) return null;

            try
            {
                var queryFilter = new BsonDocument { { "_id", _id } };
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
        /// Remove list of products by id
        /// </summary>
        public async Task<bool?> RemoveProducts(List<string> _id)
        {
            if (Status != EDatabaseServiceStatus.Connected) return null;

            try
            {
                var queryFilter = new BsonDocument("_id", new BsonDocument("$in", new BsonArray(_id)));
                var queryResult = await Collection.DeleteManyAsync(queryFilter);
                return queryResult.DeletedCount == _id.Count;
            }
            catch
            {
                SetupDisconnected();
                return null;
            }
        }

        /// <summary>
        /// Update stocks in product
        /// </summary>
        public async Task<bool?> UpdateProductsStocks(ProductOnion productToUpdate)
        {
            if (Status != EDatabaseServiceStatus.Connected) return null;

            try
            {
                var queryFilter = new BsonDocument("_id", productToUpdate._id);
                var queryUpdateDefinition = new BsonDocument("$set", new BsonDocument("stocks", productToUpdate.stocks));

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
        /// Update stocks in products list
        /// </summary>
        public async Task<bool?> UpdateProductsStocks(List<ProductOnion> productsToUpdate)
        {
            if (Status != EDatabaseServiceStatus.Connected) return null;

            try
            {
                var bulkRequestModel = new List<WriteModel<BsonDocument>>();
                foreach (var onion in productsToUpdate)
                {
                    var queryFilter = new BsonDocument("_id", onion._id);
                    var queryUpdateDefinition = new BsonDocument("$set", new BsonDocument("stocks", onion.stocks));

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
        /// Update fields by add/remove cards
        /// </summary>
        public async Task<bool?> UpdateProductsCards(ProductOnion productToUpdate)
        {
            if (Status != EDatabaseServiceStatus.Connected) return null;

            try
            {
                var queryFilter = new BsonDocument("_id", productToUpdate._id);
                var queryUpdateDefinition = new BsonDocument("$set", new BsonDocument
                {
                    { "name", productToUpdate.name },
                    { "photo", productToUpdate.photo },
                    { "articuls", new BsonArray(productToUpdate.articuls) },
                    { "barcodes", new BsonArray(productToUpdate.barcodes) },
                    { "marketplaces", new BsonArray(productToUpdate.marketplaces) },
                    { "cards", new BsonArray(productToUpdate.cards) }
                });

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
        /// Update fields by add/remove cards
        /// </summary>
        public async Task<bool?> UpdateProductsCards(List<ProductOnion> productsToUpdate)
        {
            if (Status != EDatabaseServiceStatus.Connected) return null;

            try
            {
                var bulkRequestModel = new List<WriteModel<BsonDocument>>();
                foreach (var onion in productsToUpdate)
                {
                    var queryFilter = new BsonDocument("_id", onion._id);
                    var queryUpdateDefinition = new BsonDocument("$set", new BsonDocument
                    {
                        { "name", onion.name },
                        { "photo", onion.photo },
                        { "articuls", new BsonArray(onion.articuls) },
                        { "barcodes", new BsonArray(onion.barcodes) },
                        { "marketplaces", new BsonArray(onion.marketplaces) },
                        { "cards", new BsonArray(onion.cards) }
                    });

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

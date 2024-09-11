/**
 * @QumartSeller_Client
 * https://github.com/Nailed34/QumartSeller_Server-demo.git
 *
 * Copyright (c) 2024 https://github.com/Nailed34
 * Released under the MIT license
 */

using MongoDB.Bson;
using MongoDB.Driver;

namespace ServicesDomain.Database
{
    /// <summary>
    /// Status list of data base service.
    /// None: service uncreated,
    /// Connected: success,
    /// InvalidUrl: connection couldn't be found,
    /// InvalidDB: data base with current name couldn't be found,
    /// InvalidCollection: collection in data base couldn't be found,
    /// Disconnected: connection with data base lost
    /// </summary>
    public enum EDatabaseServiceStatus
    {
        None,
        Connected,
        InvalidUrl,
        InvalidDB,
        InvalidCollection,
        Disconnected
    }

    /// <summary>
    /// Base class for mongo database services, contains database status and connection creating by constructor params
    /// </summary>
    public abstract class MongoDatabaseService
    {
        private readonly MongoClient? _mongoClient;
        private readonly IMongoDatabase? _dataBase;

        protected IMongoCollection<BsonDocument> Collection { get => _collection; }
        private IMongoCollection<BsonDocument> _collection;

        public EDatabaseServiceStatus Status { get => _status; }
        private EDatabaseServiceStatus _status = EDatabaseServiceStatus.None;

#pragma warning disable CS8618
        public MongoDatabaseService(string dataBaseUrl, string dataBaseName, string collectionName)
#pragma warning restore CS8618
        {
            try
            {
                _mongoClient = new(dataBaseUrl);
                try
                {
                    _dataBase = _mongoClient.GetDatabase(dataBaseName);
                    try
                    {
                        _collection = _dataBase.GetCollection<BsonDocument>(collectionName);
                        if (CheckSuccessResult())
                            _status = EDatabaseServiceStatus.Connected;
                    }
                    catch
                    {
                        _status = EDatabaseServiceStatus.InvalidCollection;
                    }
                }
                catch
                {
                    _status = EDatabaseServiceStatus.InvalidDB;
                }
            }
            catch
            {
                _status = EDatabaseServiceStatus.InvalidUrl;
            }
        }

        /// <summary>
        /// Try get count of documents for test connection. Return true on success
        /// </summary>
        protected bool CheckSuccessResult()
        {
            try
            {
                Collection.CountDocuments("{}");
                return true;
            }
            catch
            {
                _status = EDatabaseServiceStatus.Disconnected;
                return false;
            }
        }

        /// <summary>
        /// Use this method when database connection lost
        /// </summary>
        protected void SetupDisconnected()
        {
            _status = EDatabaseServiceStatus.Disconnected;
        }
    }
}

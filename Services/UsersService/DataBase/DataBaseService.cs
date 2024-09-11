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
using UsersServiceNamespace.DataBase;

namespace ServicesDomain.Users
{
    /// <summary>
    /// Data base service for users
    /// </summary>
    internal sealed class DataBaseService : MongoDatabaseService
    {
        public DataBaseService(string dataBaseUrl, string dataBaseName, string collectionName) : base(dataBaseUrl, dataBaseName, collectionName) { }

        /// <summary>
        /// Return user by username (if not find return empty UserCard)
        /// </summary>
        public async Task<UserCard?> GetUser(string username)
        {
            if (Status != EDatabaseServiceStatus.Connected) return null;

            try
            {
                var filter = new BsonDocument("username", username);
                var result = await Collection.Find(filter).ToListAsync();

                return result.Count > 0 ? UserCard.FromBsonDocument(result.First()) : new UserCard();
            }
            catch
            {
                SetupDisconnected();
                return null;
            }
        }
    }
}

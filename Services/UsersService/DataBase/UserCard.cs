/**
 * @QumartSeller_Client
 * https://github.com/Nailed34/QumartSeller_Server-demo.git
 *
 * Copyright (c) 2024 https://github.com/Nailed34
 * Released under the MIT license
 */

using MongoDB.Bson;

namespace UsersServiceNamespace.DataBase
{
    public class UserCard
    {
        /// <summary>
        /// Return new object of UserCard from BsonDocument presentation
        /// </summary>
        public static UserCard FromBsonDocument(BsonDocument document)
        {
            var user = new UserCard();
            try
            {
                user._id = document["_id"].ToString() ?? "";
                user.username = document["username"].ToString() ?? "";
                user.password = document["password"].ToString() ?? "";
            }
            catch
            {
                // Log if errors
            }
            return user;
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
        /// User name or login
        /// </summary>
        public string username { get; set; } = "";
        /// <summary>
        /// User password
        /// </summary>
        public string password { get; set; } = "";
    }
}

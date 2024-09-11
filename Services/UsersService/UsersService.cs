/**
 * @QumartSeller_Client
 * https://github.com/Nailed34/QumartSeller_Server-demo.git
 *
 * Copyright (c) 2024 https://github.com/Nailed34
 * Released under the MIT license
 */

using ServicesDomain;
using ServicesDomain.Users;
using UsersServiceNamespace.Actions;

namespace UsersServiceNamespace
{
    /// <summary>
    /// Service for authorize users
    /// </summary>
    public sealed class UsersService : ServiceBase
    {
        internal DataBaseService DataBase { get; init; }
        public ServiceActions Actions { get; init; }

        public UsersService(string dataBaseUrl) : base()
        {
            DataBase = new(dataBaseUrl, "users", "users");
            Actions = new(this);
        }
    }
}

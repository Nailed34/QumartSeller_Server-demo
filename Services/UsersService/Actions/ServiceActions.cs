/**
 * @QumartSeller_Client
 * https://github.com/Nailed34/QumartSeller_Server-demo.git
 *
 * Copyright (c) 2024 https://github.com/Nailed34
 * Released under the MIT license
 */

namespace UsersServiceNamespace.Actions
{
    public class ServiceActions
    {
        private UsersService Service { get; init; }
        internal ServiceActions(UsersService service) => Service = service;

        /// <summary>
        /// Try to authorize user by find him in Data Base
        /// </summary>
        public OutAuthorizeUser AuthorizeUser(InAuthorizeUser clientUser)
        {
            var user = Service.DataBase.GetUser(clientUser.Username).Result;

            // Check DB currect
            if (user == null)
                return new OutAuthorizeUser { IsAuthorized = false, DeclineReason = EAuthorizeUserDeclineReason.DataBaseError };

            // Check username
            if (user.IsEmpty())
                return new OutAuthorizeUser { IsAuthorized = false, DeclineReason = EAuthorizeUserDeclineReason.WrongData };

            // Check password
            if (user.password != clientUser.Password)
                return new OutAuthorizeUser { IsAuthorized = false, DeclineReason = EAuthorizeUserDeclineReason.WrongData };

            // Send response
            return new OutAuthorizeUser { IsAuthorized = true, DeclineReason = EAuthorizeUserDeclineReason.None };
        }
    }
}

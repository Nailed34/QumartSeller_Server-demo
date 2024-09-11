/**
 * @QumartSeller_Client
 * https://github.com/Nailed34/QumartSeller_Server-demo.git
 *
 * Copyright (c) 2024 https://github.com/Nailed34
 * Released under the MIT license
 */

namespace UsersServiceNamespace
{
    public enum EAuthorizeUserDeclineReason
    {
        None, DataBaseError, WrongData
    }

    /// <summary>
    /// Struct with data for authorize user
    /// </summary>
    public struct InAuthorizeUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    /// <summary>
    /// Struct with info about authorization
    /// </summary>
    public struct OutAuthorizeUser
    {
        public bool IsAuthorized { get; set; }
        public EAuthorizeUserDeclineReason DeclineReason { get; set; }
    }
}

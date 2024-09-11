/**
 * @QumartSeller_Client
 * https://github.com/Nailed34/QumartSeller_Server-demo.git
 *
 * Copyright (c) 2024 https://github.com/Nailed34
 * Released under the MIT license
 */

namespace ASPNetServer.ServerSettingsNamespace
{
    static class ServerSettings
    {
        private static ServerSettingsOptions _options = new ServerSettingsOptions();
        public static ServerSettingsOptions Options { get { return _options; } }

        public static bool InitSettings(WebApplicationBuilder builder)
        {
            bool isError = false;

            _options.DataBaseUrl = builder.Configuration["StartupOptions:DataBaseUrl"] ?? ""; if (_options.DataBaseUrl == "") isError = true;
            _options.JwtToken = builder.Configuration["APIOptions:JwtToken"] ?? ""; if (_options.JwtToken == "") isError = true;
            _options.OzonClientId = builder.Configuration["APIOptions:OzonClientId"] ?? ""; if (_options.OzonClientId == "") isError = true;
            _options.OzonApiKey = builder.Configuration["APIOptions:OzonApiKey"] ?? ""; if (_options.OzonApiKey == "") isError = true;

            try { _options.IsDemo = bool.Parse(builder.Configuration["StartupOptions:IsDemo"] ?? "true"); } catch { isError = true; }

            return !isError;
        }
    }

    class ServerSettingsOptions
    {
        public string DataBaseUrl { get; set; } = "mongodb://localhost:27017";
        public bool IsDemo { get; set; } = true;
        public string JwtToken { get; set; } = "";
        public string OzonClientId { get; set; } = "";
        public string OzonApiKey { get; set; } = "";
    }
}

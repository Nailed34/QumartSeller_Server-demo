/**
 * @QumartSeller_Client
 * https://github.com/Nailed34/QumartSeller_Server-demo.git
 *
 * Copyright (c) 2024 https://github.com/Nailed34
 * Released under the MIT license
 */

using UsersServiceNamespace;
using ClientServerConnection.Actions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ASPNetServer.ServerSettingsNamespace;

namespace ASPNetServer.ServerBus.ServerActions
{
    /// <summary>
    /// Class with users service actions realization
    /// </summary>
    internal class UsersActions
    {
        // Bus link
        private MainServerBus MainServerBus { get; init; }
        public UsersActions(MainServerBus mainBus) => MainServerBus = mainBus;

        /// <summary>
        /// Try to authorize user and return Json result
        /// </summary>
        public IResult AuthorizeUser(InUserAuthorization clientUser)
        {
            if (ServerSettings.Options.IsDemo)
            {
#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
                if (MainServerBus.DemoService.AuthorizeUser(clientUser))
                {
                    // Create claims
                    var claims = new List<Claim> { new Claim(ClaimTypes.Name, clientUser.Username) };

                    // Create jwt token
                    var jwt = new JwtSecurityToken(
                        issuer: "QumartSellerServer",
                        audience: "QumartSellerClient",
                        claims: claims,
                        signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ServerSettings.Options.JwtToken)), SecurityAlgorithms.HmacSha256)
                        );
                    // Token encoding
                    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                    // Send response
                    return Results.Json(new OutUserAuthorization { Success = true, Token = encodedJwt });
                }
                else
                    return Results.Json(new OutUserAuthorization { Success = false, Token = "" });
#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.
            }
            else
            {
                var request_param = new InAuthorizeUser { Username = clientUser.Username, Password = clientUser.Password };
                var request_result = MainServerBus.UsersService.Actions.AuthorizeUser(request_param);

                if (request_result.IsAuthorized)
                {
                    // Create claims
                    var claims = new List<Claim> { new Claim(ClaimTypes.Name, clientUser.Username) };

                    // Create jwt token
                    var jwt = new JwtSecurityToken(
                        issuer: "QumartSellerServer",
                        audience: "QumartSellerClient",
                        claims: claims,
                        signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ServerSettings.Options.JwtToken)), SecurityAlgorithms.HmacSha256)
                        );
                    // Token encoding
                    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                    // Send response
                    return Results.Json(new OutUserAuthorization { Success = true, Token = encodedJwt });
                }
                else
                {
                    string declineReason = "";
                    switch (request_result.DeclineReason)
                    {
                        case EAuthorizeUserDeclineReason.DataBaseError:
                            declineReason = "Сервис временно недоступен, повторите попытку позже";
                            break;
                        case EAuthorizeUserDeclineReason.WrongData:
                            declineReason = "Неверные имя пользователя или пароль";
                            break;
                    }
                    return Results.Json(new OutUserAuthorization { Success = false, Token = "", DeclineReason = declineReason });
                }
            }          
        }
    }
}

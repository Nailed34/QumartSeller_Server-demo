/**
 * @QumartSeller_Client
 * https://github.com/Nailed34/QumartSeller_Server-demo.git
 *
 * Copyright (c) 2024 https://github.com/Nailed34
 * Released under the MIT license
 */

using ClientServerConnection.Actions;
using ASPNetServer.Hubs;
using ASPNetServer.ServerBus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

ASPNetServer.ServerSettingsNamespace.ServerSettings.InitSettings(builder);

builder.Services.AddSingleton<MainServerBus>();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidIssuer = "QumartSellerServer",
        ValidateAudience = false,
        ValidAudience = "QumartSellerClient",
        ValidateLifetime = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ASPNetServer.ServerSettingsNamespace.ServerSettings.Options.JwtToken)),
        ValidateIssuerSigningKey = true,
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/main"))
                context.Token = accessToken;
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddSignalR();


var app = builder.Build();
var ServerBus = app.Services.GetService<MainServerBus>();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/auth", (InUserAuthorization clientUser) => ServerBus?.UsersActions.AuthorizeUser(clientUser));

app.MapHub<MainHUB>("/main");
app.Run();


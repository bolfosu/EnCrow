using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.ApplicationModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Builder;

namespace Encrow;

public partial class MitId : ContentPage
{
	public MitId()
	{
		InitializeComponent();
	}
    protected virtual async Task RedeemAuthorizationCodeAsync(AuthorizationCodeReceivedContext context)
    {
        var configuration = await context.Options.ConfigurationManager.GetConfigurationAsync(CancellationToken.None);
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, configuration.TokenEndpoint);
        var authInfo = $"{context.TokenEndpointRequest.ClientId}:{context.TokenEndpointRequest.ClientSecret}";
        authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", authInfo);
        var tokenEndpointRequest = context.TokenEndpointRequest.Clone();
        tokenEndpointRequest.ClientSecret = null;
        requestMessage.Content = new FormUrlEncodedContent(tokenEndpointRequest.Parameters);

        var responseMessage = await context.Backchannel.SendAsync(requestMessage);
        if (!responseMessage.IsSuccessStatusCode)
        {
            Console.WriteLine(await responseMessage.Content.ReadAsStringAsync());
            return;
        }

        try
        {
            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            var message = new OpenIdConnectMessage(responseContent);
            context.HandleCodeRedemption(message);
        }
        catch (Exception exc)
        {
            Console.WriteLine($"An error occurred: {exc.Message}");
        }
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // omitted for brevity

        services.AddAuthentication(config =>
        {
            config.DefaultAuthenticateScheme = "AuthorizationCodeClientAppCookie";
            config.DefaultSignInScheme = "AuthorizationCodeClientAppCookie";
            config.DefaultChallengeScheme = "Signicat";
        })
            .AddCookie("AuthorizationCodeClientAppCookie")
            .AddOpenIdConnect("Signicat", config =>
            {
                config.Events.OnAuthorizationCodeReceived = RedeemAuthorizationCodeAsync;

                config.Authority = "https://encrow.app.signicat.dev/auth/open";

                config.ClientId = "sandbox-cooperative-sail-561";
                config.ClientSecret = "DSjmriy2tELeR3T4nofNH45M5bTqN02kahgecvj9wdVdnZ1d";

                config.CallbackPath = "/account/callback";

                config.UsePkce = true;

                config.ResponseType = "code";

                config.Scope.Add("openid");
                config.Scope.Add("profile");

                config.GetClaimsFromUserInfoEndpoint = true;

                config.SaveTokens = true;
            });

        services.AddControllersWithViews();

        // omitted for brevity
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // omitted for brevity

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
        });

        // omitted for brevity
    }
}
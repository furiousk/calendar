using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;

namespace POB.CALENDAR
{
    public class MyCalendar
    {
        private static string client_id = "ddbd8f1a-409d-4391-934b-ed5727393eb1";
        private static string tenant_id = "e43baa31-10f6-4095-9622-7d5c0dc7cc4b";
        private static string secret = "9329V-~Xb3it0hsJaUjqTsR3A_Wbbv2-nz";
        private static string[] scopes = { "https://graph.microsoft.com/.default" };

        public static IPublicClientApplication _pca = null;
        private static IPublicClientApplication Pca
        {
            get
            {
                if (_pca == null)
                {
                    _pca = PublicClientApplicationBuilder
                        .Create(client_id)
                        .WithTenantId(tenant_id)
                        //.WithClientSecret(secret)
                        .Build();
                }
                return _pca;
            }
        }

        public static InteractiveAuthenticationProvider _authProvider = null;
        private static InteractiveAuthenticationProvider AuthProvider
        {
            get
            {
                if (_authProvider == null)
                {
                    _authProvider = new InteractiveAuthenticationProvider(Pca, scopes);
                }
                return _authProvider;
            }
        }

        private static GraphServiceClient GraphClientB
        {
            get
            {
                if (_graphClient == null)
                {
                    var confidentialClient = ConfidentialClientApplicationBuilder

                        .Create(client_id)
                        .WithAuthority($"https://login.microsoftonline.com/{tenant_id}/v2.0")
                        .WithClientSecret(secret)
                        .Build();

                    _graphClient = new GraphServiceClient(new DelegateAuthenticationProvider(async (requestMessage) =>
                    {
                        // Retrieve an access token for Microsoft Graph (gets a fresh token if needed).
                        var authResult = await confidentialClient
                            .AcquireTokenForClient(scopes)
                            .ExecuteAsync();

                        // Add the access token in the Authorization header of the API request.
                        requestMessage.Headers.Authorization =
                            new AuthenticationHeaderValue("Bearer", authResult.AccessToken);
                    }));
                }
                return _graphClient;
            }
        }

        public static GraphServiceClient _graphClient = null;
        private static GraphServiceClient GraphClient
        {
            get
            {
                if (_graphClient == null)
                {
                    _graphClient = new GraphServiceClient(AuthProvider);
                }
                return _graphClient;
            }
        }

        public static async Task Get_Me()
        {
            User user = null;
            user = await GraphClient.Me.Request().GetAsync();
            Console.WriteLine($"Display Name = {user.DisplayName}\nEmail = {user.Mail}");
        }
        public static IGraphServiceClient Create()
        {
            GraphServiceClient graphClient;
            try
            {
                // Initiate client application
                var confidentialClientApplication = ConfidentialClientApplicationBuilder
                    .Create(client_id)
                    .WithTenantId(tenant_id)
                    .WithClientSecret(secret)
                    .Build();

                // Create the auth provider
                var authProvider = new ClientCredentialProvider(confidentialClientApplication);
                // Create Graph Service Client
                graphClient = new GraphServiceClient(authProvider);
            }
            catch (ServiceException ex)
            {
                throw ex;
            }
            // Return
            return graphClient;
        }
    }
}
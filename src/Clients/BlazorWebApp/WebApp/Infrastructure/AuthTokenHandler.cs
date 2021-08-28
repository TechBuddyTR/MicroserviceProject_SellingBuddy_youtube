using Blazored.LocalStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WebApp.Extensions;

namespace WebApp.Infrastructure
{
    public class AuthTokenHandler : DelegatingHandler
    {
        private readonly ISyncLocalStorageService storageService;

        public AuthTokenHandler(ISyncLocalStorageService identityService)
        {
            this.storageService = identityService;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (storageService != null)
            {
                string token = storageService.GetToken();
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}

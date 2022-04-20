using Blazored.LocalStorage;
using HttpApiClient;
using Microsoft.AspNetCore.Components;

namespace BlazorClient
{
    public abstract class AppComponentBase : ComponentBase
    {
        [Inject]
        protected ShopClient ShopClient { get; private set; }

        [Inject]
        protected ILocalStorageService LocalStorage { get; private set; }

        protected bool IsTokenChecked { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (!IsTokenChecked)
            {
                var token = await LocalStorage.GetItemAsync<string>("AuthorizationToken");
                if (token is not null)
                {
                    ShopClient.SetAuthorizationToken(token);
                }
                IsTokenChecked = true;
            }
        }
    }
}

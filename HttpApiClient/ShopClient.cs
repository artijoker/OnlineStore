using HttpModels;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace HttpApiClient
{
    public class ShopClient
    {
        private readonly string _host;
        private readonly HttpClient _httpClient;
        public ShopClient(string? host = null, HttpClient? httpClient = null)
        {
            _host = host ?? "https://localhost:7254";
            _httpClient = httpClient ?? new();

            _httpClient.DefaultRequestHeaders.Add("apikey", "7254");
        }

        public void SetAuthorizationToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public Task<IReadOnlyList<Product>?> GetProducts() =>
            _httpClient.GetFromJsonAsync<IReadOnlyList<Product>>($"{_host}/catalog/get_products");

        public Task<Product?> GetProductById(Guid id) =>
            _httpClient.GetFromJsonAsync<Product>($"{_host}/catalog/get_product_by_id?id={id}");

        public Task<HttpResponseMessage> AddProduct(Product product) =>
            _httpClient.PostAsJsonAsync($"{_host}/catalog/add_product", product);

        public Task<IReadOnlyList<Category>?> GetCategories() =>
            _httpClient.GetFromJsonAsync<IReadOnlyList<Category>>($"{_host}/catalog/get_categories");

        public async Task<LogInResponse?> Registration(AccountRegistrationModel model)
        {
            using var responseMessage = await _httpClient.PostAsJsonAsync(
                $"{_host}/account/registration",
                model
                );
            var response = await responseMessage.Content.ReadFromJsonAsync<LogInResponse>();
            SetAuthorizationToken(response!.Token);
            return response;
        }



        public async Task<ResponseModel<ConfirmationCodeModel>?> LogIn(AccountLogInModel model)
        {
            using var responseMessage = await _httpClient.PostAsJsonAsync(
                $"{_host}/account/login",
                model
                );
            return await responseMessage.Content.ReadFromJsonAsync<ResponseModel<ConfirmationCodeModel>>();
        }

        public async Task<LogInResponse?> СonfirmСode(ConfirmationCodeModel model)
        {
            using var responseMessage = await _httpClient.PostAsJsonAsync(
                $"{_host}/account/confirm_code",
                model
                );
            var response = await responseMessage.Content.ReadFromJsonAsync<LogInResponse>();
            SetAuthorizationToken(response!.Token);
            return response;
        }

        public Task<ResponseModel<Account>?> GetAccount()
        {
            return _httpClient.GetFromJsonAsync <ResponseModel<Account>> ($"{_host}/account/get_account");
        }

        public Task<ResponseModel<IReadOnlyList<Account>>?> GetAllAccounts()
        {
            return _httpClient.GetFromJsonAsync <ResponseModel<IReadOnlyList<Account>>>($"{_host}/account/get_all_accounts");
        }

        public async Task<ResponseModel<Product>?> AddToCart(Product product)
        {
            using var responseMessage = await _httpClient.PostAsJsonAsync($"{_host}/cart/add_to_cart", product);
            return await responseMessage.Content.ReadFromJsonAsync<ResponseModel<Product>>();
        }

        public Task<ResponseModel<Cart>?> GetCart()
        {
            return _httpClient.GetFromJsonAsync<ResponseModel<Cart>>($"{_host}/cart/get_cart");
        }

        public Task<ResponseModel<Order>?> CreateOrder()
        {
            return _httpClient.GetFromJsonAsync<ResponseModel<Order>>($"{_host}/cart/create_order");
        }
    }
}
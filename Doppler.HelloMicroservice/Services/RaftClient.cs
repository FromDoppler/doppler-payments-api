// unset:error

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Doppler.HelloMicroservice.Models;
using Microsoft.Extensions.Configuration;

namespace Doppler.HelloMicroservice.Services
{
    public class RaftClient
    {
        private readonly HttpClient _http;
        private readonly bool _mock;

        public RaftClient(HttpClient http, IConfiguration config)
        {
            _http = http;
            _mock = bool.Parse(config["Raft:Mock"] ?? "false");

            if (!_mock)
            {
                var username = config["Raft:Username"];
                var password = config["Raft:Password"];
                var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
                _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

        private async Task<T> PostAsync<T>(string url, object body)
        {
            if (_mock)
            {
                return MockResponse<T>(body);
            }

            var res = await _http.PostAsync(url, new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json"));
            res.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<T>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }

        private async Task<T> GetAsync<T>(string url)
        {
            if (_mock)
            {
                return MockResponse<T>(null);
            }

            var res = await _http.GetAsync(url);
            res.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<T>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }

        private async Task DeleteAsync(string url)
        {
            if (_mock)
            {
                return;
            }
                
            var res = await _http.DeleteAsync(url);
            res.EnsureSuccessStatusCode();
        }

        public Task<TokenResponse> CreateTokenAsync(TokenRequest req) => PostAsync<TokenResponse>("/tokens", req);
        public Task<TokenResponse> VerifyTokenAsync(VerifiedTokenRequest req) => PostAsync<TokenResponse>("/verified-tokens/oneTime", req);
        public Task<PaymentResponse> MakePaymentAsync(PaymentRequest req) => PostAsync<PaymentResponse>("/api/payments", req);
        public Task<TokenResponse> GetTokenAsync(string tokenId) => GetAsync<TokenResponse>($"/tokens/{tokenId}");
        public Task DeleteTokenAsync(string tokenId) => DeleteAsync($"/tokens/{tokenId}");

        private static T MockResponse<T>(object? body)
        {
            object mock = typeof(T) switch
            {
                _ when typeof(T) == typeof(TokenResponse) => new TokenResponse
                {
                    Id = "mock-token-123",
                    Status = "APPROVED",
                    Last4 = "1111",
                    ExpiryMonth = "12",
                    ExpiryYear = "2030"
                },
                _ when typeof(T) == typeof(PaymentResponse) => new PaymentResponse
                {
                    Id = "mock-payment-123",
                    Status = "SUCCESS",
                    Amount = body is PaymentRequest p ? p.Amount : 0,
                    Currency = body is PaymentRequest pc ? pc.Currency : "USD",
                    CreatedAt = DateTime.UtcNow
                },
                _ => throw new NotImplementedException($"Mock para {typeof(T).Name} no implementado")
            };

            return (T)mock;
        }
    }

}

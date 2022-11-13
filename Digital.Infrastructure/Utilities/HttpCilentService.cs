using Digital.Infrastructure.Interface;
using Digital.Infrastructure.Model;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Digital.Infrastructure.Utilities
{
    public class HttpCilentService
    {
        protected readonly HttpClient _httpClient;
        protected readonly IJwtTokenService _tokenService;
        protected JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };
        protected readonly IConfiguration _configuration;
        public HttpCilentService(HttpClient httpClient, IJwtTokenService tokenService, IConfiguration configuration)
        {
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", tokenService.GenerateToken());
            httpClient.Timeout = TimeSpan.FromMinutes(5);
            _httpClient = httpClient;
            _tokenService = tokenService;
            _configuration = configuration;
        }

        protected async Task<T> GetResultAsync<T>(HttpResponseMessage response)
        {
            var a = JsonSerializer.Deserialize<object>(
                await response.Content.ReadAsStringAsync(), _jsonOptions);
            return JsonSerializer.Deserialize<APIResult<T>>(
                await response.Content.ReadAsStringAsync(), _jsonOptions)!.Result!;
        }
    }
}

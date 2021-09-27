using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Enums;
using CarsInfo.WebApi.IntegrationTest.Configuration;
using CarsInfo.WebApi.IntegrationTest.Configuration.Database;
using CarsInfo.WebApi.ViewModels.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace CarsInfo.WebApi.IntegrationTest
{
    public abstract class AbstractIntegrationTest : IDisposable
    {
        private readonly TempHttpClientWrapper _tempHttpClientWrapper;

        protected AbstractIntegrationTest()
        {
            _tempHttpClientWrapper = TempHttpClientWrapper.Create(TestDatabaseConfiguration.GetTempTestDbConnectionString());
        }

        protected HttpClient TestClient => _tempHttpClientWrapper.HttpClient;

        public virtual async Task AddCarToFavoriteAsync(int carId, HttpClient httpClient = null)
        {
            httpClient ??= TestClient;
            ToggleFavoriteStatus result;
            do
            {
                var response = await httpClient.PutAsJsonAsync($"/cars/{carId}/favorite", new { });
                result = await response.Content.ReadFromJsonAsync<ToggleFavoriteStatus>();
            } while (result == ToggleFavoriteStatus.DeleteFromFavorite);
        }

        public virtual async Task AuthenticateAdminAsync(HttpClient httpClient = null)
        {
            httpClient ??= TestClient;
            var response = await httpClient.PostAsJsonAsync("/login", new LoginViewModel
            {
                Email = "admin@email.com",
                Password = "admin@email.com"
            });

            var loginResponse = await response.Content.ReadFromJsonAsync<AuthViewModel>();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                JwtBearerDefaults.AuthenticationScheme, 
                loginResponse?.AccessToken);
        }

        public virtual async Task AuthenticateNewUserAsync(HttpClient httpClient = null)
        {
            httpClient ??= TestClient;
            var randomString = RandomStringGenerator.GenerateRandom(15);
            var response = await httpClient.PostAsJsonAsync("/register", new RegisterViewModel
            {
                FirstName = "Integration",
                LastName = "Test",
                Email = $"{randomString}@test.com",
                Password = randomString
            });

            var registrationResponse = await response.Content.ReadFromJsonAsync<AuthViewModel>();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                JwtBearerDefaults.AuthenticationScheme, registrationResponse?.AccessToken);
        }

        public void Dispose()
        {
            _tempHttpClientWrapper?.Dispose();
        }
    }
}

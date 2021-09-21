using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.WebApi.IntegrationTest.Configuration;
using CarsInfo.WebApi.ViewModels.Car;
using CarsInfo.WebApi.ViewModels.Error;
using FluentAssertions;
using Xunit;

namespace CarsInfo.WebApi.IntegrationTest
{
    public class CarsControllerTests : AbstractIntegrationTest
    {
        [Fact]
        public async Task Get_ReturnsCars_WhenCarsExist()
        {
            var response = await TestClient.GetAsync("/cars");

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);
            (await response.Content.ReadFromJsonAsync<IEnumerable<CarDto>>())
                .Should()
                .NotBeEmpty()
                .And
                .AllBeOfType<CarDto>();
        }

        [Fact]
        public async Task Get_WithFilter_ReturnsFilteredCars()
        {
            var response = await TestClient.GetAsync("/cars?brands=BMW");

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);
            (await response.Content.ReadFromJsonAsync<IEnumerable<CarDto>>())
                .Should()
                .OnlyContain(car => car.Brand == "BMW");
        }

        [Fact]
        public async Task Get_ReturnsCar_WhenCarExists()
        {
            var response = await TestClient.GetAsync("/cars/1");

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);
            (await response.Content.ReadFromJsonAsync<CarDto>())
                .Should()
                .NotBeNull()
                .And
                .BeOfType<CarDto>();
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenCarNotExist()
        {
            var response = await TestClient.GetAsync("/cars/0");

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.NotFound);
            (await response.Content.ReadAsStringAsync())
                .Should()
                .BeEmpty();
        }

        [Fact]
        public async Task GetEditor_ReturnsCarEditor_WhenCarExists()
        {
            await AuthenticateAdminAsync();

            var response = await TestClient.GetAsync("/cars/1/editor");

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);
            (await response.Content.ReadFromJsonAsync<CarEditorViewModel>())
                .Should()
                .NotBeNull()
                .And
                .BeOfType<CarEditorViewModel>();
        }

        [Fact]
        public async Task GetEditor_ReturnsNotFound_WhenCarNotExist()
        {
            await AuthenticateAdminAsync();

            var response = await TestClient.GetAsync("/cars/0/editor");

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.NotFound);
            (await response.Content.ReadAsStringAsync())
                .Should()
                .BeEmpty();
        }

        [Fact]
        public async Task Create_ReturnsUnauthorized_WhenUserNotAuthorized()
        {
            var car = new CarEditorViewModel
            {
                BrandId = 1,
                Model = "Test",
                Description = "Test",
                CarPicturesUrls = new List<string>
                {
                    "someurl.com"
                }
            };

            var response = await TestClient.PostAsJsonAsync("/cars", car);

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Create_ReturnsCreatedResponse_WhenDataIsValid()
        {
            await AuthenticateAdminAsync();
            var car = new CarEditorViewModel
            {
                BrandId = 1,
                Model = "Test",
                Description = "Test",
                CarPicturesUrls = new List<string>
                {
                    "someurl.com"
                }
            };

            var response = await TestClient.PostAsJsonAsync("/cars", car);

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Create_ReturnsErrorResponse_WhenDataIsInvalid()
        {
            await AuthenticateAdminAsync();
            var car = new CarEditorViewModel
            {
                BrandId = 1,
                Model = "Test",
                Description = "Test"
            };

            var response = await TestClient.PostAsJsonAsync("/cars", car);

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.BadRequest);
            (await response.Content.ReadFromJsonAsync<ErrorResponse>())
                .Should()
                .Match(error => (error as ErrorResponse).ValidationErrors.Any());
        }

        [Fact]
        public async Task Update_ReturnsErrorResponse_WhenCarNotExist()
        {
            await AuthenticateAdminAsync();
            var car = new CarEditorViewModel
            {
                BrandId = 1,
                Model = "Test",
                Description = "Test",
                CarPicturesUrls = new List<string>
                {
                    "someurl.com"
                }
            };

            var response = await TestClient.PutAsJsonAsync("/cars/0", car);

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.BadRequest);
            (await response.Content.ReadFromJsonAsync<ErrorResponse>())
                .Should()
                .Match(error => !string.IsNullOrWhiteSpace((error as ErrorResponse).ApplicationError));
        }

        [Fact]
        public async Task Update_ReturnsNoContent_WhenDataIsValid()
        {
            await AuthenticateAdminAsync();
            var car = new CarEditorViewModel
            {
                BrandId = 1,
                Model = "Test",
                Description = "Test",
                CarPicturesUrls = new List<string>
                {
                    "someurl.com"
                }
            };

            var response = await TestClient.PutAsJsonAsync("/cars/1", car);

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Patch_ReturnsErrorResponse_WhenCarNotExist()
        {
            await AuthenticateAdminAsync();

            var response = await TestClient.PatchAsync("/cars/0", JsonContent.Create(new List<object>
            {
                new
                {
                    path = "/model",
                    op = "replace",
                    value = "X7"
                }
            }));

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.BadRequest);
            (await response.Content.ReadFromJsonAsync<ErrorResponse>())
                .Should()
                .Match(error => !string.IsNullOrWhiteSpace((error as ErrorResponse).ApplicationError));
        }

        [Fact]
        public async Task Patch_ReturnsNoContent_WhenDataIsValid()
        {
            await AuthenticateAdminAsync();

            var response = await TestClient.PatchAsync("/cars/1", JsonContent.Create(new List<object>
            {
                new
                {
                    path = "/model",
                    op = "replace",
                    value = "X7"
                }
            }));

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Favorite_ReturnsEmptyResponse_WhenFavoriteCarsNotExist()
        {
            await AuthenticateNewUserAsync();

            var response = await TestClient.GetAsync("/cars/favorite");

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);
            (await response.Content.ReadFromJsonAsync<IEnumerable<CarDto>>())
                .Should()
                .BeEmpty();
        }

        [Fact]
        public async Task Favorite_ReturnsFavoriteCars_WhenFavoriteCarsExist()
        {
            await AuthenticateAdminAsync();
            await AddCarToFavoriteAsync(1);

            var response = await TestClient.GetAsync("/cars/favorite");

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);
            (await response.Content.ReadFromJsonAsync<IEnumerable<CarDto>>())
                .Should()
                .NotBeNullOrEmpty()
                .And
                .AllBeOfType<CarDto>();
        }

        [Fact]
        public async Task FavoriteCarsIds_ReturnsEmptyResponse_WhenFavoriteCarsNotExist()
        {
            await AuthenticateNewUserAsync();

            var response = await TestClient.GetAsync("/cars/favorite/ids");

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);
            (await response.Content.ReadFromJsonAsync<IEnumerable<int>>())
                .Should()
                .BeEmpty();
        }

        [Fact]
        public async Task FavoriteCarsIds_ReturnsFavoriteCarsIds_WhenFavoriteCarsExist()
        {
            await AuthenticateAdminAsync();
            await AddCarToFavoriteAsync(1);

            var response = await TestClient.GetAsync("/cars/favorite/ids");

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);
            (await response.Content.ReadFromJsonAsync<IEnumerable<int>>())
                .Should()
                .NotBeNullOrEmpty()
                .And
                .AllBeOfType<int>();
        }

        [Fact]
        public async Task Delete_ReturnsErrorResponse_WhenCarNotExist()
        {
            await AuthenticateAdminAsync(TestClient);

            var response = await TestClient.DeleteAsync("/cars/0");

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.BadRequest);
            (await response.Content.ReadFromJsonAsync<ErrorResponse>())
                .Should()
                .Match(error => !string.IsNullOrWhiteSpace((error as ErrorResponse).ApplicationError));
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenCarExists()
        {
            await AuthenticateAdminAsync(TestClient);
           
            var response = await TestClient.DeleteAsync("/cars/2");

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task ToggleFavorite_ReturnsErrorResponse_WhenCarNotExist()
        {
            await AuthenticateAdminAsync();

            var response = await TestClient.PutAsJsonAsync("/cars/0/favorite", new {});

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.BadRequest);
            (await response.Content.ReadFromJsonAsync<ErrorResponse>())
                .Should()
                .Match(error => !string.IsNullOrWhiteSpace((error as ErrorResponse).ApplicationError));
        }

        [Fact]
        public async Task ToggleFavorite_ReturnsNoContent_WhenCarExists()
        {
            await AuthenticateAdminAsync();

            var response = await TestClient.PutAsJsonAsync("/cars/1/favorite", new { });

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);
        }
    }
}

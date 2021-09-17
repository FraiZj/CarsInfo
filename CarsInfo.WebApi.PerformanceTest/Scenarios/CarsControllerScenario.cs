using NBomber.Contracts;
using NBomber.CSharp;
using System;
using System.Net.Http;

namespace CarsInfo.WebApi.PerfomanceTest.Scenarios
{
    public static class CarsControllerScenario
    {
        private static IClientFactory<HttpClient> HttpFactory;
        private static string BaseUrl;

        public static Scenario GetScenario(string baseUrl, IClientFactory<HttpClient> httpFactory)
        {
            HttpFactory = httpFactory;
            BaseUrl = baseUrl;
            return ScenarioBuilder
                .CreateScenario("cars_controller_scenario", 
                    CarsGetAllStep(), CarsGetByIdStep())
                .WithWarmUpDuration(TimeSpan.FromSeconds(10))
                .WithLoadSimulations(Simulation.InjectPerSec(rate: 100, during: TimeSpan.FromSeconds(30)));
        }
        
        private static IStep CarsGetAllStep()
        {
            var feed = Feed.CreateRandom(
                name: "cars_get_all_feed",
                new[] { 
                    "?skip=0&take=100&orderBy=BrandNameDesc",
                    "?skip=100&take=100&orderBy=BrandNameDesc&brands=bmw",
                    string.Empty
                }
            );

            return Step.Create("cars_get_all", HttpFactory, feed: feed, async context =>
            {
                var filter = context.FeedItem;
                var response = await context.Client.GetAsync($"{BaseUrl}/cars{filter}", context.CancellationToken);
                var responseAsByteArray = await response.Content.ReadAsByteArrayAsync();
                var responseAsString = await response.Content.ReadAsStringAsync();

                return response.IsSuccessStatusCode ?
                    Response.Ok(statusCode: (int)response.StatusCode, sizeBytes: responseAsByteArray.Length) :
                    Response.Fail(error: responseAsString, statusCode: (int)response.StatusCode, sizeBytes: responseAsByteArray.Length);
            });
        }

        private static IStep CarsGetByIdStep()
        {
            var feed = Feed.CreateRandom(
                name: "cars_get_by_id_feed",
                new[] { 1, 2, 3, 4, 5 }
            );

            return Step.Create("cars_get_by_id", HttpFactory, feed: feed, async context =>
            {
                var carId = context.FeedItem;
                var response = await context.Client.GetAsync($"{BaseUrl}/cars/{carId}", context.CancellationToken);
                var responseAsByteArray = await response.Content.ReadAsByteArrayAsync();
                var responseAsString = await response.Content.ReadAsStringAsync();

                return response.IsSuccessStatusCode ?
                    Response.Ok(statusCode: (int)response.StatusCode, sizeBytes: responseAsByteArray.Length) :
                    Response.Fail(
                        error: responseAsString, 
                        statusCode: (int)response.StatusCode, 
                        sizeBytes: responseAsByteArray.Length);
            });
        }
    }
}

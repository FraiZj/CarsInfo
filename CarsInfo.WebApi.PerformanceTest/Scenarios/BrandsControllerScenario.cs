using NBomber.Contracts;
using NBomber.CSharp;
using System;
using System.Net.Http;

namespace CarsInfo.WebApi.PerfomanceTest.Scenarios
{
    public static class BrandsControllerScenario
    {
        private static IClientFactory<HttpClient> HttpFactory;
        private static string BaseUrl;

        public static Scenario GetScenario(string baseUrl, IClientFactory<HttpClient> httpFactory)
        {
            HttpFactory = httpFactory;
            BaseUrl = baseUrl;
            return ScenarioBuilder
                .CreateScenario("brands_controller_scenario",
                    BrandsGetAllStep())
                .WithWarmUpDuration(TimeSpan.FromSeconds(10))
                .WithLoadSimulations(Simulation.InjectPerSec(rate: 100, during: TimeSpan.FromSeconds(30)));
        }

        private static IStep BrandsGetAllStep()
        {
            var feed = Feed.CreateRandom(
                name: "brands_get_all_feed",
                new[] { 
                    "?name=b",
                    "?name=lada",
                    string.Empty
                }
            );

            return Step.Create("brands_get_all", HttpFactory, feed: feed, async context =>
            {
                var filter = context.FeedItem;
                var response = await context.Client.GetAsync($"{BaseUrl}/brands{filter}", context.CancellationToken);
                var responseAsByteArray = await response.Content.ReadAsByteArrayAsync();
                var responseAsString = await response.Content.ReadAsStringAsync();

                return response.IsSuccessStatusCode ?
                    Response.Ok(statusCode: (int)response.StatusCode, sizeBytes: responseAsByteArray.Length) :
                    Response.Fail(error: responseAsString, statusCode: (int)response.StatusCode, sizeBytes: responseAsByteArray.Length);
            });
        }

        private static IStep BrandsGetByIdStep()
        {
            var feed = Feed.CreateRandom(
                name: "brands_get_by_id_feed",
                new[] {
                    1,
                    2,
                    3
                }
            );

            return Step.Create("brands_get_by_id", HttpFactory, feed: feed, async context =>
            {
                var brandId = context.FeedItem;
                var response = await context.Client.GetAsync($"{BaseUrl}/brands/{brandId}", context.CancellationToken);
                var responseAsByteArray = await response.Content.ReadAsByteArrayAsync();
                var responseAsString = await response.Content.ReadAsStringAsync();

                return response.IsSuccessStatusCode ?
                    Response.Ok(statusCode: (int)response.StatusCode, sizeBytes: responseAsByteArray.Length) :
                    Response.Fail(error: responseAsString, statusCode: (int)response.StatusCode, sizeBytes: responseAsByteArray.Length);
            });
        }
    }
}

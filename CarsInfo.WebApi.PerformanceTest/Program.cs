using CarsInfo.WebApi.PerfomanceTest.Scenarios;
using NBomber.CSharp;
using NBomber.Plugins.Http.CSharp;

namespace CarsInfo.WebApi.PerformanceTest
{
    public static class Program
    {
        private const string BaseUrl = "https://localhost:5001";

        private static void Main(string[] _)
        {
            var httpClientFactory = HttpClientFactory.Create();
            NBomberRunner.RegisterScenarios(
                    CarsControllerScenario.GetScenario(BaseUrl, httpClientFactory),
                    BrandsControllerScenario.GetScenario(BaseUrl, httpClientFactory)
                ).Run();
        }
    }
}

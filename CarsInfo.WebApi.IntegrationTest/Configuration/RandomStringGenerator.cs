using System;
using System.Linq;

namespace CarsInfo.WebApi.IntegrationTest.Configuration
{
    public static class RandomStringGenerator
    {
        private static readonly Random Random = new();
        
        public static string GenerateRandom(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
    }
}

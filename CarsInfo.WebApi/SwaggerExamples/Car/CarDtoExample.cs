using System.Collections.Generic;
using CarsInfo.Application.BusinessLogic.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace CarsInfo.WebApi.SwaggerExamples.Car
{
    public class CarDtoListExample : IExamplesProvider<IEnumerable<CarDto>>
    {
        public IEnumerable<CarDto> GetExamples()
        {
            return new List<CarDto>
            {
                new CarDto
                {
                    Id = 1,
                    Brand = "BMW",
                    Model = "X5 M",
                    Description = "BMW X5 M",
                    CarPicturesUrls = new List<string>
                    {
                        "https://imgd.aeplcdn.com/1056x594/n/cw/ec/51529/x5-m-exterior-right-front-three-quarter.jpeg"
                    }
                }
            };
        }
    }
}

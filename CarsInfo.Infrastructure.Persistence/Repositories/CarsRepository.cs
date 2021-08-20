using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarsInfo.Application.Persistence.Contracts;
using CarsInfo.Application.Persistence.Filters;
using CarsInfo.Domain.Entities;

namespace CarsInfo.Infrastructure.Persistence.Repositories
{
    public class CarsRepository : GenericRepository<Car>, ICarsRepository
    {
        public CarsRepository(IDbContext context) 
            : base(context)
        { }

        public async Task<IEnumerable<Car>> GetAllWithBrandAndPicturesAsync(IList<FilterModel> filters = null, int skip = 0, int take = 6)
        {
            var filter = filters?.Any() ?? false ? ConfigureFilter(filters) : string.Empty;
            var sql = $@"SELECT * FROM (
	                        SELECT Car.Id AS CarId FROM Car
	                        LEFT JOIN Brand
	                        ON Car.BrandId = Brand.Id
                            { filter }
	                        GROUP BY Car.Id, Brand.Name
	                        ORDER BY Brand.Name 
	                        OFFSET {skip} ROWS
	                        FETCH NEXT {take} ROWS ONLY
	                    ) as CarsIds
                        LEFT JOIN Car
                        ON CarsIds.CarId = Car.Id
                        LEFT JOIN Brand
                        ON Car.BrandId = Brand.Id
                        LEFT JOIN CarPicture
                        ON Car.Id = CarPicture.CarId";

            var cars = await Context.QueryAsync<Car, Brand, CarPicture>(sql,
                (car, brand, carPicture) =>
                {
                    car.Brand = brand;

                    if (carPicture is not null)
                    {
                        car.CarPictures.Add(carPicture);
                    }

                    return car;
                });

            return GroupSet(cars);
        }

        public async Task<IEnumerable<Car>> GetUserCarsAsync(
            string userId, IList<FilterModel> filters = null, int skip = 0, int take = 6)
        {
            var filter = filters?.Any() ?? false ? ConfigureFilter(filters) : string.Empty;
            var sql = $@"SELECT * FROM (
	                        SELECT Car.Id AS CarId FROM Car
                            INNER JOIN UserCar
                            ON Car.Id = UserCar.CarId AND UserCar.UserId = { userId }
	                        LEFT JOIN Brand
	                        ON Car.BrandId = Brand.Id
                            { filter }
	                        GROUP BY Car.Id, Brand.Name
	                        ORDER BY Brand.Name 
	                        OFFSET { skip } ROWS
	                        FETCH NEXT { take } ROWS ONLY
	                    ) as CarsIds
                        LEFT JOIN Car
                        ON CarsIds.CarId = Car.Id
                        LEFT JOIN Brand
                        ON Car.BrandId = Brand.Id
                        LEFT JOIN CarPicture
                        ON Car.Id = CarPicture.CarId";

            var cars = await Context.QueryAsync<Car, Brand, CarPicture>(sql,
                (car, brand, carPicture) =>
                {
                    car.Brand = brand;

                    if (carPicture is not null)
                    {
                        car.CarPictures.Add(carPicture);
                    }

                    return car;
                });

            return GroupSet(cars);
        }

        public async Task<Car> GetWithAllIncludesAsync(int id)
        {
            var sql = @$"SELECT * FROM {TableName} car
                         LEFT JOIN Brand
                         ON car.BrandId = Brand.Id
                         LEFT JOIN CarPicture
                         ON car.Id = CarPicture.CarId
                         LEFT JOIN Comment
                         ON car.Id = Comment.CarId
                         WHERE car.Id=@id";

            
            return await Context.QueryFirstOrDefaultAsync<Car, Brand, CarPicture, Comment>(sql,
                (car, brand, carPicture, comment) =>
                {
                    car.Brand = brand;

                    if (carPicture is not null)
                    {
                        car.CarPictures.Add(carPicture);
                    }

                    if (comment is not null)
                    {
                        car.Comments.Add(comment);
                    }
                    
                    return car;
                }, new { id });
        }

        private IEnumerable<Car> GroupSet(IEnumerable<Car> cars)
        {
            return cars.GroupBy(c => c.Id).Select(g =>
            {
                var groupedCar = g.First();
                if (!groupedCar.CarPictures.Any())
                {
                    groupedCar.CarPictures = new List<CarPicture>();
                }

                foreach (var car in g.Skip(1))
                {
                    if (car.CarPictures.Any())
                    {
                        groupedCar.CarPictures.Add(car.CarPictures.First());
                    }
                }

                return groupedCar;
            });
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarsInfo.DAL.Assistance;
using CarsInfo.DAL.Contracts;
using CarsInfo.DAL.Entities;

namespace CarsInfo.DAL.Repositories
{
    public class CarsRepository : GenericRepository<Car>, ICarsRepository
    {
        public CarsRepository(IDbContext context) 
            : base(context)
        { }

        public async Task<IEnumerable<Car>> GetAllWithBrandAndPicturesAsync(IList<FilterModel> filters = null)
        {
            var sql = @$"SELECT * FROM {TableName} car
                         LEFT JOIN Brand
                         ON car.BrandId = Brand.Id
                         LEFT JOIN CarPicture
                         ON car.Id = CarPicture.CarId";

            if (filters is not null && filters.Any())
            {
                var filter = ConfigureFilter(filters);
                sql += $" {filter}";
            }
            
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

        //public async Task<Car> GetAsyncWithAllIncludesAsync()
        //{

        //    var properties = ParseProperties(filter);
        //    var sqlPairs = GetSqlPairs(properties.AllNames, " AND ");
        //    var sql = @$"SELECT TOP 1 * FROM [{TableName}] c
        //                 INNER JOIN [tbl.Brands] br 
        //                 ON c.BrandId = br.Id
        //                 INNER JOIN [tbl.BodyTypes] bt
        //                 ON c.BodyTypeId = br.Id
        //                 INNER JOIN [tbl.CarPictures] cp
        //                 ON c.Id = cp.CarId 
        //                 WHERE {sqlPairs}";

        //    return await Context.QueryFirstOrDefaultAsync<Car, Brand, BodyType, CarPicture>(sql,
        //        (car, brand, bodyType, carPicture) =>
        //        {
        //            car.Brand = brand;
        //            car.BodyType = bodyType;
        //            car.CarPictures.Add(carPicture);
        //            return car;
        //        }, sqlPairs);
        //}

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

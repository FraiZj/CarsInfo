﻿using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CarsInfo.Application.Persistence.Contracts;
using CarsInfo.Application.Persistence.Filters;
using CarsInfo.Domain.Entities;
using CarsInfo.Infrastructure.Persistence.Configurators;

namespace CarsInfo.Infrastructure.Persistence.Repositories
{
    public class CarsRepository : GenericRepository<Car>, ICarsRepository
    {
        public CarsRepository(IDbContext context) 
            : base(context)
        { }

        public async Task<IEnumerable<Car>> GetAsync(FilterModel filter)
        {
            filter ??= new FilterModel();
            
            var orderBy = SqlQueryConfigurator.ConfigureOrderBy(filter.OrderBy);
            var filters = SqlQueryConfigurator.ConfigureFilter(TableName, filter.Filters, filter.IncludeDeleted);
            var sql = $@"SELECT * FROM (
	                        SELECT Car.Id AS CarId FROM Car
	                        LEFT JOIN Brand
	                        ON Car.BrandId = Brand.Id
                            { filters }
	                        GROUP BY Car.Id, Brand.Name
	                        { orderBy }
	                        OFFSET { filter.Skip } ROWS
	                        FETCH NEXT { filter.Take } ROWS ONLY
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

        public async Task<IEnumerable<Car>> GetUserFavoriteCarsAsync(
            int userId, FilterModel filter)
        {
            filter ??= new FilterModel();
            var orderBy = SqlQueryConfigurator.ConfigureOrderBy(filter.OrderBy);
            var filters = SqlQueryConfigurator.ConfigureFilter(TableName, filter.Filters, filter.IncludeDeleted);
            var sql = $@"SELECT * FROM (
	                        SELECT Car.Id AS CarId FROM Car
                            INNER JOIN UserCar
                            ON Car.Id = UserCar.CarId AND UserCar.UserId = { userId }
	                        LEFT JOIN Brand
	                        ON Car.BrandId = Brand.Id
                            { filters }
	                        GROUP BY Car.Id, Brand.Name
	                        { orderBy }
	                        OFFSET { filter.Skip } ROWS
	                        FETCH NEXT { filter.Take } ROWS ONLY
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
        
        public async Task<Car> GetByIdAsync(int id, bool includeDeleted = false)
        {
            var sql = @$"SELECT * FROM Car
                         LEFT JOIN Brand
                         ON Car.BrandId = Brand.Id
                         LEFT JOIN CarPicture
                         ON Car.Id = CarPicture.CarId
                         WHERE Car.Id=@id";

            if (!includeDeleted)
            {
                sql += " AND Car.IsDeleted = 0";
            }

            var cars  = await Context.QueryAsync<Car, Brand, CarPicture>(sql,
                (car, brand, carPicture) =>
                {
                    car.Brand = brand;

                    if (carPicture is not null)
                    {
                        car.CarPictures.Add(carPicture);
                    }
                    
                    return car;
                }, new { id });

            return GroupSet(cars).FirstOrDefault();
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

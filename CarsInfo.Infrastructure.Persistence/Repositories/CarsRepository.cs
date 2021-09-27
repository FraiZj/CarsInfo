using System;
using System.Collections.Generic;
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
        {
        }

        public override Task<IEnumerable<Car>> GetAllAsync(FilterModel filter)
        {
            filter ??= new FilterModel();
            var orderBy = SqlQueryConfigurator.ConfigureOrderBy(filter.OrderBy);
            var filters = SqlQueryConfigurator.ConfigureFilter(TableName, filter.Filters, filter.IncludeDeleted);
            var selectCars = $@"SELECT * FROM Car
                                LEFT JOIN Brand
                                ON Car.BrandId = Brand.Id
                                {filters}
                                {orderBy}
                                OFFSET {filter.Skip} ROWS
	                            FETCH NEXT {filter.Take} ROWS ONLY";

            return GetCarsAsync(selectCars, LoadFirstPictureForEachAsync);
        }

        public Task<IEnumerable<Car>> GetUserFavoriteCarsAsync(int userId, FilterModel filter)
        {
            filter ??= new FilterModel();
            var orderBy = SqlQueryConfigurator.ConfigureOrderBy(filter.OrderBy);
            var filters = SqlQueryConfigurator.ConfigureFilter(TableName, filter.Filters, filter.IncludeDeleted);
            var selectUserCars = $@"SELECT * FROM Car
                                    INNER JOIN UserCar
                                    ON Car.Id = UserCar.CarId AND UserCar.UserId = @userId
                                    LEFT JOIN Brand
                                    ON Car.BrandId = Brand.Id
                                    {filters}
                                    {orderBy}
                                    OFFSET {filter.Skip} ROWS
	                                FETCH NEXT {filter.Take} ROWS ONLY";

            return GetCarsAsync(selectUserCars, LoadFirstPictureForEachAsync,
                new
                {
                    userId
                });
        }

        public override async Task<Car> GetAsync(int id, bool includeDeleted = false)
        {
            var selectCarById = @"SELECT TOP 1 * FROM Car
                                LEFT JOIN Brand
                                ON Car.BrandId = Brand.Id
                                WHERE Car.Id = @id";

            if (!includeDeleted)
            {
                selectCarById += " AND Car.IsDeleted = 0";
            }

            return (await GetCarsAsync(selectCarById, LoadAllPicturesForEachAsync,
                new
                {
                    id
                })).FirstOrDefault();
        }

        private async Task<IEnumerable<Car>> GetCarsAsync(
            string sql,
            Func<IEnumerable<Car>, Task> resultHandler,
            object parameters = null)
        {
            var cars = (await Context.QueryAsync<Car, Brand>(sql,
                (car, brand) =>
                {
                    car.Brand = brand;
                    return car;
                }, parameters)).ToList();

            await resultHandler(cars);

            return cars;
        }

        private async Task LoadFirstPictureForEachAsync(IEnumerable<Car> cars)
        {
            const string selectFirstCarPictureByCarId =
                "SELECT TOP 1 * FROM CarPicture WHERE CarId = @carId AND IsDeleted = 0";
            foreach (var car in cars)
            {
                car.CarPictures.Add(await Context.QueryFirstOrDefaultAsync<CarPicture>(
                    selectFirstCarPictureByCarId,
                    new
                    {
                        carId = car.Id
                    }));
            }
        }

        private async Task LoadAllPicturesForEachAsync(IEnumerable<Car> cars)
        {
            const string selectCarPicturesByCarId =
                "SELECT * FROM CarPicture WHERE CarId = @carId AND IsDeleted = 0";
            foreach (var car in cars)
            {
                car.CarPictures = (await Context.QueryAsync<CarPicture>(
                    selectCarPicturesByCarId,
                    new
                    {
                        carId = car.Id
                    })).ToList();
            }
        }
    }
}
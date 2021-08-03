using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.DAL.Contracts;
using CarsInfo.DAL.Entities;

namespace CarsInfo.DAL.Repositories
{
    public class CarsRepository : GenericRepository<Car>, ICarsRepository
    {
        public CarsRepository(IDbContext context) 
            : base(context)
        { }

        public async Task<IEnumerable<Car>> GetAllWithBrandAndPicturesAsync()
        {
            var sql = @$"SELECT * FROM {TableName} c
                         INNER JOIN Brand br 
                         ON c.BrandId = br.Id
                         INNER JOIN CarPicture cp
                         ON c.Id = cp.CarId";

            //if (filter is null)
            //{
            //    return await Context.QueryAsync<Car, Brand, CarPicture>(sql,
            //        (car, brand, carPicture) =>
            //        {
            //            car.Brand = brand;
            //            car.CarPictures.Add(carPicture);
            //            return car;
            //        });
            //}

            //var properties = ParseProperties(filter);
            //var sqlPairs = GetSqlPairs(properties.AllNames, " AND ");
            //sql += $" WHERE {sqlPairs}";

            return await Context.QueryAsync<Car, Brand, CarPicture>(sql,
                (car, brand, carPicture) =>
                {
                    car.Brand = brand;
                    car.CarPictures.Add(carPicture);
                    return car;
                });
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
            var sql = @$"SELECT TOP 1 * FROM {TableName} c
                         INNER JOIN Brand br 
                         ON c.BrandId = br.Id
                         INNER JOIN BodyType bt
                         ON c.BodyTypeId = bt.Id
                         INNER JOIN FuelType ft
                         ON c.FuelTypeId = ft.Id
                         INNER JOIN Country co
                         ON c.CountryId = co.Id
                         INNER JOIN Gearbox g
                         ON c.CountryId = g.Id
                         INNER JOIN CarPicture cp
                         ON c.Id = cp.CarId
                         INNER JOIN Comment com
                         ON c.Id = com.Id
                         WHERE c.Id=@id";

            return await Context.QueryFirstOrDefaultAsync<Car, Brand, BodyType, CarPicture>(sql,
                (car, brand, bodyType, carPicture) =>
                {
                    car.Brand = brand;
                    car.BodyType = bodyType;
                    car.CarPictures.Add(carPicture);
                    return car;
                }, new { id });
        }
    }
}

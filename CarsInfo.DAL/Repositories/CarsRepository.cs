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

        public async Task<IEnumerable<Car>> GetAllAsyncWithIncludes(object filter = null)
        {
            var sql = @$"SELECT * FROM [{TableName}] c
                         INNER JOIN [tbl.Brands] br 
                         ON c.BrandId = br.Id
                         INNER JOIN [tbl.BodyTypes] bt
                         ON c.BodyTypeId = br.Id
                         INNER JOIN [tbl.CarPictures] cp
                         ON c.Id = cp.CarId";

            return await Context.QueryAsync<Car, Brand, BodyType, CarPicture, Car>(sql,
                (car, brand, bodyType, carPicture) =>
                {
                    car.Brand = brand;
                    car.BodyType = bodyType;
                    car.CarPictures.Add(carPicture);
                    return car;
                });
        }

        public Task<Car> GetAsyncWithIncludes(object filter)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Car> GetAsyncWithIncludes(int id)
        {
            var sql = @$"SELECT * FROM [{TableName}] c
                         INNER JOIN [tbl.Brands] br 
                         ON c.BrandId = br.Id
                         INNER JOIN [tbl.BodyTypes] bt
                         ON c.BodyTypeId = br.Id
                         INNER JOIN [tbl.CarPictures] cp
                         ON c.Id = cp.CarId
                         WHERE c.Id=@id ";

            return await Context.QueryFirstOrDefaultAsync<Car, Brand, BodyType, CarPicture, Car>(sql,
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

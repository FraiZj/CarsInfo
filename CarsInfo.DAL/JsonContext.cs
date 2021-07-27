using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CarsInfo.DAL.Contracts;
using CarsInfo.DAL.Entities;
using Newtonsoft.Json;

namespace CarsInfo.DAL
{
    public class JsonContext : IContext
    {
        const string DataPath = @"C:\tempdata";

        public async Task AddAsync<T>(T entity) where T : BaseEntity
        {
            var entities = await ReadAllAsync<T>();
            entities.Add(entity);
            await WriteAllAsync(entities);
        }
        
        public async Task DeleteAsync<T>(int id) where T : BaseEntity
        {
            var entities = await ReadAllAsync<T>();
            await WriteAllAsync(entities.Where(e => e.Id != id));
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>() where T : BaseEntity
        {
            return await ReadAllAsync<T>();
        }

        public async Task<T> GetAsync<T>(int id) where T : BaseEntity
        {
            var entities = await ReadAllAsync<T>();
            return entities.FirstOrDefault(e => e.Id == id);
        }

        public async Task UpdateAsync<T>(T entity) where T : BaseEntity
        {
            var entities = await ReadAllAsync<T>();
            var entityToUpdate = entities.FirstOrDefault(e => e.Id == entity.Id);
            entities.Remove(entityToUpdate);
            entities.Add(entity);
            await WriteAllAsync(entities);
        }

        private string GetTableName(Type type)
        {
            var tableAttribute = Attribute.GetCustomAttribute(type, typeof(TableAttribute)) as TableAttribute;
            return tableAttribute?.Name;
        }

        private async Task<ICollection<T>> ReadAllAsync<T>()
        {
            var tableName = GetTableName(typeof(T));
            var fileInfo = new FileInfo($"{DataPath}/{tableName}.json");
            using var sr = new StreamReader(fileInfo.Open(FileMode.OpenOrCreate));
            var data = await sr.ReadToEndAsync();
            var entities = JsonConvert.DeserializeObject<ICollection<T>>(data) ?? new List<T>();
            
            return entities;
        }

        private async Task WriteAllAsync<T>(IEnumerable<T> entities)
        {
            var tableName = GetTableName(typeof(T));
            var fileInfo = new FileInfo($"{DataPath}/{tableName}.json");
            using var sr = new StreamWriter(fileInfo.Open(FileMode.Create));
            var json = JsonConvert.SerializeObject(entities);
            await sr.WriteAsync(json);
        }
    }
}

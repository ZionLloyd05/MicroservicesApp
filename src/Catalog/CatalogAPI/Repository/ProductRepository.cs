using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.API.Data.Interfaces;
using Catalog.API.Entities;
using Catalog.API.Repository.Interfaces;
using MongoDB.Driver;

namespace Catalog.API.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext context;

        public ProductRepository(ICatalogContext context)
        {
            this.context = context;
        }

        public async Task Create(Product product)
        {
            await this.context.Products.InsertOneAsync(product);  
        }

        public async Task<bool> Delete(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter
                .Eq(product => product.Id, id);

            DeleteResult deleteResult = await this.context.
                Products
                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }

        public async Task<Product> GetProduct(string id)
        {
            return await this.context
               .Products
               .Find(product => product.Id == id)
               .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string Name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter
                .ElemMatch(product => product.Name, Name);

            return await this.context
               .Products
               .Find(filter)
               .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await this.context
               .Products
               .Find(product => true)
               .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategory(string categoryName)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter
                .ElemMatch(product => product.Category, categoryName);

            return await this.context
               .Products
               .Find(filter)
               .ToListAsync();
        }

        public async Task<bool> Update(Product product)
        {
            var updateResult = await this.context
                .Products
                .ReplaceOneAsync(filter: product => product.Id == product.Id, replacement: product);

            return updateResult.IsAcknowledged
                && updateResult.MatchedCount > 0;
        }
    }
}

using ProductApi.Models;
namespace ProductApi.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Products>> GetAllAsync();
        Task<Products> GetByIdAsync(int id);
        Task<bool> ExistsAsync(string name, string author);
        Task AddAsync(Products product);
        Task UpdateAsync(Products product);
        Task DeleteAsync(Products product);
    }
}

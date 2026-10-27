using ABM_Productos.Data;
using ABM_Productos.Entities;
using ABM_Productos.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ABM_Productos.Repositories.Implementations;

public class ProductRepository : IProductRepository
{
    private readonly ABMProductosContext _context;

    public ProductRepository(ABMProductosContext context)
    {
        _context = context;
    }

    public List<Product> GetAllProducts()
    {
        return _context.Products
            .AsNoTracking()               
            .OrderBy(product => product.Name)
            .ToList();
    }

    public Product? GetProductById(int id)
    {
        return _context.Products
            .FirstOrDefault(product => product.Id == id);
    }

    public void AddProduct(Product product)
    {
        _context.Products.Add(product);
        _context.SaveChanges();
    }

    public void UpdateProduct(Product product)
    {
        var existingProduct = _context.Products
            .FirstOrDefault(item => item.Id == product.Id);

        if (existingProduct is null)
        {
            return;
        }

        existingProduct.Name = product.Name;
        existingProduct.Price = product.Price;

        _context.SaveChanges();
    }

    public void DeleteProduct(Product product)
    {
        _context.Products.Remove(product);
        _context.SaveChanges();
    }

    public List<Product> SearchProductsByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return GetAllProducts();
        }

        return _context.Products
            .AsNoTracking()
            .Where(product => EF.Functions.Like(product.Name, $"%{name}%"))
            .ToList();
    }
}
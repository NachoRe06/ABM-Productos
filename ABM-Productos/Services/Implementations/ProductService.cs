using ABM_Productos.Entities;
using ABM_Productos.Models.DTOs.Requests;
using ABM_Productos.Models.DTOs.Responses;
using ABM_Productos.Repositories.Interfaces;
using ABM_Productos.Services.Interfaces;

namespace ABM_Productos.Services.Implementations;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public List<ProductForReadDto> GetAllProducts()
    {
        var products = _repository.GetAllProducts();

        return products.Select(p => new ProductForReadDto
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price
        }).ToList();
    }

    public ProductForReadDto? GetProductById(int id)
    {
        var product = _repository.GetProductById(id);

        if (product is null)
        {
            return null;
        }

        return new ProductForReadDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price
        };
    }

    public ProductForReadDto CreateProduct(ProductForCreateDto dto)
    {
        var exists = _repository.GetAllProducts()
            .Any(p => p.Name.Equals(dto.Name.Trim(), StringComparison.OrdinalIgnoreCase));

        if (exists)
        {
            throw new InvalidOperationException($"Ya existe un producto con el nombre '{dto.Name}'.");
        }

        var newProduct = new Product
        {
            Name = dto.Name.Trim(),
            Price = dto.Price
        };

        _repository.AddProduct(newProduct);

        return new ProductForReadDto
        {
            Id = newProduct.Id,
            Name = newProduct.Name,
            Price = newProduct.Price
        };
    }

    public void UpdateProduct(int id, ProductForUpdateDto dto)
    {
        var existingProduct = _repository.GetProductById(id);

        if (existingProduct is not null)
        {
            var nameConflict = _repository.GetAllProducts()
                .Any(p => p.Id != id && p.Name.Equals(dto.Name.Trim(), StringComparison.OrdinalIgnoreCase));

            if (nameConflict)
            {
                throw new InvalidOperationException($"Ya existe otro producto con el nombre '{dto.Name}'.");
            }

            existingProduct.Name = dto.Name.Trim();
            existingProduct.Price = dto.Price;

            _repository.UpdateProduct(existingProduct);
        }
    }

    public void DeleteProduct(int id)
    {
        var existingProduct = _repository.GetProductById(id);

        if (existingProduct is not null)
        {
            _repository.DeleteProduct(existingProduct);
        }
    }

    public List<ProductForReadDto> SearchProductsByName(string name)
    {
        var products = _repository.SearchProductsByName(name);

        return products.Select(p => new ProductForReadDto
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price
        }).ToList();
    }

    public ProductStatsDto GetStats()
    {
        var products = _repository.GetAllProducts();

        if (!products.Any())
        {
            return new ProductStatsDto
            {
                Total = 0,
                AveragePrice = 0m,
                MostExpensiveName = string.Empty
            };
        }

        var mostExpensive = products.OrderByDescending(p => p.Price).First();

        return new ProductStatsDto
        {
            Total = products.Count,
            AveragePrice = products.Average(p => p.Price),
            MostExpensiveName = mostExpensive.Name
        };
    }
}
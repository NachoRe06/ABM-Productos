using System.Collections.Generic;
using ABM_Productos.Entities;
using ABM_Productos.Models.DTOs.Requests;
using ABM_Productos.Models.DTOs.Responses;
using ABM_Productos.Repositories.Implementations;
using ABM_Productos.Services.Interfaces;

namespace ABM_Productos.Services.Implementations;

public class ProductService : IProductService
{
    private ProductRepository _repository = new ProductRepository();

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
        var newProduct = new Product
        {
            Name = dto.Name,
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

            existingProduct.Name = dto.Name;
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
}

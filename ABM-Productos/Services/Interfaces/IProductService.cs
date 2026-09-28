using ABM_Productos.Models.DTOs.Requests;
using ABM_Productos.Models.DTOs.Responses;

namespace ABM_Productos.Services.Interfaces;

public interface IProductService
{
    List<ProductForReadDto> GetAllProducts();
    ProductForReadDto? GetProductById(int id);
    ProductForReadDto CreateProduct(ProductForCreateDto dto);
    void UpdateProduct(int id, ProductForUpdateDto dto);
    void DeleteProduct(int id);
}

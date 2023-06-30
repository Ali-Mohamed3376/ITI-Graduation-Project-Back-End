﻿

using Final.Project.DAL;

namespace Final.Project.BL;

public interface IProductsManager
{
   public ProductDetailsDto GetProductByID(int id);
   IEnumerable<ProductChildDto> GetAllProductsWithAvgRating();

    public IEnumerable<ProductChildDto> GetAllProductWithDiscount();

    IEnumerable<ProductReadDto> GetAllProducts();
    public bool AddProduct(ProductAddDto productDto, string requestHost, string requestScheme);
    bool EditProduct(ProductEditDto productEditDto);
    bool DeleteProduct(int Id);
}

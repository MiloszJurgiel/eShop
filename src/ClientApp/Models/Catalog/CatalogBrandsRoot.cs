namespace eShop.ClientApp.Models.Catalog;

public class CatalogBrandsRoot
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int Count { get; set; }
    public List<CatalogBrand> Data { get; set; }
}

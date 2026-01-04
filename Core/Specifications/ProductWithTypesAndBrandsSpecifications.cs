using Core.Entities;
using Core.Spcefications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class ProductWithTypesAndBrandsSpecifications:BaseSpecification<Product>
    {
        public ProductWithTypesAndBrandsSpecifications(ProductSpecParams productSpecParams)
            :base(
                 x=>(!productSpecParams.brandId.HasValue || x.ProductBrandId==productSpecParams.brandId) &&
                 (!productSpecParams.typeId.HasValue || x.ProductTypeId == productSpecParams.typeId))
        {
            AddIncludes(x => x.ProductType);
            AddIncludes(x => x.ProductBrand);
            #region Sortting
            AddOrderBy(x => x.Name);
            if (!string.IsNullOrEmpty(productSpecParams.sort))
            {
                switch (productSpecParams.sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(p => p.Price);
                        break;
                    default:
                        AddOrderBy(n => n.Name);
                        break;
                }
            }
            #endregion

            #region Paging
            ApplyPaging(productSpecParams.pageSize * (productSpecParams.PageIndex - 1),productSpecParams.pageSize);
            #endregion

        }
        public ProductWithTypesAndBrandsSpecifications(int id) :base(x => x.Id == id) 
        {
            AddIncludes(x => x.ProductType);
            AddIncludes(x => x.ProductBrand);
        }
    }
}

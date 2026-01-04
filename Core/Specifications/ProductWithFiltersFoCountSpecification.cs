using Core.Entities;
using Core.Spcefications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class ProductWithFiltersFoCountSpecification : BaseSpecification<Product>
    {
        public ProductWithFiltersFoCountSpecification(ProductSpecParams productSpecParams)
            : base(x=>
            (string.IsNullOrEmpty(productSpecParams.Search) || x.Name.ToLower().Contains(productSpecParams.Search)) &&
            (!productSpecParams.brandId.HasValue ||x.ProductBrandId == productSpecParams.brandId )&&
            (!productSpecParams.typeId.HasValue || x.ProductTypeId == productSpecParams.typeId) 

            )
        {
            
        }
    }
}

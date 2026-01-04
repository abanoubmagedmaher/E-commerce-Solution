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
        public ProductWithTypesAndBrandsSpecifications()
        {
            AddIncludes(x => x.ProductType);
            AddIncludes(x => x.ProductBrand);
        }
        public ProductWithTypesAndBrandsSpecifications(int id) :base(x => x.Id == id) 
        {
            AddIncludes(x => x.ProductType);
            AddIncludes(x => x.ProductBrand);
        }
    }
}

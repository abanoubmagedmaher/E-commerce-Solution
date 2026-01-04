using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using E_commerce.Api.Dtos;
using Infrastrucure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase 
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<ProductBrand> _brandRepo;
        private readonly IGenericRepository<ProductType> _typeRepo;
        private readonly IMapper _mapper;

        public ProductController(IGenericRepository<Product> ProductRepo, IGenericRepository<ProductBrand> BrandRepo, IGenericRepository<ProductType> TypeRepo,IMapper mapper)
        {
            _productRepo = ProductRepo;
            _brandRepo = BrandRepo;
            _typeRepo = TypeRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var spec = new ProductWithTypesAndBrandsSpecifications();
            var products = await _productRepo.ListAsyncSpec(spec) ;
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductWithTypesAndBrandsSpecifications(id);
            var product = await _productRepo.GetEntityWithSpec(spec);
            return Ok( _mapper.Map<Product, ProductToReturnDto>(product)) ;
        }

        [HttpGet("brands")]
        public async Task<ActionResult<List<ProductBrand>>> GetProdductBrands()
        {
            return Ok(await _brandRepo.ListAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<List<ProductType>>> GetProductTypes()
        {
            return Ok(await _typeRepo.ListAllAsync());
        }
    }
}

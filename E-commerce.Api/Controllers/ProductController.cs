using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using E_commerce.Api.Dtos;
using E_commerce.Api.Helpers;
using Infrastrucure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<ProductBrand> _brandRepo;
        private readonly IGenericRepository<ProductType> _typeRepo;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;

        public ProductController(IGenericRepository<Product> ProductRepo,
            IGenericRepository<ProductBrand> BrandRepo, IGenericRepository<ProductType> TypeRepo,
            IMapper mapper , IProductService productService
            )
        {
            _productRepo = ProductRepo;
            _brandRepo = BrandRepo;
            _typeRepo = TypeRepo;
            _mapper = mapper;
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<Product>>> GetProducts([FromQuery] ProductSpecParams productSpecParams)
        {
            var spec = new ProductWithTypesAndBrandsSpecifications(productSpecParams);
            var countSpec = new ProductWithFiltersFoCountSpecification(productSpecParams);
            var TotalItems = await _productRepo.CountAsync(countSpec);
            var products = await _productRepo.ListAsyncSpec(spec) ;
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
            return Ok(new Pagination<ProductToReturnDto>(productSpecParams.PageIndex,productSpecParams.pageSize,TotalItems,data));
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

        [HttpPost("createProduct")]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            var createdProduct = await _productService.CreatAsync(product);
            return Ok(createdProduct);
        }
    }
}

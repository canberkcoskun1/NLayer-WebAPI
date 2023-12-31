﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayerWebAPI.API.Filters;
using NLayerWebAPI.Core.DTOs;
using NLayerWebAPI.Core.Entities;
using NLayerWebAPI.Core.Services;

namespace NLayerWebAPI.API.Controllers
{

	public class ProductController : CustomBaseController
	{
		private readonly IMapper _mapper;
		
		private readonly IProductService _service;


		public ProductController(IProductService service,IMapper mapper)
		{
			_service = service;
			_mapper = mapper;
			
		}

		// Get metodları çakışır. Biz özel bir metot yazdığımız için hangisini çağıracağını bilemez. Bu yüzden  bu özel metota [HttpGet("[action]")] belirttik.
		// api.com/products/getproductswithcategory 
		[HttpGet("[action]")]
		public async Task<IActionResult> GetProductWithCategory()
		{
			return CreateActionResult(await _service.GetProductsWithCategory());
		}

		[HttpGet]
		public async Task<IActionResult> All()
		{
			var products = await _service.GetAllAsync();
			// Mapping
			var productsDto = _mapper.Map<List<ProductDto>>(products.ToList());

			return CreateActionResult(CustomResponseDto<List<ProductDto>>.Success(200, productsDto));

		}
		[ServiceFilter(typeof(NotFoundFilter<Product>))]
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var products = await _service.GetByIdAsync(id);
			// Mapping
			var productDto = _mapper.Map<ProductDto>(products);

			return CreateActionResult(CustomResponseDto<ProductDto>.Success(200, productDto));

		}
		[HttpPost]
		public async Task<IActionResult> Save(ProductDto productDto)
		{
			var product = await _service.AddAsync(_mapper.Map<Product>(productDto));
			// Mapping
			var productsDto = _mapper.Map<ProductDto>(product);

			return CreateActionResult(CustomResponseDto<ProductDto>.Success(201, productsDto));

		}
		[HttpPut]
		public async Task<IActionResult> Update(ProductUpdateDto productUpdateDto)
		{
			// Geriye döndüreceği bişey yok ve maplenecek bişey yok o yüzden kaldırdık.
			await _service.UpdateAsync(_mapper.Map<Product>(productUpdateDto));

			return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));

		}
		[HttpDelete("{id}")]
		public async Task<IActionResult> Remove(int id)
		{

			var productId = await _service.GetByIdAsync(id);

			await _service.RemoveAsync(productId);

			return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));

		}
	}

	
}

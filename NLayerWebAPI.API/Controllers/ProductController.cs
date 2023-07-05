﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayerWebAPI.Core.DTOs;
using NLayerWebAPI.Core.Entities;
using NLayerWebAPI.Core.Services;

namespace NLayerWebAPI.API.Controllers
{

	public class ProductController : CustomBaseController
	{
		private readonly IMapper _mapper;
		private readonly IService<Product> _service;



		public ProductController(IService<Product> service, IMapper mapper)
		{
			_service = service;
			_mapper = mapper;
		}
		[HttpGet]
		public async Task<IActionResult> All()
		{
			var products = await _service.GetAllAsync();
			// Mapping
			var productsDto = _mapper.Map<List<ProductDto>>(products.ToList());

			return CreateActionResult(CustomResponseDto<List<ProductDto>>.Success(200, productsDto));

		}
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

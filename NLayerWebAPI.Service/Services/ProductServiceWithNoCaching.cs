using AutoMapper;
using NLayerWebAPI.Core.DTOs;
using NLayerWebAPI.Core.Entities;
using NLayerWebAPI.Core.Repository;
using NLayerWebAPI.Core.Services;
using NLayerWebAPI.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayerWebAPI.Service.Services
{
	public class ProductServiceWithNoCaching : Service<Product>, IProductService
	{
		private readonly IProductRepository _productRepository;
		private readonly IMapper _mapper;
		// UnitOfWork'te context ile ilgili işlem yaptığımız için otomatik olarak gelir.
		public ProductServiceWithNoCaching(IGenericRepository<Product> repository, IUnitOfWork unitOfWork, IMapper mapper, IProductRepository productRepository) : base(repository, unitOfWork)
		{

			_mapper = mapper;
			_productRepository = productRepository;
		}

		public async Task<CustomResponseDto<List<ProductsWithCategoryDto>>> GetProductsWithCategory()
		{
			var products = await _productRepository.GetProductsWithCategory();
			
			var productsDto = _mapper.Map<List<ProductsWithCategoryDto>>(products);

			return CustomResponseDto<List<ProductsWithCategoryDto>>.Success(200,productsDto);

		}
	}
}

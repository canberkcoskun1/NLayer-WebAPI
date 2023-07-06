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
	public class CategoryService : Service<Category>, ICategoryService
	{
		private readonly ICategoryRepository _categoryRepository;
		private readonly IMapper _mapper;
		public CategoryService(IGenericRepository<Category> repository, IUnitOfWork unitOfWork, IMapper mapper, ICategoryRepository categoryRepository) : base(repository, unitOfWork)
		{

			_categoryRepository = categoryRepository;
			_mapper = mapper;
		}

		public async Task<CustomResponseDto<CategoryWithProductsDto>> GetSingleCategoryByIdWithProductAsync(int categoryId)
		{
			var category = await _categoryRepository.GetSingleCategoryByIdWithProductAsync(categoryId);
			var categoryDto = _mapper.Map<CategoryWithProductsDto>(category);
			return CustomResponseDto<CategoryWithProductsDto>.Success(200, categoryDto);
		}
	}
}

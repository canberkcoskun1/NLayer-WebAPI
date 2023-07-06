using NLayerWebAPI.Core.DTOs;
using NLayerWebAPI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayerWebAPI.Core.Services
{
	public interface ICategoryService : IService<Category>
	{
		public Task<CustomResponseDto<CategoryWithProductsDto>> GetSingleCategoryByIdWithProductAsync(int categoryId);
	}
}

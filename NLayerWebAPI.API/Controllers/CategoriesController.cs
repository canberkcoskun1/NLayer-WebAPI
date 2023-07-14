using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLayerWebAPI.API.Filters;
using NLayerWebAPI.API.Middlewares;
using NLayerWebAPI.Core.Entities;
using NLayerWebAPI.Core.Services;
using NLayerWebAPI.Service.Exceptions;

namespace NLayerWebAPI.API.Controllers
{
	
	public class CategoriesController : CustomBaseController
	{
		private readonly ICategoryService _categoryService;

		public CategoriesController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		
		[HttpGet("[action]/{categoryId}")]
		public async Task<IActionResult> GetSingleCategoryByIdWithProducts(int categoryId)
		{
			return CreateActionResult(await _categoryService.GetSingleCategoryByIdWithProductAsync(categoryId));
		}
	}
}

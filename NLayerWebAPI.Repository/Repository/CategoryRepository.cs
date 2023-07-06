using Microsoft.EntityFrameworkCore;
using NLayerWebAPI.Core.Entities;
using NLayerWebAPI.Core.Repository;
using NLayerWebAPI.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayerWebAPI.Repository.Repository
{
	public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
	{
		public CategoryRepository(AppDbContext context) : base(context)
		{
		}

		public async Task<Category> GetSingleCategoryByIdWithProductAsync(int categoryId)
		{
			// Kategorideki ilişkili  productsları içersin ve x burada category'dir. Id ve categoryId eşleşip ilk bulduğu datayı getirsin.
			return await _context.Categories.Include(x => x.Products).Where(x => x.Id == categoryId).SingleOrDefaultAsync();
		}
	}
}

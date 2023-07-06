using NLayerWebAPI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayerWebAPI.Core.Repository
{
	public interface ICategoryRepository : IGenericRepository<Category>
	{
		Task<Category> GetSingleCategoryByIdWithProductAsync(int categoryId);
	}
}

using NLayerWebAPI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayerWebAPI.Core.Repository
{
	public interface IProductRepository : IGenericRepository<Product>
	{
		Task<List<Product>> GetProductsWithCategory();
	}
}

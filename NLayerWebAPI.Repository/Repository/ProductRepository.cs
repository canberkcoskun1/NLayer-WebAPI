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
	public class ProductRepository : GenericRepository<Product>, IProductRepository
	{
		public ProductRepository(AppDbContext context) : base(context)
		{

		}

		public async Task<List<Product>> GetProductsWithCategory()
		{
			return await _context.Products.Include(x => x.Category).ToListAsync();
		}
	}
}

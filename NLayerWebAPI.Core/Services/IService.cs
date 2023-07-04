using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayerWebAPI.Core.Services
{
	public interface IService<T> where T : class
	{
		// API isteklerini işlemek ve iş mantığını uygulamak için vardır. IGenericRepos arabirimini kullanarak veritabanı işlemlerini çağırır.

		Task<IEnumerable<T>> GetAllAsync();
		Task<T> GetByIdAsync(int id);
		IQueryable<T> Async(Expression<Func<T, bool>> expression);
		Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
		Task<T> AddAsync(T entity);
		Task UpdateAsync(T entity);
		Task RemoveAsync(int id);
		Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
		Task RemoveRangeAsync(IEnumerable<T> entities);

	}
}

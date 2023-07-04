using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayerWebAPI.Core.Repository
{
	public interface IGenericRepository<T> where T : class
	{
		// Veritabanında temel sorgular burada yapılır. 

		//GetAll Neden IQueryable? Çünkü değişkene bağlı bir yapı kullandığımızdan ötürü ve anlık olarak değer aldığımız için IEnumerable kullanmıyoruz.
		IQueryable<T> GetAll();
		IQueryable<T> Where(Expression<Func<T, bool>> expression);	
		// Bool olmasının sebebi diyelim ki bir sorgu yazalım; productRepos.Where(x => x.Id > 5) olsun. Eğerki 5'ten büyük IDler varsa true olarak dönecek.
		Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
		Task<T> GetByIdAsync(int id);
		Task AddAsync(T entity);
		void Update(T entity);
		void Remove(T entity);
		void RemoveRange(IEnumerable<T> entities);
		Task AddRangeAsync(IEnumerable<T> entities);
	}
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NLayerWebAPI.Core.DTOs;
using NLayerWebAPI.Core.Entities;
using NLayerWebAPI.Core.Repository;
using NLayerWebAPI.Core.Services;
using NLayerWebAPI.Core.UnitOfWorks;
using NLayerWebAPI.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayerWebAPI.Caching
{
	public class ProductServiceWithCaching : IProductService
	{
		// Solid prensiplerine göre yazacağız. Bize sabit bir CacheKey lazım o yüzden 

		private const string CacheProductKey = "productCache";
		private readonly IMemoryCache _memoryCache;
		private readonly IMapper _mapper;
		private readonly IProductRepository _repository;
		private readonly IUnitOfWork _unitOfWork;

		public ProductServiceWithCaching(IMemoryCache memoryCache, IMapper mapper, IProductRepository repository, IUnitOfWork unitOfWork)
		{
			_memoryCache = memoryCache;
			_mapper = mapper;
			_repository = repository;
			_unitOfWork = unitOfWork;

			if(!_memoryCache.TryGetValue(CacheProductKey, out _))
			{
				//Burada sadece Product'lar cacheleniyor, ama bizim GetProductWithCategory metodumuz olduğundan değişiklik yaptık
				_memoryCache.Set(CacheProductKey, _repository.GetProductsWithCategory().Result);

				//_memoryCache.Set(CacheProductKey,_repository.GetAll().ToList());
			}
		}

		public async Task<Product> AddAsync(Product entity)
		{
			await _repository.AddAsync(entity);
			await _unitOfWork.CommitAsync();
			// Caching
			await CacheAllProductsAsync();
			return entity;
		}

		public async Task<IEnumerable<Product>> AddRangeAsync(IEnumerable<Product> entities)
		{
			await _repository.AddRangeAsync(entities);
			await _unitOfWork.CommitAsync();
			await CacheAllProductsAsync();
			return entities;
		}

		public Task<bool> AnyAsync(Expression<Func<Product, bool>> expression)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Product>> GetAllAsync()
		{
			// Yine Asenkron bir metot ve Senkron bir değer elde edeceğiz bu yüzden Task.FromResult kullanacağız
			return Task.FromResult(_memoryCache.Get<IEnumerable<Product>>(CacheProductKey));
		}

		public Task<Product> GetByIdAsync(int id)
		{
			// Önce Idye göre getirme işlemi
			var product = _memoryCache.Get<List<Product>>(CacheProductKey).FirstOrDefault(x => x.Id == id);
			// Daha sonra ürün null geliyorsa hata fırlatmamız gerekiyor. Ürünün ismini ve idsini alıp hata fırlattık.
			if (product == null)
			{
				throw new NotFoundException($"{typeof(Product).Name}({id}) not found.");
			}
			//** Asenkron dönüş tipi bir metot kullandık ama elimizde senkron bir dönüş tipi varsa Task.FromResult() kullanabiliriz.
			return Task.FromResult(product);
		}

		public Task<CustomResponseDto<List<ProductsWithCategoryDto>>> GetProductsWithCategory()
		{
			//
			var products = _memoryCache.Get<IEnumerable<Product>>(CacheProductKey);
			// Dto dönüştür
			var productWithCategoryDto = _mapper.Map<List<ProductsWithCategoryDto>>(products);
			// Elimizde yine Senkron bir değer var asenkrona dönüştüreceğimiz için Task.FromResult() kullanacağız
			return Task.FromResult(CustomResponseDto<List<ProductsWithCategoryDto>>.Success(200,productWithCategoryDto));

		}

		public async Task RemoveAsync(Product entity)
		{
			_repository.Remove(entity);
			await _unitOfWork.CommitAsync();
			await CacheAllProductsAsync();
			
			
		}

		public async Task RemoveRangeAsync(IEnumerable<Product> entities)
		{
			_repository.RemoveRange(entities);
			await _unitOfWork.CommitAsync();
			await CacheAllProductsAsync();

		}

		public async Task UpdateAsync(Product entity)
		{
			_repository.Update(entity);
			await _unitOfWork.CommitAsync();
			await CacheAllProductsAsync();

		}

		public IQueryable<Product> Where(Expression<Func<Product, bool>> expression)
		{
			// Cache üzerinden CacheProductKey olarak list olarak tutulup, koşulu sağlayan öğeleri filtreleyip Compile ile derledik. Daha sonra AsQueryable() ile bu kolleksiyonu IQueryable nesnesine dönüştürdük. Sorgulanabilir bir kolleksiyon haline getirdik.
			return _memoryCache.Get<List<Product>>(CacheProductKey).Where(expression.Compile()).AsQueryable();
		}
		// Caching için bize bir metot lazım. Bunu çağırdığımız zaman caching işlemini gerçekleştirsin

		public async Task CacheAllProductsAsync()
		{
			_memoryCache.Set(CacheProductKey, _repository.GetAll().ToListAsync());
		}
	}
}

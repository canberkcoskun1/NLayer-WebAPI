using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLayerWebAPI.Core.DTOs;
using NLayerWebAPI.Core.Entities;
using NLayerWebAPI.Core.Services;

namespace NLayerWebAPI.API.Filters
{
	public class NotFoundFilter<T> : IAsyncActionFilter where T : BaseEntity
	{
		private readonly IService<T> _service;

		public NotFoundFilter(IService<T> service)
		{
			_service = service;
		}

		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			// Idye sahip entity varmı diye kontrol gerçekleştirilir.
			var idValues = context.ActionArguments.Values.FirstOrDefault();
			// Id varsa devam etsin 
			if (idValues == null)
			{
				await next.Invoke();
				return;
			}

			// int değer tutmak için casting yapılır
			var id = (int)idValues;
			// Entity varmı diye kontrolü yapılacak.
			var anyEntity = await _service.AnyAsync(x => x.Id ==  id);
			// Entity varmı diye kontrol ettik, varsa Filter'a girmeden devam etmesini istedik.
			if(anyEntity) {

				await next.Invoke();
				return;
			}
			// Burada bir entity veya ID yoksa hata döndürmemiz lazım bu yüzden sonuç olarak CustomResponseDto'nun NotFound ve Fail metodu kullalıp hata kodu döndürmemiz gerekiyor.
			context.Result = new NotFoundObjectResult(CustomResponseDto<NoContentDto>.Fail(404, $"{typeof(T).Name}({id}) not found"));
			


		}
	}
}

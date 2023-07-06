using Microsoft.AspNetCore.Diagnostics;
using NLayerWebAPI.Core.DTOs;
using NLayerWebAPI.Service.Exceptions;
using System.Text.Json;

namespace NLayerWebAPI.API.Middlewares
{
	public static class UseCustomExceptionHandler
	{
		// Extension metodları ilgili sınıflara işlevsellik eklemek ve istemci kodu tarafından doğrudan erişilebilir hale getirmek için statik bir sınıfta tanımlamak gerekmektedir.
		public static void UserCustomException(this IApplicationBuilder app)
		{
			// Run sonlandırıcı middlewaredir. 
			app.UseExceptionHandler(config =>
			{
				config.Run(async context =>
				{

					// Bu kısımda hatanın döneceği tipi belirledik.
					context.Response.ContentType = "application/json";
					// Burada ise hatayı verecek interface'i çağırıp fırlatılan hatayı tutarız. Bunu ayırmamız gerekiyor client tarafından bir hata olabilir bu yüzden bir Client'a ait exception sınıfı oluşturacağız. Uygulamadan kaynaklı hata mı yoksa clienttan kaynaklı olup olmadığını ayırmak için yapıyoruz.

					var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
					// Servis tarafında Exceptions adlı klasör oluşturup ClientSideException olarak ayırt edecek. "ClientSideException.cs"

					// Burada 400 ve 500'e ait hataları karşılaştırmak için switch kullandık. 
					var statusCode = exceptionFeature.Error switch
					{
						ClientSideException => 400,
						_ => 500
					};

					context.Response.StatusCode = statusCode;
					// response'u dtomuzda döndürmemiz gerekiyor , başarısız durum olduğundan dolayı NoContentDto çağırılır.
					var response = CustomResponseDto<NoContentDto>.Fail(statusCode, exceptionFeature.Error.Message);
					// En sonunda ise bu hatamızı JSON formatına döndürmemiz gerektiğinden ötürü Serialize etmemiz gerekiyor.

					await context.Response.WriteAsync(JsonSerializer.Serialize(response));  
					
				});
			});
		}
	}
}

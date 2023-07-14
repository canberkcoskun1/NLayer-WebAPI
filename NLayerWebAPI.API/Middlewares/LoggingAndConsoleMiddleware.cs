using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NLayerWebAPI.API.Middlewares;
using Serilog;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

public class LoggingAndConsoleMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<LoggingAndConsoleMiddleware> _logger;

	public LoggingAndConsoleMiddleware(RequestDelegate next, ILogger<LoggingAndConsoleMiddleware> logger)
	{
		_next = next;
		_logger = logger;
	}
	public async Task Invoke(HttpContext context)
	{
		//Burada request methodu ve requestin path'ini tuttuk.
		_logger.LogInformation($"REQUEST_METHOD: {context.Request.Method}");
		_logger.LogInformation($"REQUEST_PATH: {context.Request.Path}");

		// Response'nin body'sini yakalamak için responseStream adlı değişken oluşturduk
		var responseStream = context.Response.Body;

		using (var memoryStream = new MemoryStream())
		{

			// Response gövdesini depolamak için memoryStream kullandık. StreamReader ile bu depoya ulaşarak ReadToEndAsync() metodu ile deponun tamamı yani request'in body'sini okumayı sağladık.
			context.Response.Body = memoryStream;

			await _next(context);

			// bu metotla okuma ve yazma konumunu değiştirmek için kullanılır. 0 parametresi öteleme yapılmayacağını, 2.ci parametre ötelemenin başlangıç konumunu belirtir. Bu sayede başlangıç konumuna geri döndürür ve baştan itibaren okunmasını ve kopyalanmasını sağlar.
			memoryStream.Seek(0, SeekOrigin.Begin);

			// memoryStream'i bir string değere atayabilelim ki loglama işlemi gerçekleşsin. Encoding UTF8 Kullanarak metinleri dönüştürmeyi sağladık.
			var responseBody = await new StreamReader(memoryStream, Encoding.UTF8).ReadToEndAsync();

			
			Log.Information($"RESPONSE_BODY: {responseBody}");
			Log.Information($"RESPONSE_STATUS_CODE: {context.Response.StatusCode}");

			// bu metotla okuma ve yazma konumunu değiştirmek için kullanılır. 0 parametresi öteleme yapılmayacağını, 2.ci parametre ötelemenin başlangıç konumunu belirtir. Bu sayede başlangıç konumuna geri döndürür ve baştan itibaren okunmasını ve kopyalanmasını sağlar.
			memoryStream.Seek(0, SeekOrigin.Begin);
			// buradaki metot ise memoryStreamdeki verileri responseStream'e kopyalar çünkü orjinal akışına geri döndürebilelim.
			await memoryStream.CopyToAsync(responseStream);
		}
	}

	

}

using System.Text;

namespace NLayerWebAPI.API.Middlewares
{

	public class LogConsoleMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<LogConsoleMiddleware> _logger;
		public LogConsoleMiddleware(RequestDelegate next, ILogger<LogConsoleMiddleware> logger)
		{
			_next = next;
			// loglama için gerekli.
			_logger = logger;
		}

		public async Task Invoke(HttpContext context)
		{
			// Request bilgilerini loglama
			LogRequestInformation(context.Request);

			// Response gövdesini yakalamak ve depolamak için memoryStream kullandık. StreamReader ile bu depoya ulaşarak ReadToEndAsync() metodu ile deponun tamamı yani request'in body'sini okumayı sağladık.
			var responseStream = context.Response.Body;
			// Bellekte veri depolamayı sağlayan veri okuma ve yazılması için memoryStream nesnesi oluşturduk. usingle başlatmamızın sebebi, bu kod bloğu tamamlandığında nesne temizlenir. Bellekte yer kaplamaması sağlanır.
			using (var memoryStream = new MemoryStream())
			{
				//Response body'si memoryStream'e aktarılır
				context.Response.Body = memoryStream;

				await _next.Invoke(context);

				//Bu metotla okuma ve yazma konumunu değiştirmek için kullanılır. 0 parametresi öteleme yapılmayacağını, 2.ci parametre ötelemenin başlangıç konumunu belirtir. Bu sayede başlangıç konumuna geri döndürür ve baştan itibaren okunmasını ve kopyalanmasını sağlar.
				memoryStream.Seek(0, SeekOrigin.Begin);
				// MemoryStream nesnesi üzerinden StreamReader kullanarak verileri okuruz.  UTF-8, UNICODE karakter setini temsil eden bir karakter kodlaması. Farklı dillerdeki tüm karakterleri kapsar. Buradaki amacı memoryStream içindeki verileri okurken UTF8 kullandığından StreamReader doğru karakter kodlamasını kullanarak memoryStreamden okuma yapar. ReadToEnd sayesinde dizindeki dosyada tüm değerler elde edilir.
				var responseBody = await new StreamReader(memoryStream, Encoding.UTF8).ReadToEndAsync();

				// Response bilgilerini loglamak için LogResponseInfo metodu çağırılır.
				LogResponseInformation(context.Response, responseBody);
				// bu metotla okuma ve yazma konumunu değiştirmek için kullanılır. 0 parametresi öteleme yapılmayacağını, 2.ci parametre ötelemenin başlangıç konumunu belirtir. Bu sayede başlangıç konumuna geri döndürür ve baştan itibaren okunmasını ve kopyalanmasını sağlar.
				memoryStream.Seek(0, SeekOrigin.Begin);

				await memoryStream.CopyToAsync(responseStream);
			}
		}

		// Logları tutmak , txt dosyasına yazdırmak ve console'da yazdırmak için Request ve Response durumları için Metot oluşturduk.
		private void LogRequestInformation(HttpRequest request)
		{
			string logMessage = $"REQUEST_METHOD:{request.Method}\nREQUEST_PATH:{request.Path}";

			_logger.LogInformation(logMessage);

			// StreamWriter nesnesi oluşturulup değişkene atadık, AppendText sayesinde dosya oluşturmaya sağlar, burada da log.txt dosyamızı oluşturdu.
			using (StreamWriter writer = File.AppendText("log.txt"))
			{
				writer.WriteLine(logMessage);
			}
		}

		private void LogResponseInformation(HttpResponse response, string responseBody)
		{
			string logMessage = $"RESPONSE_BODY:{responseBody}\nRESPONSE_STATUSCODE:{response.StatusCode}";
			// logger sayesinde Console'a loglarımızı gösterebiliyoruz.
			_logger.LogInformation(logMessage);
			// tanımladığımız logMessage değişkeni ile StreamWriter sayesinde log.txt dosyası açılır ve verim eklemek için StreamWriter nesnesi oluşturduk. writer.WriteLine sayesinde ise StreamWriter nesnesi üzerinden .txt dosyamıza logları aktarırız.
			using (StreamWriter writer = File.AppendText("log.txt"))
			{
				writer.WriteLine(logMessage);
			}
		}
	}

}


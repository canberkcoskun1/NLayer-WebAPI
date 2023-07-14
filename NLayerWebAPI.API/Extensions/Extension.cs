using NLayerWebAPI.API.Middlewares;

namespace NLayerWebAPI.API.Extensions
{
	public static class Extension
	{
		public static IApplicationBuilder UseLoggingAndConsoleMiddleware(this IApplicationBuilder applicationBuilder)
		{
			return applicationBuilder.UseMiddleware<LoggingAndConsoleMiddleware>();
		}
	}
}

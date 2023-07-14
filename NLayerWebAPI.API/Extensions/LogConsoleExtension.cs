using NLayerWebAPI.API.Middlewares;

namespace NLayerWebAPI.API.Extensions
{
	public static class LogConsoleExtension
	{
		public static IApplicationBuilder UseLogConsoleMiddleware(this IApplicationBuilder applicationBuilder)
		{
			return applicationBuilder.UseMiddleware<LogConsoleMiddleware>();
		}
	}
}

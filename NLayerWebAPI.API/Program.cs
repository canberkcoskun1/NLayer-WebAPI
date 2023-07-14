
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NLayerWebAPI.API.Extensions;
using NLayerWebAPI.API.Filters;
using NLayerWebAPI.API.Middlewares;
using NLayerWebAPI.API.Modules;
using NLayerWebAPI.Core.Repository;
using NLayerWebAPI.Core.Services;
using NLayerWebAPI.Core.UnitOfWorks;
using NLayerWebAPI.Repository.Context;
using NLayerWebAPI.Repository.Repository;
using NLayerWebAPI.Repository.UnitOfWorks;
using NLayerWebAPI.Service.Mapping;
using NLayerWebAPI.Service.Services;
using NLayerWebAPI.Service.Validations;
using Serilog;
using Serilog.Events;
using System.Reflection;

namespace NLayerWebAPI.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers(options => options.Filters.Add(new ValidateFilterAttribute())).AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>());
			builder.Services.Configure<ApiBehaviorOptions>(options =>
			{
				// Kendi yazdýðýmýz filtreyi kullanmamýzý saðlar.
				options.SuppressModelStateInvalidFilter = true;
			});
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			//Cache
			builder.Services.AddMemoryCache();
			// Filter
			builder.Services.AddScoped(typeof(NotFoundFilter<>));
			//builder.Services.AddLogging();


			//AutoMapper
			builder.Services.AddAutoMapper(typeof(MapProfile));

			//DbContext
			builder.Services.AddDbContext<AppDbContext>(x =>
			{
				x.UseSqlServer(builder.Configuration.GetConnectionString("NLayerWebApiString"), option =>
				{
					option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
				});
			});

			// Autofac çaðýrýlýr.
			builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
			// Oluþturulan modülümüzü ekledik.
			builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepoServiceModule()));
			//Log.Logger = new LoggerConfiguration()
			//	.WriteTo.Console()
			//	.WriteTo.File("Testlog-.txt", rollingInterval: RollingInterval.Day)
			//	.CreateLogger();
			

			builder.Logging.AddSerilog();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			

			app.UseHttpsRedirection();
			//app.UseMiddleware<UseCustomExceptionHandler>();
			//app.UseLoggingAndConsoleMiddleware();
			app.UseLogConsoleMiddleware();



			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
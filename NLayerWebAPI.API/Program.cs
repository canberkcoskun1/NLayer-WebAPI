
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NLayerWebAPI.API.Filters;
using NLayerWebAPI.Core.Repository;
using NLayerWebAPI.Core.Services;
using NLayerWebAPI.Core.UnitOfWorks;
using NLayerWebAPI.Repository.Context;
using NLayerWebAPI.Repository.Repository;
using NLayerWebAPI.Repository.UnitOfWorks;
using NLayerWebAPI.Service.Mapping;
using NLayerWebAPI.Service.Services;
using NLayerWebAPI.Service.Validations;
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


			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
			builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
			builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));



			builder.Services.AddScoped<IProductRepository, ProductRepository>();
			builder.Services.AddScoped<IProductService, ProductService>();
			
			builder.Services.AddScoped<ICategoryService, CategoryService>();
			builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

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
			
			


			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();
			app.UseExceptionHandler();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
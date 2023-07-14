using Module = Autofac.Module;
using NLayerWebAPI.Repository.Context;
using NLayerWebAPI.Service.Mapping;
using System.Reflection;
using Autofac;
using NLayerWebAPI.Repository.Repository;
using NLayerWebAPI.Core.Repository;
using NLayerWebAPI.Service.Services;
using NLayerWebAPI.Core.Services;
using NLayerWebAPI.Repository.UnitOfWorks;
using NLayerWebAPI.Core.UnitOfWorks;
using NLayerWebAPI.Caching;

namespace NLayerWebAPI.API.Modules
{
	public class RepoServiceModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{

			// Service ve Repos için generic classlarımız da eklenmelidir.

			builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
			builder.RegisterGeneric(typeof(Service<>)).As(typeof(IService<>)).InstancePerLifetimeScope();

			// Uow de eklenir 
			builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

			//Caching
			builder.RegisterType<ProductServiceWithCaching>().As<IProductService>();


			// Assemblyler alınır.
			var apiAssembly = Assembly.GetExecutingAssembly();
			var repoAssembly = Assembly.GetAssembly(typeof(AppDbContext));
			var serviceAssembly = Assembly.GetAssembly(typeof(MapProfile));


			// Interface'lerin son ekinden ve Repositorylerin son ekinden yararlanıp (service, repos vb.) implement edeceğiz ve AddScoped yerine geçen InstanceForLifetimeScope özelliğini kullanacağız.
			// Repos
			builder.RegisterAssemblyTypes(apiAssembly,repoAssembly,serviceAssembly).Where(x => x.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();
			builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope();

		}
	}
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineStore.AppServices.Attributes.Repositories;
using OnlineStore.AppServices.Attributes.Services;
using OnlineStore.DataAccess.Attributes.Repositories;
using OnlineStore.DataAccess.Common;
using OnlineStore.Infrastructure.Mappings;

namespace OnlineStore.ComponentRegistar
{
	/// <summary>
	/// Класс регистрации компонентов приложения
	/// </summary>
	public static class OnlineStoreRegistar
	{
		public static void AddComponents(IServiceCollection Services, IConfiguration Configuration)
		{
			RegisterRepositories(Services,Configuration);
			RegisterServices(Services);
			RegisterMapper(Services,Configuration);
			
		}

		private static void RegisterRepositories(IServiceCollection Services, IConfiguration Configuration)
		{
			Services.AddDbContext<OnlineStoreDbContext>(options =>
			{
				var connectionString = Configuration.GetConnectionString("DefaultConnection");
				options.UseSqlServer(connectionString);
			});

			Services.AddTransient<IAttributeRepository, AttributeRepository>();
		}

		private static void RegisterServices(IServiceCollection Services)
		{
			Services.AddScoped<IProductAttributeService, ProductAttributeService>();
		}

		private static void RegisterMapper(IServiceCollection Services, IConfiguration Configuration)
		{
			var mapperConfig = new MapperConfiguration(mc =>
			{
				mc.AddProfile(new ProductAttributeMappingProfile());
			}
			);
			IMapper mapper = mapperConfig.CreateMapper();
			Services.AddSingleton(mapper);
		}
	}
}

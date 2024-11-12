using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineStore.AppServices.Attributes.Repositories;
using OnlineStore.AppServices.Attributes.Services;
using OnlineStore.AppServices.Common.CacheService;
using OnlineStore.AppServices.Common.CacheServices;
using OnlineStore.AppServices.Common.Models;
using OnlineStore.AppServices.Common.Redis;
using OnlineStore.AppServices.Products.Repositories;
using OnlineStore.AppServices.Products.Services;
using OnlineStore.DataAccess.Attributes.Repositories;
using OnlineStore.DataAccess.Common;
using OnlineStore.DataAccess.Products.Repositories;
using OnlineStore.Domain.Entities;
using OnlineStore.Infrastructure.Mappings;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;


namespace OnlineStore.ComponentRegistar
{
	/// <summary>
	/// Класс регистрации компонентов приложения
	/// </summary>
	public static class OnlineStoreRegistar
	{
		public static void AddComponents(IServiceCollection Services, IConfiguration Configuration)
		{
			Services.AddIdentity<ApplicationUser,ApplicationRole>()
				.AddEntityFrameworkStores<MutableOnlineStoreDbContext>()
				.AddDefaultTokenProviders();
			RegisterRepositories(Services,Configuration);
			RegisterServices(Services, Configuration);
			RegisterMapper(Services,Configuration);
			
		}

		private static void RegisterRepositories(IServiceCollection Services, IConfiguration Configuration)
		{
			Services.AddDbContext<MutableOnlineStoreDbContext>(options =>
			{
				var connectionString = Configuration.GetConnectionString("DefaultConnection");
				options.UseSqlServer(connectionString);
			});
			Services.AddDbContext<ReadOnlyOnlineStoreDbContext>(options =>
			{
				var connectionString = Configuration.GetConnectionString("DefaultConnection");
				options.UseSqlServer(connectionString);
			});

			Services.AddTransient<IAttributeRepository, AttributeRepository>();
			Services.AddScoped<IProductRepository,ProductRepository>();
		}

		private static void RegisterServices(IServiceCollection Services,IConfiguration Configuration)
		{
			var redisConfiguration = Configuration
				.GetSection("Redis")
				.Get<RedisConfiguration>();

			Services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>(redisConfiguration);

			Services.AddScoped<IProductAttributeService, ProductAttributeService>();
			Services.Decorate<IProductAttributeService, CachedProductAttributeService>();

			Services.AddScoped<IProductsService,ProductsService>();

			Services.AddSingleton<IRedisCache, RedisCache>();
			Services.AddSingleton<ICacheService, RedisCacheService>();

			Services.Configure<DecoratorSetting>(Configuration.GetSection("DecoratorSetting"));
			var decorationSetting = Configuration.GetSection("DecoratorSetting").Get<DecoratorSetting>();

			if (decorationSetting?.EnableDecorator == true)
			{
				Services.Decorate<IProductAttributeService, CachedProductAttributeService>();
			}

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

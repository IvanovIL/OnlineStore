using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineStore.AppServices.Attributes.Repositories;
using OnlineStore.AppServices.Attributes.Services;
using OnlineStore.AppServices.Authentication.Services;
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
using OnlineStore.Infrastructure.JwtGenerator;
using OnlineStore.Infrastructure.Mappings;
using OnlineStoreApiClients;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using OnlineStore.DataAccess.Middlewares;
using OnlineStore.DataAccess.Events;
using OnlineStore.AppServices.Common.DataTimeProviders;
using OnlineStore.AppServices.Common.NotificationServices;
using OnlineStore.AppServices.Common.Events.Handlers;
using OnlineStore.AppServices.Common.Events.Common;
using OnlineStore.AppServices.Categories.Repositories;
using OnlineStore.DataAccess.Categories.Repositories;
using OnlineStore.AppServices.Categories.Services;

namespace OnlineStore.ComponentRegistar
{
    /// <summary>
    /// Класс регистрации компонентов приложения
    /// </summary>
    public static class OnlineStoreRegistar
    {
        public static void AddComponents(IServiceCollection Services, IConfiguration Configuration)
        {
            Services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<MutableOnlineStoreDbContext>()
                .AddDefaultTokenProviders();

            var jwtOptions = Configuration.GetSection("JwtOptions").Get<JwtOptions>();
            Services.AddAuthentication()
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))
                    };
                });

            Services.Configure<JwtOptions>(Configuration.GetSection("JwtOptions"));
            Services.Configure<OnlineStoreApiClientOptions>(Configuration.GetSection("OnlineStoreApiClient"));

            RegisterRepositories(Services, Configuration);
            RegisterServices(Services, Configuration);
            RegisterMapper(Services, Configuration);
            RegisterApiClient(Services, Configuration);
            RegisterScheduler(Services, Configuration);
        }

        public static void RegisterMiddlewares(WebApplication app)
        {
            app.UseMiddleware<TransactionMiddleware>();
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
            Services.AddScoped<IProductRepository, ProductRepository>();
            Services.AddScoped<ICategoryRepository, CategoryRepository>();
        }

        private static void RegisterServices(IServiceCollection Services, IConfiguration Configuration)
        {
            var redisConfiguration = Configuration
                .GetSection("Redis")
                .Get<RedisConfiguration>();

            Services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>(redisConfiguration);

            Services.AddScoped<IProductAttributeService, ProductAttributeService>();

            Services.AddScoped<IProductsService, ProductsService>();
            Services.AddScoped<IAuthenticationService, AuthenticationService>();
            Services.AddScoped<ICategoryService, CategoryService>();

            Services.AddSingleton<IRedisCache, RedisCache>();
            Services.AddSingleton<ICacheService, RedisCacheService>();
            Services.AddSingleton<IJwtGenerator, JwtGenerator>();
            Services.AddSingleton<IDataTimeProvider, DataTimeProvider>();


            Services.AddScoped<IEventDispatcher, EventDispatcher>();
            Services.AddScoped<IEventAccumulator, EventAccumulator>();
            Services.AddScoped<INotificationService, EmailNotificationService>();

            Services.Configure<DecoratorSettings>(Configuration.GetSection("DecoratorSettings"));
            var decorationSettings = Configuration.GetSection("DecoratorSettings").Get<DecoratorSettings>();
            if (decorationSettings?.EnableDecorator == true)
            {
                Services.Decorate<IProductAttributeService, CachedProductAttributeService>();
            }

            Services.Scan(Scan =>
            {
                Scan.FromAssemblyOf<AddProductEventHandler>()
                .AddClasses(Classes => Classes.AssignableTo(typeof(IDomainEventHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime();

            });

        }

        private static void RegisterMapper(IServiceCollection Services, IConfiguration Configuration)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ProductAttributeMappingProfile());
                mc.AddProfile(new ProductMappingProfile());
            }
            );
            IMapper mapper = mapperConfig.CreateMapper();
            Services.AddSingleton(mapper);
        }


        public static void RegisterScheduler(IServiceCollection Services, IConfiguration Configuration)
        {
            Services.AddHangfire(conf =>
                conf.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection"))
            );
            Services.AddHangfireServer();
        }
        private static void RegisterApiClient(IServiceCollection Services, IConfiguration Configuration)
        {
            Services.AddHttpClient<IOnlineStoreApiClient, OnlineStoreApiClient>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7223/api/");

            });

        }
    }
}

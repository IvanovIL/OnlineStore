using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineStore.AppServices.Attributes.Repositories;
using OnlineStore.AppServices.Attributes.Services;
using OnlineStore.ComponentRegistar;
using OnlineStore.DataAccess.Attributes.Repositories;
using OnlineStore.Infrastructure.Mappings;



namespace OnlineStore.MVC
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();

			OnlineStoreRegistar.AddComponents(builder.Services, builder.Configuration);

			//builder.Services.AddDbContext<DbContext>(options =>
			//{
			//	var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
			//	options.UseSqlServer(connectionString);
			//}
			//);

			//builder.Services.AddTransient<IAttributeRepository, AttributeRepository>();
			//builder.Services.AddScoped<IProductAttributeService, ProductAttributeService>();
			//var mapperConfig = new MapperConfiguration(mc =>
			//{
			//	mc.AddProfile(new ProductAttributeMappingProfile());
			//}
			//);
			//IMapper mapper = mapperConfig.CreateMapper();
			//builder.Services.AddSingleton(mapper);

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}

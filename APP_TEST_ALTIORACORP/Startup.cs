using APP_TEST_ALTIORACORP.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

namespace APP_TEST_ALTIORACORP
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<AltioraContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services
                .AddMvc()
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Clientes/Error");
            }

            app.UseStaticFiles();
            app.UseMvc(c => RouteConfig.Use(c));


        }

        public static class RouteConfig
        {
            public static IRouteBuilder Use(IRouteBuilder routeBuilder)
            {

                routeBuilder.MapRoute(name: "default",
                    template: "{controller=Clientes}/{action=Index}");

                routeBuilder.MapRoute(
                    name: "Pedidos",
                    template: "{controller=Pedidos}/{action=PeIndex}");

                routeBuilder.MapRoute(
                name: "Productos",
                template: "{controller=Productos}/{action=PIndex}");

                return routeBuilder;
            }
        }
    }
}

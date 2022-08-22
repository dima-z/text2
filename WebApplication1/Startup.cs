using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebApplication
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration
    {
      get;
    }

    public void ConfigureServices(IServiceCollection services) => services.AddControllers();

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
        //endpoints.MapControllerRoute("1", "{get}", new
        //{
        //  controller = "Home",
        //  action = "Get"
        //});
        //endpoints.MapControllerRoute("2", "{get2}", new
        //{
        //  controller = "Home",
        //  action = "Get2"
        //});
      });
    }
  }
}

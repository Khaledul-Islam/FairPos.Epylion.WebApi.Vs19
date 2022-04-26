using FairPos.Epylion.Logger.Contracts;
using FairPos.Epylion.WebApi.Vs19.Extension;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FairPos.Epylion.WebApi.Vs19
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

            services.AddCors();
            services.ConfigureDatabaseContext(Configuration);//Adding context
            services.ConfigureRepositoryWrapper();//Calling Repositories
            services.ConfigureJWTBearer(Configuration);// configure jwt token
            services.ConfigureSwagger();//calling swagger
            services.AuthorizationWrapper();//calling Authorization    
            services.ConfigureAutoMapper();//Configure Mapper
            services.ConnfigControllers();//config controllers
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,ILog logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
            }
            app.ConfigureExceptionHandler(logger);
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FairPos.Epylion.WebApi.Vs19 v1"));

            app.UseHttpsRedirection();

            app.UseRouting();
          

            // global cors policy
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); // allow credentials

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}");
                //endpoints.MapControllers();
            });
        }
    }
}

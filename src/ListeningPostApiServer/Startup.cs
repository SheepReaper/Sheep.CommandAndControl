using ListeningPostApiServer.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ListeningPostApiServer
{
    /// <summary>
    /// Startup class handles Dependency Injection Container configuration and Http Pipeline
    /// configuration. Consumed by a WebHostBuilder.
    /// </summary>
    public class Startup
    {
        #region Fields

        /// <summary>
        /// This is the "Name" of the CORS policy. Arbitrary, but required.
        /// </summary>
        /// <remarks>I've yet to see it actually referenced anywhere besides the constructor.</remarks>
        private const string MyAllowSpecificOrigins = "AllowAll";

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup" /> class.
        /// </summary>
        /// <param name="configuration">The Configuration Interface injected by the HostBuilder.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Exposes the configuration environment.
        /// </summary>
        /// <value>Configuration environment Interface.</value>
        /// <remarks>
        /// Grants access to the configuration environment as implemented externally. Injected via
        /// Dependency Injection by the Host Container.
        /// </remarks>
        public IConfiguration Configuration { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Configures the Http Pipeline.
        /// </summary>
        /// <param name="app">An Application Builder interface.</param>
        /// <param name="env">Interface to the Hosting Environment.</param>
        /// <remarks>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </remarks>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app
                    .UseDeveloperExceptionPage()
                    .UseCors(MyAllowSpecificOrigins);
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production
                // scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            app
                //.UseHttpsRedirection()
                .UseStaticFiles()
                .UseSwagger()
                .UseSwaggerUI(options =>
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Listening Post API v1"))
                .UseMvc();
        }

        /// <summary>
        /// Configures the Dependency Injection Container. (Registers Services).
        /// </summary>
        /// <param name="services">The Service Collection (Container) to configure.</param>
        /// <remarks>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </remarks>
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .ConfigureAppDbContext()
                .ConfigureRepositoryInjection()
                .ConfigureSwagger()
                //.ConfigureHttps()
                .ConfigureCors(MyAllowSpecificOrigins);

            services.ConfigureMvc().AddMvcOptions(options => options.EnableEndpointRouting = false);
        }

        #endregion Methods
    }
}

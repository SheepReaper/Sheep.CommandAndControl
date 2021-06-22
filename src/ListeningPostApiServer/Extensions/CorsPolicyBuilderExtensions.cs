using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ListeningPostApiServer.Extensions
{
    ///
    public static class CorsPolicyBuilderExtensions
    {
        ///
        public static CorsPolicyBuilder WithSmartOrigin(this CorsPolicyBuilder builder, IServiceCollection services,
            IConfiguration appConfig) =>
            services.BuildServiceProvider().GetService<IWebHostEnvironment>()?.IsDevelopment() ?? false
                ? builder.AllowAnyOrigin()
                : builder.WithOrigins(appConfig["CORS.AllowOrigins"]);
    }
}
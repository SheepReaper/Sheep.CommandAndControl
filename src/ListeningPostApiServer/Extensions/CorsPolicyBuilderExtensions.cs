using System;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ListeningPostApiServer.Extensions
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class CorsOptions
    {
        public const string ConfigKey = "Cors";
        public string[] AllowOrigins { get; set; } = Array.Empty<string>();

        public void Deconstruct(out string[] origins) => origins = AllowOrigins;
    }

    ///
    public static class CorsPolicyBuilderExtensions
    {
        ///
        public static CorsPolicyBuilder WithSmartOrigin(this CorsPolicyBuilder builder, IServiceCollection services,
            IConfiguration appConfig) =>
            services.BuildServiceProvider().GetService<IWebHostEnvironment>()?.IsDevelopment() ?? false
                ? builder.AllowAnyOrigin()
                : builder.WithOrigins(appConfig.GetSection(CorsOptions.ConfigKey).Get<CorsOptions>().AllowOrigins);
    }
}
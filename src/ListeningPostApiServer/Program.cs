using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ListeningPostApiServer
{
    /// <summary>
    ///     Type definition of an instance of the Api Server Program.
    /// </summary>
    public class Program
    {
        #region Methods

        /// <summary>
        ///     Creates and returns a WebHostBuilder object via an IWebHostBuilder interface.
        /// </summary>
        /// <param name="args">
        ///     Typically some or all of the command line parameters passed into the executable.
        /// </param>
        /// <returns>A Builder object compatible with the IWebHostBuilder interface.</returns>
        /// <remarks>
        ///     The WebHostBuilder returned can be used to generate a concrete WebHost. The WebHost is
        ///     the core of the server program, hosted inside of a .NET Core Application Host Thread.
        /// </remarks>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        /// <summary>
        ///     Defines the entry point of the application.
        /// </summary>
        /// <param name="args">Command line parameters passed in.</param>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        #endregion Methods
    }
}
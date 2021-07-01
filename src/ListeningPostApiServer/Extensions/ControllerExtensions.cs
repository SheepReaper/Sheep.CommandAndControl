using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ListeningPostApiServer.Extensions
{
    /// <summary>
    ///     A collection of helpers, mostly for debugging.
    /// </summary>
    public static class ControllerExtensions
    {
        /// <summary>
        ///     Logs the specified message to console and saves a bunch of typing..
        /// </summary>
        /// <param name="_">The controller.</param>
        /// <param name="message">   The message.</param>
        public static void Log<TController>(this TController _, string message) where TController : ControllerBase
        {
            var fancy = "".PadLeft(18, '-');
            var category = typeof(TController).Name;

            Debug.WriteLine($"{fancy}\n{category} - {message}\n{fancy}");
        }
    }
}
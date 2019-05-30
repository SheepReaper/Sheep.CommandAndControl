﻿using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ListeningPostApiServer.Extensions
{
    /// <summary>
    /// A collection of helpers, mostly for debugging.
    /// </summary>
    public static class ControllerExtensions
    {
        #region Methods

        /// <summary>
        /// Logs the specified message to console and saves a bunch of typing..
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="message">   The message.</param>
        public static void Log(this ControllerBase controller, string message)
        {
            var fancy = "".PadLeft(18, '-');

            Debug.WriteLine($"{fancy}\n{message}\n{fancy}");
        }

        #endregion Methods
    }
}

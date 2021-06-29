namespace ListeningPostApiServer.Controllers
{
    /// <summary>
    ///     Enum-Like that defines the names of the CORS policies available in the server
    /// </summary>
    public static class CorsPolicyType
    {
        /// <summary>
        ///     This policy includes only the origin configured in appsettings.json. Only good for the most basic of GETs
        /// </summary>
        public const string MinimalGet = "MinimalGet";

        /// <summary>
        ///     This policy includes the configured origin as well as the content-type header.
        /// </summary>
        public const string MinimalPost = "MinimalPost";

        /// <summary>
        ///     This policy includes the configured origin and the headers sent by dropzone.js
        /// </summary>
        public const string DropzoneUpload = "DropzoneUpload";
    }
}
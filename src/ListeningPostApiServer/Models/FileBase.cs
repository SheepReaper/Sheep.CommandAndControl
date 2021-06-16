using System;
using Newtonsoft.Json;

namespace ListeningPostApiServer.Models
{
    /// <inheritdoc />
    /// <summary>
    ///     The Base class of File Records in the database. Implements the
    ///     <see
    ///         cref="T:ListeningPostApiServer.Models.EntityBase" />
    /// </summary>
    /// <seealso cref="T:ListeningPostApiServer.Models.EntityBase" />
    public abstract class FileBase : EntityBase
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:ListeningPostApiServer.Models.FileBase" /> class.
        /// </summary>
        protected FileBase() => Updated = DateTime.UtcNow;

        #endregion Constructors

        #region Properties

        /// <summary>
        ///     Gets or sets the actual name of the file (File Name excluding Path).
        /// </summary>
        /// <value>The actual name of the file.</value>
        [JsonProperty("filename")]
        public string ActualFileName { get; set; } = "";

        /// <summary>
        ///     Gets or sets the type of the file (Payload, or Exfiltrated).
        /// </summary>
        /// <value>The type of the file.</value>
        public FileType FileType { get; set; }

        /// <summary>
        ///     Gets or sets the temporary file path.
        /// </summary>
        /// <value>The temporary file path.</value>
        /// <remarks>
        ///     All files in this system persist only within the temporary directories of the application
        ///     host. The temp file path is the real directory for the file, as stored on the server.
        ///     Needed for retrieval.
        /// </remarks>
        public string TempFilePath { get; set; } = "";

        /// <summary>
        ///     Gets or sets the date/time the file record was updated.
        /// </summary>
        /// <value>The updated.</value>
        public DateTime Updated { get; set; }

        #endregion Properties
    }
}
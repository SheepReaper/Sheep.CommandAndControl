namespace ListeningPostApiServer.Models
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a File record in the database, specifically a file that was retrieved from a
    /// remote system by an agent. Implements the <see
    /// cref="T:ListeningPostApiServer.Models.FileBase" />
    /// </summary>
    /// <seealso cref="T:ListeningPostApiServer.Models.FileBase" />
    public class ExfiltratedFile : FileBase
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="T:ListeningPostApiServer.Models.ExfiltratedFile" /> class.
        /// </summary>
        public ExfiltratedFile()
        {
            FileType = FileType.Exfiltrated;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the associated implant.
        /// </summary>
        /// <value>From implant.</value>
        public virtual Implant FromImplant { get; set; }

        #endregion Properties
    }
}

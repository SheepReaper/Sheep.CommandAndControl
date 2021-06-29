using System.Collections.Generic;

namespace ListeningPostApiServer.Models
{
    /// <inheritdoc />
    /// <summary>
    ///     Represents a File Record in the database, specifically one that is meant to deploy and
    ///     execute on an agent Implements the <see cref="T:ListeningPostApiServer.Models.FileBase" />
    /// </summary>
    /// <seealso cref="T:ListeningPostApiServer.Models.FileBase" />
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class PayloadFile : FileBase
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:ListeningPostApiServer.Models.PayloadFile" /> class.
        /// </summary>
        public PayloadFile()
        {
            FileType = FileType.Payload;
            Version = 1;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        ///     Gets or sets the assigned implants.
        /// </summary>
        /// <value>The assigned implants.</value>
        public virtual List<Implant> AssignedImplants { get; set; } = new();

        /// <summary>
        ///     Gets or sets the assigned to tasks.
        /// </summary>
        /// <value>The assigned to tasks.</value>
        public virtual List<DeployTask> AssignedToTasks { get; set; } = new();

        /// <summary>
        ///     Gets or sets the database version of the file. (useful if you change the contents of a
        ///     Payload file).
        /// </summary>
        /// <value>The version.</value>
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public int Version { get; set; }

        #endregion Properties
    }
}
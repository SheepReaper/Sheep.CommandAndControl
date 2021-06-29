using System.Collections.Generic;

namespace ListeningPostApiServer.Models
{
    /// <inheritdoc />
    /// <summary>
    ///     Represents an agent record in the database. Implements the
    ///     <see
    ///         cref="T:ListeningPostApiServer.Models.EntityBase" />
    /// </summary>
    /// <seealso cref="T:ListeningPostApiServer.Models.EntityBase" />
    public class Implant : EntityBase
    {
        /// <summary>
        ///     Gets or sets the associated exfiltrated files.
        /// </summary>
        /// <value>The exfiltrated files.</value>
        public virtual List<ExfiltratedFile> ExfiltratedFiles { get; set; } = new();

        /// <summary>
        ///     Gets or sets the associated payload file.
        /// </summary>
        /// <value>The payload file.</value>
        public virtual PayloadFile? PayloadFile { get; set; }

        /// <summary>
        ///     Gets or sets the tasks associated with this Agent.
        /// </summary>
        /// <value>The tasks.</value>
        public virtual List<TaskBase> Tasks { get; set; } = new();
    }
}
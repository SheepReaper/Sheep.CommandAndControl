using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// ReSharper disable UnusedMember.Global

namespace ListeningPostApiServer.Models
{
    /// <inheritdoc />
    /// <summary>
    /// Represents an agent record in the database. Implements the <see
    /// cref="T:ListeningPostApiServer.Models.EntityBase" />
    /// </summary>
    /// <seealso cref="T:ListeningPostApiServer.Models.EntityBase" />
    public class Implant : EntityBase
    {
        #region Properties

        /// <summary>
        /// Gets or sets the associated exfiltrated files.
        /// </summary>
        /// <value>The exfiltrated files.</value>
        public virtual IEnumerable<ExfiltratedFile> ExfiltratedFiles { get; set; } = new HashSet<ExfiltratedFile>();

        /// <summary>
        /// Gets or sets the associated payload file.
        /// </summary>
        /// <value>The payload file.</value>
        public virtual PayloadFile PayloadFile { get; set; }

        /// <summary>
        /// Gets or sets the tasks associated with this Agent.
        /// </summary>
        /// <value>The tasks.</value>
        public virtual IEnumerable<TaskBase> Tasks { get; set; } = new HashSet<TaskBase>();

        #endregion Properties

        #region Methods

        /// <summary>
        /// Helper method to handle directly assigning a Command task to an agent. (No longer used).
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;TaskBase&gt;.</returns>
        [Obsolete]
        public async Task<TaskBase> AssignNewTask(string command)
        {
            await Task.CompletedTask;

            var newTask = new TaskBase
            {
                Command = command
            };

            Tasks.Append(newTask);

            return newTask;
        }

        #endregion Methods
    }
}

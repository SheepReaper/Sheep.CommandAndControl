// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace ListeningPostApiServer.Models
{
    /// <inheritdoc />
    /// <summary>
    ///     The Base Class for Task Types. Represents a Task record in the database. Implements the
    ///     <see
    ///         cref="T:ListeningPostApiServer.Models.EntityBase" />
    /// </summary>
    /// <seealso cref="T:ListeningPostApiServer.Models.EntityBase" />
    public class TaskBase : EntityBase
    {
        /// <summary>
        ///     Gets or sets the command string.
        /// </summary>
        /// <value>The command.</value>
        public string Command { get; set; } = "";

        /// <summary>
        ///     Gets or sets the implant that's assigned to this task.
        /// </summary>
        /// <value>The implant.</value>
        public virtual Implant? Implant { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this Task has been picked up by the agent.
        /// </summary>
        /// <value><c>true</c> if this instance is picked up; otherwise, <c>false</c>.</value>
        public bool IsPickedUp { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this task is completed (agent responded with task completion).
        /// </summary>
        /// <value><c>true</c> if this instance is returned; otherwise, <c>false</c>.</value>
        public bool IsReturned { get; set; }

        /// <summary>
        ///     Gets or sets the result associated with this task.
        /// </summary>
        /// <value>The result.</value>
        public virtual Result? Result { get; set; }

        /// <summary>
        ///     Gets or sets the type of the task.
        /// </summary>
        /// <value>The type of the task.</value>
        public TaskType TaskType { get; set; }
    }
}
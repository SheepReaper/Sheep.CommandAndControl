namespace ListeningPostApiServer.Models
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a Task record in the database, specifically a Direct bash command Type of task.
    /// Implements the <see cref="T:ListeningPostApiServer.Models.TaskBase" />
    /// </summary>
    /// <seealso cref="T:ListeningPostApiServer.Models.TaskBase" />
    public class CommandTask : TaskBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandTask" /> class.
        /// </summary>
        public CommandTask()
        {
            TaskType = TaskType.Command;
        }

        #endregion Constructors
    }
}

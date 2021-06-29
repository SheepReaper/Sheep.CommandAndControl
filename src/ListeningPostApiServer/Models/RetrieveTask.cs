namespace ListeningPostApiServer.Models
{
    /// <inheritdoc />
    /// <summary>
    ///     Represents a Task record in the database, specifically a PULL task where an agent is to
    ///     upload an exfiltrated file. Implements the
    ///     <see
    ///         cref="T:ListeningPostApiServer.Models.TaskBase" />
    /// </summary>
    /// <seealso cref="T:ListeningPostApiServer.Models.TaskBase" />
    // ReSharper disable once ClassNeverInstantiated.Global
    public class RetrieveTask : TaskBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RetrieveTask" /> class.
        /// </summary>
        public RetrieveTask() => TaskType = TaskType.Retrieve;
    }
}
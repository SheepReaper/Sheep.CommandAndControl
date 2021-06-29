namespace ListeningPostApiServer.Models
{
    /// <inheritdoc />
    /// <summary>
    ///     Represents a Task record in the database, specifically a PUSH task, deploying a Payload File
    ///     to the agent. Implements the <see cref="T:ListeningPostApiServer.Models.TaskBase" />
    /// </summary>
    /// <seealso cref="T:ListeningPostApiServer.Models.TaskBase" />
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DeployTask : TaskBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DeployTask" /> class.
        /// </summary>
        public DeployTask() => TaskType = TaskType.Deploy;
    }
}
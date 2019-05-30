namespace ListeningPostApiServer.Models
{
    /// <summary>
    /// Provides validation against the types of tasks supported by the database.
    /// </summary>
    public enum TaskType
    {
        /// <summary>
        /// A task that simply issues a bash command to the remote system.
        /// </summary>
        Command,

        /// <summary>
        /// A special task that will deploy a payload file to a remote system via an agent.
        /// </summary>
        Deploy,

        /// <summary>
        /// A special task that will retrieve a target file from a remote system via an agent.
        /// </summary>
        Retrieve
    }
}

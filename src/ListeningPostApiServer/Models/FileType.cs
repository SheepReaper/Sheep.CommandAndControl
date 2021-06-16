namespace ListeningPostApiServer.Models
{
    /// <summary>
    ///     Enumerable types that are Valid in the context of File Records in the database.
    /// </summary>
    public enum FileType
    {
        /// <summary>
        ///     A File exfiltrated from a remote system by an agent.
        /// </summary>
        Exfiltrated,

        /// <summary>
        ///     A File uploaded to server, ready to be deployed to remote system agents.
        /// </summary>
        Payload
    }
}
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ListeningPostApiServer.Models
{
    /// <inheritdoc />
    /// <summary>
    ///     Represents a result record in the database. Implements the
    ///     <see
    ///         cref="T:ListeningPostApiServer.Models.EntityBase" />
    /// </summary>
    /// <seealso cref="T:ListeningPostApiServer.Models.EntityBase" />
    public class Result : EntityBase
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the error message returned by the agent.
        /// </summary>
        /// <value>The error.</value>
        [JsonProperty("error")]
        public string Error { get; set; } = "";

        /// <summary>
        ///     Gets or sets the implant that completed the task that generated this response.
        /// </summary>
        /// <value>The implant.</value>
        public virtual Implant? Implant { get; set; }

        /// <summary>
        ///     Gets or sets the result message returned by the agent.
        /// </summary>
        /// <value>The results.</value>
        [JsonProperty("results")]
        public string Results { get; set; } = "";

        /// <summary>
        ///     Gets or sets the task that resulted in this result.
        /// </summary>
        /// <value>The task base.</value>
        [ForeignKey("TaskId")]
        public virtual TaskBase? TaskBase { get; set; }

        /// <summary>
        ///     This is the Foreign key for the Task record associated with this response.
        /// </summary>
        /// <value>The task identifier.</value>
        [JsonProperty("task_id")]
        public int TaskId { get; set; }

        #endregion Properties
    }
}
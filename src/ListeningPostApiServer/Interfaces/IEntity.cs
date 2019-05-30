using System;

namespace ListeningPostApiServer.Interfaces
{
    /// <summary>
    /// Contract definition for all Entities used in this project
    /// </summary>
    public interface IEntity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        /// <value>The unique identifier.</value>
        Guid Guid { get; set; }

        // ReSharper disable once UnusedMember.Global
        /// <summary>
        /// Gets or sets the other unique identifier.
        /// </summary>
        /// <value>The identifier.</value>
        int Id { get; set; }

        #endregion Properties
    }
}

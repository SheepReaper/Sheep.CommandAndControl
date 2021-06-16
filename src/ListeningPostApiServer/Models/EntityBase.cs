using System;
using System.ComponentModel.DataAnnotations;
using ListeningPostApiServer.Interfaces;

namespace ListeningPostApiServer.Models
{
    /// <inheritdoc />
    /// <summary>
    ///     The Base Entity Class for this Domain that all other Entities inherit from. Implements the
    ///     <see cref="T:ListeningPostApiServer.Interfaces.IEntity" />
    /// </summary>
    /// <seealso cref="T:ListeningPostApiServer.Interfaces.IEntity" />
    public class EntityBase : IEntity
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="EntityBase" /> class.
        /// </summary>
        public EntityBase() => Guid = Guid.NewGuid();

        #endregion Constructors

        #region Properties

        /// <inheritdoc />
        /// <summary>
        ///     Gets or sets the database unique identifier. (Global)
        /// </summary>
        /// <value>The unique identifier.</value>
        /// <remarks>
        ///     In the context of an agent record, this is different and distinct from the nodeId which
        ///     is self-reported by the agent and not guaranteed to be unique.
        /// </remarks>
        public Guid Guid { get; set; }

        /// <inheritdoc />
        /// <summary>
        ///     Gets or sets the identifier. (Class Scope)
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public int Id { get; set; }

        #endregion Properties
    }
}
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable UnusedMember.Global

namespace ListeningPostApiServer.Interfaces
{
    /// <summary>
    /// The contract for all Repositories used in this project.
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        #region Methods

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="entity">           The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task&lt;TEntity&gt;.</returns>
        Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="entity">           The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task.</returns>
        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Edits the asynchronous.
        /// </summary>
        /// <param name="entity">           The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task&lt;TEntity&gt;.</returns>
        Task<TEntity> EditAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task&lt;IEnumerable&lt;TEntity&gt;&gt;.</returns>
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the by unique identifier asynchronous.
        /// </summary>
        /// <param name="id">               The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task&lt;TEntity&gt;.</returns>
        Task<TEntity> GetByGuidAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">               The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task&lt;TEntity&gt;.</returns>
        Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets some asynchronous.
        /// </summary>
        /// <param name="predicate">        The predicate.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task&lt;IEnumerable&lt;TEntity&gt;&gt;.</returns>
        Task<IEnumerable<TEntity>> GetSomeAsync(Func<TEntity, bool> predicate,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Saves the changes asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task.</returns>
        Task SaveChangesAsync(CancellationToken cancellationToken = default);

        #endregion Methods
    }
}

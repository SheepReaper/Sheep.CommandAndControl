using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable UnusedMember.Global

namespace ListeningPostApiServer.Interfaces
{
    /// <summary>
    ///     The contract for all Repositories used in this project.
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        /// <summary>
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="saveImmediately"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TEntity> AddAsync(TEntity entity, bool saveImmediately = true,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="saveImmediately"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TEntity?> DeleteAsync(int id, bool saveImmediately = true,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="saveImmediately"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TEntity?> DeleteAsync(Guid id, bool saveImmediately = true, CancellationToken cancellationToken = default);

        /// <summary>
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="saveImmediately"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TEntity> EditAsync(TEntity entity, bool saveImmediately = true,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<TEntity>> GetAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<TEntity>> GetAsync(Func<TEntity, bool> predicate,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TEntity?> GetAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TEntity?> GetAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<(bool parsed, TEntity? result)> TryParseIdAsync(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
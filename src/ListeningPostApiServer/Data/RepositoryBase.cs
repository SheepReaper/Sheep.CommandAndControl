using ListeningPostApiServer.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ListeningPostApiServer.Data
{
    /// <inheritdoc />
    /// <summary>
    /// The base implementation for all Repositories in this application. Provides a data access
    /// layer. Implements the <see cref="T:ListeningPostApiServer.Interfaces.IRepository`1" />
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    /// <seealso cref="T:ListeningPostApiServer.Interfaces.IRepository`1" />
    public class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        #region Fields

        /// <summary>
        /// The database context
        /// </summary>
        protected readonly DbContext DbContext;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase{TEntity}" /> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public RepositoryBase(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        #endregion Constructors

        #region Methods

        /// <inheritdoc />
        /// <summary>
        /// create as an asynchronous operation.
        /// </summary>
        /// <param name="entity">           The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task&lt;TEntity&gt;.</returns>
        public async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await DbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
            return entity;
        }

        /// <inheritdoc />
        /// <summary>
        /// delete as an asynchronous operation.
        /// </summary>
        /// <param name="entity">           The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task.</returns>
        public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            DbContext.Set<TEntity>().Remove(entity);
        }

        /// <inheritdoc />
        /// <summary>
        /// edit as an asynchronous operation.
        /// </summary>
        /// <param name="entity">           The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task&lt;TEntity&gt;.</returns>
        public async Task<TEntity> EditAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await DbContext.Set<TEntity>().FindAsync(entity, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
            return entity;
        }

        /// <inheritdoc />
        /// <summary>
        /// get all as an asynchronous operation.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task&lt;IEnumerable&lt;TEntity&gt;&gt;.</returns>
        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            var resultSet = DbContext.Set<TEntity>().AsEnumerable();
            return resultSet;
        }

        /// <inheritdoc />
        /// <summary>
        /// get by unique identifier as an asynchronous operation.
        /// </summary>
        /// <param name="id">               The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task&lt;TEntity&gt;.</returns>
        public async Task<TEntity> GetByGuidAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await DbContext.Set<TEntity>().FirstOrDefaultAsync(e => e.Guid == id, cancellationToken);
        }

        /// <inheritdoc />
        /// <summary>
        /// get by identifier as an asynchronous operation.
        /// </summary>
        /// <param name="id">               The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task&lt;TEntity&gt;.</returns>
        public async Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var found = await DbContext.Set<TEntity>().FindAsync(id, cancellationToken);
            return found;
        }

        /// <inheritdoc />
        /// <summary>
        /// get some as an asynchronous operation.
        /// </summary>
        /// <param name="predicate">        The predicate.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task&lt;IEnumerable&lt;TEntity&gt;&gt;.</returns>
        public async Task<IEnumerable<TEntity>> GetSomeAsync(Func<TEntity, bool> predicate,
                            CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            var resultSet = DbContext.Set<TEntity>().Where(predicate);
            return resultSet;
        }

        /// <inheritdoc />
        /// <summary>
        /// save changes as an asynchronous operation.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task.</returns>
        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        #endregion Methods
    }
}

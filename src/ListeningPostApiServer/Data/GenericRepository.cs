using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ListeningPostApiServer.Extensions;
using ListeningPostApiServer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ListeningPostApiServer.Data
{
    /// <inheritdoc />
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GenericRepository{TEntity}" /> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public GenericRepository(AppDbContext dbContext)
        {
            _ctx = dbContext;
            _entities = dbContext.Set<TEntity>();
        }

        #endregion Constructors

        #region Fields

        private readonly AppDbContext _ctx;

        private readonly DbSet<TEntity> _entities;

        #endregion Fields

        #region Methods

        /// <inheritdoc />
        public async Task<TEntity> AddAsync(TEntity entity, bool saveImmediately,
            CancellationToken cancellationToken)
        {
            await _entities.AddAsync(entity, cancellationToken);

            if (saveImmediately)
                await _ctx.SaveChangesAsync(cancellationToken);

            return entity;
        }

        private async Task<TEntity?> _deleteAsync(TEntity? entity, bool saveImmediately,
            CancellationToken cancellationToken)
        {
            if (entity == null)
                return null;

            _entities.Remove(entity);

            if (saveImmediately)
                await _ctx.SaveChangesAsync(cancellationToken);

            return entity;
        }

        /// <inheritdoc />
        public async Task<TEntity?> DeleteAsync(int id, bool saveImmediately,
            CancellationToken cancellationToken) =>
            await _deleteAsync(await GetAsync(id, cancellationToken), saveImmediately, cancellationToken);

        /// <inheritdoc />
        public async Task<TEntity?>
            DeleteAsync(Guid id, bool saveImmediately, CancellationToken cancellationToken) =>
            await _deleteAsync(await GetAsync(id, cancellationToken), saveImmediately, cancellationToken);

        /// <inheritdoc />
        public async Task<TEntity> EditAsync(TEntity entity, bool saveImmediately, CancellationToken cancellationToken)
        {
            _ctx.Entry(entity).State = EntityState.Modified;

            if (saveImmediately)
                await _ctx.SaveChangesAsync(cancellationToken);

            return entity;
        }


        /// <inheritdoc />
        public Task<TEntity?> GetAsync(int id, CancellationToken cancellationToken) =>
            _entities.FindByIdAsync(cancellationToken, id);

        /// <inheritdoc />
        public async Task<TEntity?> GetAsync(Guid id, CancellationToken cancellationToken) =>
            await _entities.FirstOrDefaultAsync(e => e.Guid == id, cancellationToken);

        /// <inheritdoc />
        public async Task<(bool parsed, TEntity? result)> TryParseIdAsync(string id,
            CancellationToken cancellationToken)
        {
            if (Guid.TryParse(id, out var guid))
                return (true, await _entities.FindByIdAsync(cancellationToken, guid));

            return int.TryParse(id, out var intId)
                ? (true, await _entities.FindByIdAsync(cancellationToken, intId))
                : (false, null);
        }

        /// <inheritdoc />
        public Task<IList<TEntity>> GetAsync(CancellationToken cancellationToken) => _entities
            .ToListAsync(cancellationToken)
            .ContinueWith<IList<TEntity>>(t => t.Result, TaskContinuationOptions.ExecuteSynchronously);

        /// <inheritdoc />
        public Task<IList<TEntity>>
            GetAsync(Func<TEntity, bool> predicate, CancellationToken cancellationToken) =>
            Task.Run<IList<TEntity>>(() => _entities.Where(predicate).ToList(), cancellationToken);

        /// <inheritdoc />
        public Task SaveChangesAsync(CancellationToken cancellationToken) => _ctx.SaveChangesAsync(cancellationToken);

        #endregion Methods
    }
}
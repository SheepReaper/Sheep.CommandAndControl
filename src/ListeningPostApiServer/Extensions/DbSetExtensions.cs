using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace ListeningPostApiServer.Extensions
{
    /// <summary>
    /// </summary>
    public static class DbSetExtensions
    {
        /// <summary>
        /// </summary>
        /// <param name="dbSet"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="keyValues"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static async Task<TEntity?> FindByIdAsync<TEntity>(this DbSet<TEntity> dbSet,
            CancellationToken cancellationToken = default,
            params object[] keyValues) where TEntity : class =>
            await dbSet.FindAsync(keyValues, cancellationToken);
    }
}
using ListeningPostApiServer.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace ListeningPostApiServer.Data
{
    /// <inheritdoc />
    /// <summary>
    /// Provides access to the Repository of agents to the rest of the application. Implements the
    /// <see cref="T:ListeningPostApiServer.Models.Implant" />
    /// </summary>
    /// <seealso cref="T:ListeningPostApiServer.Models.Implant" />
    public class ImplantRepository : RepositoryBase<Implant>
    {
        #region Fields

        /// <summary>
        /// The context
        /// </summary>
        public DbContext Context;

        #endregion Fields

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="T:ListeningPostApiServer.Data.ImplantRepository" /> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public ImplantRepository(DbContext dbContext) : base(dbContext)
        {
            Context = dbContext;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Gets the by identifier (nodeId).
        /// </summary>
        /// <param name="id">               The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task&lt;Implant&gt;.</returns>
        public async Task<Implant> GetById(int id, CancellationToken cancellationToken = default)
        {
            var foundImplant = await DbContext.Set<Implant>().FindAsync(id);
            return foundImplant;
        }

        #endregion Methods
    }
}

using ListeningPostApiServer.Models;
using Microsoft.EntityFrameworkCore;

namespace ListeningPostApiServer.Data
{
    /// <inheritdoc />
    /// <summary>
    /// Provides access to the Data repository in the context of Tasks Implements the <see
    /// cref="T:ListeningPostApiServer.Models.TaskBase" />
    /// </summary>
    /// <seealso cref="T:ListeningPostApiServer.Models.TaskBase" />
    public class TaskRepository : RepositoryBase<TaskBase>
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
        /// cref="T:ListeningPostApiServer.Data.TaskRepository" /> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public TaskRepository(DbContext dbContext) : base(dbContext)
        {
            Context = dbContext;
        }

        #endregion Constructors
    }
}

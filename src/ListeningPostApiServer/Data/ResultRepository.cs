using ListeningPostApiServer.Interfaces;
using ListeningPostApiServer.Models;
using Microsoft.EntityFrameworkCore;

namespace ListeningPostApiServer.Data
{
    /// <inheritdoc />
    /// <summary>
    /// Provides access to the Data repository in the Result context Implements the <see
    /// cref="T:ListeningPostApiServer.Models.Result" />
    /// </summary>
    /// <seealso cref="T:ListeningPostApiServer.Models.Result" />
    public class ResultRepository : RepositoryBase<Result>
    {
        #region Fields

        /// <summary>
        /// The context
        /// </summary>
        public DbContext Context;

        /// <summary>
        /// The task repository
        /// </summary>
        public TaskRepository TaskRepository;

        #endregion Fields

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="T:ListeningPostApiServer.Data.ResultRepository" /> class.
        /// </summary>
        /// <param name="dbContext">        The database context.</param>
        /// <param name="implantRepository">The implant repository.</param>
        public ResultRepository(DbContext dbContext, IRepository<TaskBase> implantRepository) : base(dbContext)
        {
            TaskRepository = (TaskRepository)implantRepository;
            Context = dbContext;
        }

        #endregion Constructors
    }
}

using ListeningPostApiServer.Models;
using Microsoft.EntityFrameworkCore;

namespace ListeningPostApiServer.Data
{
    /// <inheritdoc />
    /// <summary>
    /// Provides access to and File record specific methods to the rest of the application Implements
    /// the <see cref="T:ListeningPostApiServer.Models.FileBase" />
    /// </summary>
    /// <seealso cref="T:ListeningPostApiServer.Models.FileBase" />
    public class FileRepository : RepositoryBase<FileBase>
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
        /// cref="T:ListeningPostApiServer.Data.FileRepository" /> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public FileRepository(DbContext dbContext) : base(dbContext)
        {
            Context = dbContext;
        }

        #endregion Constructors
    }
}

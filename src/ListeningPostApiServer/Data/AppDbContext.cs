using ListeningPostApiServer.Models;
using Microsoft.EntityFrameworkCore;

// ReSharper disable UnusedMember.Global

namespace ListeningPostApiServer.Data
{
    /// <inheritdoc />
    /// <summary>
    ///     This context defines the data model that EntityFrameworkCore will then implement. Implements
    ///     the <see cref="T:Microsoft.EntityFrameworkCore.DbContext" />
    /// </summary>
    /// <seealso cref="T:Microsoft.EntityFrameworkCore.DbContext" />
    public class AppDbContext : DbContext
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:ListeningPostApiServer.Data.AppDbContext" /> class.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        #endregion Constructors

        #region Methods

        /// <inheritdoc />
        /// <summary>
        ///     Override this method to further configure the model that was discovered by convention
        ///     from the entity types exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" />
        ///     properties on your derived context. The resulting model may be cached and re-used for
        ///     subsequent instances of your derived context.
        /// </summary>
        /// <param name="modelBuilder">
        ///     The builder being used to construct the model for this context. Databases (and other
        ///     extensions) typically define extension methods on this object that allow you to configure
        ///     aspects of the model that are specific to a given database.
        /// </param>
        /// <remarks>
        ///     If a model is explicitly set on the options for this context (via
        ///     <see
        ///         cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />
        ///     ) then this method will not be run.
        /// </remarks>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EntityBase>().ToTable("Entities")
                .HasAlternateKey(e => e.Guid);
            modelBuilder.Entity<TaskBase>().ToTable("Tasks");
            modelBuilder.Entity<FileBase>().ToTable("Files");
        }

        #endregion Methods

        #region Properties

        /// <summary>
        ///     Gets or sets the command tasks.
        /// </summary>
        /// <value>The command tasks.</value>
        public DbSet<CommandTask>? CommandTasks { get; set; }

        /// <summary>
        ///     Gets or sets the deploy tasks.
        /// </summary>
        /// <value>The deploy tasks.</value>
        public DbSet<DeployTask>? DeployTasks { get; set; }

        /// <summary>
        ///     Gets or sets the implants.
        /// </summary>
        /// <value>The implants.</value>
        public DbSet<Implant>? Implants { get; set; }

        /// <summary>
        ///     Gets or sets the payload files.
        /// </summary>
        /// <value>The payload files.</value>
        public DbSet<PayloadFile>? PayloadFiles { get; set; }

        /// <summary>
        ///     Gets or sets the results.
        /// </summary>
        /// <value>The results.</value>
        public DbSet<Result>? Results { get; set; }

        /// <summary>
        ///     Gets or sets the retrieved files.
        /// </summary>
        /// <value>The retrieved files.</value>
        public DbSet<ExfiltratedFile>? RetrievedFiles { get; set; }

        /// <summary>
        ///     Gets or sets the retrieve tasks.
        /// </summary>
        /// <value>The retrieve tasks.</value>
        public DbSet<RetrieveTask>? RetrieveTasks { get; set; }

        #endregion Properties
    }
}
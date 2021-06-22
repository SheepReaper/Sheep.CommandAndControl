using System.Threading.Tasks;
using ListeningPostApiServer.Interfaces;
using ListeningPostApiServer.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ListeningPostApiServer.Controllers
{
    /// <inheritdoc />
    /// <summary>
    ///     An Api Controller for registering Agents with the server and database operations (CRUD)
    ///     Implements the <see cref="T:Microsoft.AspNetCore.Mvc.ControllerBase" />
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Mvc.ControllerBase" />
    /// <remarks>Used by both Agents and Users</remarks>
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    [ProducesErrorResponseType(typeof(NotFoundResult))]
    public class ImplantController : ControllerBase

    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ImplantController" /> class.
        /// </summary>
        /// <param name="implantRepository">The implant repository.</param>
        /// <remarks>Requires an IRespository implementation via Injection.</remarks>
        public ImplantController(IRepository<Implant> implantRepository) => ImplantRepository = implantRepository;

        #endregion Constructors

        #region Properties

        /// <summary>
        ///     Provides a Repository Implementation for controller methods
        /// </summary>
        /// <value>The implant repository implementation.</value>
        private IRepository<Implant> ImplantRepository { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        ///     Gets metadata for the specified agent, or all agents if id is not specified.
        /// </summary>
        /// <param name="id">The identifier (agent-reported).</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [EnableCors(CorsPolicyType.MinimalGet)]
        [HttpGet("{id:int?}")]
        public async Task<IActionResult> Get(int id = 0)
        {
            if (id == 0)
            {
                var results = await ImplantRepository.GetAsync();
                return Ok(results);
            }

            var result = await ImplantRepository.GetAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        #endregion Methods
    }
}
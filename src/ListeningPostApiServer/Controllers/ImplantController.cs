using ListeningPostApiServer.Data;
using ListeningPostApiServer.Interfaces;
using ListeningPostApiServer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;

namespace ListeningPostApiServer.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// An Api Controller for registering Agents with the server and database operations (CRUD)
    /// Implements the <see cref="T:Microsoft.AspNetCore.Mvc.ControllerBase" />
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Mvc.ControllerBase" />
    /// <remarks>Used by both Agents and Users</remarks>
    [Route("[Controller]")]
    [EnableCors("AllowAll")]
    [Produces("application/json")]
    [ProducesErrorResponseType(typeof(NotFoundResult))]
    [ApiController]
    public class ImplantController : ControllerBase

    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ImplantController" /> class.
        /// </summary>
        /// <param name="implantRepository">The implant repository.</param>
        /// <remarks>Requires an IRespository implementation via Injection.</remarks>
        public ImplantController(IRepository<Implant> implantRepository)
        {
            ImplantRepository = (ImplantRepository)implantRepository;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Provides a Repository Implementation for controller methods
        /// </summary>
        /// <value>The implant repository implementation.</value>
        private ImplantRepository ImplantRepository { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets metadata for the specified agent, or all agents if id is not specified.
        /// </summary>
        /// <param name="id">The identifier (agent-reported).</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [HttpGet("{id?}")]
        public async Task<IActionResult> Get(int id)
        {
            return id == 0 ? Ok(await ImplantRepository.GetAllAsync()) : Ok(await ImplantRepository.GetById(id));
        }

        #endregion Methods
    }
}

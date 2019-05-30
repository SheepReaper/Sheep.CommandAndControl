using ListeningPostApiServer.Data;
using ListeningPostApiServer.Extensions;
using ListeningPostApiServer.Interfaces;
using ListeningPostApiServer.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;

namespace ListeningPostApiServer.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// An Api Controller that provides a way for agent to report task completion and results.
    /// Provides an interface to query entities from the database. Implements the <see
    /// cref="T:Microsoft.AspNetCore.Mvc.ControllerBase" />
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    [EnableCors("AllowAll")]
    [Route("[Controller]")]
    [Produces("application/json")]
    [ProducesErrorResponseType(typeof(NotFoundResult))]
    public class ResultsController : ControllerBase
    {
        #region Fields

        /// <summary>
        /// Provides repository access to controller methods.
        /// </summary>
        private readonly ResultRepository _repository;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultsController" /> class.
        /// </summary>
        /// <param name="repository">A repository interface.</param>
        /// <remarks>Requires a repository implementation via dependency injection.</remarks>
        public ResultsController(IRepository<Result> repository)
        {
            _repository = (ResultRepository)repository;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Gets the specified piece of result data or all result data if no Id is specified.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [HttpGet("{id?}")]
        public async Task<IActionResult> GetResults(string id)
        {
            if (Guid.TryParse(id, out var guid))
            {
                return Ok(await _repository.GetByGuidAsync(guid));
            }

            if (!int.TryParse(id, out var taskId)) return Ok(await _repository.GetAllAsync());
            var task = await _repository.GetByIdAsync(taskId);
            return Ok(task.Results);
        }

        /// <summary>
        /// Posts a result item, registering the agent that delivered it, and associating to the Task
        /// in the database.
        /// </summary>
        /// <param name="nodeId">The node identifier.</param>
        /// <param name="result">The result object (note not all properties are required).</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [HttpPost("{nodeId}")]
        public async Task<IActionResult> PostResult(int nodeId, [FromBody] Result result)
        {
            this.Log($"Implant returning. Implant Id:{nodeId}");

            var task = await _repository.Context.Set<TaskBase>().FindAsync(result.TaskId);

            this.Log($"TaskId: {result.TaskId}");

            var newResult = new Result
            {
                TaskBase = task,
                Error = result.Error,
                Implant = task.Implant,
                Results = result.Results
            };

            await _repository.CreateAsync(newResult);
            task.IsReturned = true;
            await _repository.SaveChangesAsync();

            return CreatedAtAction("GetResults", new { newResult.Id }, null);
        }

        #endregion Methods
    }
}

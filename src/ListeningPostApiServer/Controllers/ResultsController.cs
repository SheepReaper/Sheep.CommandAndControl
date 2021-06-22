using System;
using System.Threading.Tasks;
using ListeningPostApiServer.Extensions;
using ListeningPostApiServer.Interfaces;
using ListeningPostApiServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace ListeningPostApiServer.Controllers
{
    /// <inheritdoc />
    /// <summary>
    ///     An Api Controller that provides a way for agent to report task completion and results.
    ///     Provides an interface to query entities from the database. Implements the
    ///     <see
    ///         cref="T:Microsoft.AspNetCore.Mvc.ControllerBase" />
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    [ProducesErrorResponseType(typeof(NotFoundResult))]
    public class ResultsController : ControllerBase
    {
        #region Constructors

        /// <summary>
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="taskRepo"></param>
        public ResultsController(IRepository<Result> repository, IRepository<TaskBase> taskRepo)
        {
            _repository = repository;
            _tasks = taskRepo;
        }

        #endregion Constructors

        #region Fields

        /// <summary>
        ///     Provides repository access to controller methods.
        /// </summary>
        private readonly IRepository<Result> _repository;

        private readonly IRepository<TaskBase> _tasks;

        #endregion Fields

        #region Methods

        /// <summary>
        ///     Gets the specified piece of result data or all result data if no Id is specified.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [HttpGet("{id?}")]
        public async Task<IActionResult> GetResults(string id)
        {
            if (Guid.TryParse(id, out var guid)) return Ok(await _repository.GetAsync(guid));

            if (!int.TryParse(id, out var taskId)) return Ok(await _repository.GetAsync());
            var task = await _repository.GetAsync(taskId);

            if (task == null)
                return NotFound(new {message = "task d not found"});
            return Ok(task.Results);
        }

        /// <summary>
        ///     Posts a result item, registering the agent that delivered it, and associating to the Task
        ///     in the database.
        /// </summary>
        /// <param name="nodeId">The node identifier.</param>
        /// <param name="result">The result object (note not all properties are required).</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [HttpPost("{nodeId:int}")]
        public async Task<IActionResult> PostResult(int nodeId, [FromBody] Result result)
        {
            this.Log($"Implant returning. Implant Id:{nodeId}");

            var task = await _tasks.GetAsync(result.TaskId);

            if (task == null)
                return NotFound(new {message = "the requested task Id doesn't exist"});

            this.Log($"TaskId: {result.TaskId}");

            task.IsReturned = true;

            var newResult = new Result
            {
                TaskBase = task,
                Error = result.Error,
                Implant = task.Implant,
                Results = result.Results
            };

            await _repository.AddAsync(newResult);

            return CreatedAtAction("GetResults", new {newResult.Id}, null);
        }

        #endregion Methods
    }
}
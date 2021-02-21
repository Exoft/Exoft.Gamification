using System;
using System.Threading;
using System.Threading.Tasks;

using Exoft.Gamification.Api.Services.Interfaces.Services;

using Microsoft.AspNetCore.Mvc;

namespace Exoft.Gamification.Api.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FilesController : GamificationController
    {
        private readonly IFileService _fileService;

        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        /// <summary>
        /// Get file stream
        /// </summary>
        /// <responce code="200">Return file stream</responce>
        /// <response code="404">When file with current Id is not found</response> 
        [HttpGet("{fileId}")]
        public async Task<IActionResult> GetFile(Guid fileId, CancellationToken cancellationToken)
        {
            var file = await _fileService.GetFileByIdAsync(fileId, cancellationToken);
            if (file == null)
            {
                return NotFound();
            }

            return base.File(file.Data, file.ContentType);
        }
    }
}
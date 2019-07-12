using Exoft.Gamification.Api.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

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

        [HttpGet("{fileId}")]
        public async Task<IActionResult> GetFile(Guid fileId)
        {
            var file = await _fileService.GetFileByIdAsync(fileId);
            if(file == null)
            {
                return NotFound();
            }

            return base.File(file.Data, file.ContentType);
        }
    }
}
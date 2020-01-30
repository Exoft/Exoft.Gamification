using Exoft.Gamification.Api.Common.Models.ReferenceBook;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Controllers
{
    [Route("api/reference-book")]
    [Authorize]
    [ApiController]
    public class ReferenceBookController : GamificationController
    {
        private readonly IReferenceBookService _referenceBookService;

        public ReferenceBookController(IReferenceBookService referenceBookService)
        {
            _referenceBookService = referenceBookService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetList()
        {
            var list = await _referenceBookService.GetAllChaptersAsync(new PagingInfo { CurrentPage = 1, PageSize = 100 });

            return Ok(list);
        }

        [HttpPost("edit-article")]
        [Authorize(Roles = GamificationRole.Admin)]
        public async Task<ActionResult> EditArticle(UpdateArticleModel article)
        {
            if (article == null)
            {
                return BadRequest();
            }

            if (await _referenceBookService.UpdateArticleAsync(article))
            {
                return NoContent();
            }
            return BadRequest();
        }

        [HttpPut("add-article")]
        [Authorize(Roles = GamificationRole.Admin)]
        public async Task<ActionResult> AddArticleToChapter(CreateArticleModel article)
        {
            if (article == null)
            {
                return BadRequest();
            }

            if (await _referenceBookService.AddArticleAsync(article))
            {
                return NoContent();
            }
            return BadRequest();
        }

        [HttpPut("add-chapter")]
        [Authorize(Roles = GamificationRole.Admin)]
        public async Task<ActionResult> AddArticle(CreateChapterModel chapterModel)
        {
            if (chapterModel == null)
            {
                return BadRequest();
            }

            if (await _referenceBookService.AddChapterAsync(chapterModel))
            {
                return NoContent();
            }
            return BadRequest();
        }

        [HttpDelete("delete-chapter/{chapterId}")]
        [Authorize(Roles = GamificationRole.Admin)]
        public async Task<ActionResult> DeleteChapter(Guid chapterId)
        {
            var chapter = await _referenceBookService.GetChapterById(chapterId);
            if (chapter == null)
            {
                return NotFound();
            }

            await _referenceBookService.DeleteChapterAsync(chapter);
            return NoContent();
        }

        [HttpDelete("delete-article/{articleId}")]
        [Authorize(Roles = GamificationRole.Admin)]
        public async Task<ActionResult> DeleteArticle(Guid articleId)
        {
            var article = await _referenceBookService.GetArticleById(articleId);
            if (article == null)
            {
                return NotFound();
            }

            await _referenceBookService.DeleteArticleAsync(article);
            return NoContent();
        }
    }
}

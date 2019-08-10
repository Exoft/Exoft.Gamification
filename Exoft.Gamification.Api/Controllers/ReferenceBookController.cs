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

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetList()
        {
            var list = await _referenceBookService.GetAllChaptersAsync(new PagingInfo { CurrentPage = 1, PageSize = 100 });
            return Ok(list);
        }

        [HttpPost]
        [Route("edit-article")] 
        [Authorize(Policy ="IsAdmin")]
        public async Task<ActionResult> EditArticle(UpdateArticleModel article)
        {
            if (article == null) return BadRequest();
            if(await _referenceBookService.UpdateArticleAsync(article)) return NoContent();
            return BadRequest();
        }

        [HttpPut]
        [Route("add-article")]
        [Authorize(Policy = "IsAdmin")]
        public async Task<ActionResult> AddArticleToChapter(CreateArticleModel article)
        {
            if (article == null) return BadRequest();
            if (await _referenceBookService.AddArticleAsync(article)) return NoContent();
            return BadRequest();
        }

        [HttpPut]
        [Route("add-chapter")]
        [Authorize(Policy = "IsAdmin")]
        public async Task<ActionResult> AddArticle(CreateChapterModel chapterModel)
        {
            if (chapterModel == null) return BadRequest();
            if (await _referenceBookService.AddChapterAsync(chapterModel)) return NoContent();
            return BadRequest();
        }

        [HttpDelete]
        [Route("delete-chapter/{chapterId}")]
        [Authorize(Policy = "IsAdmin")]
        public async Task<ActionResult> DeleteChapter(Guid chapterId)
        {
            try
            {
                await _referenceBookService.DeleteChapterAsync(chapterId);
                return NoContent();
            }
            catch(Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpDelete]
        [Route("delete-article/{articleId}")]
        [Authorize(Policy = "IsAdmin")]
        public async Task<ActionResult> DeleteArticle(Guid articleId)
        {
            try
            {
                await _referenceBookService.DeleteArticleAsync(articleId);
                return NoContent();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}

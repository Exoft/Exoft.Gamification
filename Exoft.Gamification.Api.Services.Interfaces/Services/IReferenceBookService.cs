using Exoft.Gamification.Api.Common.Models.ReferenceBook;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using System;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services.Interfaces.Services
{
    public interface IReferenceBookService
    {
        Task<Chapter> GetChapterById(Guid Id);
        Task<Article> GetArticleById(Guid Id);
        Task<ReturnPagingInfo<Chapter>> GetAllChaptersAsync(PagingInfo pagingInfo);
        Task<bool> UpdateArticleAsync(UpdateArticleModel updatedArticle);
        Task<bool> AddChapterAsync(CreateChapterModel chapterModel);
        Task<bool> AddArticleAsync(CreateArticleModel articleModel);
        Task DeleteChapterAsync(Chapter chapter);
        Task DeleteArticleAsync(Article article);
    }
}

using Exoft.Gamification.Api.Common.Models.ReferenceBook;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Services.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services
{
    public class ReferenceBookService : IReferenceBookService
    {
        private readonly IChapterRepository _chapterRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IArticleRepository _articleRepository;

        public ReferenceBookService(IChapterRepository chapterRepository, IArticleRepository articleRepository, IUnitOfWork unitOfWork)
        {
            _chapterRepository = chapterRepository;
            _articleRepository = articleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ReturnPagingInfo<Chapter>> GetAllChaptersAsync(PagingInfo pagingInfo)
        {
            var data = await _chapterRepository.GetAllDataAsync(pagingInfo);
            data.Data?.ToList().ForEach(c => c.Articles = c.Articles.OrderBy(a => a.UnitNumber).ToList());

            return data;
        }

        public async Task<bool> UpdateArticleAsync(UpdateArticleModel updatedArticle)
        {
            try
            {
                var article = await _articleRepository.GetByIdAsync(updatedArticle.Id);
                if (article != null)
                {
                    article.Text = updatedArticle.Text;
                    article.Title = updatedArticle.Title;
                    article.UnitNumber = updatedArticle.UnitNumber;

                    _articleRepository.Update(article);

                    await _unitOfWork.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddChapterAsync(CreateChapterModel chapterModel)
        {
            try
            {
                var chapter = new Chapter();
                var maxOrderId = _chapterRepository.GetMaxOrderId();
                chapter.OrderId = maxOrderId + 1;
                chapter.Title = chapterModel.Title;
                int i = 0;
                chapter.Articles = new List<Article>(chapterModel.Articles.Select(a => new Article { Text = a.Text, Title = a.Title, UnitNumber = double.Parse($"{chapter.OrderId}.{++i}", CultureInfo.InvariantCulture) }));
                await _chapterRepository.AddAsync(chapter).ContinueWith(c => _unitOfWork.SaveChangesAsync());
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddArticleAsync(CreateArticleModel articleModel)
        {
            try
            {
                if (articleModel.ChapterId != null && articleModel.ChapterId != default(Guid))
                {
                    var chapter = await _chapterRepository.GetByIdAsync(articleModel.ChapterId.Value);
                    var article = new Article();
                    article.Text = articleModel.Text;
                    article.Title = articleModel.Title;
                    int maxUnitNumber = 0;
                    try
                    {
                        var tempArt = chapter.Articles.LastOrDefault(d => d.UnitNumber != null);
                        maxUnitNumber = tempArt != null ? int.Parse(tempArt.UnitNumber.ToString().Split(".")[1]) : 0;
                    }
                    catch { }
                    article.UnitNumber = double.Parse($"{chapter.OrderId}.{++maxUnitNumber}", CultureInfo.InvariantCulture);
                    chapter.Articles.Add(article);
                    _chapterRepository.Update(chapter);
                    await _unitOfWork.SaveChangesAsync();
                    return true;
                }
                else return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task DeleteChapterAsync(Chapter chapter)
        {
            _chapterRepository.Delete(chapter);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteArticleAsync(Article article)
        {
            _articleRepository.Delete(article);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<Chapter> GetChapterById(Guid Id)
        {
            var chapter = await _chapterRepository.GetByIdAsync(Id);

            return chapter;
        }

        public async Task<Article> GetArticleById(Guid Id)
        {
            var article = await _articleRepository.GetByIdAsync(Id);

            return article;
        }
    }
}

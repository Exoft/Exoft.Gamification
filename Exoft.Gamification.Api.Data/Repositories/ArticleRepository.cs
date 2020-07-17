using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using System;
using System.Linq;

namespace Exoft.Gamification.Api.Data.Repositories
{
    [Obsolete("Wll remove in next release.", false)]
    public class ArticleRepository : Repository<Article>, IArticleRepository
    {
        public ArticleRepository(UsersDbContext context) : base(context)
        {
        }

        protected override IQueryable<Article> IncludeAll()
        {
            return DbSet;
        }
    }
}

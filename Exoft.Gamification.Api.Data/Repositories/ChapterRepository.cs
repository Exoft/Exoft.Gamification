﻿using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data.Repositories
{
    public class ChapterRepository : Repository<Chapter>, IChapterRepository
    {
        public ChapterRepository(UsersDbContext context) : base(context)
        {
        }

        protected override IQueryable<Chapter> IncludeAll()
        {
            return DbSet.Include(c => c.Articles);
        }

        public override async Task<ReturnPagingInfo<Chapter>> GetAllDataAsync(PagingInfo pagingInfo)
        {
            var items = await IncludeAll()
                .OrderBy(s => s.OrderId)
                .Skip((pagingInfo.CurrentPage - 1) * pagingInfo.PageSize)
                .Take(pagingInfo.PageSize)
                .ToListAsync();

            int allItemsCount = await IncludeAll().CountAsync();

            var result = new ReturnPagingInfo<Chapter>()
            {
                CurrentPage = pagingInfo.CurrentPage,
                PageSize = items.Count,
                TotalItems = allItemsCount,
                TotalPages = (int)Math.Ceiling((double)allItemsCount / pagingInfo.PageSize),
                Data = items
            };

            return result;
        }

        public int GetMaxOrderId()
        {
            return DbSet.Max(c => c.OrderId);
        }
    }
}
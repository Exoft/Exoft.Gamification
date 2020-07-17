using AutoMapper;
using Exoft.Gamification.Api.Data.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Exoft.Gamification.Api.Test.DumbData
{
    public static class ReturnPagingInfoDumbData
    {
        public static ReturnPagingInfo<T> GetForModel<T>(PagingInfo pagingInfo, List<T> data)
        {
            var users = new ReturnPagingInfo<T>
            {
                Data = data,
                CurrentPage = pagingInfo.CurrentPage,
                PageSize = pagingInfo.PageSize,
                TotalItems = data.Count,
                TotalPages = (int)Math.Ceiling((double)data.Count / (pagingInfo.PageSize == 0 ? data.Count : pagingInfo.PageSize))
            };
            return users;
        }

        public static ReturnPagingInfo<TModel> GetWithModels<TModel, TEntity>(ReturnPagingInfo<TEntity> page, IMapper _mapper)
        {
            var readUserModel = page.Data.Select(i => _mapper.Map<TModel>(i)).ToList();
            var result = new ReturnPagingInfo<TModel>()
            {
                CurrentPage = page.CurrentPage,
                PageSize = page.PageSize,
                TotalItems = page.TotalItems,
                TotalPages = page.TotalPages,
                Data = readUserModel
            };

            return result;
        }
    }
}

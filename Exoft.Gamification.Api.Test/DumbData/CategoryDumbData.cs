using Exoft.Gamification.Api.Common.Models.Category;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Test.TestData;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exoft.Gamification.Api.Test.DumbData
{
    public class CategoryDumbData
    {
        public static Category GetRandomEntity()
        {
            return new Category
            {
                Name = RandomHelper.GetRandomString(20)
            };
        }

        public static List<Category> GetRandomEntities(int number)
        {
            var list = new List<Category>();
            for (int i = 0; i < number; i++)
            {
                list.Add(GetRandomEntity());
            }
            return list;
        }

        public static ReadCategoryModel GetRandomReadCategoryModel()
        {
            return new ReadCategoryModel
            {
                Id = Guid.NewGuid(),
                Name = RandomHelper.GetRandomString(20)
            };
        }

        public static ReadCategoryModel GetReadCategoryModel(CreateCategoryModel model)
        {
            return new ReadCategoryModel
            {
                Id = Guid.NewGuid(),
                Name = model.Name
            };
        }

        public static CreateCategoryModel GetRandomCreateCategoryModel()
        {
            return new CreateCategoryModel
            {
                Name = RandomHelper.GetRandomString(20)
            };
        }

        public static UpdateCategoryModel GetRandomUpdateCategoryModel()
        {
            return new UpdateCategoryModel
            {
                Name = RandomHelper.GetRandomString(20)
            };
        }

        public static ReadCategoryModel GetReadAchievementModel(UpdateCategoryModel model)
        {
            return new ReadCategoryModel
            {
                Id = Guid.NewGuid(),
                Name = model.Name
            };
        }

        public static Category GetEntity(UpdateCategoryModel model)
        {
            return new Category
            {
                Name = model.Name
            };
        }
    }
}

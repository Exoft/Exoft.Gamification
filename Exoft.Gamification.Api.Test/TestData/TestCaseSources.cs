using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Test.DumbData;
using NUnit.Framework;
using System;

namespace Exoft.Gamification.Api.Test.TestData
{
    public static class TestCaseSources
    {
        public static readonly TestCaseData[] PagingInfos = new TestCaseData[]
        {
            new TestCaseData(new PagingInfo()),
            new TestCaseData(new PagingInfo{ CurrentPage = 1, PageSize = 1}),
            new TestCaseData(new PagingInfo{ CurrentPage = 1, PageSize = 10})
        };

        public static readonly TestCaseData[] SingleGuid = new TestCaseData[]
        {
            new TestCaseData(Guid.Parse("3d26662f-d257-4a53-a71f-12be80cf07bb")),
            new TestCaseData(Guid.Parse("dd663aa2-08a4-4fa4-b032-2fd9aa0e4924"))
        };

        public static readonly TestCaseData[] PagingInfoWithGuid = new TestCaseData[]
       {
            new TestCaseData(new PagingInfo(), Guid.Parse("3d26662f-d257-4a53-a71f-12be80cf07bb")),
            new TestCaseData(new PagingInfo(), Guid.Parse("dd663aa2-08a4-4fa4-b032-2fd9aa0e4924")),
            new TestCaseData(new PagingInfo{ CurrentPage = 1, PageSize = 1}, Guid.Parse("3d26662f-d257-4a53-a71f-12be80cf07bb")),
            new TestCaseData(new PagingInfo{ CurrentPage = 1, PageSize = 10}, Guid.Parse("dd663aa2-08a4-4fa4-b032-2fd9aa0e4924"))
       };

        public static readonly TestCaseData[] ValidCreateUserModels = new TestCaseData[]
        {
            new TestCaseData(UserDumbData.GetRandomCreateUserModel())
        };

        public static readonly TestCaseData[] ValidCreateAchievementModels = new TestCaseData[]
        {
            new TestCaseData(AchievementDumbData.GetRandomCreateAchievementModel())
        };

        public static readonly TestCaseData[] ValidRequestAchievementModels = new TestCaseData[]
        {
            new TestCaseData(RequestAchievementDumbData.GetRequestAchievementModel())
        };

        public static readonly TestCaseData[] ValidRequestAchievements = new TestCaseData[]
        {
            new TestCaseData(RequestAchievementDumbData.GetRandomEntity())
        };

        public static readonly TestCaseData[] ValidTwoGuids = new TestCaseData[]
        {
            new TestCaseData(Guid.Parse("3d26662f-d257-4a53-a71f-12be80cf07bb"), Guid.Parse("dd663aa2-08a4-4fa4-b032-2fd9aa0e4924"))
        };

        public static readonly TestCaseData[] ValidUpdatePasswordAsyncModels = new TestCaseData[]
        {
            new TestCaseData("SomeCoolPassword2020"),
            new TestCaseData("SomeAnotherPassword2020")
        };

        public static readonly TestCaseData[] ValidUpdateFullUserModels = new TestCaseData[]
        {
            new TestCaseData(UserDumbData.GetRandomUpdateFullUserModel()),
            new TestCaseData(UserDumbData.GetRandomUpdateFullUserModel())
        };

        public static readonly TestCaseData[] ValidUpdateAchievementModels = new TestCaseData[]
        {
            new TestCaseData(AchievementDumbData.GetRandomUpdateAchievementModel()),
            new TestCaseData(AchievementDumbData.GetRandomUpdateAchievementModel())
        };

        public static readonly TestCaseData[] ValidCreateThankModels = new TestCaseData[]
        {
            new TestCaseData(ThankDumbData.GetCreateThankModel())
        };

        public static readonly TestCaseData[] ValidUpdateArticleModels = new TestCaseData[]
        {
            new TestCaseData(ArticleDumbData.GetRandomUpdateModel()),
            new TestCaseData(ArticleDumbData.GetRandomUpdateModel())
        };

        public static readonly TestCaseData[] ValidCreateArticleModels = new TestCaseData[]
        {
            new TestCaseData(ArticleDumbData.GetRandomCreateArticleModel()),
            new TestCaseData(ArticleDumbData.GetRandomCreateArticleModel())
        };

        public static readonly TestCaseData[] ValidCreateChapterModels = new TestCaseData[]
        {
            new TestCaseData(ArticleDumbData.GetRandomCreateChapterModel()),
            new TestCaseData(ArticleDumbData.GetRandomCreateChapterModel())
        };

        public static readonly TestCaseData[] ValidCreateCategoryModels = new TestCaseData[]
        {
            new TestCaseData(CategoryDumbData.GetRandomCreateCategoryModel())
        };

        public static readonly TestCaseData[] ValidUpdateCategoryModels = new TestCaseData[]
        {
            new TestCaseData(CategoryDumbData.GetRandomUpdateCategoryModel()),
            new TestCaseData(CategoryDumbData.GetRandomUpdateCategoryModel())
        };

        public static readonly TestCaseData[] ValidCreateOrderModels = new TestCaseData[]
        {
            new TestCaseData(OrderDumbData.GetCreateOrderModel()),
            new TestCaseData(OrderDumbData.GetCreateOrderModel())
        };

        public static readonly TestCaseData[] ValidUpdateOrderModels = new TestCaseData[]
        {
            new TestCaseData(OrderDumbData.GetUpdateOrderModel()),
            new TestCaseData(OrderDumbData.GetUpdateOrderModel())
        };

        public static readonly TestCaseData[] ValidCreateRequestOrderModels = new TestCaseData[]
        {
            new TestCaseData(RequestOrderDumbData.GetCreateRequestOrderModel(), Guid.Parse("3d26662f-d257-4a53-a71f-12be80cf07bb")),
            new TestCaseData(RequestOrderDumbData.GetCreateRequestOrderModel(), Guid.Parse("dd663aa2-08a4-4fa4-b032-2fd9aa0e4924"))
        };

        public static readonly TestCaseData[] ValidIFormFileWithNullableGuid = new TestCaseData[]
        {
            new TestCaseData(FileDumbData.GetFile(), Guid.Parse("3d26662f-d257-4a53-a71f-12be80cf07bb")),
            new TestCaseData(null, Guid.Parse("3d26662f-d257-4a53-a71f-12be80cf07bb")),
            new TestCaseData(FileDumbData.GetFile(), null)
        };

    }
}

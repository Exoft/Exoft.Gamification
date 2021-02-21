using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Test.DumbData;
using NUnit.Framework;
using System;

namespace Exoft.Gamification.Api.Test.TestData
{
    public static class TestCaseSources
    {
        public static readonly TestCaseData[] ValidPagingInfos = {
            new TestCaseData(new PagingInfo()),
            new TestCaseData(new PagingInfo{ CurrentPage = 1, PageSize = 1}),
            new TestCaseData(new PagingInfo{ CurrentPage = 1, PageSize = 10})
        };

        public static readonly TestCaseData[] ValidSingleGuid = {
            new TestCaseData(Guid.Parse("3d26662f-d257-4a53-a71f-12be80cf07bb")),
            new TestCaseData(Guid.Parse("dd663aa2-08a4-4fa4-b032-2fd9aa0e4924"))
        };

        public static readonly TestCaseData[] ValidCreateUserModels = {
            new TestCaseData(UserDumbData.GetRandomCreateUserModel())
        };

        public static readonly TestCaseData[] ValidCreateAchievementModels = {
            new TestCaseData(AchievementDumbData.GetRandomCreateAchievementModel())
        };

        public static readonly TestCaseData[] ValidRequestAchievementModels = {
            new TestCaseData(RequestAchievementDumbData.GetRequestAchievementModel())
        };

        public static readonly TestCaseData[] ValidRequestAchievements = {
            new TestCaseData(RequestAchievementDumbData.GetRandomEntity())
        };

        public static readonly TestCaseData[] ValidUpdatePasswordAsyncModels = {
            new TestCaseData("SomeCoolPassword2020"),
            new TestCaseData("SomeAnotherPassword2020")
        };

        public static readonly TestCaseData[] ValidUpdateFullUserModels = {
            new TestCaseData(UserDumbData.GetRandomUpdateFullUserModel()),
            new TestCaseData(UserDumbData.GetRandomUpdateFullUserModel())
        };

        public static readonly TestCaseData[] ValidUpdateAchievementModels = {
            new TestCaseData(AchievementDumbData.GetRandomUpdateAchievementModel()),
            new TestCaseData(AchievementDumbData.GetRandomUpdateAchievementModel())
        };

        public static readonly TestCaseData[] ValidCreateThankModels = {
            new TestCaseData(ThankDumbData.GetCreateThankModel())
        };

        public static readonly TestCaseData[] ValidCreateCategoryModels = {
            new TestCaseData(CategoryDumbData.GetRandomCreateCategoryModel())
        };

        public static readonly TestCaseData[] ValidUpdateCategoryModels = {
            new TestCaseData(CategoryDumbData.GetRandomUpdateCategoryModel()),
            new TestCaseData(CategoryDumbData.GetRandomUpdateCategoryModel())
        };

        public static readonly TestCaseData[] ValidCreateOrderModels = {
            new TestCaseData(OrderDumbData.GetCreateOrderModel()),
            new TestCaseData(OrderDumbData.GetCreateOrderModel())
        };

        public static readonly TestCaseData[] ValidUpdateOrderModels = {
            new TestCaseData(OrderDumbData.GetUpdateOrderModel()),
            new TestCaseData(OrderDumbData.GetUpdateOrderModel())
        };

        public static readonly TestCaseData[] ValidCreateRequestOrderModels = {
            new TestCaseData(RequestOrderDumbData.GetCreateRequestOrderModel(), Guid.Parse("3d26662f-d257-4a53-a71f-12be80cf07bb")),
            new TestCaseData(RequestOrderDumbData.GetCreateRequestOrderModel(), Guid.Parse("dd663aa2-08a4-4fa4-b032-2fd9aa0e4924"))
        };

        public static readonly TestCaseData[] ValidIFormFileWithNullableGuid = {
            new TestCaseData(FileDumbData.GetFile(), Guid.Parse("3d26662f-d257-4a53-a71f-12be80cf07bb")),
            new TestCaseData(null, Guid.Parse("3d26662f-d257-4a53-a71f-12be80cf07bb")),
            new TestCaseData(FileDumbData.GetFile(), null)
        };

    }
}

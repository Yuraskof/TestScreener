using Screener.Constants;
using Screener.Models;

namespace Screener.Utilities
{
    public static class ModelUtils
    {
        public static List<TestData> modelsList = GetModels();

        public static List<TestData> GetModels()
        {
            LoggerUtils.LogStep(nameof(GetModels) + " \"Start creating localized test data model\"");
            var json = File.ReadAllText(FileConstants.PathToTestData);
            var jsonObj = JsonUtils.ParseToJsonObject(json);
            var testData = jsonObj["Pages"].ToString();
            var modelsList = JsonUtils.ReadJsonData<List<TestData>>(testData);
            return modelsList;
        }




        //public static GetAccessTokenModel CreateGetAccessTokenModel()
        //{
        //    LoggerUtils.LogStep(nameof(CreateGetAccessTokenModel) + " 'Start creating get access token model'");
        //    GetAccessTokenModel model = new();
        //    model.Variant = FileUtils.TestData.Variant;
        //    return model;
        //}

        //public static bool IsModelsDatesDescending(List<TestModel> modelsFromPage)
        //{
        //    LoggerUtils.LogStep(nameof(IsModelsDatesDescending) + " 'Start checking tests sort'");

        //    for (int i = 1; i < modelsFromPage.Count; i++)
        //    {
        //        if (Convert.ToDateTime(modelsFromPage[i-1].StartTime) < Convert.ToDateTime(modelsFromPage[i].StartTime))
        //        {
        //            LoggerUtils.LogStep(nameof(IsModelsDatesDescending) + $" 'The latest date from previous - [{modelsFromPage[i]}]'");
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        //public static bool IsDbContainsModelsFromPage(List<TestModel> modelsFromPage, List<TestModel> modelsFromDb)
        //{
        //    LoggerUtils.LogStep(nameof(GetCountOfModelsWithSameFields) + " 'Start comparing models from database and from the page'");
        //    return GetCountOfModelsWithSameFields(modelsFromPage, modelsFromDb) == modelsFromPage.Count;
        //}

        //private static int GetCountOfModelsWithSameFields(List<TestModel> modelsFromPage, List<TestModel> modelsFromDb)
        //{
        //    LoggerUtils.Logger.Info(nameof(GetCountOfModelsWithSameFields));
        //    int matchesCount = 0;

        //    for (int i = 0; i < modelsFromPage.Count; i++)
        //    {
        //        foreach (var dbModel in modelsFromDb)
        //        {
        //            if (IsModelsHaveEqualNameAndDate(dbModel, modelsFromPage[i]))
        //            {
        //                matchesCount++;
        //                break;
        //            }
        //        }
        //    }
        //    return matchesCount;
        //}

        //private static bool IsModelsHaveEqualNameAndDate(TestModel modelFromDb, TestModel modelFromPage)
        //{
        //    LoggerUtils.Logger.Info(nameof(IsModelsHaveEqualNameAndDate) + $" 'Compare names [{modelFromDb.Name}] - [{modelFromPage.Name}]; Compare start time [{modelFromDb.StartTime}] - [{modelFromPage.StartTime}]'");
        //    modelFromPage.StartTime = StringUtils.ConvertDateTime(modelFromPage.StartTime);
        //    return modelFromDb.Name == modelFromPage.Name && modelFromDb.StartTime == modelFromPage.StartTime;
        //}
    }
}

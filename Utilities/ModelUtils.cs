using Screener.Constants;
using Screener.Models;

namespace Screener.Utilities
{
    public static class ModelUtils
    {
        public static List<TestData> modelsList = GetModels();

        public static List<TestData> GetModels()
        {
            LoggerUtils.LogStep(nameof(GetModels) + " \"Start creating test data model\"");
            var json = File.ReadAllText(FileConstants.PathToTestData);
            var jsonObj = JsonUtils.ParseToJsonObject(json);
            var testData = jsonObj["Pages"].ToString();
            var modelsList = JsonUtils.ReadJsonData<List<TestData>>(testData);
            return modelsList;
        }
    }
}

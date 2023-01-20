using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Screener.Utilities
{
    public static class JsonUtils
    {
        public static JObject ParseToJsonObject(string content)
        {
            LoggerUtils.LogStep(nameof(ParseToJsonObject) + " 'Start parsing to json object'");
            return JObject.Parse(content);
        }

        public static T ReadJsonData<T>(string content)
        {
            LoggerUtils.LogStep(nameof(ReadJsonData) + " 'Start deserializing'");
            return JsonConvert.DeserializeObject<T>(content);
        }

        public static T ReadJsonDataFromPath<T>(string path)
        {
            LoggerUtils.LogStep(nameof(ReadJsonDataFromPath) + $" 'Path - [{path}] deserialized'");
            return JsonConvert.DeserializeObject<T>(FileUtils.ReadFile(path));
        }

        public static string SerializeJsonData(object content)
        {
            LoggerUtils.LogStep(nameof(SerializeJsonData) + " 'Start serializing'");
            return JsonConvert.SerializeObject(content);
        }

        public static Dictionary<string, string> GetDataFromJson(string path)
        {
            LoggerUtils.LogStep(nameof(GetDataFromJson) + " 'Get data from json'");
            var json = File.ReadAllText(path);
            var jsonObj = JObject.Parse(json);
            Dictionary<string, string> methods = new();

            foreach (var element in jsonObj)
            {
                methods.Add(element.Key, element.Value.ToString());
            }
            return methods;
        }
    }
}

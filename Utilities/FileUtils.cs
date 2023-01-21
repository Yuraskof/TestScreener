using System.Text;
using Screener.Constants;
using Screener.Models;

namespace Screener.Utilities
{
    public static class FileUtils
    {
        public static readonly TestData TestData = JsonUtils.ReadJsonDataFromPath<TestData>(FileConstants.PathToTestData);
        //public static readonly LoginUser LoginUser = JsonUtils.ReadJsonDataFromPath<LoginUser>(FileConstants.PathToLoginUser);


        public static void ClearLogFile()
        {
            FileInfo file = new(FileConstants.PathToLogFile);

            if (file.Exists)
            {
                file.Delete();
                LoggerUtils.LogStep(nameof(ClearLogFile) + $" 'Log file deleted - [{file}]'");
            }
        }

        public static string ReadFile(string path)
        {
            LoggerUtils.LogStep(nameof(ReadFile) + $" 'File - [{path}] read'");
            StreamReader sr = new(path, Encoding.UTF8);
            return sr.ReadToEnd();
        }
    }
}
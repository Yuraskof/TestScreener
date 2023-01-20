using System.Text.RegularExpressions;
using Screener.Constants;

namespace Screener.Utilities
{
    public static class StringUtils
    {
        public static List<string> SeparateString(string text, char separator)
        {
            LoggerUtils.LogStep(nameof(SeparateString) + $" 'Start separating string - [{text}]'");
            try
            {
                string[] separatedData = text.Split(separator);

                List<string> userInfoFields = new();

                for (int i = 0; i < separatedData.Length; i++)
                {
                    userInfoFields.Add(separatedData[i].Trim());
                }
                return userInfoFields;
            }
            catch (Exception e)
            {
                LoggerUtils.LogError(nameof(SeparateString) + $" 'Can't separate string - [{text}]'", e);
                throw;
            }
        }

        //public static string ConvertDateTime(string date)
        //{
        //    LoggerUtils.Logger.Info(string.Format("Start converting {0}", date));
        //    DateTime dateTime = Convert.ToDateTime(date);
        //    return dateTime.ToString(FileUtils.TestData.DateTimeFormat);
        //}
        
        //public static string CreateGetTestModelSqlRequest(TestModel model)
        //{
        //    LoggerUtils.LogStep(nameof(CreateGetTestModelSqlRequest) + " 'Start creating get test model by name sql request'");
        //    string request = FileUtils.SqlRequests["getTestInfo"];
        //    return request.Replace("{0}", model.Name);
        //}

        //public static string CreateSendAttachmentsSqlRequest(string content, string contentType, int testId)
        //{
        //    LoggerUtils.LogStep(nameof(CreateSendAttachmentsSqlRequest) + " 'Start creating Send attachment sql request'");
        //    return $"{FileUtils.SqlRequests["addAttachments"]} ('{content}', '{contentType}', {testId})";
        //}

        //public static string CreateSendLogsSqlRequest(string content, int testId)
        //{
        //    LoggerUtils.LogStep(nameof(CreateSendLogsSqlRequest) + " 'Start creating send logs sql request'");
        //    return $"{FileUtils.SqlRequests["addLogs"]} ('{content}', {testId})";
        //}

        //public static string CreateSendTestSqlRequest(TestModel model)
        //{
        //    LoggerUtils.LogStep(nameof(CreateSendTestSqlRequest) + " 'Start creating Send test  sql request'");
        //    return $"{FileUtils.SqlRequests["SendTest"]} ('{model.Name}', {model.ProjectId}, '{model.MethodName}', {model.SessionId}, '{model.Env}', '{model.Browser}')";
        //}

        //public static string CreateSessionSqlRequest(SessionModel model)
        //{
        //    LoggerUtils.LogStep(nameof(CreateSessionSqlRequest) + " 'Start creating Set session sql request'");
        //    return $"{FileUtils.SqlRequests["CreateSession"]} ('{model.SessionKey} ', {model.BuildNumber})";
        //}

        //public static string CreateGetProjectIdByNameRequest(string name)
        //{
        //    LoggerUtils.LogStep(nameof(CreateGetProjectIdByNameRequest) + " 'Start creating get project id sql request'");
        //    return $"{FileUtils.SqlRequests["getProjectIdByName"]} '{name}'";
        //}

        //public static string CreateGetSessionIdRequest(SessionModel model)
        //{
        //    LoggerUtils.LogStep(nameof(CreateGetSessionIdRequest) + " 'Start creating get session sql request'");
        //    return $"{FileUtils.SqlRequests["getSessionId"]} '{model.SessionKey}'";
        //}

        public static string ConvertLogsToString()
        {
            LoggerUtils.LogStep(nameof(ConvertLogsToString) + " 'Start converting file to string'");
            string logs = FileUtils.ReadFile(FileConstants.PathToLogFile);
            return logs.Replace("'", "");
        }

        public static string ConvertToBase64(byte[] imageArray)
        {
            LoggerUtils.LogStep(nameof(ConvertToBase64) + " 'Start convertation byte[] to string'");
            return Convert.ToBase64String(imageArray);
        }

        public static string FormatLogs(string logs)
        {
            LoggerUtils.LogStep(nameof(FormatLogs) + " 'Start formating logs'");
            return logs.Replace("\r\n", " ").Replace("   ", " ").Replace("\\", "").Replace("  ", " ");
        }

        public static string RegexReplace(string pattern, string replacement, string text)
        {
            LoggerUtils.LogStep(nameof(RegexReplace) + $" 'Start replacing string -[{text}]'");
            Regex regex = new(pattern);
            return regex.Replace(text, replacement);
        }
    }
}

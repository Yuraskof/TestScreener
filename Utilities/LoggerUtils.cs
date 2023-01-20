using System.Runtime.CompilerServices;
using Aquality.Selenium.Browsers;
using Aquality.Selenium.Core.Logging;

namespace Screener.Utilities
{
    public static class LoggerUtils
    {
        public static Logger Logger => AqualityServices.Get<Logger>();
        private static void LogStep(string stepInfo, string stepType)
        {
            var shift = new string('#', 10);
            Logger.Info($"{shift} {stepType} {shift} {Environment.NewLine} {stepInfo}");
        }

        public static void LogError(string description, Exception exception)
        {
            Logger.Fatal($"Fatal: {description}", exception);
        }

        public static void LogStep([CallerMemberName] string stepInfo = "")
        {
            LogStep(stepInfo, stepType: "Action");
        }
    }
}

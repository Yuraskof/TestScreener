using Aquality.Selenium.Browsers;
using Screener.Utilities;

namespace Screener.Base
{
    public abstract class BaseTest
    {
        [SetUp]
        public void Setup()
        {
            FileUtils.ClearLogFile();
            LoggerUtils.Logger.Info($"Start scenario");
        }

        [TearDown]
        public virtual void AfterEach()
        {
            AqualityServices.Browser.Quit();
        }
    }
}
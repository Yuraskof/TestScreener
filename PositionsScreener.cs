using Aquality.Selenium.Browsers;
using Screener.Base;
using Screener.Forms.Pages;
using Screener.Utilities;

namespace Screener
{
    public class PositionsScreener : BaseTest
    {
        private DateTime dateTime = DateTime.Now;

        [Test(Description = "Checking the website functionality using UI and Database")]
        public void TestWebUiAndDatabase()
        {
            AqualityServices.Browser.GoTo(ModelUtils.modelsList[0].Url); // 0 Wan
            AqualityServices.Browser.Tabs().OpenInNewTab(ModelUtils.modelsList[1].Url); // 1 Balance
            AqualityServices.Browser.Tabs().OpenInNewTab(ModelUtils.modelsList[2].Url); // 2 Agressive
            AqualityServices.Browser.Maximize();

            MainPage mainPage = new();

            while (true)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (dateTime.AddMinutes(60) < DateTime.Now)
                    {
                        AqualityServices.Browser.Refresh();
                        LoggerUtils.Logger.Info($"refresh {DateTime.Now}");
                    }

                    AqualityServices.Browser.Tabs().SwitchToTab(i);
                    ExecuteMethod(AqualityServices.Browser.CurrentUrl, mainPage);
                }

                if (dateTime.AddMinutes(60) < DateTime.Now)
                {
                    LoggerUtils.Logger.Info($"datetime = {dateTime}, update to {DateTime.Now}");
                    dateTime = DateTime.Now;
                }

                Task.Delay(10000).Wait(); // 10 sec timer
            }
        }

        private void ExecuteMethod(string url, MainPage mainPage)
        {
            if (url == ModelUtils.modelsList[0].Url)
            {
                mainPage.AddPositionsToDb(ModelUtils.modelsList[0].PositionsTable, ModelUtils.modelsList[0].TradesTable);
            }
            else if (url == ModelUtils.modelsList[1].Url)
            {
                mainPage.AddPositionsToDb(ModelUtils.modelsList[1].PositionsTable, ModelUtils.modelsList[1].TradesTable);
            }
            else
            {
                mainPage.AddPositionsToDb(ModelUtils.modelsList[2].PositionsTable, ModelUtils.modelsList[2].TradesTable);
            }
        } 
    }
}

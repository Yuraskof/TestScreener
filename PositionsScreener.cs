using Aquality.Selenium.Browsers;
using Aquality.Selenium.Core.Utilities;
using Screener.Base;
using Screener.Forms;
using Screener.Forms.Pages;
using Screener.Models;
using Screener.Utilities;

namespace Screener
{
    public class PositionsScreener: BaseTest
    {
        [Test(Description = "Checking the website functionality using UI and Database")]
        
        public void TestWebUiAndDatabase()
        {
            AqualityServices.Browser.GoTo(ModelUtils.modelsList[0].Url); // 0 Wan
            AqualityServices.Browser.Maximize();
            //AqualityServices.Browser.Tabs().OpenInNewTab(ModelUtils.modelsList[1].Url); // 1 Balance
            //AqualityServices.Browser.Tabs().OpenInNewTab(ModelUtils.modelsList[2].Url); // 2 Agressive


            MainPage mainPage = new();

            mainPage.AddPositionsToDb(ModelUtils.modelsList[0].PositionsTable, ModelUtils.modelsList[0].TradesTable);

            //Assert.IsTrue(loginPage.State.WaitForDisplayed(), $"{loginPage.Name} should be presented");

            //loginPage.AcceptCookies();
            //loginPage.Login(FileUtils.LoginUser.Login, FileUtils.LoginUser.Password);

            //MainNavigationForm mainNavigationForm = new MainNavigationForm();

            //mainNavigationForm.SearchText();
            //mainNavigationForm.ExpandAllPeople();

            //SearchPage searchPage = new SearchPage();

            //try
            //{
            //    searchPage.AddContacts();
            //}
            //catch (Exception e)
            //{
            //    AqualityServices.Browser.Driver.GetScreenshot().SaveAsFile($"../../../exScreen{DateTime.Now.Millisecond}.jpg");
            //    LoggerUtils.Logger.Error(e.Message + " See screenshot");
            //    AqualityServices.Browser.Refresh();
            //    searchPage.AddContacts();
            //}


            //LoggerUtils.Logger.Info("Invitations are sent");

            //mainNavigationForm.GoToAllMessages();

            //MessagesPage messagesPage = new MessagesPage();

            //messagesPage.GetMessages();

            //messagesPage.SendSecondMessage();

        }
    }
}

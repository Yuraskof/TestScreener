using Aquality.Selenium.Elements.Interfaces;
using Aquality.Selenium.Forms;
using OpenQA.Selenium;
using Screener.Utilities;

namespace Screener.Forms
{
    public class MainNavigationForm : Form
    {
        private IButton SearchButton => ElementFactory.GetButton(By.Id("global-nav-typeahead"), "Search button");
        private ITextBox SearchTextBox => ElementFactory.GetTextBox(By.XPath("//input[@class='search-global-typeahead__input']"), "Search text box");
        private IButton AllPeopleButton => ElementFactory.GetButton(By.XPath("//a[contains (text(), 'See all people results')]"), "All people button");
        private IButton MessageButton => ElementFactory.GetButton(By.XPath("//a[contains(@href, \"messaging\")]"), "Message button");

        public MainNavigationForm() : base(By.Id("global-nav-typeahead"), "Main navigation form") 
        {
        }

        public void SearchText()
        {
            SearchButton.State.WaitForEnabled();
            SearchButton.Click();
            SearchTextBox.SendKeys(FileUtils.TestData.SearchText + Keys.Enter);
        }

        public void ExpandAllPeople()
        {
            AllPeopleButton.Click();
        }

        public void GoToAllMessages()
        {
            MessageButton.State.WaitForEnabled();
            MessageButton.Click();
        }
    }
}

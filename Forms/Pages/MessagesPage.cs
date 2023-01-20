using Aquality.Selenium.Core.Elements;
using Aquality.Selenium.Elements.Interfaces;
using Aquality.Selenium.Forms;
using OpenQA.Selenium;
using Screener.Utilities;

namespace Screener.Forms.Pages
{
    public class MessagesPage: Form
    {
        private IList<IButton> AllMessages => FormElement.FindChildElements<IButton>(By.XPath("//div[@class = \"msg-conversation-card__content--selectable\"]"), expectedCount: ElementsCount.MoreThenZero);
        private IList<IButton> UnreadMessages => FormElement.FindChildElements<IButton>(By.XPath("//span[contains(@class, \"notification-badge__count \")]"), "Unread message button");
        private IList<IButton> AllMessagesInChat => FormElement.FindChildElements<IButton>(By.XPath("//p[contains(@class, \"msg-s-event-listitem__body\")]"), "AllMessagesInChat");
        private ITextBox MessageTextBox => ElementFactory.GetTextBox(By.XPath("//div[contains(@class, \"msg-form__contenteditable\")]"), "Message text box");
        private IButton SendButton => ElementFactory.GetButton(By.XPath("//button[contains(@class, \"msg-form__send-button\")]"), "Send button");

        
        public MessagesPage() : base(By.XPath("//div[contains(@class, \"scaffold-layout__list-detail-inner\")]"), "Message page")
        {
        }

        public void GetMessages()
        {
            for (int i = 0; i < FileUtils.TestData.MessagesLookBack/7; i++)
            {
                AllMessages[^1].State.WaitForEnabled();
                AllMessages[^1].JsActions.ScrollIntoView();
            }
        }

        public void SendSecondMessage()
        {
            int anotherMessages = 0;

            while(UnreadMessages.Count > 0)
            {
                if (UnreadMessages.Count == anotherMessages)
                {
                    break;
                }

                UnreadMessages[0].JsActions.ScrollIntoView();
                UnreadMessages[0].Click();

                string firstMessage;

                try
                { 
                    firstMessage = AllMessagesInChat[0].Text;
                }
                catch (Exception e)
                {
                    continue;
                }
                

                if (AllMessagesInChat.Count <= 2 && firstMessage.Contains(FileUtils.TestData.Message))
                {
                    MessageTextBox.SendKeys(FileUtils.TestData.Message2);
                    SendButton.State.WaitForEnabled();
                    SendButton.Click();
                }
                else
                {
                    anotherMessages++;
                }
            }
        }
    }
}

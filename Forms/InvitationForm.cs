using Aquality.Selenium.Elements.Interfaces;
using Aquality.Selenium.Forms;
using LiteDB;
using OpenQA.Selenium;
using Screener.Utilities;

namespace Screener.Forms
{
    public class InvitationForm : Form
    {
        private IButton AddNoteButton => ElementFactory.GetButton(By.XPath("//button[@aria-label='Add a note']"), "Add note button");
        private ITextBox MessageTextBox => ElementFactory.GetTextBox(By.XPath("//textarea[@id ='custom-message']"), "Message text box");
        private IButton SendButton => ElementFactory.GetButton(By.XPath("//button[@aria-label =\"Send now\"]"), "Send button");
        private ITextBox SendToTextBox => ElementFactory.GetTextBox(By.XPath("//span[contains(@class, 'flex-1')]"), "Send to text box");

        private IButton CloseButton => ElementFactory.GetButton(By.XPath("//div[contains(@class, \"send-invite\")]//button[@aria-label = \"Dismiss\"]"), "Close button");
        
        public InvitationForm() : base(By.XPath("//div[contains (@class,\"artdeco-modal__actionbar\")]"), "Invitation form")
        {
        }

        public void SendInvitation()
        {
            try
            {
                string fullName = GetFullName();

                if (IsContactExistInDb(fullName))
                {
                    CloseButton.State.WaitForEnabled();
                    CloseButton.Click();
                    return;
                }

                string name = GetName(fullName);

                AddNoteButton.State.WaitForEnabled();
                AddNoteButton.Click();
                MessageTextBox.SendKeys($"{FileUtils.TestData.OpenerMessage} {name}! {FileUtils.TestData.Message}");
                SendButton.State.WaitForEnabled();
                SendButton.Click();
                SaveContact(fullName);
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                throw;
            }
        }

        private string GetFullName()
        {
            try
            {
                SendToTextBox.State.WaitForEnabled();
            }
            catch (Exception e)
            {
                CloseButton.Click();
                throw;
            }

            //SendToTextBox.State.WaitForEnabled();
            string messageWithName = SendToTextBox.Text;
            string[] splited = messageWithName.Split("invitation to");
            return splited[1].Trim();
        }

        private string GetName(string name)
        {
            string[] splitedName = name.Split(" ");
            return splitedName[0];
        }

        public void SaveContact(string contactName)
        {
            using (var db = new LiteDatabase(@"Filename = ../../../Contacts.db; connection = shared"))
            {
                // Get a collection (or create, if doesn't exist)
                var col = db.GetCollection<Contact>("Contacts");

                var currentContact = new Contact();
                    currentContact.Name = contactName;
                    
                col.Upsert(currentContact);
                db.Commit();
            }
        }

        public bool IsContactExistInDb(string contactName)
        {
            using (var db = new LiteDatabase(@"Filename = ../../../Contacts.db; connection = shared"))
            {
                // Get a collection (or create, if doesn't exist)
                var col = db.GetCollection<Contact>("Contacts"); //.FindAll();

                var contact = col.FindOne(x => x.Name == contactName);

                if (contact == null)
                {
                    return false;
                }

                return true;
            }
        }
    }
}

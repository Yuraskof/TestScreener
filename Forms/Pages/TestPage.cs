using OpenQA.Selenium;
using Aquality.Selenium.Forms;
using ExamTaskDockerUiDb.Models;
using Aquality.Selenium.Elements.Interfaces;
using ExamTaskDockerUiDb.Utilities;

namespace ExamTaskDockerUiDb.Forms.Pages
{
    public class TestPage : Form
    {
        private ITextBox ProjectNameTextBox => ElementFactory.GetTextBox(By.XPath("//*[@class='list-group-item-heading'][contains (text(), 'Project name')]//following-sibling::*[@class ='list-group-item-text']"), "Project name text box");
        private ITextBox TestNameTextBox => ElementFactory.GetTextBox(By.XPath("//*[@class='list-group-item-heading'][contains (text(), 'Test name')]//following-sibling::*[@class ='list-group-item-text']"), "Test name text box");
        private ITextBox TestMethodNameTextBox => ElementFactory.GetTextBox(By.XPath("//*[@class='list-group-item-heading'][contains (text(), 'Test method name')]//following-sibling::*[@class ='list-group-item-text']"), "Test method name text box");
        private ITextBox StatusTextBox(string status) => ElementFactory.GetTextBox(By.XPath(string.Format("//*[@class='list-group-item-heading'][contains (text(), 'Status')]//following-sibling::*//span[contains (text(), '{0}')]", status)), "Status text box");
        private ITextBox StartTime => ElementFactory.GetTextBox(By.XPath("//*[@class='list-group-item-heading'][contains (text(), 'Time info')]//following-sibling::*[contains (text(), 'Start time')]"), "Start time text box");
        private ITextBox Duration => ElementFactory.GetTextBox(By.XPath("//*[@class='list-group-item-heading'][contains (text(), 'Time info')]//following-sibling::*[contains (text(), 'Duration')]"), "Duration text box");
        private ITextBox EndTime => ElementFactory.GetTextBox(By.XPath("//*[@class='list-group-item-heading'][contains (text(), 'Time info')]//following-sibling::*[contains (text(), 'End time')]"), "End time text box");
        private ITextBox EnvionmentNameTextBox => ElementFactory.GetTextBox(By.XPath("//*[@class='list-group-item-heading'][contains (text(), 'Environment')]//following-sibling::*[@class ='list-group-item-text']"), "Environment name text box");
        private ITextBox BrowserNameTextBox => ElementFactory.GetTextBox(By.XPath("//*[@class='list-group-item-heading'][contains (text(), 'Browser')]//following-sibling::*[@class ='list-group-item-text']"), "Browser name text box");
        private ITextBox LogsTextBox => ElementFactory.GetTextBox(By.XPath("//div[@class='panel-heading'][contains (text(), 'Logs')]//following::td[contains (text(), 'Action')]"), "Logs text box");
        private ILabel ScreenshotLabel => ElementFactory.GetLabel(By.XPath("//div[@class='panel-heading'][contains (text(), 'Attachments')]//following-sibling::table//img"), "Screenshot label");

        private TestModel testModel;

        public TestPage() : base(By.XPath("//div[contains (@class, 'fail-reason-block')]"), "Test page")
        {
            testModel = new TestModel();
        }

        public TestModel GeTestModel() 
        {
            SetTestName();
            SetTestMethod();
            SetStartTime();
            SetEnvironmentName();
            SetBrowserName();
            return testModel;
        }

        public string GetProjectName()
        {
            return ProjectNameTextBox.GetText();
        }

        private void SetTestName()
        {
            testModel.Name = TestNameTextBox.GetText();
        }

        public string GetLogs()
        {
            return LogsTextBox.GetText();
        }
        public string GetImage()
        {
            return ScreenshotLabel.GetAttribute("src");
        }

        private void SetEnvironmentName()
        {
            testModel.Env = EnvionmentNameTextBox.GetText();
        }

        private void SetBrowserName()
        {
            testModel.Browser = BrowserNameTextBox.GetText();
        }

        private void SetTestMethod()
        {
            testModel.MethodName = TestMethodNameTextBox.GetText();
        }

        public bool IsStatusTextboxEnabled(TestModel model)
        {
            string status = null;
            if (model.StatusId == "")
            {
                status = "In progress";
            }
            return StatusTextBox(status).State.WaitForEnabled();
        }
        private void SetStartTime()
        {
            string date = StartTime.GetText();
            date = StringUtils.RegexReplace(": ", "!", date);
            date = StringUtils.SeparateString(date,'!')[1];
            date = StringUtils.ConvertDateTime(date);
            testModel.StartTime = date;
        }

        public bool IsDurationHasCorrectValue(TestModel model)
        {
            string duration = Duration.GetText();

            if (model.StatusId == "")
            {
                return duration.Contains("00:00:00.000");
            }
            return false;
        }

        public bool IsEndTimeHasCorrectValue(TestModel model)
        {
            string endTime = EndTime.GetText();

            if (model.StatusId == "")
            {
                var endTimeStrings = StringUtils.SeparateString(endTime, ':');
                return endTimeStrings[1] == "";
            }
            return false;
        }
    }
}

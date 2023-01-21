using Aquality.Selenium.Core.Elements;
using Aquality.Selenium.Elements.Interfaces;
using Aquality.Selenium.Forms;
using LiteDB;
using OpenQA.Selenium;
using Screener.Models;
using Screener.Utilities;

namespace Screener.Forms.Pages
{
    public class MainPage : Form
    {
        private IList<ITextBox> AllPositionsTextBoxes => FormElement.FindChildElements<ITextBox>(By.XPath("//table//tbody//tr"), expectedCount: ElementsCount.MoreThenZero);

        //span[contains(@class, "")]
        private IButton AcceptCookiesButton => ElementFactory.GetButton(By.XPath("//button[@action-type= 'ACCEPT']"), "Accept cookies button");
        private IButton LoginButton => ElementFactory.GetButton(By.XPath("//button[@class = 'sign-in-form__submit-button']"), "Login button");
        private ITextBox LoginTextBox => ElementFactory.GetTextBox(By.XPath("//input[@autocomplete = 'username']"), "Login text box");
        private ITextBox PasswordTextBox => ElementFactory.GetTextBox(By.XPath("//input[@autocomplete = 'current-password']"), "Password text box");


        public MainPage() : base(By.XPath("//section[contains(@class, \"flex\")]//div[contains(@class, \"overflow-x-auto\")]"), "Login page")
        {
        }

        public void AddPositionsToDb(string positionsCollectionName, string tradesCollectionName)
        {
            if(true)
            {
                // TODO Обновлено несколько секунд назад; минуту назад a minute ago a few seconds ago
                // TODO брать общий баланс, вычислять потом размер в позиции %
                //return;
            }

            try
            {
                List<PositionModel> positions = new List<PositionModel>();

                foreach (var position in AllPositionsTextBoxes)
                {
                    PositionModel positionForDb = new PositionModel();

                    IList<ITextBox> PositionAllTextBoxes = position.FindChildElements<ITextBox>(By.XPath("//span[contains(@class, \"\")]"), expectedCount: ElementsCount.MoreThenZero);

                    for (int i = 0; i < PositionAllTextBoxes.Count; i++)
                    {
                        switch (i)
                        {
                            case 0: // Name
                                positionForDb.Name = PositionAllTextBoxes[i].Text;
                                continue;
                            case 1: // Volume (Coins)
                                string volumeCoin = PositionAllTextBoxes[i].Text;

                                if (volumeCoin.Contains("-")) // Direction
                                {
                                    positionForDb.Direction = "Short";
                                }
                                else
                                {
                                    positionForDb.Direction = "Long";
                                }

                                positionForDb.VolumeCoin = StringUtils.Trim(volumeCoin);
                               
                                continue;
                            case 2: // Entry Price
                                PositionAllTextBoxes[i].JsActions.HoverMouse();
                                var price = PositionAllTextBoxes[i].FindChildElement<ITextBox>(By.XPath("//parent::div//following-sibling:: div//p")).Text;
                                positionForDb.EntryPrice = StringUtils.Trim(price);
                                continue;
                            case 3: // Volume (USDT)
                                positionForDb.VolumeUSDT = StringUtils.Trim(PositionAllTextBoxes[i].Text);
                                continue;
                            case 5: // Leverage
                                positionForDb.Leverage = PositionAllTextBoxes[i].Text;
                                continue;
                        }
                    }

                    positions.Add(positionForDb);
                }

                DatabaseUtils.SavePosition(positions, positionsCollectionName, tradesCollectionName);
            }
            catch (Exception e)
            {
                Logger.Info($"{e.Message} or No positions");
            }
        }
    }
}
using Aquality.Selenium.Core.Elements;
using Aquality.Selenium.Elements.Interfaces;
using Aquality.Selenium.Forms;
using OpenQA.Selenium;
using Screener.Models;
using Screener.Utilities;

namespace Screener.Forms.Pages
{
    public class MainPage : Form
    {
        private IList<ITextBox> AllPositionsTextBoxes => FormElement.FindChildElements<ITextBox>(By.XPath("//table//tbody//tr"), expectedCount: ElementsCount.MoreThenZero);

        private ITextBox DepoValue => ElementFactory.GetTextBox(By.XPath("//section[contains(@class, \"flex flex-col gap-1\")]//div[@class = \"text-base font-medium\"]//span[contains(text(), \"₮\")]"), "Depo text box");


        public MainPage() : base(By.XPath("//section[contains(@class, \"flex\")]//div[contains(@class, \"overflow-x-auto\")]"), "Login page")
        {
        }

        public void AddPositionsToDb(string positionsCollectionName, string tradesCollectionName)
        {
            decimal depo = StringUtils.Trim(DepoValue.Text);

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

                DatabaseUtils.SavePosition(positions, positionsCollectionName, tradesCollectionName, depo);
            }
            catch (Exception e)
            {
                Logger.Info($"{e.Message} or No positions");
            }
        }
    }
}
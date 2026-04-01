using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Interactions;
using System;

public class PRFlow
{
    private IWebDriver driver;
    private WebDriverWait wait;

    public static string prNumber = "";

    public PRFlow(IWebDriver driver)
    {
        this.driver = driver;
        this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
    }

    public void ScrollToElement(IWebElement element)
    {
        ((IJavaScriptExecutor)driver).ExecuteScript(
            "arguments[0].scrollIntoView({block:'center'});", element
        );
        //Thread.Sleep(100);
    }
    

    public void CreatePR()
    {
        try
        {

wait.Until(ExpectedConditions.ElementIsVisible(By.Id("subtype")));

Console.WriteLine("✅ Already on PR page");

            // ================= SUBTYPE =================
            IWebElement subType = wait.Until(d => d.FindElement(By.Id("subtype")));
            ScrollToElement(subType);
            subType.Click();
            subType.SendKeys("Purchase");
            Thread.Sleep(100);
            subType.SendKeys(Keys.Tab);

            // ================= PLANT =================
            IWebElement plant = wait.Until(d => d.FindElement(By.Id("plantID")));
            ScrollToElement(plant);
            plant.Click();
            plant.Clear();
            plant.SendKeys("1000");
            Thread.Sleep(100);
            plant.SendKeys(Keys.ArrowDown);
            plant.SendKeys(Keys.Enter);

            // ================= ASSET TYPE =================
            IWebElement assetType = wait.Until(d => d.FindElement(By.Id("assetType")));
            ScrollToElement(assetType);
            assetType.Click();
            assetType.Clear();
            assetType.SendKeys("Existing");
            Thread.Sleep(100);
            assetType.SendKeys(Keys.ArrowDown);
            assetType.SendKeys(Keys.Enter);

            Thread.Sleep(100);

            // ================= ASSET ID =================
            IWebElement assetId = wait.Until(d => d.FindElement(By.Id("assetID")));
            ScrollToElement(assetId);
            assetId.Clear();
            assetId.SendKeys("000030000302");
            Thread.Sleep(100);
            assetId.SendKeys(Keys.ArrowDown);
            assetId.SendKeys(Keys.Enter);

            // ================= QTY =================
            IWebElement qty = wait.Until(d => d.FindElement(By.Id("requiredQty")));
            ScrollToElement(qty);
            qty.Clear();
            qty.SendKeys("1");

            // ================= PRICE =================
            IWebElement price = wait.Until(d => d.FindElement(By.Id("unitPrice")));
            ScrollToElement(price);
            price.Clear();
            price.SendKeys("1");

            // ================= UNIT =================
            IWebElement unit = wait.Until(d => d.FindElement(By.Id("purchaseUnitID")));
            ScrollToElement(unit);
            unit.Click();
            unit.Clear();
            unit.SendKeys("NOS");
            Thread.Sleep(100);
            unit.SendKeys(Keys.ArrowDown);
            unit.SendKeys(Keys.Enter);

            // ================= COST CENTER =================
            IWebElement cost = wait.Until(d => d.FindElement(By.Id("costCenterID")));
            ScrollToElement(cost);
            cost.Click();
            cost.Clear();
            cost.SendKeys("90013200");
            Thread.Sleep(100);
            cost.SendKeys(Keys.ArrowDown);
            cost.SendKeys(Keys.Enter);

            // ================= GL ACCOUNT =================
            IWebElement gl = wait.Until(d => d.FindElement(By.Id("glAcctID")));
            ScrollToElement(gl);
            gl.Click();
            gl.SendKeys(Keys.Control + "a");
            gl.SendKeys(Keys.Delete);
            gl.SendKeys("21000250");
            Thread.Sleep(100);
            gl.SendKeys(Keys.ArrowDown);
            gl.SendKeys(Keys.Enter);

            // ================= ITEM GROUP =================
            IWebElement itemGroup = wait.Until(d => d.FindElement(By.Id("8684")));
            ScrollToElement(itemGroup);
            itemGroup.Click();
            itemGroup.Clear();
            itemGroup.SendKeys("M001");
            Thread.Sleep(800);
            itemGroup.SendKeys(Keys.ArrowDown);
            itemGroup.SendKeys(Keys.Enter);

            // ================= ADD =================
            IWebElement addBtn = wait.Until(d => d.FindElement(By.XPath("//button[normalize-space()='Add']")));
            ScrollToElement(addBtn);
            addBtn.Click();

            Console.WriteLine("✅ Item Added");

            // ================= SELECT ROW =================
            IWebElement checkbox = wait.Until(d => d.FindElement(
                By.XPath("(//table//tbody//tr)[1]//input")
            ));
            ScrollToElement(checkbox);
            checkbox.Click();

            // ================= ACTION =================
            IWebElement action = wait.Until(d => d.FindElement(
                By.XPath("//button[.//span[normalize-space()='Action']]")
            ));
            ScrollToElement(action);
            action.Click();

            // ================= GENERATE PR =================
            IWebElement generatePR = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//*[contains(text(),'Generate PR')]")
            ));
            generatePR.Click();

            IWebElement prText = wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//*[contains(text(),'PR No')]")
            ));

            string fullText = prText.Text;

            if (fullText.Contains(":"))
                prNumber = fullText.Split(':')[1].Trim();
            else
                prNumber = fullText;

            Console.WriteLine("🎉 PR Generated: " + prNumber);
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ ERROR: " + ex.Message);
            throw;
        }
    }
}
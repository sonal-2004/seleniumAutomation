using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

public class ApprovalFlow
{
    // 🔥 LOGIN
    public void Login(IWebDriver driver, WebDriverWait wait, string user, string pass)
    {
        driver.Navigate().GoToUrl("https://elabcorpinvfe-dev-hsawf8c9ctfgccdh.centralindia-01.azurewebsites.net/");

        wait.Until(ExpectedConditions.ElementIsVisible(
            By.XPath("//label[contains(text(),'Login-ID')]/following::input[1]")))
            .SendKeys(user);

        driver.FindElement(By.XPath("//label[contains(text(),'Password')]/following::input[1]"))
              .SendKeys(pass);

        driver.FindElement(By.XPath("//button[normalize-space()='Log in']")).Click();

        // ✅ STRONG WAIT
        wait.Until(ExpectedConditions.ElementIsVisible(
            By.XPath("//div[contains(.,'INVENTORY')]")
        ));

        Console.WriteLine($"✅ Login success: {user}");
    }

    // 🔥 APPROVAL LOGIC
    public void ApprovePR(IWebDriver driver, WebDriverWait wait, string prNumber, string action)
    {
        // ===== NAVIGATION =====
        wait.Until(ExpectedConditions.ElementToBeClickable(
            By.XPath("//div[contains(.,'INVENTORY')]"))).Click();

        wait.Until(ExpectedConditions.ElementToBeClickable(
            By.XPath("//span[text()='Purchase Request']"))).Click();

        wait.Until(ExpectedConditions.ElementToBeClickable(
            By.XPath("//span[text()='PR Approval Flow']"))).Click();

        wait.Until(ExpectedConditions.ElementToBeClickable(
            By.XPath("//span[normalize-space()='My Pending List']"))).Click();

        Console.WriteLine("✅ My Pending List opened");

        // ===== WAIT TABLE =====
        wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//table//tbody//tr")));

        // ===== CLICK CHECKBOX =====
        IWebElement checkbox = wait.Until(ExpectedConditions.ElementToBeClickable(
            By.XPath($"//table//tbody//tr[td[contains(text(),'{prNumber}')]]//td[1]//span")
        ));
        checkbox.Click();

        // ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", checkbox);

        Console.WriteLine("✅ Checkbox clicked");

        // ===== CLICK ACTION =====
        wait.Until(ExpectedConditions.ElementToBeClickable(
            By.XPath("//button[contains(.,'Action')]")
        )).Click();

        Console.WriteLine("✅ Action clicked");

        // ===== CLICK VERIFY / APPROVE =====
        wait.Until(ExpectedConditions.ElementToBeClickable(
            By.XPath($"//span[normalize-space()='{action}']")
        )).Click();

        Console.WriteLine($"➡️ {action} clicked");

        // ===== VERIFY =====
        if (action == "Verify")
        {
            wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//*[contains(text(),'Verified') or contains(text(),'successfully')]")
            ));

            Console.WriteLine("✅ Verify completed");
        }

        // ===== APPROVE =====
        if (action == "Approve")
        {
            IWebElement commentBox = wait.Until(
                ExpectedConditions.ElementIsVisible(By.XPath("//textarea"))
            );

            commentBox.SendKeys("Auto approved");

            IWebElement finalBtn = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//button[normalize-space()='Approve']")
            ));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", finalBtn);

            Console.WriteLine("🎉 Approved successfully");
        }
    }

    // 🔥 RUN METHOD
    public void RunApproval(string user, string action)
    {
        IWebDriver driver = new ChromeDriver();
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

        try
        {
            Login(driver, wait, user, "12345");
            ApprovePR(driver, wait, PRFlow.prNumber, action);
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ ERROR: " + ex.Message);
        }
        finally
        {
            driver.Quit();
        }
    }

    // 🔥 FLOWS
    public void SecondLogin_Verify()
     => RunApproval("FU_1000", "Verify");
    public void ThirdLogin_Approve()
     => RunApproval("LH_1000", "Approve");
    public void FourthLogin_Approve()
     => RunApproval("LA_1000", "Approve");
    public void FifthLogin_Approve()
     => RunApproval("BH_1000", "Approve");
}
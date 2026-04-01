using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using NUnit.Framework;
using System;
using System.IO;

public class BaseTest
{

    protected IWebDriver driver;
    protected WebDriverWait wait;
    public static string RunMode = "TEST"; 
// FLOW / TEST / ALL
 public void NavigateToPR()
{
    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

    // Step 1: Click Inventory PROPERLY
    IWebElement inventory = wait.Until(
        ExpectedConditions.ElementToBeClickable(
            By.XPath("//div[contains(.,'INVENTORY')]")
        )
    );

    //inventory.Click(); 
    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", inventory);
inventory.Click();

    Console.WriteLine("✅ Inventory clicked");

    // Step 2: Wait for Purchase Request 
    IWebElement prMenu = wait.Until(
        ExpectedConditions.ElementIsVisible(
            By.XPath("//*[contains(text(),'Purchase Request')]")
        )
    );

    //Thread.Sleep(1000); 

    prMenu.Click();

    Console.WriteLine("✅ Purchase Request clicked");

    //  Step 3: Click New PR
    IWebElement newPR = wait.Until(
        ExpectedConditions.ElementToBeClickable(
            By.XPath("//*[contains(text(),'New PR')]")
        )
    );

    newPR.Click();

    //  Step 4: Capex PR
    IWebElement capex = wait.Until(
        ExpectedConditions.ElementToBeClickable(
            By.XPath("//*[contains(text(),'Capex PR')]")
        )
    );

    capex.Click();

    // Step 5: Manual Form
    IWebElement manualForm = wait.Until(
        ExpectedConditions.ElementToBeClickable(
            By.XPath("//button[normalize-space()='Manual Form']")
        )
    );

    manualForm.Click();
wait.Until(d => d.FindElements(By.Id("subtype")).Count > 0);
    Console.WriteLine("✅ Fully navigated to PR form");
}

[OneTimeSetUp]
public void start()
{
    try {
    new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);

    var options = new ChromeOptions();

    //options.AddArgument("--headless=new");  // 🔥 KEY FIX
    options.AddArgument("--no-sandbox");
    options.AddArgument("--disable-dev-shm-usage");
    options.AddArgument("--disable-gpu");
    options.AddArgument("--disable-software-rasterizer");
    options.AddArgument("--disable-extensions");
    options.AddArgument("--window-size=1920,1080");
    ReportManager.InitReport(); 

    driver = new ChromeDriver(options);

    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

    driver.Navigate().GoToUrl("https://elabcorpinvfe-dev-hsawf8c9ctfgccdh.centralindia-01.azurewebsites.net/");

    IWebElement username = wait.Until(
        ExpectedConditions.ElementIsVisible(
            By.XPath("//label[contains(text(),'Login-ID')]/following::input[1]")
        )
    );
    username.SendKeys("SSO_1000");

    driver.FindElement(By.XPath("//label[contains(text(),'Password')]/following::input[1]"))
          .SendKeys("12345");

    driver.FindElement(By.XPath("//button[normalize-space()='Log in']")).Click();

    //wait.Until(d => !d.Url.Contains("login"));
    wait.Until(ExpectedConditions.ElementIsVisible(
    By.XPath("//div[normalize-space()='INVENTORY']")
));

    Console.WriteLine("✅ Login successful");
    Console.WriteLine("➡️ Navigating to PR (only once)");
     NavigateToPR();
    } 
    catch(Exception ex)
{
    Console.WriteLine("❌ Setup failed: " + ex.Message);
    throw;  
}

}
 [OneTimeTearDown]
public void TearDown()
{
    try
    {
        if (driver != null)
        {
            driver.Quit();
            driver.Dispose();
        }

        ReportManager.FlushReport();
    }
    catch (Exception e)
    {
        Console.WriteLine("TearDown error: " + e.Message);
    }
}
}
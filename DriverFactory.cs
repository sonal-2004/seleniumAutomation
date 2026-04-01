using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomationProject.Utilities
{
    public class DriverFactory
    {
        public static IWebDriver GetDriver()
        {
            ChromeOptions option = new ChromeOptions();
            option.AddArgument("--incognito");

            IWebDriver driver = new ChromeDriver(option);
            driver.Manage().Window.Maximize();

            return driver;
        }
    }
}
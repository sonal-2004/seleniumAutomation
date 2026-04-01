using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;  
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace AutomationProject.Tests
{
    [TestFixture]
    public class TestCases : BaseTest
    {

        public IWebElement SafeWait(By locator)
{
    try
    {
        return wait.Until(ExpectedConditions.ElementIsVisible(locator));
    }
    catch (WebDriverTimeoutException)
    {
        Console.WriteLine($"❌ Element not found: {locator}");
        Assert.Fail($"Element not found: {locator}");
        return null;
    }
}

//[SetUp]
[OneTimeSetUp]
public void BeforeEachTest()
{
    if (RunMode != "TEST" && RunMode != "ALL")
    {
        Assert.Ignore("Skipping TestCases");
    }
    //NavigateToPR();

    wait.Until(d => ((IJavaScriptExecutor)d)
        .ExecuteScript("return document.readyState").ToString() == "complete");
}
public void SelectDropdownOption(By inputLocator, string value)
{
    // 🔹 Open dropdown
    var input = wait.Until(ExpectedConditions.ElementToBeClickable(inputLocator));
    input.Click();
    input.Clear();
    input.SendKeys(value);

    // 🔹 Wait for dropdown option and click exact match
    var option = wait.Until(d =>
    {
        var elements = d.FindElements(By.XPath($"//div[@role='option']//span[contains(text(),'{value}')]"));
        return elements.Count > 0 ? elements[0] : null;
    });

    option.Click();

    // 🔹 Close dropdown (important)
    driver.FindElement(By.TagName("body")).Click();
}

    // ================= TEST CASES =================

[Test]
public void TC_CPX01_SubTypeVisible()
{
    Console.WriteLine("➡️ Running: " + TestContext.CurrentContext.Test.Name);

    ReportManager.test = ReportManager.extent.CreateTest("TC_CPX01_SubTypeVisible");

    try
    {
        IWebElement element = SafeWait(By.Id("subtype"));

        Assert.That(element.Displayed, Is.True);

        ReportManager.test.Pass("✅ SubType dropdown is visible");

        Console.WriteLine("✅ Completed: " + TestContext.CurrentContext.Test.Name);
    }
    catch (Exception ex)
    {
        ReportManager.test.Fail("❌ Test Failed: " + ex.Message);
        throw;
    }
}

        [Test]

public void TC_CPX02_PlantVisible()
{
    Console.WriteLine("➡️ Running: " + TestContext.CurrentContext.Test.Name);

    ReportManager.test = ReportManager.extent.CreateTest("TC_CPX02_PlantVisible");

    try
    {
        IWebElement element = wait.Until(
            ExpectedConditions.ElementIsVisible(By.Id("plantID"))
        );
        

        Assert.That(element.Displayed, Is.True, "Plant field not visible");

        ReportManager.test.Pass("✅ Plant field is visible");

        Console.WriteLine("✅ Completed: " + TestContext.CurrentContext.Test.Name);
    }
    catch (Exception ex)
    {
        ReportManager.test.Fail("❌ Test Failed: " + ex.Message);
        throw;
    }
}

[Test]
public void TC_CPX03_PlantAutoFill()
{
    Console.WriteLine("➡️ Running: " + TestContext.CurrentContext.Test.Name);

    ReportManager.test = ReportManager.extent.CreateTest("TC_CPX03_PlantAutoFill");

    try
    {
        // IWebElement plant = wait.Until(
        //     ExpectedConditions.ElementToBeClickable(By.Id("plantID"))
        // );
        var plant = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("plantID")));

        // Step 1: Click + Type
        plant.Click();
        plant.Clear();
        plant.SendKeys("1000");

        // 🔥 Step 2: Wait until value starts changing OR suggestions triggered
        wait.Until(d =>
        {
            string val = plant.GetAttribute("value");
            return val.Contains("1000");
        });

        // 🔥 Step 3: SMALL SAFE WAIT (better than Thread.Sleep)
        System.Threading.Thread.Sleep(50); 

        // 🔥 Step 4: Use keyboard (your UI supports this best)
        plant.SendKeys(Keys.ArrowDown);
        plant.SendKeys(Keys.Enter);

        // Step 5: Assertion
        string value = plant.GetAttribute("value");

        Assert.That(value, Does.Contain("1000"), "❌ Plant auto-fill failed");

        ReportManager.test.Pass("✅ Plant Autofill Working");

        Console.WriteLine("✅ Completed: " + TestContext.CurrentContext.Test.Name);
    }
    catch (Exception ex)
    {
        ReportManager.test.Fail("❌ Test Failed: " + ex.Message);
        throw;
    }
}
        [Test]
        public void TC_CPX04_AssetTypeVisible()
{
    Console.WriteLine("➡️ Running: " + TestContext.CurrentContext.Test.Name);
    ReportManager.test = ReportManager.extent.CreateTest("TC_CPX04_AssetTypeVisible");

    try
    {
        IWebElement element = wait.Until(
            ExpectedConditions.ElementToBeClickable(By.Id("assetType"))
        );

        Assert.That(element.Displayed, Is.True);
        ReportManager.test.Pass("✅ asset type is visible");
        Console.WriteLine("✅ Completed: " + TestContext.CurrentContext.Test.Name);
    }
    catch (Exception ex)
    {
        ReportManager.test.Fail("❌ Test Failed: " + ex.Message);
        throw;
    }
}
        

[Test]
public void TC_CPX05_AssetCodeVisible()
{
    Console.WriteLine("➡️ Running: " + TestContext.CurrentContext.Test.Name);

    ReportManager.test = ReportManager.extent.CreateTest("TC_CPX05_AssetCodeVisible");

    try
    {
        IWebElement element;

        try
        {
            element = wait.Until(
                ExpectedConditions.ElementIsVisible(By.Id("assetID"))
            );
        }
        catch (WebDriverTimeoutException)
        {
            Console.WriteLine("❌ Asset Code field not visible");
            Assert.Fail("Asset Code not visible");
            return;
        }

        // Assertion
        Assert.That(element.Displayed, Is.True, "Asset Code not visible");

        ReportManager.test.Pass("✅ Asset Code visible");
        Console.WriteLine("✅ Completed: " + TestContext.CurrentContext.Test.Name);
    }
    catch (Exception ex)
    {
        ReportManager.test.Fail("❌ " + ex.Message);
        throw;
    }
}

[Test]
public void TC_CPX10_ItemNameDisabled()
{
    ReportManager.test = ReportManager.extent.CreateTest("TC_CPX10_ItemNameDisabled");
    wait.Until(d => d.FindElement(By.Id("subtype")));
    driver.FindElement(By.Id("subtype"))
          .SendKeys("Purchase" + Keys.Tab);

    driver.FindElement(By.Id("plantID"))
          .SendKeys("1000" + Keys.ArrowDown + Keys.Enter);

    driver.FindElement(By.Id("assetType"))
          .SendKeys("Existing" + Keys.ArrowDown + Keys.Enter);

    driver.FindElement(By.Id("assetID"))
          .SendKeys("000030000302" + Keys.ArrowDown + Keys.Enter);
    Thread.Sleep(100); 

    IWebElement itemName = driver.FindElement(By.Id("itemName"));

    bool isDisabled = !itemName.Enabled ||
                      itemName.GetAttribute("readonly") != null;

    Assert.That(isDisabled, Is.True);
    ReportManager.test.Pass("✅ Item Name disable");
}
        [Test]
public void TC_CPX14_PRDateCheck()
{
    Console.WriteLine("➡️ Running: " + TestContext.CurrentContext.Test.Name);
    ReportManager.test = ReportManager.extent.CreateTest("TC_CPX14_PRDateCheck");

    try
    {
        IWebElement prDate = wait.Until(
            ExpectedConditions.ElementToBeClickable(By.Id("8664"))
        );

        string actual = prDate.GetAttribute("value");

        Assert.That(actual, Does.Contain(DateTime.Now.ToString("dd")));

        ReportManager.test.Pass("✅ PR Date verified");
        Console.WriteLine("✅ Completed: " + TestContext.CurrentContext.Test.Name);
    }
    catch (Exception ex)
    {
        ReportManager.test.Fail("❌ " + ex.Message);
        throw;
    }
}

[Test]
public void TC_CPX16_QuantityNumeric()
{
    Console.WriteLine("➡️ Running: " + TestContext.CurrentContext.Test.Name);
    ReportManager.test = ReportManager.extent.CreateTest("TC_CPX16_QuantityNumeric");

    try
    {
        IWebElement qty = wait.Until(
            ExpectedConditions.ElementToBeClickable(By.Id("requiredQty"))
        );

        qty.Clear(); 
        qty.SendKeys("10");

        Assert.That(qty.GetAttribute("value"), Is.EqualTo("10"));

        ReportManager.test.Pass("✅ Quantity accepts numeric input");
        Console.WriteLine("✅ Completed: " + TestContext.CurrentContext.Test.Name);
    }
    catch (Exception ex)
    {
        ReportManager.test.Fail("❌ " + ex.Message);
        throw;
    }
}

[Test]
public void TC_CPX22_CostCenterSelection()
{
    Console.WriteLine("➡️ Running: " + TestContext.CurrentContext.Test.Name);
    ReportManager.test = ReportManager.extent.CreateTest("TC_CPX22_CostCenterSelection");

    try
    {
        IWebElement cc = wait.Until(
            ExpectedConditions.ElementToBeClickable(By.Id("costCenterID"))
        );

        cc.Clear();
        cc.SendKeys("90013200");

        Assert.That(cc.GetAttribute("value"), Is.Not.Empty);

        ReportManager.test.Pass("✅ Cost Center selected");
        Console.WriteLine("✅ Completed: " + TestContext.CurrentContext.Test.Name);
    }
    catch (Exception ex)
    {
        ReportManager.test.Fail("❌ " + ex.Message);
        throw;
    }
}

[Test]
public void TC_CPX23_AddButtonVisible()
{
    Console.WriteLine("➡️ Running: " + TestContext.CurrentContext.Test.Name);
    ReportManager.test = ReportManager.extent.CreateTest("TC_CPX23_AddButtonVisible");

    try
    {
        IWebElement addBtn = wait.Until(
            ExpectedConditions.ElementIsVisible(By.XPath("//button[.='Add']"))
        );

        Assert.That(addBtn.Displayed, Is.True);

        ReportManager.test.Pass("✅ Add button visible");
        Console.WriteLine("✅ Completed: " + TestContext.CurrentContext.Test.Name);
    }
    catch (Exception ex)
    {
        ReportManager.test.Fail("❌ " + ex.Message);
        throw;
    }
}

[Test]

public void TC_CPX24_MandatoryValidation()
{
    Console.WriteLine("➡️ Running: " + TestContext.CurrentContext.Test.Name);
    ReportManager.test = ReportManager.extent.CreateTest("TC_CPX24_MandatoryValidation");

    try
    {
        // Step 1: Click Add
        IWebElement addBtn = wait.Until(
            ExpectedConditions.ElementToBeClickable(By.XPath("//button[contains(.,'Add')]"))
        );

        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", addBtn);
        addBtn.Click();

        // Step 2: Wait for validation (SAFE)
        IWebElement error;

        try
        {
            error = wait.Until(d =>
            {
                var elements = d.FindElements(By.XPath(
                    "//*[contains(text(),'Required') or contains(text(),'required')]"
                ));
                return elements.Count > 0 ? elements[0] : null;
            });
        }
        catch (WebDriverTimeoutException)
        {
            Console.WriteLine("❌ Validation message not found");
            Assert.Fail("Required validation not shown"); 
            return;
        }

        // Step 3: Assert
        Assert.That(error.Displayed, Is.True);

        ReportManager.test.Pass("✅ Mandatory validation working");
        Console.WriteLine("✅ Completed: " + TestContext.CurrentContext.Test.Name);
    }
    catch (Exception ex)
    {
        ReportManager.test.Fail("❌ " + ex.Message);
        throw;
    }
}

[Test]
public void TC_CPX26_ClearButton()
{
    Console.WriteLine("➡️ Running: " + TestContext.CurrentContext.Test.Name);
    ReportManager.test = ReportManager.extent.CreateTest("TC_CPX26_ClearButton");

    try
    {
        IWebElement clearBtn = wait.Until(
            ExpectedConditions.ElementToBeClickable(
                By.XPath("/html/body/div/div/div[1]/div/div[2]/div/div[2]/div[1]/div/div[3]/div[1]/div/div[2]/button[1]/span")
            )
        );

        // scroll (optional but safe)
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", clearBtn);

        // click
        clearBtn.Click();

        // validation
        IWebElement qty = wait.Until(
            ExpectedConditions.ElementIsVisible(By.Id("requiredQty"))
        );

        Assert.That(qty.GetAttribute("value"), Is.EqualTo(""));

        ReportManager.test.Pass("✅ Clear button is working");
        Console.WriteLine("✅ Completed: " + TestContext.CurrentContext.Test.Name);
    }
    catch (Exception ex)
    {
        ReportManager.test.Fail("❌ Test Failed: " + ex.Message);
        throw;
    }
}

[Test]
public void TC_CPX32_DeleteWithoutSelection()
{
    Console.WriteLine("➡️ Running: " + TestContext.CurrentContext.Test.Name);
    ReportManager.test = ReportManager.extent.CreateTest("TC_CPX32_DeleteWithoutSelection");

    try
    {
        IWebElement deleteBtn = wait.Until(
            ExpectedConditions.ElementToBeClickable(By.XPath("//button[contains(.,'Delete')]"))
        );

        ((IJavaScriptExecutor)driver).ExecuteScript(
            "arguments[0].scrollIntoView({block:'center'});", deleteBtn
        );

        Thread.Sleep(50);

        try
        {
            deleteBtn.Click();
        }
        catch
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", deleteBtn);
        }

        IWebElement msg = wait.Until(d =>
        {
            var elements = d.FindElements(By.XPath(
                "//*[contains(text(),'select') or contains(text(),'Select') or contains(text(),'required')]"
            ));
            return elements.FirstOrDefault(e => e.Displayed);
        });

        Console.WriteLine("Message found: " + msg.Text);

        Assert.That(msg.Text.ToLower(),
            Does.Contain("select").Or.Contain("required"),
            "❌ Validation message not shown"
        );

        ReportManager.test.Pass("✅ Delete validation working");
    }
    catch (Exception ex)
    {
        ReportManager.test.Fail("❌ " + ex.Message);
        throw;
    }
}
[Test]
public void TC_CPX06_AssetClassAutoFill()
{
    Console.WriteLine("➡️ Running: TC_CPX06");
    ReportManager.test = ReportManager.extent.CreateTest("TC_CPX06_AssetClassAutoFill");

    try
    {
        wait.Until(d => d.FindElement(By.Id("subtype")));

        driver.FindElement(By.Id("subtype"))
              .SendKeys("Purchase" + Keys.Tab);

        driver.FindElement(By.Id("plantID"))
              .SendKeys("1000" + Keys.ArrowDown + Keys.Enter);

        driver.FindElement(By.Id("assetType"))
              .SendKeys("Existing" + Keys.ArrowDown + Keys.Enter);

        driver.FindElement(By.Id("assetID"))
              .SendKeys("000030000302" + Keys.ArrowDown + Keys.Enter);

        IWebElement assetClass = wait.Until(d =>
        {
            var el = d.FindElement(By.Id("assetClass"));

            string val = el.GetAttribute("value");
            string text = el.Text;

            return (!string.IsNullOrEmpty(val) || !string.IsNullOrEmpty(text)) ? el : null;
        });
        string value = assetClass.GetAttribute("value");

        if (string.IsNullOrEmpty(value))
            value = assetClass.Text;

        Assert.That(value, Does.Contain("COMPUTER"));
             ReportManager.test.Pass("✅Asset Class auto-filled");

        Console.WriteLine("✅ Asset Class auto-filled: " + value);
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ " + ex.Message);
        throw;
    }
}
[Test]
public void TC_CPX08_ItemCodeDisabled()
{
    Console.WriteLine("➡️ Running: " + TestContext.CurrentContext.Test.Name);
    ReportManager.test = ReportManager.extent.CreateTest("TC_CPX08_ItemCodeDisabled");

    try
    {
        wait.Until(d => d.FindElement(By.Id("subtype")));

        driver.FindElement(By.Id("subtype"))
              .SendKeys("Purchase" + Keys.Tab);

        driver.FindElement(By.Id("plantID"))
              .SendKeys("1000" + Keys.ArrowDown + Keys.Enter);

        driver.FindElement(By.Id("assetType"))
              .SendKeys("Existing" + Keys.ArrowDown + Keys.Enter);

        driver.FindElement(By.Id("assetID"))
              .SendKeys("000030000302" + Keys.ArrowDown + Keys.Enter);

        Thread.Sleep(100);

        // 🔥 CORRECT ELEMENT (LABEL BASED)
        IWebElement element = driver.FindElement(
            By.XPath("//*[@id='root']/div/div[1]/div/div[2]/div/div[2]/div[1]/div/div[3]/div[1]/div/div[1]/div[7]/div/div")
        );

        // 🔥 CHECK DISABLED
        bool isDisabled = element.GetAttribute("class").Contains("disabled") ||
                          element.GetAttribute("aria-disabled") == "true";

        Assert.That(isDisabled, Is.True);

        ReportManager.test.Pass("✅ Item Code Disabled Verified");
    }
    catch (Exception ex)
    {
        ReportManager.test.Fail("❌ " + ex.Message);
        throw;
    }
}

    [Test]
public void TC_CPX09_DuplicateItemValidation()
{
    Console.WriteLine("➡️ Running: " + TestContext.CurrentContext.Test.Name);
    ReportManager.test = ReportManager.extent.CreateTest("TC_CPX09_DuplicateItemValidation");

    try
    {
        // ===== STEP 0: PRE-CONDITION (MANDATORY FIELDS) =====
        wait.Until(d => d.FindElement(By.Id("subtype")));

        driver.FindElement(By.Id("subtype"))
              .SendKeys("Purchase" + Keys.Tab);

        driver.FindElement(By.Id("plantID"))
              .SendKeys("1000" + Keys.ArrowDown + Keys.Enter);
              
        driver.FindElement(By.Id("itemCode"))
              .SendKeys("4000000663" + Keys.ArrowDown + Keys.Enter);

        // ===== STEP 2: ADD FIRST =====
        IWebElement addBtn = wait.Until(
            ExpectedConditions.ElementToBeClickable(By.XPath("//button[normalize-space()='Add']"))
        );
        addBtn.Click();

        wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//table/tbody/tr")));
        Console.WriteLine("✅ First item added");

    driver.FindElement(By.Id("itemCode"))
              .SendKeys("4000000663" + Keys.ArrowDown + Keys.Enter);

        // ===== STEP 4: ADD AGAIN =====
        addBtn.Click();

        // ===== STEP 5: VALIDATION =====
        IWebElement error = wait.Until(d =>
        {
            var e = d.FindElements(By.XPath(
                "//*[contains(text(),'already') or contains(text(),'duplicate')]"
            ));
            return e.Count > 0 ? e[0] : null;
        });

        Assert.That(error.Displayed, Is.True);

        Console.WriteLine("✅ Duplicate validation working");
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ " + ex.Message);
        ReportManager.test.Fail("❌ " + ex.Message);
        throw;
    }
}
    [Test]
    public void TC_CPX11_ItemGroupSelection()
    {
        Console.WriteLine("➡️ Running: " + TestContext.CurrentContext.Test.Name);
        ReportManager.test = ReportManager.extent.CreateTest("TC_CPX11_ItemGroupSelection");

        try
        {
            IWebElement itemGroup = wait.Until(
                ExpectedConditions.ElementIsVisible(By.Id("8684"))
            );

            itemGroup.SendKeys("M001");
            itemGroup.SendKeys(Keys.ArrowDown);
            itemGroup.SendKeys(Keys.Enter);

            Assert.That(itemGroup.GetAttribute("value"), Is.Not.Empty);

            ReportManager.test.Pass("✅ Item group selected");
            Console.WriteLine("✅ Completed: " + TestContext.CurrentContext.Test.Name);
        }
        catch (Exception ex)
        {
            ReportManager.test.Fail("❌ " + ex.Message);
            throw;
        }
    }
    [Test]
    public void TC_CPX17_UnitOfPurchaseDropdown()
    {
        Console.WriteLine("➡️ Running: " + TestContext.CurrentContext.Test.Name);
        ReportManager.test = ReportManager.extent.CreateTest("TC_CPX17_UnitOfPurchaseDropdown");

        try
        {
            IWebElement dropdown = wait.Until(
                ExpectedConditions.ElementIsVisible(By.Id("purchaseUnitID"))
            );

            dropdown.SendKeys("NOS");

            Assert.That(dropdown.GetAttribute("value"), Is.Not.Empty);

            ReportManager.test.Pass("✅ Unit of purchase selected");
            Console.WriteLine("✅ Completed: " + TestContext.CurrentContext.Test.Name);
        }
        catch (Exception ex)
        {
            ReportManager.test.Fail("❌ " + ex.Message);
            throw;
        }
    }

    [Test]
    public void TC_CPX18_DeliveryDate()
    {
        Console.WriteLine("➡️ Running: " + TestContext.CurrentContext.Test.Name);
        ReportManager.test = ReportManager.extent.CreateTest("TC_CPX18_DeliveryDate");

        try
        {
            IWebElement date = wait.Until(
                ExpectedConditions.ElementIsVisible(By.Id("deliveryDate"))
            );

            date.SendKeys("15-May-2025");

            Assert.That(date.GetAttribute("value"), Is.Not.Empty);

            ReportManager.test.Pass("✅ Delivery date accepted");
            Console.WriteLine("✅ Completed: " + TestContext.CurrentContext.Test.Name);
        }
        catch (Exception ex)
        {
            ReportManager.test.Fail("❌ " + ex.Message);
            throw;
        }
    }

[Test]
public void TC_CPX19_PastDateNotAllowed()
{
    Console.WriteLine("➡️ Running: " + TestContext.CurrentContext.Test.Name);
    ReportManager.test = ReportManager.extent.CreateTest("TC_CPX19_PastDateNotAllowed");

    try
    {
        IWebElement date = wait.Until(
            ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div/div/div[1]/div/div[2]/div/div[2]/div[1]/div/div[3]/div[1]/div/div[1]/div[16]/div/div/span/span/input"))
        );

        date.Clear();
        date.SendKeys("01-Jan-2020");
        date.SendKeys(Keys.Tab);

        IWebElement addBtn = wait.Until(
            ExpectedConditions.ElementToBeClickable(By.XPath("//button[.='Add']"))
        );

        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", addBtn);
        addBtn.Click();

        // 🔥 WAIT for validation message
        var errors = wait.Until(driver =>
        {
            var elems = driver.FindElements(By.XPath(
                "//*[contains(text(),'invalid') or contains(text(),'past') or contains(text(),'required')]"
            ));
            return elems.Count > 0 ? elems : null;
        });

        Assert.That(errors.Count, Is.GreaterThan(0), "❌ No validation message shown");

        ReportManager.test.Pass("✅ Past date validation working");
        Console.WriteLine("✅ Completed: " + TestContext.CurrentContext.Test.Name);
    }
    catch (Exception ex)
    {
        ReportManager.test.Fail("❌ " + ex.Message);
        throw;
    }
}

  [Test]
public void TC_CPX20_RequestBy()
{
    Console.WriteLine("➡️ Running: " + TestContext.CurrentContext.Test.Name);

    ReportManager.test = ReportManager.extent.CreateTest("TC_CPX20_RequestBy");

    try
    {
        IWebElement field = wait.Until(
            ExpectedConditions.ElementIsVisible(
                By.XPath("/html/body/div/div/div[1]/div/div[2]/div/div[2]/div[1]/div/div[3]/div[1]/div/div[1]/div[17]/div/div/div/input")
            )
        );

        field.Clear();
        field.SendKeys("Aditi");

        // ✅ NUnit Assertion
        Assert.That(field.GetAttribute("value"), Is.EqualTo("Aditi"), "Text not accepted");

        ReportManager.test.Pass("✅ Text accepted");
        Console.WriteLine("✅ Completed: " + TestContext.CurrentContext.Test.Name);
    }
    catch (Exception ex)
    {
        ReportManager.test.Fail("❌ " + ex.Message);
        throw;
    }
}

 [Test]
public void TC_CPX21_NoteField()
{
    Console.WriteLine("➡️ Running: " + TestContext.CurrentContext.Test.Name);

    ReportManager.test = ReportManager.extent.CreateTest("TC_CPX21_NoteField");

    try
    {
        IWebElement note = wait.Until(
            ExpectedConditions.ElementIsVisible(
                By.XPath("/html/body/div/div/div[1]/div/div[2]/div/div[2]/div[1]/div/div[3]/div[1]/div/div[1]/div[18]/div/div/div/input")
            )
        );

        note.Clear();
        note.SendKeys("Test Note");

        // ✅ NUnit Assertion
        Assert.That(note.GetAttribute("value"), Is.EqualTo("Test Note"), "Note failed");

        ReportManager.test.Pass("✅ Note working");
        Console.WriteLine("✅ Completed: " + TestContext.CurrentContext.Test.Name);
    }
    catch (Exception ex)
    {
        ReportManager.test.Fail("❌ " + ex.Message);
        throw;
    }
}

    [Test]

public void TC_CPX25_AddConfirmationPopup()
{
    Console.WriteLine("➡️ Running: " + TestContext.CurrentContext.Test.Name);

    ReportManager.test = ReportManager.extent.CreateTest("TC_CPX25_AddConfirmationPopup");

    try
    {
        // 🔹 Subtype
        driver.FindElement(By.Id("subtype"))
              .SendKeys("Purchase" + Keys.Tab);
              wait.Until(ExpectedConditions.ElementIsVisible(By.Id("plantID")));

        // 🔹 Plant
        driver.FindElement(By.Id("plantID"))
              .SendKeys("1000" + Keys.ArrowDown + Keys.Enter);

              //Thread.Sleep(100); 
              
        wait.Until(ExpectedConditions.ElementIsVisible(By.Id("itemCode")));

        // 🔹 Item Code
        driver.FindElement(By.Id("itemCode"))
              .SendKeys("Test" + Keys.Enter);

        // 🔹 Quantity
        driver.FindElement(By.XPath("//input[@type='number']"))
              .SendKeys("1");
              wait.Until(ExpectedConditions.ElementIsVisible(By.Id("unitPrice")));

        // 🔹 Unit Price
        driver.FindElement(By.XPath("//input[contains(@id,'unitPrice')]"))
              .SendKeys("100");

        // 🔹 Unit Of Purchase
        driver.FindElement(By.XPath("//label[text()='Unit Of Purchase']/following::input[1]"))
              .SendKeys("NOS" + Keys.Enter);

        // 🔹 Cost Center
        driver.FindElement(By.XPath("//label[text()='Cost Center']/following::input[1]"))
              .SendKeys("Admin" + Keys.Enter);

        // 🔹 G/L Account
        driver.FindElement(By.XPath("//label[text()='G/L Account']/following::input[1]"))
              .SendKeys("General" + Keys.Enter);

        // 🔹 Item Group
        driver.FindElement(By.XPath("//label[text()='Item Group']/following::input[1]"))
              .SendKeys("Default" + Keys.Enter);

        // 🔹 Click Add
        IWebElement addBtn = driver.FindElement(By.XPath("//button[normalize-space()='Add']"));

        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", addBtn);

        addBtn.Click();

        // 🔹 Validate Popup
        IWebElement toast = wait.Until(ExpectedConditions.ElementIsVisible(
            By.XPath("//*[contains(text(),'Item Added Successfully')]")
        ));

        Assert.That(toast.Displayed, Is.True, "❌ Confirmation popup not displayed");

        ReportManager.test.Pass("✅ Confirmation popup displayed successfully");
        Console.WriteLine("✅ Popup Verified");
    }
    catch (Exception ex)
    {
        ReportManager.test.Fail("❌ " + ex.Message);
        throw;
    }
}
  [Test]
public void TC_CPX27_ItemDataTable()
{
    Console.WriteLine("➡️ Running: " + TestContext.CurrentContext.Test.Name);
    ReportManager.test = ReportManager.extent.CreateTest("TC_CPX27_ItemDataTable");

    try
    {
        // 🔥 STEP 1: Fill required quantity
        IWebElement qty = wait.Until(
            ExpectedConditions.ElementIsVisible(By.Id("requiredQty"))
        );
        qty.Clear();
        qty.SendKeys("1");

        // 🔥 STEP 2: Select item
        IWebElement item = wait.Until(
            ExpectedConditions.ElementToBeClickable(By.Id("itemCode"))
        );

        item.Clear();
        item.SendKeys("I");

        // wait for dropdown
        wait.Until(d => d.FindElements(By.XPath("//div[contains(@class,'option')]")).Count > 0);

        item.SendKeys(Keys.ArrowDown);
        item.SendKeys(Keys.Enter);

        // 🔥 STEP 3: Click Add
        IWebElement addBtn = wait.Until(
            ExpectedConditions.ElementToBeClickable(By.XPath("//button[normalize-space()='Add']"))
        );

        addBtn.Click();

        // 🔥 STEP 4: WAIT for table rows (IMPORTANT)
        var rows = wait.Until(d =>
        {
            var r = d.FindElements(By.XPath("//table/tbody/tr"));
            return r.Count > 0 ? r : null;
        });

        // ✅ ASSERT
        Assert.That(rows.Count, Is.GreaterThan(0), "❌ Table is empty");

        ReportManager.test.Pass("✅ Table data present");
        Console.WriteLine("✅ Completed: " + TestContext.CurrentContext.Test.Name);
    }
    catch (Exception ex)
    {
        ReportManager.test.Fail("❌ " + ex.Message);
        throw;
    }
}

    [Test]
public void TC_CPX30_GeneratePRAfterUpdate()
{
    Console.WriteLine("➡️ Running: " + TestContext.CurrentContext.Test.Name);
    ReportManager.test = ReportManager.extent.CreateTest("TC_CPX30_GeneratePRAfterUpdate");

    try
    {
        driver.FindElement(By.TagName("body")).Click();

        // ===== STEP 1: FILL FORM =====
       // FillPRFormQuick();
        Console.WriteLine("✅ Item selected");

        // ===== STEP 2: ADD =====
        IWebElement addBtn = wait.Until(ExpectedConditions.ElementToBeClickable(
            By.XPath("//button[normalize-space()='Add']")
        ));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", addBtn);
        Console.WriteLine("✅ Item Added");

        // ===== STEP 3: EDIT =====
        IWebElement editBtn = wait.Until(ExpectedConditions.ElementToBeClickable(
            By.XPath("//button[normalize-space()='Edit']")
        ));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", editBtn);
        Console.WriteLine("✏️ Edit clicked");

        // 🔥 WAIT FOR FORM LOAD (VERY IMPORTANT)
        wait.Until(d =>
            d.FindElement(By.Id("assetDescription")).GetAttribute("value") != ""
        );

        // ===== STEP 4: UPDATE FIELD =====
        IWebElement qty = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("requiredQty")));
        qty.Clear();
        qty.SendKeys("2");
        Console.WriteLine("🔄 Quantity updated");

        // ===== STEP 5: UPDATE =====
        IWebElement updateBtn = wait.Until(ExpectedConditions.ElementToBeClickable(
            By.XPath("//button[contains(.,'Update')]")
        ));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateBtn);
        Console.WriteLine("✅ Update clicked");

    //     // 🔥 VERY IMPORTANT FIX (FIELDS RESET ISSUE)
    //     driver.FindElement(By.Id("purchaseUnitID")).SendKeys("NOS" + Keys.ArrowDown + Keys.Enter);
    driver.FindElement(By.Id("costCenterID")).SendKeys("90013200" + Keys.ArrowDown + Keys.Enter);
    //     driver.FindElement(By.TagName("body")).Click();

        // ===== WAIT FOR TABLE =====
        wait.Until(ExpectedConditions.ElementIsVisible(
            By.XPath("//table//tbody//tr")
        ));

        // ===== STEP 6: SELECT CHECKBOX =====
        IWebElement checkbox = wait.Until(ExpectedConditions.ElementToBeClickable(
            By.XPath("//table//tbody//tr[1]//td[1]//span")
        ));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", checkbox);
        Console.WriteLine("✅ Checkbox selected");

        // ===== STEP 7: ACTION =====
        IWebElement actionBtn = wait.Until(ExpectedConditions.ElementToBeClickable(
            By.XPath("//button[contains(.,'Action')]")
        ));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", actionBtn);
        Console.WriteLine("✅ Action clicked");

        // ===== STEP 8: GENERATE PR =====
        IWebElement genBtn = wait.Until(ExpectedConditions.ElementToBeClickable(
            By.XPath("//span[normalize-space()='Generate PR']")
        ));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", genBtn);
        Console.WriteLine("🎉 Generate PR clicked");

        // ===== STEP 9: VERIFY =====
        IWebElement prText = wait.Until(ExpectedConditions.ElementIsVisible(
            By.XPath("//*[contains(text(),'PR')]")
        ));

        Assert.That(prText.Text, Does.Contain("PR"));

        ReportManager.test.Pass("✅ PR Generated after Update");
        Console.WriteLine("✅ Completed: " + TestContext.CurrentContext.Test.Name);
    }
    catch (Exception ex)
    {
        ReportManager.test.Fail("❌ " + ex.Message);
        throw;
    }
}

[Test]
public void TC_CPX36_GeneratePR()
{
    Console.WriteLine("➡️ Running TC_CPX36 - Generate PR");

    ReportManager.test = ReportManager.extent.CreateTest("TC_CPX36_GeneratePR");

    try
    {

        PRFlow pr = new PRFlow(driver);
        pr.CreatePR();
        Console.WriteLine("🎉 Generate PR clicked");
        var prText = wait.Until(d =>
        {
            var el = d.FindElement(By.XPath("//*[contains(text(),'PR')]"));
            return el.Displayed ? el : null;
        });

        Console.WriteLine("🎉 PR Generated: " + prText.Text);
        Assert.That(prText.Text, Does.Contain("PR"));

        ReportManager.test.Pass("✅ PR Generated Successfully: " + prText.Text);

        Console.WriteLine("✅ TC_CPX36 PASSED");
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ ERROR: " + ex.Message);
        ReportManager.test.Fail("❌ " + ex.Message);
        throw;
    }
}
[Test]
public void TC_CPX31_DeleteFunctionality()
{
    Console.WriteLine("➡️ Running: TC_CPX31_DeleteFunctionality");

    try
    {
        // ===== STEP 1: CREATE PR FORM =====
        PRFlow pr = new PRFlow(driver);
        pr.CreatePR();

        // ===== STEP 2: CLICK ADD =====
        IWebElement addBtn = wait.Until(
            ExpectedConditions.ElementToBeClickable(
                By.XPath("//button[normalize-space()='Add']")
            )
        );
        addBtn.Click();
        Console.WriteLine("✅ Item Added");

        // ===== STEP 3: WAIT FOR TABLE ROWS =====
        var rows = wait.Until(d =>
        {
            var elements = d.FindElements(By.XPath("//table/tbody/tr"));
            return elements.Count > 0 ? elements : null;
        });

        int before = rows.Count;
        Console.WriteLine($"📊 Rows before delete: {before}");

        // ===== STEP 4: SELECT FIRST ROW =====
        IWebElement checkbox = wait.Until(
            ExpectedConditions.ElementToBeClickable(
                By.XPath("(//table//tbody//input[@type='checkbox'])[1]")
            )
        );
        checkbox.Click();
        Console.WriteLine("✅ Row selected");

        // ===== STEP 5: CLICK DELETE =====
        IWebElement deleteBtn = wait.Until(
            ExpectedConditions.ElementToBeClickable(
                By.XPath("//button[contains(.,'Delete')]")
            )
        );
        deleteBtn.Click();
        Console.WriteLine("🗑️ Delete clicked");

        // ===== STEP 6: CONFIRM DELETE =====
        IWebElement confirmBtn = wait.Until(
            ExpectedConditions.ElementToBeClickable(
                By.XPath("//button[contains(.,'Yes') or contains(.,'OK')]")
            )
        );
        confirmBtn.Click();
        Console.WriteLine("✅ Delete confirmed");

        // ===== STEP 7: WAIT FOR ROW REMOVAL =====
        wait.Until(d =>
            d.FindElements(By.XPath("//table/tbody/tr")).Count < before
        );

        int after = driver.FindElements(By.XPath("//table/tbody/tr")).Count;
        Console.WriteLine($"📉 Rows after delete: {after}");

        // ===== STEP 8: ASSERT =====
        Assert.That(after, Is.LessThan(before), "❌ Row was not deleted");

        Console.WriteLine("🎉 Delete functionality working correctly");
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ ERROR: " + ex.Message);
        throw;
    }
}
}}
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
public void FillMandatoryFields()
{
    Console.WriteLine("➡️ Filling Mandatory Fields");

    wait.Until(ExpectedConditions.ElementIsVisible(By.Id("subtype")));

    // close any open dropdown
    driver.FindElement(By.TagName("body")).Click();

    // ===== SUBTYPE =====
    var subtype = driver.FindElement(By.Id("subtype"));
    subtype.SendKeys("Purchase");
    subtype.SendKeys(Keys.ArrowDown);
    subtype.SendKeys(Keys.Enter);
    subtype.SendKeys(Keys.Tab);

    // ===== PLANT =====
    var plant = driver.FindElement(By.Id("plantID"));
    plant.SendKeys("1000");
    plant.SendKeys(Keys.ArrowDown);
    plant.SendKeys(Keys.Enter);

    wait.Until(d =>
        !string.IsNullOrEmpty(d.FindElement(By.Id("plantID")).GetAttribute("value"))
    );

    plant.SendKeys(Keys.Tab);

    // ===== ASSET TYPE =====
    var assetType = driver.FindElement(By.Id("assetType"));
    assetType.SendKeys("Existing");
    assetType.SendKeys(Keys.ArrowDown);
    assetType.SendKeys(Keys.Enter);
    assetType.SendKeys(Keys.Tab);

    // ===== ASSET ID =====
    var assetID = driver.FindElement(By.Id("assetID"));
    assetID.SendKeys("000030000302");
    assetID.SendKeys(Keys.ArrowDown);
    assetID.SendKeys(Keys.Enter);
    assetID.SendKeys(Keys.Tab);

    // ===== QTY (FIXED) =====
    var qty = driver.FindElement(By.Id("requiredQty"));
    qty.Clear();                  
    qty.SendKeys("1");

    // ===== PRICE (FIXED) =====
    var price = driver.FindElement(By.Id("unitPrice"));
    price.Clear();               
    price.SendKeys("1");

    // ===== UNIT =====
    var unit = driver.FindElement(By.Id("purchaseUnitID"));
    unit.SendKeys("NOS");
    unit.SendKeys(Keys.ArrowDown);
    unit.SendKeys(Keys.Enter);
    unit.SendKeys(Keys.Tab);

    // ===== COST CENTER (MANDATORY - FIXED) =====
    // var cost = driver.FindElement(By.Id("costCenterID"));
    // cost.SendKeys("90013200");
    // cost.SendKeys(Keys.ArrowDown);
    // cost.SendKeys(Keys.Enter);
    // cost.SendKeys(Keys.Tab);

    // ===== GL ACCOUNT =====
    var gl = driver.FindElement(By.Id("glAcctID"));
    gl.SendKeys("21000250");
    gl.SendKeys(Keys.ArrowDown);
    gl.SendKeys(Keys.Enter);
    gl.SendKeys(Keys.Tab);

    // ===== ITEM GROUP =====
    var itemGroup = driver.FindElement(By.Id("8684"));
    itemGroup.SendKeys("M001");
    itemGroup.SendKeys(Keys.ArrowDown);
    itemGroup.SendKeys(Keys.Enter);
    itemGroup.SendKeys(Keys.Tab);

    Console.WriteLine("✅ Mandatory Fields Filled");
}
// public void ClearForm()
// {
//     Console.WriteLine("🧹 Clearing form");

//     driver.FindElement(By.Id("requiredQty")).Clear();
//     driver.FindElement(By.Id("unitPrice")).Clear();

//     // reset dropdowns (important)
//     driver.FindElement(By.Id("subtype")).Clear();
//     driver.FindElement(By.Id("plantID")).Clear();

//     driver.FindElement(By.TagName("body")).Click(); // close dropdown
// }
//[SetUp]
[OneTimeSetUp]
public void BeforeEachTest()
{
   // driver.Navigate().Refresh();
    if (RunMode != "TEST" && RunMode != "ALL")
    {
        Assert.Ignore("Skipping TestCases");
    }
    //NavigateToPR();

    wait.Until(d => ((IJavaScriptExecutor)d)
        .ExecuteScript("return document.readyState").ToString() == "complete");
}
public void SelectDropdownFast(By locator, string value)
{
    var el = driver.FindElement(locator);

    el.Click();
    el.SendKeys(value);
    el.SendKeys(Keys.Enter);

    // minimal blur (important)
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
        IWebElement plant = wait.Until(
            ExpectedConditions.ElementToBeClickable(By.Id("plantID"))
        );

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
public void TC_CPX23_VerifyAddButtonFunctionality()
{
    Console.WriteLine("➡️ Running TC_CPX23 - Add Button Functionality");
    ReportManager.test = ReportManager.extent.CreateTest("TC_CPX23_VerifyAddButtonFunctionality");

    try
    {
         //ClearForm();
        // ===== STEP 1: LOAD PAGE =====
        wait.Until(ExpectedConditions.ElementIsVisible(By.Id("subtype")));
        Console.WriteLine("✅ PR Page Loaded");

        // ===== STEP 2: FILL ALL MANDATORY FIELDS =====
        FillMandatoryFields();

        // ===== STEP 3: CLICK ADD =====
        var addBtn = wait.Until(ExpectedConditions.ElementExists(
            By.XPath("//button[normalize-space()='Add']")
        ));

        ((IJavaScriptExecutor)driver)
            .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", addBtn);

        Thread.Sleep(100);

        ((IJavaScriptExecutor)driver)
            .ExecuteScript("arguments[0].click();", addBtn);

        Console.WriteLine("✅ Add button clicked");

        // ===== STEP 4: VERIFY ITEM ADDED IN TABLE =====
        var rows = wait.Until(d =>
        {
            var r = d.FindElements(By.XPath("//table//tbody//tr"));
            return r.Count > 0 ? r : null;
        });

        Console.WriteLine($"📊 Rows found: {rows.Count}");

        // ===== STEP 5: ASSERT =====
        Assert.That(rows.Count, Is.GreaterThan(0), "❌ Item not added");

        Console.WriteLine("✅ TC_CPX23 PASSED - Add functionality working");
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ ERROR: " + ex.Message);
        ReportManager.test.Fail("❌ " + ex.Message);
        throw;
    }
}

[Test]
public void TC_CPX24_VerifyMandatoryFieldValidation()
{
    Console.WriteLine("➡️ Running TC_CPX24 - Mandatory Field Validation");
    ReportManager.test = ReportManager.extent.CreateTest("TC_CPX24_VerifyMandatoryFieldValidation");

    try
    {
        // ===== PAGE LOAD =====
        wait.Until(ExpectedConditions.ElementIsVisible(By.Id("subtype")));

        wait.Until(d =>
            ((IJavaScriptExecutor)d)
                .ExecuteScript("return document.readyState")
                .ToString() == "complete"
        );

        Console.WriteLine("✅ PR Page Loaded");

        // ===== ADD BUTTON =====
        var addBtn = wait.Until(ExpectedConditions.ElementExists(
            By.XPath("//button[normalize-space()='Add']")
        ));

        // scroll
        ((IJavaScriptExecutor)driver)
            .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", addBtn);

        // small stabilization
       // Thread.Sleep(100);

        // safe click
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", addBtn);

        Console.WriteLine("⚠️ Clicked Add without filling fields");

        // ===== VALIDATION =====
        var errors = wait.Until(d =>
        {
            var els = d.FindElements(By.XPath(
                "//*[contains(text(),'required') or contains(text(),'mandatory')]"
            ));
            return els.Count > 0 ? els : null;
        });

        Assert.That(errors.Count, Is.GreaterThan(0));

        Console.WriteLine("✅ Validation working");
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ ERROR: " + ex.Message);
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
public void TC_CPX25_VerifyAddButtonFunctionality()
{
    Console.WriteLine("➡️ Running TC_CPX25 - Add Button Functionality");

    ReportManager.test = ReportManager.extent.CreateTest("TC_CPX25_VerifyAddButtonFunctionality");

    try
    {
        // ===== STEP 1: PAGE LOAD =====
        wait.Until(ExpectedConditions.ElementIsVisible(By.Id("subtype")));
        Console.WriteLine("✅ PR Page Loaded");

        // ===== STEP 2: FILL ALL MANDATORY FIELDS =====
        FillMandatoryFields();   // your reusable method

        Console.WriteLine("✅ Mandatory fields filled");

        // ===== STEP 3: CLICK ADD =====
        var addBtn = wait.Until(ExpectedConditions.ElementExists(
            By.XPath("//button[normalize-space()='Add']")
        ));

        ((IJavaScriptExecutor)driver)
            .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", addBtn);

        Thread.Sleep(100);

        ((IJavaScriptExecutor)driver)
            .ExecuteScript("arguments[0].click();", addBtn);

        Console.WriteLine("✅ Add button clicked");

        // ===== STEP 4: VERIFY ITEM ADDED IN TABLE =====
        var rows = wait.Until(d =>
        {
            var r = d.FindElements(By.XPath("//table//tbody//tr"));
            return r.Count > 0 ? r : null;
        });

        Console.WriteLine($"📊 Rows found: {rows.Count}");

        // ===== STEP 5: ASSERT =====
        Assert.That(rows.Count, Is.GreaterThan(0), "❌ Item not added");

        // ===== STEP 6: REPORT =====
        ReportManager.test.Pass("✅ Item added successfully and displayed in table");

        Console.WriteLine("✅ TC_CPX25 PASSED");
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ ERROR: " + ex.Message);

        ReportManager.test.Fail("❌ Test Failed: " + ex.Message);

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
public void TC_CPX28_VerifyEditFunctionality()
{
    Console.WriteLine("➡️ Running TC_CPX28 - Edit Functionality");
    ReportManager.test = ReportManager.extent.CreateTest("TC_CPX28_VerifyEditFunctionality");


    try
    {
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

        // ===== STEP 1: GET CURRENT ROW COUNT =====
        int before = driver.FindElements(By.XPath("//table//tbody//tr")).Count;

        // ===== STEP 2: ADD NEW ITEM =====
        FillMandatoryFields();

        // close any dropdown
        driver.FindElement(By.TagName("body")).Click();

        var addBtn = wait.Until(ExpectedConditions.ElementToBeClickable(
            By.XPath("//button[normalize-space()='Add']")
        ));

        ((IJavaScriptExecutor)driver)
            .ExecuteScript("arguments[0].click();", addBtn);

        // wait for new row
        wait.Until(d =>
            d.FindElements(By.XPath("//table//tbody//tr")).Count > before
        );

        Console.WriteLine("✅ New item added");

        // ===== STEP 3: CLICK 3 DOTS (ACTION MENU) =====
        var actionBtn = wait.Until(ExpectedConditions.ElementToBeClickable(
            By.XPath("(//table//tbody//tr[last()]//button)[last()]")
        ));

        ((IJavaScriptExecutor)driver)
            .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", actionBtn);

        driver.FindElement(By.TagName("body")).Click(); // close overlay
        Thread.Sleep(200);

        ((IJavaScriptExecutor)driver)
            .ExecuteScript("arguments[0].click();", actionBtn);

        Console.WriteLine("✅ Action menu opened");

        // ===== STEP 4: CLICK EDIT =====
        var editOption = wait.Until(ExpectedConditions.ElementToBeClickable(
            By.XPath("//span[contains(text(),'Edit')] | //button[contains(.,'Edit')]")
        ));

        ((IJavaScriptExecutor)driver)
            .ExecuteScript("arguments[0].click();", editOption);

        Console.WriteLine("✏️ Edit clicked");

        // ===== STEP 5: VERIFY DATA FILLED IN FORM =====
        var qty = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("requiredQty")));
        var price = driver.FindElement(By.Id("unitPrice"));

        string qtyValue = qty.GetAttribute("value");
        string priceValue = price.GetAttribute("value");

        Console.WriteLine("Qty: " + qtyValue);
        Console.WriteLine("Price: " + priceValue);

        // ===== STEP 6: ASSERT =====
        Assert.That(qtyValue, Is.Not.Empty, "❌ Qty not populated");
        Assert.That(priceValue, Is.Not.Empty, "❌ Price not populated");

        Console.WriteLine("✅ TC_CPX28 PASSED - Edit functionality working");
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ ERROR: " + ex.Message);
        ReportManager.test.Fail("❌ " + ex.Message);
        throw;
    }
}

[Test]

public void TC_CPX24_VerifyUpdateButtonFunctionality()
{
    Console.WriteLine("➡️ Running TC_CPX24 - Update Button Functionality");
    ReportManager.test = ReportManager.extent.CreateTest("TC_CPX24_VerifyUpdateButtonFunctionality");

    try
    {
        // ===== STEP 1: LOAD PAGE =====
        wait.Until(ExpectedConditions.ElementIsVisible(By.Id("subtype")));
        Console.WriteLine("✅ PR Page Loaded");

        // ===== STEP 2: ADD ITEM =====
        FillMandatoryFields();

        var addBtn = wait.Until(ExpectedConditions.ElementToBeClickable(
            By.XPath("//button[normalize-space()='Add']")
        ));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", addBtn);

        Console.WriteLine("✅ Item Added");

        // ===== STEP 3: WAIT FOR ROW =====
        var firstRow = wait.Until(d =>
        {
            var row = d.FindElement(By.XPath("//table//tbody//tr[1]"));
            return row.Displayed ? row : null;
        });

        Console.WriteLine("✅ Row available");
 var actionBtn = wait.Until(ExpectedConditions.ElementToBeClickable(
            By.XPath("(//table//tbody//tr[last()]//button)[last()]")
        ));

        ((IJavaScriptExecutor)driver)
            .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", actionBtn);

        driver.FindElement(By.TagName("body")).Click(); // close overlay
        Thread.Sleep(100);

        ((IJavaScriptExecutor)driver)
            .ExecuteScript("arguments[0].click();", actionBtn);

        Console.WriteLine("✅ Action menu opened");

        // ===== STEP 4: CLICK EDIT =====
        var editOption = wait.Until(ExpectedConditions.ElementToBeClickable(
            By.XPath("//span[contains(text(),'Edit')] | //button[contains(.,'Edit')]")
        ));

        ((IJavaScriptExecutor)driver)
            .ExecuteScript("arguments[0].click();", editOption);

        Console.WriteLine("✏️ Edit clicked");

        // ===== STEP 5: VERIFY DATA FILLED IN FORM =====
        var qty = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("requiredQty")));
        var price = driver.FindElement(By.Id("unitPrice"));

        string qtyValue = qty.GetAttribute("value");
        string priceValue = price.GetAttribute("value");

        Console.WriteLine("Qty: " + qtyValue);
        Console.WriteLine("Price: " + priceValue);

        // ===== STEP 8: CLICK UPDATE =====
        var updateBtn = wait.Until(ExpectedConditions.ElementToBeClickable(
            By.XPath("//button[normalize-space()='Update']")
        ));

        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateBtn);
        Console.WriteLine("✅ Update clicked");

        // ===== STEP 9: VERIFY =====
        var updatedRow = wait.Until(d =>
        {
            var row = d.FindElement(By.XPath("//table//tbody//tr[1]"));
            return row.Text.Contains("5") ? row : null;
        });

        string rowText = updatedRow.Text;
        Console.WriteLine("Updated Row: " + rowText);

        Assert.That(rowText, Does.Contain("5"), "❌ Quantity not updated");
        Assert.That(rowText, Does.Contain("10"), "❌ Price not updated");

        Console.WriteLine("✅ TC_CPX24 PASSED");
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ ERROR: " + ex.Message);
        ReportManager.test.Fail("❌ " + ex.Message);
        throw;
    }
}
[Test]
public void TC_CPX31_VerifyDeleteFunctionality()
{
    Console.WriteLine("➡️ Running TC_CPX31 - Delete Functionality");
    ReportManager.test = ReportManager.extent.CreateTest("TC_CPX31_VerifyDeleteFunctionality");

    try
    {
        // ===== STEP 1: PAGE LOAD =====
        wait.Until(ExpectedConditions.ElementIsVisible(By.Id("subtype")));
        Console.WriteLine("✅ PR Page Loaded");

        // ===== STEP 2: ADD ITEM (PRE-CONDITION) =====
        FillMandatoryFields();

        var addBtn = wait.Until(ExpectedConditions.ElementToBeClickable(
            By.XPath("//button[normalize-space()='Add']")
        ));

        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", addBtn);

        Console.WriteLine("✅ Item Added");

        // ===== STEP 3: WAIT FOR ROW =====
        var rowsBefore = wait.Until(d =>
        {
            var r = d.FindElements(By.XPath("//table//tbody//tr"));
            return r.Count > 0 ? r : null;
        });

        int initialCount = rowsBefore.Count;
        Console.WriteLine("📊 Rows before delete: " + initialCount);

        // ===== STEP 4: SELECT ROW CHECKBOX =====
        var checkbox = wait.Until(ExpectedConditions.ElementToBeClickable(
            By.XPath("//table//tbody//tr[1]//input[@type='checkbox']")
        ));

        ((IJavaScriptExecutor)driver)
            .ExecuteScript("arguments[0].click();", checkbox);

        Console.WriteLine("✅ Row selected");

        // ===== STEP 5: CLICK DELETE =====
        var deleteBtn = wait.Until(ExpectedConditions.ElementToBeClickable(
            By.XPath("//button[normalize-space()='Delete']")
        ));

        ((IJavaScriptExecutor)driver)
            .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", deleteBtn);

        ((IJavaScriptExecutor)driver)
            .ExecuteScript("arguments[0].click();", deleteBtn);

        Console.WriteLine(" Delete clicked");

        // ===== STEP 6: HANDLE CONFIRMATION (IF POPUP EXISTS) =====
        try
        {
            var confirmBtn = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//button[contains(.,'Yes') or contains(.,'Confirm')]")
            ));

            confirmBtn.Click();
            Console.WriteLine("Confirmation accepted");
        }
        catch
        {
            Console.WriteLine("No confirmation popup");
        }

        // ===== STEP 7: VERIFY ROW DELETED =====
        wait.Until(d =>
            d.FindElements(By.XPath("//table//tbody//tr")).Count < initialCount
        );

        int finalCount = driver.FindElements(By.XPath("//table//tbody//tr")).Count;
        Console.WriteLine("Rows after delete: " + finalCount);

        // ===== STEP 8: ASSERT =====
        Assert.That(finalCount, Is.LessThan(initialCount), "❌ Item not deleted");

        Console.WriteLine("✅ TC_CPX31 PASSED - Delete working");
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ ERROR: " + ex.Message);
        ReportManager.test.Fail("❌ " + ex.Message);
        throw;
    }
}
[Test]
public void TC_CPX30_GeneratePRAfterUpdate()
{
    Console.WriteLine("➡️ Running: TC_CPX30_GeneratePRAfterUpdate");
    ReportManager.test = ReportManager.extent.CreateTest("TC_CPX30_GeneratePRAfterUpdate");

    try
    {
        wait.Until(ExpectedConditions.ElementIsVisible(By.Id("subtype")));
        Console.WriteLine("✅ PR Page Loaded");

        // ===== STEP 2: ADD ITEM =====
        FillMandatoryFields();

        driver.FindElement(By.TagName("body")).Click();

        
        var addBtn = wait.Until(ExpectedConditions.ElementToBeClickable(
            By.XPath("//button[normalize-space()='Add']")
        ));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", addBtn);

        // ===== STEP 2: OPEN ACTION MENU (⋮) =====
         var actionBtn = wait.Until(ExpectedConditions.ElementToBeClickable(
            By.XPath("(//table//tbody//tr[last()]//button)[last()]")
        ));

        ((IJavaScriptExecutor)driver)
            .ExecuteScript("arguments[0].scrollIntoView({block:'center'});", actionBtn);

        driver.FindElement(By.TagName("body")).Click(); // close overlay
        Thread.Sleep(100);

        ((IJavaScriptExecutor)driver)
            .ExecuteScript("arguments[0].click();", actionBtn);

        Console.WriteLine("✅ Action menu opened");

        // ===== STEP 4: CLICK EDIT =====
        var editOption = wait.Until(ExpectedConditions.ElementToBeClickable(
            By.XPath("//span[contains(text(),'Edit')] | //button[contains(.,'Edit')]")
        ));

        ((IJavaScriptExecutor)driver)
            .ExecuteScript("arguments[0].click();", editOption);

        Console.WriteLine("✏️ Edit clicked");

        // ===== STEP 5: VERIFY DATA FILLED IN FORM =====
        var qty = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("requiredQty")));
        var price = driver.FindElement(By.Id("unitPrice"));

        string qtyValue = qty.GetAttribute("value");
        string priceValue = price.GetAttribute("value");

        Console.WriteLine("Qty: " + qtyValue);
        Console.WriteLine("Price: " + priceValue);

        // ===== STEP 8: CLICK UPDATE =====
        var updateBtn = wait.Until(ExpectedConditions.ElementToBeClickable(
            By.XPath("//button[normalize-space()='Update']")
        ));

        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updateBtn);
        Console.WriteLine("✅ Update clicked");

        // ===== STEP 9: VERIFY =====
        var updatedRow = wait.Until(d =>
        {
            var row = d.FindElement(By.XPath("//table//tbody//tr[1]"));
            return row.Text.Contains("5") ? row : null;
        });

        string rowText = updatedRow.Text;
        Console.WriteLine("Updated Row: " + rowText);

        Assert.That(rowText, Does.Contain("5"), "❌ Quantity not updated");
        Assert.That(rowText, Does.Contain("10"), "❌ Price not updated");

        // ===== STEP 6: SELECT UPDATED ROW =====
        var checkbox = wait.Until(ExpectedConditions.ElementToBeClickable(
            By.XPath("(//table//tbody//tr[last()]//input[@type='checkbox'])")
        ));
        ((IJavaScriptExecutor)driver)
            .ExecuteScript("arguments[0].click();", checkbox);

        Console.WriteLine("✅ Checkbox selected");

        // ===== STEP 7: CLICK ACTION BUTTON =====
        var actionMenu = wait.Until(ExpectedConditions.ElementToBeClickable(
            By.XPath("//button[contains(.,'Action')]")
        ));
        ((IJavaScriptExecutor)driver)
            .ExecuteScript("arguments[0].click();", actionMenu);

        // ===== STEP 8: CLICK GENERATE PR =====
        IWebElement generatePR = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//*[contains(text(),'Generate PR')]")
            ));
            generatePR.Click();
        Console.WriteLine("✅ TC_CPX30 PASSED");
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ ERROR: " + ex.Message);
        ReportManager.test.Fail("❌ " + ex.Message);
        throw;
    }
}
[Test]
public void TC_CPX36_GeneratePR()
{
    Console.WriteLine("➡️ Running TC_CPX36 - Generate PR");
     ReportManager.test = ReportManager.extent.CreateTest("TC_CPX30_GeneratePR");

    PRFlow pr = new PRFlow(driver);
    pr.CreatePR();
    Console.WriteLine("🎉 PR Generated: " + PRFlow.prNumber);

    Assert.That(PRFlow.prNumber, Is.Not.Empty);

    Console.WriteLine("✅ TC PASSED");
}

[Test]
public void TC_CPX37_VerifyItemSelectionValidation()
{
    Console.WriteLine("➡️ Running TC_CPX37 - Item Selection Validation");
    ReportManager.test = ReportManager.extent.CreateTest("TC_CPX37_VerifyItemSelectionValidation");

    try
    {
        // ===== STEP 1: LOAD PAGE =====
        wait.Until(ExpectedConditions.ElementIsVisible(By.Id("subtype")));
        Console.WriteLine("✅ PR Page Loaded");

        var plant = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("plantID")));
        plant.Click();
        plant.SendKeys("1000");
        plant.SendKeys(Keys.ArrowDown);
        plant.SendKeys(Keys.Enter);

        var qty = driver.FindElement(By.Id("requiredQty"));
        qty.Clear();
        qty.SendKeys("1");

        var price = driver.FindElement(By.Id("unitPrice"));
        price.Clear();
        price.SendKeys("1");

        // ===== STEP 3: CLICK ADD =====
        var addBtn = wait.Until(ExpectedConditions.ElementToBeClickable(
            By.XPath("//button[normalize-space()='Add']")
        ));
        addBtn.Click();

        // wait for row
        wait.Until(d => d.FindElements(By.XPath("//table//tbody//tr")).Count > 0);

        Console.WriteLine("✅ Item added but NOT selected");

        // ===== STEP 4: CLICK GENERATE PR =====
        var genBtn = wait.Until(ExpectedConditions.ElementIsVisible(
    By.XPath("//button[contains(.,'Generate PR')]")
));

// scroll to button
((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", genBtn);

// 🔥 WAIT until clickable (IMPORTANT)
wait.Until(ExpectedConditions.ElementToBeClickable(
    By.XPath("//button[contains(.,'Generate PR')]")
));

genBtn.Click();

        Console.WriteLine("⚠️ Generate PR clicked without selecting item");

        // ===== STEP 5: VALIDATE ERROR MESSAGE =====
        var errorMsg = wait.Until(d =>
        {
            var els = d.FindElements(By.XPath("//*[contains(text(),'Select Item')]"));
            return els.Count > 0 ? els[0] : null;
        });

        Console.WriteLine("📢 Error Message: " + errorMsg.Text);

        // ===== STEP 6: ASSERT =====
        Assert.That(errorMsg.Text.ToLower(),
            Does.Contain("select item"),
            "❌ Validation message not shown");

        Console.WriteLine("✅ TC_CPX37 PASSED");
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ ERROR: " + ex.Message);
        ReportManager.test.Fail("❌ " + ex.Message);
        throw;
    }
}

}}
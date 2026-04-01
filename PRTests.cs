
[TestFixture]
public class PRTests : BaseTest
{
    private PRFlow prFlow;

    [SetUp]
    public void Setup()
    {
        // 🔥 Control execution
        if (RunMode != "FLOW" && RunMode != "ALL")
        {
            Assert.Ignore("Skipping PR Flow");
        }

        prFlow = new PRFlow(driver);
    }

   [Test]

public void TC_PR_Generate()
{
    Console.WriteLine("➡️ Running Full PR Flow");

    // Step 1: Generate PR
    PRFlow prFlow = new PRFlow(driver);
    prFlow.CreatePR();

    Console.WriteLine("🎉 PR Created: " + PRFlow.prNumber);

    // 🔥 Step 2: Use ApprovalFlow (NOT prFlow)
    ApprovalFlow approval = new ApprovalFlow();

    approval.SecondLogin_Verify();
    approval.ThirdLogin_Approve();
    approval.FourthLogin_Approve();
    approval.FifthLogin_Approve();

    Console.WriteLine("✅ Full Approval Flow Completed");
}
}
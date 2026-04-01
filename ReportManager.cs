using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System;
using System.IO;

public class ReportManager
{
    public static ExtentReports extent;
    public static ExtentTest test;

    public static void InitReport()
    {
        string reportDir = Path.Combine(Directory.GetCurrentDirectory(), "Reports");

        if (!Directory.Exists(reportDir))
        {
            Directory.CreateDirectory(reportDir);
        }

        string path = Path.Combine(reportDir, "ExtentReport.html");

        var spark = new ExtentSparkReporter(path);
        extent = new ExtentReports();
        extent.AttachReporter(spark);
    }

    public static void FlushReport()
    {
        extent.Flush();
    }
}
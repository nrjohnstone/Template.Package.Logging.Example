using FluentAssertions;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using Serilog.Sinks.InMemory;

namespace ExampleLib.Tests.Unit;

public class SerilogTests
{
    [TearDown]
    public void TearDown()
    {
        Log.ClearLogger();
    }
    
    [Test]
    public void WhenLoggerIsSetExplicitly_ShouldLogToThatLogger()
    {
        var logProvider = CreateSerilogLogProvider();
        GlobalConfiguration.SetLogger(logProvider);
        
        AwesomeLibraryFunction sut = new AwesomeLibraryFunction();
        sut.DoSomethingAndLogIt();
        
        // assert
        InMemorySink.Instance.LogEvents.Count().Should().Be(1);
        InMemorySink.Instance.LogEvents.First().MessageTemplate.Text.Should().Be("Doing something !!");
    }
    
    [Test]
    public void WhenLoggerIsSetExplicitly_ShouldAddCategoryName_AsSourceContextProperty()
    {
        var logProvider = CreateSerilogLogProvider();
        GlobalConfiguration.SetLogger(logProvider);
        
        AwesomeLibraryFunction sut = new AwesomeLibraryFunction();
        sut.DoSomethingAndLogIt();
        
        // assert
        InMemorySink.Instance.LogEvents.Count().Should().Be(1);
        InMemorySink.Instance.LogEvents.First().Properties.Should().ContainKey("SourceContext");
        StringWriter sourceContext = new StringWriter();
        InMemorySink.Instance.LogEvents.First().Properties["SourceContext"].Render(sourceContext);
        sourceContext.ToString().Should().Be("\"AwesomeLibrary\"");
    }
    
    [Test]
    public void WhenLoggerIsSetExplicitly_AndLoggingWithTemplateValues_ShouldLogTemplateValuesCorrectly()
    {
        var logProvider = CreateSerilogLogProvider();
        GlobalConfiguration.SetLogger(logProvider);
        
        AwesomeLibraryFunction sut = new AwesomeLibraryFunction();
        sut.DoSomethingWithTemplateValues("Batman", "Robin");
        
        // assert
        InMemorySink.Instance.LogEvents.Count().Should().Be(1);
        InMemorySink.Instance.LogEvents.First().MessageTemplate.Text.Should().Be("Doing something with value1 {Value1} and {Value2} !!");
        InMemorySink.Instance.LogEvents.First().RenderMessage().Should()
            .Be("Doing something with value1 \"Batman\" and \"Robin\" !!");
    }

    [Test]
    public void WhenLoggerIsSetExplicitly_AndLoggingScopeHasValues_ShouldLogScopeValuesCorrectly()
    {
        var logProvider = CreateSerilogLogProvider();
        GlobalConfiguration.SetLogger(logProvider);
        
        AwesomeLibraryFunction sut = new AwesomeLibraryFunction();
        sut.DoSomethingWithScopeValues();
        
        // assert
        InMemorySink.Instance.LogEvents.Count().Should().Be(2);
        var logEvents = InMemorySink.Instance.LogEvents.ToList();
        logEvents[0].MessageTemplate.Text.Should().Be("Doing something with scope values !!");
        logEvents[0].Properties.Count.Should().Be(3);
        
        logEvents[0].Properties.Should().ContainKey("MeaningOfLife");
        var meaningOfLife = new StringWriter();
        logEvents[0].Properties["MeaningOfLife"].Render(meaningOfLife);
        meaningOfLife.ToString().Should().Be("42");

        logEvents[0].Properties.Should().ContainKey("TransactionId");
        var transactionId = new StringWriter();
        logEvents[0].Properties["TransactionId"].Render(transactionId);
        transactionId.ToString().Should().Be("67b3ba3c-7387-4686-b9a4-a132791dfc52");
    }
    
    private static ILoggerProvider CreateSerilogLogProvider()
    {
        Serilog.ILogger serilogLogger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.InMemory()
            .CreateLogger();

        return new SerilogLoggerProvider(serilogLogger);
    }

}
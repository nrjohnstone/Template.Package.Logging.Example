using System.Reflection;
using FluentAssertions;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Extensions.AspNetCore;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using log4net.Util;
using Microsoft.Extensions.Logging;

namespace ExampleLib.Log4Net.Tests.Unit;

public class Log4NetTests
{
    private MemoryAppender _memoryAppender;

    [SetUp]
    public void Setup()
    {
        Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository(Assembly.GetExecutingAssembly());

        _memoryAppender = new MemoryAppender();
        _memoryAppender.ActivateOptions();
        
        hierarchy.Root.AddAppender(_memoryAppender);

        hierarchy.Root.Level = Level.Info;
        hierarchy.Configured = true;
        
        BasicConfigurator.Configure(hierarchy);
        
        var log4NetLogProvider = new log4netLogProvider(true);
        ILoggerProvider logProvider = log4NetLogProvider;
        GlobalConfiguration.SetLogger(logProvider);
    }

    [TearDown]
    public void TearDown()
    {
        Log.ClearLogger();
    }

    [Test]
    public void WhenLoggerIsSetExplicitly_ShouldLogToThatLogger()
    {
        AwesomeLibraryFunction sut = new AwesomeLibraryFunction();
        
        // act
        sut.DoSomethingAndLogIt();
        
        // assert
        var loggingEvents = _memoryAppender.GetEvents();
        
        loggingEvents.Length.Should().Be(1);
        loggingEvents[0].MessageObject.Should().Be("Doing something !!");
    }
    
    
    [Test]
    public void WhenLoggerIsSetExplicitly_ShouldAddCategoryName_AsLoggerName()
    {
        AwesomeLibraryFunction sut = new AwesomeLibraryFunction();
        sut.DoSomethingAndLogIt();
        
        // assert
        var loggingEvents = _memoryAppender.GetEvents();
        
        loggingEvents.Length.Should().Be(1);
        loggingEvents[0].LoggerName.Should().Be("AwesomeLibrary");
    }

    [Test]
    public void WhenLoggerIsSetExplicitly_AndLoggingWithTemplateValues_ShouldLogTemplateValuesCorrectly()
    {
        AwesomeLibraryFunction sut = new AwesomeLibraryFunction();
        sut.DoSomethingWithTemplateValues("Batman", "Robin");
        
        // assert
        var loggingEvents = _memoryAppender.GetEvents();
        
        loggingEvents.Length.Should().Be(1);
        loggingEvents[0].MessageObject.Should().Be("Doing something with value1 Batman and Robin !!");
    }
    
    [Test]
    public void WhenLoggerIsSetExplicitly_AndLoggingScopeHasValues_ShouldLogScopeValuesCorrectly()
    {
        AwesomeLibraryFunction sut = new AwesomeLibraryFunction();
        sut.DoSomethingWithScopeValues();
        
        // assert
        var loggingEvents = _memoryAppender.GetEvents();
        
        loggingEvents.Length.Should().Be(2);
        
        // TODO : I have no idea how to get Log4Net to keep the properties added in a scope somewhere we can
        // validate them !!
        loggingEvents[0].MessageObject.ToString().Should().StartWith("Doing something with scope values !!");
    }
}
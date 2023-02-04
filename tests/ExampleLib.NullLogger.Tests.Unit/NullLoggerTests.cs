namespace ExampleLib.Tests.Unit;

[TestFixture]
public class NullLoggerTests
{
    [SetUp]
    public void Setup()
    {
    }

    [TearDown]
    public void TearDown()
    {
        // NOTE : The only really good way to test the static initializer is to have this test in it's own test project as the only test
        // or use an AppDomain per test but AppDomains are no longer available as of .NET Core so that's why this is in
        // it's own test project
    }

    [Test]
    public void WhenLoggerIsNotSetExplicitly_ShouldNotThrow()
    {
        AwesomeLibraryFunction sut = new AwesomeLibraryFunction();
        sut.DoSomethingAndLogIt();
    }

    [Test]
    public void WhenLoggerIsNotSetExplicitly_AndLoggingScopeHasValues_ShouldNotThrow()
    {
        AwesomeLibraryFunction sut = new AwesomeLibraryFunction();
        sut.DoSomethingWithScopeValues();
    }
    
}

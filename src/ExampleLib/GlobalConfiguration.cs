using Microsoft.Extensions.Logging;

namespace ExampleLib;

public static class GlobalConfiguration
{
    /// <summary>
    /// Set the ambient logger by providing an implementation of ILoggerProvider
    /// </summary>
    /// <param name="loggerProvider"></param>
    public static void SetLogger(ILoggerProvider loggerProvider)
    {
        // Prefer to request the user injects an ILoggerProvider so we can create a logger
        // with the SourceContext property set to our library name
        Log.SetLogger(loggerProvider.CreateLogger("AwesomeLibrary"));
    }
}
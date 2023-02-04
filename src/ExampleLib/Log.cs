using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace ExampleLib;

/// <summary>
/// Internal scope for static Log class so nothing outside the assembly may reference the logger
/// If your package has multiple assemblies consider using InternalsVisibleTo to allow only
/// the specified assemblies access to the logger
/// </summary>
internal static class Log
{
    private static object _lock = new object();
    
    // Provide an easy static logging method that is safe to use when not initialized and prevents us having to 
    // pass ILogger all over the place in our ctor/method signatures
    public static ILogger Logger { get; private set; } = NullLogger.Instance;
    
    public static void SetLogger(ILogger logger)
    {
        lock (_lock)
        {
            if (Logger is not NullLogger)
            {
                throw new InvalidOperationException("Ambient logger instance has already been set");
            }
            Logger = logger;
        }
    }
    
    /// <summary>
    /// Allow unit tests to clear the logger between tests
    /// </summary>
    internal static void ClearLogger()
    {
        lock (_lock)
        {
            Logger = NullLogger.Instance;    
        }
    }
}

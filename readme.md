# Logging Abstraction Example for Packages

## Summary

Often when developing a package to be used in any number of applications, there is a need to provide some logging to 
provide some observability with what is happening within the package. This is especially true once things start to go
wrong.

To be able to provide the ability for a consuming application to log, but not force the use of a particular logging framework is a requirement
that should be followed by a good package. A secondary concern is to make logging as painless as possible by avoiding the need to pass a logger
abstraction into all method calls etc...

Using an ambient logger is the easiest way to achieve this and with the right abstractions does not hinder testability if the correct implementation is used

## Usage

In general the ambient logger is set during the startup/bootstrapping phase of the application

```csharp
/// Bootstrapping/start of application ...
Serilog.ILogger serilogLogger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.InMemory()
            .CreateLogger();

var logProvider = new SerilogLoggerProvider(serilogLogger);

GlobalConfiguration.SetLogger(logProvider);
```

then to log from within the package simply

```csharp
Log.Logger.LogInformation("Doing something !!");
```
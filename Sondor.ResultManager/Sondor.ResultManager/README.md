# Sondor Result Manager
The Sondor Result manager package, provides an extendible manager to extend and customize the result pattern.
The result manager has direct support for localization, logging, exception handling and problem details. The result manager
can be used to provide a more functional approach to handling results, minimising the need to throw exceptions.

## Getting Started
  1. Add the `SondorResultManager` to the dependency container.
```csharp
    services.AddSondorResultManager();
```
  2. Use the `ISondorResultManager` interface to be injected.
  3. Add `Sondor.ResultManager.Extensions` namespace reference, to gain access to the extension options.
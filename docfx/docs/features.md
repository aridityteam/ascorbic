# Features

- **Observable Collections**: A collection that provides notifications when items get added, removed, or when the whole list is refreshed.
- **Performance Analyzing**: A utility for analyzing and measuring the performance used.
- **Configuration**: A utility for managing, writing, and reading configuration files for your application.
- **Disposable abstractions**: Some useful or useless abstraction classes that provides virtual functions for disposing managed or unmanaged objects.
- **Validation**: A set of validation utilities to ensure data integrity and correctness.
  (validation methods inside the library are provided by [Microsoft.VisualStudio.Validation](https://github.com/microsoft/vs-validation),
  all of their code is used in the library, and are modified to suit our needs.)
- **Proper Error-Handling**: AridityTeam.Ascorbic provides a few methods, and classes like `Result<T>` to ensure proper error handling.
- **Command-line parser**: A parser for command-line arguments that simplifies the process of handling user input.
- **Multi-text writer**: A utility for writing text to multiple outputs simultaneously, such as console standard output, console standard error, and file.
- **Logging**: A logging framework that supports various log levels and output formats, making it easier to track application behavior.
- **Extensions**: AridityTeam.Ascorbic provides variety of extension methods for base .NET types. (`string`, `TextWriter`, and `Stream`).
- **Service Management**: A utility that manages registered services, to be used in singleton instances.
- **Polyfills**: AridityTeam.Ascorbic provides extension methods to be able to use (some of) the latest .NET APIs in older framework runtimes.
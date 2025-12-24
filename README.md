# AridityTeam.Ascorbic

The main platform core library for The Aridity Team projects.

This library contains the core functionalities and abstractions that are shared across various projects within The Aridity Team ecosystem. 
It serves as the foundation for building applications and services that adhere to the team's standards and practices.

We've made this library available for anyone to use in their projects, in hopes that it will be useful, and make things simple for them in the future.

Contributions are always welcome, and it helps improve our other projects as well! To start contributing, you can read
[CONTRIBUTING.md](https://github.com/aridityteam/ascorbic/blob/master/CONTRIBUTING.md) to help you getting started on your first contribution into project.

## Features

- **Observable Collections**: A collection that provides notifications when items get added, removed, or when the whole list is refreshed.
- **Performance Analyzing**: A utility for analyzing and measuring the performance used.
- **Configuration**: A utility for managing, writing, and reading configuration files for your application.
- **Disposable abstractions**: Some useful or useless abstraction classes that provides virtual functions for disposing managed or unmanaged objects.
- **Validation**: A set of validation utilities to ensure data integrity and correctness.
  (validation methods inside the library are provided by [Microsoft.VisualStudio.Validation](https://github.com/microsoft/vs-validation),
  all of their code is used in the library, and are modified to suit our needs.)
- **Threading**: AridityTeam.Ascorbic provides a set of threading (mostly asynchronous) utilities that were not present in BCL.
- **Proper Error-Handling**: AridityTeam.Ascorbic provides a few methods, and classes like `Result<T>` to ensure proper error handling.
- **Command-line parser**: A parser for command-line arguments that simplifies the process of handling user input.
- **Multi-text writer**: A utility for writing text to multiple outputs simultaneously, such as console and file.
- **Logging**: A logging framework that supports various log levels and output formats, making it easier to track application behavior.
- **Extensions**: AridityTeam.Ascorbic provides variety of extension methods for base .NET types. (`string`, `TextWriter`, and `Stream`).
- **Service Management**: A utility that manages registered services, to be used in singleton instances.
- **Polyfills**: AridityTeam.Ascorbic provides extension methods to be able to use (some of) the latest .NET APIs in older framework runtimes, like .NET Framework v4.7.2.

## License

This library, and most of the other third-party components used is licensed under the MIT/X11 License. License terms may apply here.

### Third-party software used

Here is a list of third-party software used inside Ascorbic's code, in which made this library, and our other projects possible:

- [Nullable](https://www.nuget.org/packages/Nullable) (MIT License)
- [PolyType](https://www.nuget.org/packages/PolyType) (MIT License)
- [System.Composition](https://www.nuget.org/packages/System.Composition) (MIT License)
- [System.ComponentModel.Composition](https://www.nuget.org/packages/System.ComponentModel.Composition) (MIT License)
- [System.Text.Json](https://www.nuget.org/packages/System.Text.Json) (MIT License)
- [System.Net.Http](https://www.nuget.org/packages/System.Net.Http) (MIT License)
- [Microsoft.Bcl.AsyncInterfaces](https://www.nuget.org/packages/Microsoft.Bcl.AsyncInterfaces) (MIT License)
- [Microsoft.Bcl.Memory](https://www.nuget.org/packages/Microsoft.Bcl.Memory) (MIT License)
- [Microsoft.Bcl.HashCode](https://www.nuget.org/packages/Microsoft.Bcl.HashCode) (MIT License)
- [Microsoft.VisualStudio.Validation](https://www.nuget.org/packages/Microsoft.VisualStudio.Validation) (MIT License)

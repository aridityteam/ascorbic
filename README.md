# AridityTeam.Ascorbic

The main core platform library for The Aridity Team projects.

This library contains the core functionalities and abstractions that are shared across various projects within The Aridity Team ecosystem. 
It serves as the foundation for building applications and services that adhere to the team's standards and practices.

## Features

- **Observable Collections**: A collection that provides notifications when items get added, removed, or when the whole list is refreshed.
- **Performance Analyzing**: A utility for analyzing and measuring the performance used.
- **Validation**: A set of validation utilities to ensure data integrity and correctness.
  (validation methods inside the library are provided by [Microsoft.VisualStudio.Validation](https://github.com/microsoft/vs-validation),
  all of their code is used in the library, and are modified to suit our needs.)
- **Command-line parser**: A parser for command-line arguments that simplifies the process of handling user input.
- **Multi-text writer**: A utility for writing text to multiple outputs simultaneously, such as console and file.
- **Logging**: A logging framework that supports various log levels and output formats, making it easier to track application behavior.
- **Extensions**: AridityTeam.Ascorbic provides variety of extension methods for base .NET types. (`string`, `TextWriter`, and `Stream`).
- **Service Management**: A little utility that manages registered services.
- **Polyfills**: Some polyfill extensions to be able to use latest .NET APIs in older framework runtimes.

## License

This library, and some of other third-party components used is licensed under the MIT/X11 License. License terms may apply here.

### Third-party software used

- [Microsoft.VisualStudio.Validation](https://github.com/microsoft/vs-validation) (MIT License)

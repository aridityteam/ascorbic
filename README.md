# AridityTeam.Ascorbic
The main core platform library for The Aridity Team projects.

This library contains the core functionalities and abstractions that are shared across various projects within The Aridity Team ecosystem. 
It serves as the foundation for building applications and services that adhere to the team's standards and practices.

## Features
- **Observable Collections**: A collection that provides notifications when items get added, removed, or when the whole list is refreshed.
- **Validation**: A set of validation utilities to ensure data integrity and correctness.
  (validation methods inside the library are provided by [Microsoft.VisualStudio.Validation](https://github.com/microsoft/vs-validation),
  most of their code is used in the library.)
- **Command-line parser**: A parser for command-line arguments that simplifies the process of handling user input.
- **Multi-text writer**: A utility for writing text to multiple outputs simultaneously, such as console and file.
- **Logging**: A logging framework that supports various log levels and output formats, making it easier to track application behavior.
- **Extensions**: AridityTeam.Ascorbic provides variety of extension methods for `string`, and `Stream`.
- **Service Management**: A little utility that manages registered services.

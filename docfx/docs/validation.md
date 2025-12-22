# Validation

AridityTeam.Ascorbic provides variety of validation utilities to ensure correctness in data integrity, in which all of that is based on the [Microsoft.VisualStudio.Validation](https://github.com/microsoft/vs-validation), and which is also modified to try to suit our needs.

## Usage

### Requires

The `Requires` class provides a set of checks to ensure that the inputted parameter from a method is correct and is expected. Most notably, `Requires.NotNull`, and `Requires.NotNullOrEmpty` is the most common methods to be mostly used in our softwares.

#### Example

```cs
using System.Collections.Generic;
using AridityTeam;

namespace MyProgram {
    public class MyClass {
        public MyClass(object? nullableObj) {
            // ensures that the inputted "object" is not a null object.
            Requires.NotNull(nullableObj);
        }
        public MyClass(string myString) {
            // ensures that the string's contents is not empty.
            Requires.NotNullOrEmpty(myString);
        }
        public MyClass(IEnumerable<object> myString) {
            // ensures that the collection is not empty.
            Requires.NotNullOrEmpty(myString);
        }
    }
}
```

### Verify

The `Verify` class provides a few checks to ensure that an completed operation/task indicates an success NOT an failure. The `Verify.Operation`, and `Verify.NotDisposed` methods are usually used in our softwares.

#### Example

```cs
using AridityTeam;

namespace MyProgram {
    public class MyDisposableClass : DisposableObject {
        protected override void DisposeManagedResources() {
        }
        protected override unsafe void DisposeUnmanagedResources() {
        }
    }

    internal static class Program {
        public static void Main(string[] args) {
            Verify.Operation(true, "This does not throw an error.");
            Verify.Operation(false, "This throws an error.");

            Verify.Operation(Result.Success(new object()), "This does not throw an error.");
            Verify.Operation(Result.Failure(null), "This throws an error.");

            var disposable = new MyDisposableClass();

            // This does not throw an error.
            Verify.NotDisposed(disposable);

            disposable.Dispose();

            // This now throws an error.
            Verify.NotDisposed(disposable);
        }
    }
}
```

### Assumes

TO-DO.

#### Example

```cs
// TO-DO.
```


### Report

TO-DO.

#### Example

```cs
// TO-DO.
```

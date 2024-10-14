# EasyPathManager Library

The `EasyPathManager` library provides classes for managing file and directory paths in .NET. These classes offer methods for creating, adding, removing, and checking the existence of files and directories, making it easier to handle path-related operations.

## Classes

### FileManager<TKey>

The `FileManager` class manages file paths and provides methods for creating, adding, removing, and checking the existence of files.

#### Methods

- **FileManager()**: Initializes a new instance of the `FileManager` class with an empty file dictionary.
- **FileManager(Dictionary<TKey, string> initialFiles)**: Initializes a new instance of the `FileManager` class with the specified initial files.
- **CreatePaths()**: Creates the file paths specified in the dictionary if they do not already exist.
- **AddPath(TKey key, string path)**: Adds a new file path to the dictionary.
- **AddPath(Dictionary<TKey, string> paths)**: Adds multiple file paths to the dictionary.
- **RemovePath(TKey key)**: Removes a file path from the dictionary.
- **RemovePath(IEnumerable<TKey> keys)**: Removes multiple file paths from the dictionary.
- **ClearPaths()**: Clears all file paths from the dictionary.
- **GetPath(TKey key)**: Gets the file path associated with the specified key.
- **Exists(TKey file, bool isKey = true)**: Checks if a file exists at the path associated with the specified key or directly at the specified path.
- **Delete(TKey file, bool isKey = true)**: Deletes the file at the path associated with the specified key or directly at the specified path.
- **Read<T>(TKey file, bool isKey = true)**: Reads the content of the file at the path associated with the specified key or directly at the specified path.
- **Write<T>(TKey file, T data, bool isKey = true)**: Writes the specified data to the file at the path associated with the specified key or directly at the specified path.

### DirectoryManager<TKey>

The `DirectoryManager` class manages directory paths and provides methods for creating, adding, removing, and checking the existence of directories.

#### Methods

- **DirectoryManager()**: Initializes a new instance of the `DirectoryManager` class with an empty directory dictionary.
- **DirectoryManager(Dictionary<TKey, string> initialDirectories)**: Initializes a new instance of the `DirectoryManager` class with the specified initial directories.
- **CreatePaths()**: Creates the directory paths specified in the dictionary if they do not already exist.
- **AddPath(TKey key, string path)**: Adds a new directory path to the dictionary.
- **AddPath(Dictionary<TKey, string> paths)**: Adds multiple directory paths to the dictionary.
- **RemovePath(TKey key)**: Removes a directory path from the dictionary.
- **RemovePath(IEnumerable<TKey> keys)**: Removes multiple directory paths from the dictionary.
- **ClearPaths()**: Clears all directory paths from the dictionary.
- **GetPath(TKey directory)**: Gets the directory path associated with the specified key.
- **Exists(TKey directory, bool isKey = true)**: Checks if a directory exists at the path associated with the specified key or directly at the specified path.
- **Delete(TKey directory, bool recursive = false, bool isKey = true)**: Deletes the directory at the path associated with the specified key or directly at the specified path.
- **Create(TKey directory, string path, bool isKey = true)**: Creates a directory at the specified path and adds it to the dictionary.

## Usage Example

```csharp
using EasyPathManager;
using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // Create a file manager
        var fileManager = new FileManager<string>();
        fileManager.AddPath("exampleFile", "example.txt");

        // Create the file if it doesn't exist
        fileManager.CreatePaths();

        // Write to the file
        fileManager.Write("exampleFile", "Hello, World!");

        // Read from the file
        string content = fileManager.Read<string>("exampleFile");
        Console.WriteLine(content);

        // Create a directory manager
        var directoryManager = new DirectoryManager<string>();
        directoryManager.AddPath("exampleDir", "exampleDirectory");

        // Create the directory if it doesn't exist
        directoryManager.CreatePaths();

        Console.WriteLine("File and directory operations completed successfully!");
    }
}
```

## License

This project is licensed under the terms of the MIT license. See the LICENSE file for more details.
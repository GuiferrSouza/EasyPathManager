using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace EasyPathManager
{
    public class FileManager<TKey>
    {
        private readonly Dictionary<TKey, string> files;

        /// <summary>
        /// Initializes a new instance of the FileManager class with an empty file dictionary.
        /// </summary>
        public FileManager()
        {
            files = new Dictionary<TKey, string>();
        }

        /// <summary>
        /// Initializes a new instance of the FileManager class with the specified initial files.
        /// </summary>
        /// <param name="initialFiles">A dictionary of initial files.</param>
        public FileManager(Dictionary<TKey, string> initialFiles)
        {
            files = initialFiles;
        }

        /// <summary>
        /// Creates the file paths specified in the dictionary if they do not already exist.
        /// </summary>
        public void CreatePaths()
        {
            foreach (KeyValuePair<TKey, string> pair in files)
            {
                if (!File.Exists(pair.Value))
                {
                    File.Create(pair.Value).Dispose();
                }
            }
        }

        /// <summary>
        /// Adds a new file path to the dictionary.
        /// </summary>
        /// <param name="key">The key associated with the file path.</param>
        /// <param name="path">The file path to add.</param>
        public void AddPath(TKey key, string path)
        {
            if (!files.ContainsKey(key))
            {
                files[key] = path;
            }
        }

        /// <summary>
        /// Adds multiple file paths to the dictionary.
        /// </summary>
        /// <param name="paths">A dictionary of file paths to add.</param>
        public void AddPath(Dictionary<TKey, string> paths)
        {
            foreach (var pair in paths)
            {
                if (!files.ContainsKey(pair.Key))
                {
                    files[pair.Key] = pair.Value;
                }
            }
        }

        /// <summary>
        /// Removes a file path from the dictionary.
        /// </summary>
        /// <param name="key">The key associated with the file path to remove.</param>
        public void RemovePath(TKey key)
        {
            if (files.ContainsKey(key))
            {
                files.Remove(key);
            }
        }

        /// <summary>
        /// Removes multiple file paths from the dictionary.
        /// </summary>
        /// <param name="keys">An enumerable of keys associated with the file paths to remove.</param>
        public void RemovePath(IEnumerable<TKey> keys)
        {
            foreach (var key in keys)
            {
                if (files.ContainsKey(key))
                {
                    files.Remove(key);
                }
            }
        }

        /// <summary>
        /// Clears all file paths from the dictionary.
        /// </summary>
        public void ClearPaths()
        {
            files.Clear();
        }

        /// <summary>
        /// Gets the file path associated with the specified key.
        /// </summary>
        /// <param name="key">The key associated with the file path.</param>
        /// <returns>The file path associated with the specified key.</returns>
        public string GetPath(TKey key) => files[key];

        /// <summary>
        /// Checks if a file exists at the path associated with the specified key or directly at the specified path.
        /// </summary>
        /// <param name="file">The key or path to check.</param>
        /// <param name="isKey">Indicates whether the file parameter is a key or a direct path.</param>
        /// <returns>True if the file exists; otherwise, false.</returns>
        public bool Exists(TKey file, bool isKey = true)
        {
            if (isKey) return File.Exists(files[file]);
            else return File.Exists(file.ToString());
        }

        /// <summary>
        /// Deletes the file at the path associated with the specified key or directly at the specified path.
        /// </summary>
        /// <param name="file">The key or path of the file to delete.</param>
        /// <param name="isKey">Indicates whether the file parameter is a key or a direct path.</param>
        public void Delete(TKey file, bool isKey = true)
        {
            if (isKey)
            {
                if (File.Exists(files[file]))
                {
                    File.Delete(files[file]);
                }
            }
            else
            {
                if (File.Exists(file.ToString()))
                {
                    File.Delete(file.ToString());
                }
            }
        }

        /// <summary>
        /// Reads the content of the file at the path associated with the specified key or directly at the specified path.
        /// </summary>
        /// <typeparam name="T">The type of the content to read.</typeparam>
        /// <param name="file">The key or path of the file to read.</param>
        /// <param name="isKey">Indicates whether the file parameter is a key or a direct path.</param>
        /// <returns>The content of the file.</returns>
        public T Read<T>(TKey file, bool isKey = true)
        {
            string _file = isKey ? GetPath(file) : file.ToString();
            object result;

            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.String:
                    result = File.ReadAllText(_file);
                    break;
                case TypeCode.Object when typeof(T) == typeof(byte[]):
                    result = File.ReadAllBytes(_file);
                    break;
                case TypeCode.Object when typeof(T) == typeof(string[]):
                    result = File.ReadAllLines(_file);
                    break;
                case TypeCode.Object when typeof(T) == typeof(IEnumerable<string>):
                    result = File.ReadLines(_file);
                    break;
                default:
                    result = ReadCustomClass<T>(_file);
                    break;
            }

            return (T)result;
        }

        /// <summary>
        /// Reads a custom class from the specified file.
        /// </summary>
        /// <typeparam name="T">The type of the custom class to read.</typeparam>
        /// <param name="file">The path of the file to read.</param>
        /// <returns>The custom class read from the file.</returns>
        private T ReadCustomClass<T>(string file)
        {
            using (FileStream fs = File.OpenRead(file))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    using (JsonDocument doc = JsonDocument.Parse(sr.ReadToEnd()))
                    {
                        return JsonSerializer.Deserialize<T>(doc.RootElement.GetRawText());
                    }
                }
            }
        }

        /// <summary>
        /// Writes the specified data to the file at the path associated with the specified key or directly at the specified path.
        /// </summary>
        /// <typeparam name="T">The type of the data to write.</typeparam>
        /// <param name="file">The key or path of the file to write to.</param>
        /// <param name="data">The data to write.</param>
        /// <param name="isKey">Indicates whether the file parameter is a key or a direct path.</param>
        public void Write<T>(TKey file, T data, bool isKey = true)
        {
            string _file = isKey ? GetPath(file) : file.ToString();

            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.String:
                    File.WriteAllText(_file, data as string);
                    break;
                case TypeCode.Object when typeof(T) == typeof(byte[]):
                    File.WriteAllBytes(_file, data as byte[]);
                    break;
                case TypeCode.Object when typeof(T) == typeof(string[]):
                    File.WriteAllLines(_file, data as string[]);
                    break;
                case TypeCode.Object when typeof(T) == typeof(IEnumerable<string>):
                    File.WriteAllLines(_file, (data as IEnumerable<string>).ToArray());
                    break;
                default:
                    WriteCustomClass(_file, data);
                    break;
            }
        }

        /// <summary>
        /// Writes a custom class to the specified file.
        /// </summary>
        /// <typeparam name="T">The type of the custom class to write.</typeparam>
        /// <param name="file">The path of the file to write to.</param>
        private void WriteCustomClass<T>(string file, T data)
        {
            using (FileStream fs = File.Create(file))
            {
                using (Utf8JsonWriter writer = new Utf8JsonWriter(fs))
                {
                    JsonSerializer.Serialize(writer, data);
                }
            }
        }
    }
}

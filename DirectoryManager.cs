using System.Collections.Generic;
using System.IO;

namespace EasyPathManager
{
    public class DirectoryManager<TKey>
    {
        private readonly Dictionary<TKey, string> directories;

        /// <summary>
        /// Initializes a new instance of the DirectoryManager class with an empty directory dictionary.
        /// </summary>
        public DirectoryManager()
        {
            directories = new Dictionary<TKey, string>();
        }

        /// <summary>
        /// Initializes a new instance of the DirectoryManager class with the specified initial directories.
        /// </summary>
        /// <param name="initialDirectories">A dictionary of initial directories.</param>
        public DirectoryManager(Dictionary<TKey, string> initialDirectories)
        {
            directories = initialDirectories;
        }

        /// <summary>
        /// Creates the directory paths specified in the dictionary if they do not already exist.
        /// </summary>
        public void CreatePaths()
        {
            foreach (KeyValuePair<TKey, string> pair in directories)
            {
                if (!Directory.Exists(pair.Value))
                {
                    Directory.CreateDirectory(pair.Value);
                }
            }
        }

        /// <summary>
        /// Adds a new directory path to the dictionary.
        /// </summary>
        /// <param name="key">The key associated with the directory path.</param>
        /// <param name="path">The directory path to add.</param>
        public void AddPath(TKey key, string path)
        {
            if (!directories.ContainsKey(key))
            {
                directories[key] = path;
            }
        }

        /// <summary>
        /// Adds multiple directory paths to the dictionary.
        /// </summary>
        /// <param name="paths">A dictionary of directory paths to add.</param>
        public void AddPath(Dictionary<TKey, string> paths)
        {
            foreach (var pair in paths)
            {
                if (!directories.ContainsKey(pair.Key))
                {
                    directories[pair.Key] = pair.Value;
                }
            }
        }

        /// <summary>
        /// Removes a directory path from the dictionary.
        /// </summary>
        /// <param name="key">The key associated with the directory path to remove.</param>
        public void RemovePath(TKey key)
        {
            if (directories.ContainsKey(key))
            {
                directories.Remove(key);
            }
        }

        /// <summary>
        /// Removes multiple directory paths from the dictionary.
        /// </summary>
        /// <param name="keys">An enumerable of keys associated with the directory paths to remove.</param>
        public void RemovePath(IEnumerable<TKey> keys)
        {
            foreach (var key in keys)
            {
                if (directories.ContainsKey(key))
                {
                    directories.Remove(key);
                }
            }
        }

        /// <summary>
        /// Clears all directory paths from the dictionary.
        /// </summary>
        public void ClearPaths()
        {
            directories.Clear();
        }

        /// <summary>
        /// Gets the directory path associated with the specified key.
        /// </summary>
        /// <param name="directory">The key associated with the directory path.</param>
        /// <returns>The directory path associated with the specified key.</returns>
        public string GetPath(TKey directory) => directories[directory];

        /// <summary>
        /// Checks if a directory exists at the path associated with the specified key or directly at the specified path.
        /// </summary>
        /// <param name="directory">The key or path to check.</param>
        /// <param name="isKey">Indicates whether the directory parameter is a key or a direct path.</param>
        /// <returns>True if the directory exists; otherwise, false.</returns>
        public bool Exists(TKey directory, bool isKey = true)
        {
            if (isKey)
            {
                return Directory.Exists(directories[directory]);
            }
            else
            {
                return Directory.Exists(directory.ToString());
            }
        }

        /// <summary>
        /// Deletes the directory at the path associated with the specified key or directly at the specified path.
        /// </summary>
        /// <param name="directory">The key or path of the directory to delete.</param>
        /// <param name="recursive">Indicates whether to delete directories, subdirectories, and files in the directory.</param>
        /// <param name="isKey">Indicates whether the directory parameter is a key or a direct path.</param>
        public void Delete(TKey directory, bool recursive = false, bool isKey = true)
        {
            if (isKey)
            {
                if (Directory.Exists(directories[directory]))
                {
                    Directory.Delete(directories[directory], recursive);
                }
            }
            else
            {
                if (Directory.Exists(directory.ToString()))
                {
                    Directory.Delete(directory.ToString(), recursive);
                }
            }
        }

        /// <summary>
        /// Creates a directory at the specified path and adds it to the dictionary.
        /// </summary>
        /// <param name="directory">The key or path of the directory to create.</param>
        /// <param name="path">The path where the directory should be created.</param>
        /// <param name="isKey">Indicates whether the directory parameter is a key or a direct path.</param>
        public void Create(TKey directory, string path, bool isKey = true)
        {
            if (isKey)
            {
                if (!directories.ContainsKey(directory))
                {
                    directories[directory] = path;
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                }
            }
            else
            {
                if (!directories.ContainsKey((TKey)(object)path))
                {
                    directories[(TKey)(object)path] = directory.ToString();
                    if (!Directory.Exists(directory.ToString()))
                    {
                        Directory.CreateDirectory(directory.ToString());
                    }
                }
            }
        }
    }
}

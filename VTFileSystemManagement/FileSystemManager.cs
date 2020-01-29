using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;

namespace VTFileSystemManagement
{
    /// <summary>
    /// Basic library setup to read and write JSON documents from within a Data Directory in the base location of a program. 
    /// </summary>
    public class FileSystemManager
    {
        private string _DirectoryPath = Path.Combine(Path.GetFullPath(Directory.GetCurrentDirectory()), "Data");

        /// <summary>
        /// Constructor makes sure a "Data" Directory as JSON documents are saved here by default in this library. 
        /// </summary>
        public FileSystemManager()
        {
            if (!Directory.Exists(_DirectoryPath))
            {
                Directory.CreateDirectory(_DirectoryPath);
            }
        }

        /// <summary>
        /// Constructor overload that allows a user to create their own directory within the root location of the program using this library. 
        /// </summary>
        /// <param name="createDirectoryName"></param>
        public FileSystemManager(string createDirectoryName)
        {
            _DirectoryPath = Path.Combine(Path.GetFullPath(Directory.GetCurrentDirectory()), createDirectoryName);
            if (!Directory.Exists(createDirectoryName))
            {
                Directory.CreateDirectory(createDirectoryName);                
            }
        }

        /// <summary>
        /// Reads a JSON file by file name and returns it in a string format. 
        /// This defaults it's location inside directory <see cref="_DataDirectory"/>
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string ReadJsonFile(string fileName)
        {
            string file = BuildFilePath(fileName);

            if (File.Exists(file))
            {
                return File.ReadAllText(file);
            }
            return null;            
        }

        public string ReadJsonFileFromSpecificLocation(string fileName, string fileLocation)
        {
            string file = Path.Combine(fileLocation, fileName);

            if (File.Exists(file))
            {
                return File.ReadAllText(file);
            }
            return null;
        }

        private protected bool IsFileClosed(string filePath)
        {
            try
            {
                using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                return false;
            }
            return true;
        }

        public bool WaitUntilFileClosed(Func<object> func, string filePath, int waitInMilliseconds = 5000)
        {
            var stopWatch = new Stopwatch();
            var isFileClosed = IsFileClosed(filePath);
            stopWatch.Start();
            while (!isFileClosed && stopWatch.ElapsedMilliseconds < waitInMilliseconds)
            {
                isFileClosed = IsFileClosed(filePath);
                func.Invoke();                
            }
            stopWatch.Stop();
            if (stopWatch.ElapsedMilliseconds > waitInMilliseconds)
            {
                return false;
            }
            stopWatch.Stop();
            return true;          
        }

        /// <summary>
        /// Saves an object to a JSON file by serializing using NewtonSoft.
        /// This defaults it's location to <see cref="_DataDirectory"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonData"></param>
        /// <param name="fileName"></param>
        public void SaveJsonFile<T>(T jsonData, string fileName)
        {
            using (StreamWriter file = File.CreateText(BuildFilePath(fileName)))
            {
                var serializer = new JsonSerializer
                {
                    Formatting = Formatting.Indented

                };
                serializer.Serialize(file, jsonData);
            }
        }

        /// <summary>
        /// Let's you save file to a specific file path. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonData"></param>
        /// <param name="fileLocation"></param>
        /// <param name="fileName"></param>
        public void SaveJsonFileToSpecificLocation<T>(T jsonData, string fileLocation, string fileName)
        {
            using (StreamWriter file = File.CreateText(Path.Combine(fileLocation, fileName)))
            {
                var serializer = new JsonSerializer
                {
                    Formatting = Formatting.Indented

                };
                serializer.Serialize(file, jsonData);
            }
        }

        public DateTime GetFilesLastModifiedTime(string fileLocation, string fileName)
        {
            return File.GetLastWriteTime(Path.Combine(fileLocation, fileName));
        }

        public void LogException(Exception ex, string fileName = null)
        {
            var filePath = BuildFilePath(fileName ?? $"{Environment.MachineName}_{DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss")}.txt");
            using (StreamWriter writer = File.CreateText(filePath))
            {
                writer.WriteLine(ex.GetType().FullName);
                writer.WriteLine("Message : " + ex.Message);
                writer.WriteLine("StackTrace : " + ex.StackTrace);
                writer.WriteLine("Inner Exception : " + ex.InnerException.Message);                
            }
        }

        /// <summary>
        /// Checks if a file exists in the root directory of a program inside "Data" Directory.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool IsFileExists(string fileName)
        {
            return File.Exists(BuildFilePath(fileName));
        }

        /// <summary>
        /// Override that allows the user to check if a file exists in a specific location. 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileLocation"></param>
        /// <returns></returns>
        public bool IsFileExists(string fileName, string fileLocation)
        {
            return File.Exists(Path.Combine(fileLocation, fileName));
        }

        /// <summary>
        /// Conbines <see cref="file"/> and <see cref="_DataDirectory"/> to create the full file path of a text file by name. 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private string BuildFilePath(string file)
        {
            return Path.Combine(_DirectoryPath, file);
        }
    }
}

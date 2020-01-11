using Newtonsoft.Json;
using System.IO;

namespace VTFileSystemManagement
{
    /// <summary>
    /// Basic library setup to read and write JSON documents from within a Data Directory in the base location of a program. 
    /// </summary>
    public class FileSystemManager
    {
        private string _DataDirectory = Path.Combine(Path.GetFullPath(Directory.GetCurrentDirectory()), "Data");

        /// <summary>
        /// Constructor makes sure a "Data" Directory as JSON documents are saved here by default in this library. 
        /// </summary>
        public FileSystemManager()
        {
            if (!Directory.Exists(_DataDirectory))
            {
                Directory.CreateDirectory(_DataDirectory);
            }
        }

        /// <summary>
        /// Constructor overload that allows a user to create their own directory within the root location of the program using this library. 
        /// </summary>
        /// <param name="createDirectoryName"></param>
        public FileSystemManager(string createDirectoryName)
        {
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

        public bool IsFileExists(string fileName)
        {
            return File.Exists(BuildFilePath(fileName));
        }

        /// <summary>
        /// Conbines <see cref="file"/> and <see cref="_DataDirectory"/> to create the full file path of a text file by name. 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private string BuildFilePath(string file)
        {
            return Path.Combine(_DataDirectory, file);
        }
    }
}

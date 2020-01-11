using Newtonsoft.Json;
using System.IO;

namespace VTFileSystemManagement
{
    public class FileSystemManager
    {
        private static string _DataDirectory = Path.Combine(Path.GetFullPath(Directory.GetCurrentDirectory()), "Data");
        public FileSystemManager()
        {
            if (!Directory.Exists(_DataDirectory))
            {
                Directory.CreateDirectory(_DataDirectory);
            }
        }
        public string ReadJsonFile(string fileName)
        {
            string file = BuildFilePath(fileName);

            if (File.Exists(file))
            {
                return File.ReadAllText(file);
            }
            return null;            
        }

        public string GetFilePath(string file)
        {
            return BuildFilePath(file);
        }

        public void SaveJsonFile<T>(T jsonData, string fileName)
        {
            if (File.Exists(BuildFilePath(fileName)))
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
        }

        private string BuildFilePath(string file)
        {
            return Path.Combine(_DataDirectory, file);
        }
    }
}

using System;
using System.IO;

namespace CsvZipExecutor.Services
{
    internal static class DirectoriesService
    {
        private static readonly string RandomFolderName;
        static DirectoriesService()
        {
            RandomFolderName = Guid.NewGuid().ToString();
        }

        public static DirectoryInfo GetTemporaryDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                throw new DirectoryNotFoundException();
            }

            return Directory.CreateDirectory($"{directoryPath}/{RandomFolderName}");
        }

        public static void RemoveDirectoryWithFiles(DirectoryInfo directoryInfo)
        {
            directoryInfo.Delete(true);
        }
    }
}

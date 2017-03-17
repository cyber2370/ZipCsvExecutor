using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace CsvZipExecutor.Services
{
    internal static class ZipArchivator
    {
        public static void CheckZipArchive(string zipArchivePath)
        {
            if (!Path.GetExtension(zipArchivePath)?.Equals(".zip") ?? true)
            {
                throw new FileNotFoundException("Incorrect file extension.");
            }

            if (!File.Exists(zipArchivePath))
            {
                throw new FileNotFoundException("File not found.");
            }
        }

        public static IEnumerable<ZipArchiveEntry> GetFilesFromZipArchive(string zipArchivePath,
            string fileExtension = null)
        {
            IEnumerable<ZipArchiveEntry> files;
            using (ZipArchive zipArchive = ZipFile.OpenRead(zipArchivePath))
            {
                files = zipArchive.Entries;
            }

            if (fileExtension != null)
            {
                files = files.Where(x => Path.GetExtension(x.FullName)?.Equals(fileExtension) ?? false);
            }

            return files.ToList();
        }

        public static void UnpackZipArchive(string zipArchivePath, DirectoryInfo destinationDir,
            string fileExtension = null)
        {
            using (ZipArchive archive = ZipFile.OpenRead(zipArchivePath))
            {
                IEnumerable<ZipArchiveEntry>  csvFilesEnumerable = archive.Entries;

                if (fileExtension != null)
                {
                    csvFilesEnumerable =
                        csvFilesEnumerable.Where(x => Path.GetExtension(x.FullName)?.Equals(fileExtension) ?? false);
                }

                foreach (var zipArchiveEntry in csvFilesEnumerable)
                {
                    zipArchiveEntry.ExtractToFile($"{destinationDir.FullName}/{zipArchiveEntry.Name}");
                }
            }
        }
    }
}
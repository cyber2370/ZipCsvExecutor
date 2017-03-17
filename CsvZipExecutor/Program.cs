using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using CsvZipExecutor.Services;

namespace CsvZipExecutor
{
    class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("You have to call this program with path to zip file!");
            }
            else
            {
                Execute(args[0]);
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }

        private static void Execute(string zipFullPath)
        {
            try
            {
                ZipArchivator.CheckZipArchive(zipFullPath);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            DirectoryInfo tmpDir = null;
            try
            {
                tmpDir = DirectoriesService.GetTemporaryDirectory(Path.GetDirectoryName(zipFullPath));

                Console.WriteLine("Temporary directory " + tmpDir.FullName + " has been successfully created!\n");
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine("Incorrect path to file: " + zipFullPath + ".\n");

                HandleException(ex);
                return;
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("Run program with administrator permissions.\n");

                HandleException(ex);
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unknown exception.\n");

                HandleException(ex);
                return;
            }

            try
            {
                ZipArchivator.UnpackZipArchive(zipFullPath, tmpDir, ".csv");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("Run program with administrator permissions.\n");
                A(tmpDir);

                HandleException(ex);
                return;
            }
            catch (InvalidDataException ex)
            {
                Console.WriteLine("This zip archive is invalid: " + zipFullPath + ". \n");
                A(tmpDir);

                HandleException(ex);
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unknown exception.\n");
                A(tmpDir);

                HandleException(ex);
                return;
            }

            foreach (var unpackedFile in tmpDir.EnumerateFiles())
            {
                B(unpackedFile);
            }
            Console.WriteLine("Files have been proccessed successfully!\n");

            A(tmpDir);
        }

        private static void A(DirectoryInfo dir)
        {
            try
            {
                DirectoriesService.RemoveDirectoryWithFiles(dir);
            }
            catch (IOException ex)
            {
                Console.WriteLine("Error while removing temporary directory.");

                HandleException(ex);
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unknown exception.");

                HandleException(ex);
                return;
            }

            Console.WriteLine("Temporary directory " + dir.FullName + " has been successfully removed!\n");
        }

        private static void B(FileInfo file)
        {
            Console.WriteLine("Full file name: " + file.FullName);
        }

        private static void HandleException(Exception ex)
        {
#if DEBUG
            Console.WriteLine("Exception message: " + ex.Message);
            Console.WriteLine("Exception stack trace: " + ex.StackTrace + "\n");
#endif
        }
    }
}
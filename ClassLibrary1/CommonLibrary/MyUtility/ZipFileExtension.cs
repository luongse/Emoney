using System;
using System.IO;
using System.Text;
using System.IO.Compression;

namespace MyUtility
{
    public class ZipFileExtension
    {
        public static bool IsZipFile(string zipFilePath)
        {
            try
            {
                var zipArchive = ZipFile.OpenRead(zipFilePath);
                return zipArchive != null;
            }
            catch
            {
                return false;
            }
        }

        public static bool CreateFromFile(string sourceFileName, string sourceFilePath, string zipFilePath)
        {
            if (!File.Exists(zipFilePath)) File.Create(zipFilePath);
            using (var archive = ZipFile.Open(zipFilePath, ZipArchiveMode.Update))
            {
                archive.CreateEntryFromFile(sourceFilePath, sourceFileName, CompressionLevel.Fastest);
            }

            return true;
        }

        public static bool CreateFromDirectory(string directorySourcePath, string destinationArchivePath, bool includeBaseDirectory = true)
        {
            ZipFile.CreateFromDirectory(directorySourcePath, destinationArchivePath, CompressionLevel.Fastest, includeBaseDirectory);
            return true;
        }

        public static bool ExtractToDirectory(string directoryArchivePath, string destinationExtractPath, bool isOverwrite = false)
        {
            ZipFile.ExtractToDirectory(directoryArchivePath, destinationExtractPath, Encoding.UTF8);
            return true;
        }
    }
}
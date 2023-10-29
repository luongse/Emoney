using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace MyUtility
{
    public static class FileExtension
    {
        public static DataTable CsvCardToDataTable(Stream stream)
        {
            var dt = new DataTable();
            var csvreader = new StreamReader(stream);

            string[] headers = csvreader.ReadLine().Split(',');
            foreach (string header in headers)
            {
                dt.Columns.Add(string.IsNullOrEmpty(header) ? string.Empty : header.Trim());
            }
            while (!csvreader.EndOfStream)
            {
                var line = csvreader.ReadLine();
                var rows = (line ?? "").Split(',');
                DataRow dr = dt.NewRow();
                for (int i = 0; i < headers.Length; i++)
                {
                    dr[i] = string.IsNullOrEmpty(rows[i]) ? string.Empty : rows[i].Trim();
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// Kiểm tra xem file có phải là file ảnh không
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static bool IsValidImage(byte[] bytes)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    Image.FromStream(ms);
                    return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        /// <summary>
        /// Kiểm tra xem file có phải là file ảnh không
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="imageInfo"></param>
        /// <returns></returns>
        public static bool IsValidImage(byte[] bytes, out Image imageInfo)
        {
            try
            {
                using (var ms = new MemoryStream(bytes))
                {
                    imageInfo = Image.FromStream(ms);
                    return true;
                }
            }
            catch (Exception)
            {
                imageInfo = null;
            }
            return false;
        }

        /// <summary>
        /// Kiểm tra Extension ten file
        /// </summary>
        /// <returns></returns>
        public static bool IsValidExtension(string fileName, List<string> availableExentions = null)
        {
            if (string.IsNullOrEmpty(fileName))
                return false;

            var extension = GetExtension(fileName);

            if (availableExentions == null)
            {
                switch (extension)
                {
                    case "jpg":
                    case "jpeg":
                    case "gif":
                    case "png":
                        return true;
                }
            }
            else
            {
                return availableExentions.Any(x => extension.Equals(x, StringComparison.OrdinalIgnoreCase));
            }

            return false;
        }

        /// <summary>
        /// Kiểm tra Extension ten file
        /// </summary>
        /// <returns></returns>
        public static string GetExtension(string fileName)
        {
            var splitFileName = fileName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (splitFileName.Length > 0)
            {
                var length = splitFileName.Length;
                var extension = splitFileName[length - 1].ToLower().Trim();
                return extension;
            }

            return string.Empty;
        }

        /// <summary>
        /// Kiểm tra zip file
        /// </summary>
        /// <returns></returns>
        public static bool IsZipFile(byte[] bytes)
        {
            try
            {
                using (var ms = new MemoryStream(bytes))
                {
                    using (var archive = new ZipArchive(ms, ZipArchiveMode.Update))
                    {
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }
    }
}

using System;
using System.Data;
using System.Drawing;
using System.IO;

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
                dt.Columns.Add(header);
            }
            while (!csvreader.EndOfStream)
            {
                var line = csvreader.ReadLine();
                var rows = (line??"").Split(',');
                DataRow dr = dt.NewRow();
                for (int i = 0; i < headers.Length; i++)
                {
                    dr[i] = rows[i];
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
                var converter = new ImageConverter();
                var img = (Image)converter.ConvertFrom(bytes);
                //Image.FromStream(ms);
                return img != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Kiểm tra Extension ten file
        /// </summary>
        /// <returns></returns>
        public static bool IsValidExtension(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return false;

            var splitFileName = fileName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (splitFileName.Length > 0)
            {
                var length = splitFileName.Length;

                switch (splitFileName[length - 1].ToLower())
                {
                    case "jpg":
                    case "jpeg":
                    case "gif":
                    case "png":
                        return true;

                }
            }

            return false;
        }

        /// <summary>
        /// Kiểm tra Extension ten file
        /// </summary>
        /// <returns></returns>
        public static string GetExtension(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return string.Empty;

            var splitFileName = fileName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (splitFileName.Length > 0)
            {
                var length = splitFileName.Length;
                return splitFileName[length - 1].ToLower();
            }

            return string.Empty;
        }

        public static Image ByteArrayToImage(byte[] bytesArr)
        {
            MemoryStream memstr = new MemoryStream(bytesArr);
            Image img = Image.FromStream(memstr);
            return img;
        }
    }
}

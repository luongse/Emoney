using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace MyUtility
{
    public static class CsvUtility
    {
        public static MemoryStream GetCsv(DataTable data)
        {
            string[] fieldsToExpose = new string[data.Columns.Count];
            for (int i = 0; i < data.Columns.Count; i++)
            {
                fieldsToExpose[i] = data.Columns[i].ColumnName;
            }

            return GetCsv(fieldsToExpose, data);
        }

        public static MemoryStream GetCsv(string[] fieldsToExpose, DataTable data)
        {
            var stream = new MemoryStream();

            var sw = new StreamWriter(stream, Encoding.UTF8);
            //sw.Write("{");

            //foreach (var kvp in keysAndValues)
            //{
            //    sw.Write("'{0}':", kvp.Key);
            //    sw.Flush();
            //    ser.WriteObject(stream, kvp.Value);
            //}
            for (int i = 0; i < fieldsToExpose.Length; i++)
            {
                if (i != 0) { sw.Write(","); }
                sw.Write("\"");
                sw.Write(fieldsToExpose[i].Replace("\"", "\"\""));
                sw.Write("\"");
            }
            sw.Write("\n");

            foreach (DataRow row in data.Rows)
            {
                for (int i = 0; i < fieldsToExpose.Length; i++)
                {
                    if (i != 0) { sw.Write(","); }
                    sw.Write("\"");
                    sw.Write(row[fieldsToExpose[i]].ToString()
                        .Replace("\"", "\"\""));
                    sw.Write("\"");
                }

                sw.Write("\n");
            }
            //sw.Write("}");
            sw.Flush();
            stream.Position = 0;
            int c = Convert.ToInt32(stream.Length);
            //stream.Write(sw.GetBuffer(), 0, (int)ms2.Length);
            return stream;
            //return Encoding.UTF8.GetString(stream.GetBuffer(), 0, (int)stream.Length);
        }

        public static void SaveStreamToFile(string fileFullPath, MemoryStream stream)
        {
            if (File.Exists(fileFullPath)) return;

            File.Exists(fileFullPath);
            if (stream.Length == 0) return;
            // Create a FileStream object to write a stream to a file
            using (FileStream fileStream = File.Create(fileFullPath, (int)stream.Length))
            {
                // Fill the bytes[] array with the stream data
                byte[] bytesInStream = new byte[stream.Length];
                stream.Read(bytesInStream, 0, bytesInStream.Length);

                // Use FileStream object to write to the specified file
                fileStream.Write(bytesInStream, 0, bytesInStream.Length);
            }
        }
    }

    public class CsvRow : List<string>
    {
        public string LineText { get; set; }
    }

    public class CsvFileReader : StreamReader
    {
        public CsvFileReader(Stream stream)
            : base(stream)
        {
        }

        public CsvFileReader(string filename)
            : base(filename)
        {
        }

        /// <summary>
        /// Reads a row of data from a CSV file
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public bool ReadRow(CsvRow row)
        {
            row.LineText = ReadLine();
            if (String.IsNullOrEmpty(row.LineText))
                return false;

            int pos = 0;
            int rows = 0;

            while (pos < row.LineText.Length)
            {
                string value;

                // Special handling for quoted field
                if (row.LineText[pos] == '"')
                {
                    // Skip initial quote
                    pos++;

                    // Parse quoted value
                    int start = pos;
                    while (pos < row.LineText.Length)
                    {
                        // Test for quote character
                        if (row.LineText[pos] == '"')
                        {
                            // Found one
                            pos++;

                            // If two quotes together, keep one
                            // Otherwise, indicates end of value
                            if (pos >= row.LineText.Length || row.LineText[pos] != '"')
                            {
                                pos--;
                                break;
                            }
                        }
                        pos++;
                    }
                    value = row.LineText.Substring(start, pos - start);
                    value = value.Replace("\"\"", "\"");
                }
                else
                {
                    // Parse unquoted value
                    int start = pos;
                    while (pos < row.LineText.Length && row.LineText[pos] != ',')
                        pos++;
                    value = row.LineText.Substring(start, pos - start);
                }

                // Add field to list
                if (rows < row.Count)
                    row[rows] = value;
                else
                    row.Add(value);
                rows++;

                // Eat up to and including next comma
                while (pos < row.LineText.Length && row.LineText[pos] != ',')
                    pos++;
                if (pos < row.LineText.Length)
                    pos++;
            }
            // Delete any unused items
            while (row.Count > rows)
                row.RemoveAt(rows);

            // Return true if any columns read
            return (row.Count > 0);
        }
    }
}

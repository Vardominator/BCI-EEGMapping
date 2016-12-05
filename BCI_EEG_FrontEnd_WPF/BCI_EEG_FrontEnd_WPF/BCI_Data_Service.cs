using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI_EEG_FrontEnd_WPF
{
    public class BCI_Data_Service
    {
        public static DataTable ReadFile(string filePath)
        {
            //var lines = File.ReadAllLines(filePath);

            //var data = from line in lines.Skip(1)
            //           let split = line.Split(',').Select(float.Parse).ToArray()
            //           select new BCI_DataPoint
            //           {
            //               features = split.Take(split.Length - 1).ToArray(),
            //               classification = (int)split[split.Length - 1]
            //           };

            //return data.ToList();

            //List<string[]> rows = File.ReadAllLines(filePath).Select(x => x.Split(',')).ToList();

            //DataTable dt = new DataTable();

            //string[] columns = rows[0];

            //int featureNumber = 0;

            //foreach (string column in columns)
            //{
            //    dt.Columns.Add(, typeof());
            //}


            //for (int i = 1; i < rows.Count; i++)
            //{
            //    DataRow newRow = dt.NewRow();
            //    string[] currentRow = rows[i];

            //    for (int j = 0; j < currentRow.Length; j++)
            //    {
            //        newRow[j] = currentRow[j];
            //    }

            //    dt.Rows.Add(newRow);
            //}

            //return dt;

            var dt = new DataTable();

            File.ReadLines(filePath).Take(1)
                .SelectMany(x => x.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                .ToList()
                .ForEach(x => dt.Columns.Add(x.Trim()));

            File.ReadLines(filePath).Skip(1)
                .Select(x => x.Split(','))
                .ToList()
                .ForEach(line => dt.Rows.Add(line));

            return dt;
        }
    }
}

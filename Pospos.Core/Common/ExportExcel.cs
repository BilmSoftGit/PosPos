using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;

namespace Pospos.Core.Common
{
    public class ExportExcel
    {
        public static void ExportExcelSave(Dictionary<List<string[]>, List<string[]>> list, string ExcelExportAddress)
        {
            ExcelPackage excel = new ExcelPackage();
            try
            {
                int i = 1;
                foreach (var _worksheet in list)
                {
                    excel.Workbook.Worksheets.Add("Sayfa" + i.ToString());

                    var headerRow = _worksheet.Key;
                    var rows = _worksheet.Value;

                    string headerRange = "A1:" /*+ Char.ConvertFromUtf32(headerRow[0].Length + 64)*/ + "1";

                    var worksheet = excel.Workbook.Worksheets["Sayfa" + i.ToString()];

                    worksheet.Cells[headerRange].LoadFromArrays(headerRow);

                    worksheet.Cells[headerRange].Style.Font.Bold = true;

                    worksheet.Cells[headerRange.Replace("1", "2")].LoadFromArrays(rows);

                    i++;
                }

                FileInfo excelFile = new FileInfo(ExcelExportAddress);
                excel.SaveAs(excelFile);
            }
            catch (System.Exception ex)
            { }
            finally { excel.Dispose(); }

            //using (ExcelPackage excel = new ExcelPackage())
            //{
            //    int i = 1;
            //    foreach (var _worksheet in list)
            //    {
            //        excel.Workbook.Worksheets.Add("Sayfa" + i.ToString());

            //        var headerRow = _worksheet.Key;
            //        var rows = _worksheet.Value;

            //        string headerRange = "A1:" /*+ Char.ConvertFromUtf32(headerRow[0].Length + 64)*/ + "1";

            //        var worksheet = excel.Workbook.Worksheets["Sayfa" + i.ToString()];

            //        worksheet.Cells[headerRange].LoadFromArrays(headerRow);

            //        worksheet.Cells[headerRange].Style.Font.Bold = true;

            //        worksheet.Cells[headerRange.Replace("1", "2")].LoadFromArrays(rows);

            //        i++;
            //    }

            //    FileInfo excelFile = new FileInfo(ExcelExportAddress);
            //    excel.SaveAs(excelFile);
            //}
        }
    }
}

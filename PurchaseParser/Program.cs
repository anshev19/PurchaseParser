using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseParser
{
    class Program
    {
        private static string _filePath;
        static void Main(string[] args)
        {
            
            if (args.Length == 0)
            {
                Console.WriteLine("Please specify path of output file");
                _filePath = Console.ReadLine();                
            }
            else _filePath = args[0];
            while (!Directory.Exists(_filePath))
            {
                Console.WriteLine($"Path '{_filePath}' isn't exist. Please enter existing path");
                _filePath = Console.ReadLine();
            }

            var purchaseDataList = new List<PurchaseData>();
            for (uint i = 1; i <= 10; i++)
            {
                purchaseDataList.AddRange(PageParser.GetPurchaseDataObjects(10, i));
            }

            UploadDataToExcell(purchaseDataList);            
        }

        
        public static void UploadDataToExcell(IList<PurchaseData> purchaseData)
        {
            var excelApp = new Excel.Application();
            excelApp.Visible = true;
            excelApp.Workbooks.Add();
            var workSheet = excelApp.ActiveSheet;//new Excel.Worksheet();
            workSheet.Cells[1, "A"] = "Имя закупки";
            workSheet.Cells[1, "B"] = "Начальная цена";
            workSheet.Cells[1, "C"] = "Имя заказчика";
            workSheet.Cells[1, "D"] = "Дата размещения";
            workSheet.Cells[1, "E"] = "Дата обновления";
            workSheet.Cells[1, "F"] = "Номер закупки";
            workSheet.Cells[1, "G"] = "Раздел";
            workSheet.Cells[1, "H"] = "Тип закупки";
            workSheet.Cells[1, "I"] = "Статус";

            for (var i=0; i<purchaseData.Count; i++)
            {
                workSheet.Cells[i + 2, "A"] = purchaseData[i].Title;
                workSheet.Cells[i + 2, "B"] = purchaseData[i].Price;
                workSheet.Cells[i + 2, "C"] = purchaseData[i].Customer;
                workSheet.Cells[i + 2, "D"] = purchaseData[i].AllocationDate;
                workSheet.Cells[i + 2, "E"] = purchaseData[i].UpdatedDate;
                workSheet.Cells[i + 2, "F"] = purchaseData[i].PurchaseNumber;
                workSheet.Cells[i + 2, "G"] = purchaseData[i].PartitionFz;
                workSheet.Cells[i + 2, "H"] = purchaseData[i].PurchaseType;
                workSheet.Cells[i + 2, "I"] = purchaseData[i].PurchaseStatus;
            }

            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;
            var day = DateTime.Now.Day;
            var fileName = $"{_filePath}\\Данные по закупкам {day}-{month}-{year}.xlsx";

            while (File.Exists(fileName))
            {
                fileName = $"{fileName.Split('.')[0]}_{new Random().Next()}.xlsx";                
            }
            
            workSheet.SaveAs(fileName);
            excelApp.Quit();

            Console.WriteLine($"File '{fileName}' is successfully uploaded");
        }
    }
}

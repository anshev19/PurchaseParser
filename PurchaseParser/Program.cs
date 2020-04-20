using System;
using System.Collections.Generic;
using System.IO;

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
            Console.WriteLine("Collecting data from web...");
            var purchaseDataList = new List<PurchaseData>();
            try
            {                
                for (uint i = 1; i <= 10; i++)
                {
                    purchaseDataList.AddRange(PageParser.GetPurchaseDataObjects(10, i));
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return;
            }
            Console.WriteLine("Collecting data is finished");

            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;
            var day = DateTime.Now.Day;
            var fileName = $"{_filePath}\\Данные по закупкам {day}-{month}-{year}.xlsx";

            while (File.Exists(fileName))
            {
                fileName = $"{fileName.Split('.')[0]}_{new Random().Next()}.xlsx";
            }

            Console.WriteLine("Uploading data to excel file...");
            try
            {
                DataUploader.UploadDataToExcell(purchaseDataList, fileName);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return;
            }
            Console.WriteLine($"File '{fileName}' is successfully uploaded");
        }       
    }
}

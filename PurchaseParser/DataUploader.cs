using System.Collections.Generic;

namespace PurchaseParser
{
    public static class DataUploader
    {
        public static void UploadDataToExcell(IList<PurchaseData> purchaseData, string fileName)
        {
            var excelApp = new Microsoft.Office.Interop.Excel.Application();
            excelApp.Visible = true;
            excelApp.Workbooks.Add();
            var workSheet = excelApp.ActiveSheet;
            workSheet.Cells[1, "A"] = "Имя закупки";
            workSheet.Cells[1, "B"] = "Начальная цена";
            workSheet.Cells[1, "C"] = "Имя заказчика";
            workSheet.Cells[1, "D"] = "Дата размещения";
            workSheet.Cells[1, "E"] = "Дата обновления";
            workSheet.Cells[1, "F"] = "Номер закупки";
            workSheet.Cells[1, "G"] = "Раздел";
            workSheet.Cells[1, "H"] = "Тип закупки";
            workSheet.Cells[1, "I"] = "Статус";

            for (var i = 0; i < purchaseData.Count; i++)
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

            workSheet.SaveAs(fileName);
            excelApp.Quit();
        }
    }
}

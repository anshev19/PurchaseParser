using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace PurchaseParser
{
    public static class PageParser
    {
        private const string Url = "https://zakupki.gov.ru/epz/order/extendedsearch/results.html";
        public static string GetPageContent(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.163 Safari/537.36";
            request.KeepAlive = true;
            request.Host = "zakupki.gov.ru";
            request.Timeout = 30000;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode != HttpStatusCode.OK) throw new Exception($"Status code: {response.StatusCode}");

            var receiveStream = response.GetResponseStream();
            var readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
            var content = readStream.ReadToEnd();
            response.Close();
            readStream.Close();

            return content;
        }

        public static IList<PurchaseData> GetPurchaseDataObjects(uint recordsPerPage, uint pageNumber)
        {
            var requestUrl = Url + "?searchString=&morphology=on&search-filter=%D0%94%D0%B0%D1%82%D0%B5+%D1%80%D0%B0%D0%B7%D0%BC%D0%B5%D1%89%D0%B5%D0%BD%D0%B8%D1%8F&pageNumber="+pageNumber+"&sortDirection=false&recordsPerPage=_"+recordsPerPage+"&showLotsInfoHidden=false&savedSearchSettingsIdHidden=&sortBy=UPDATE_DATE&fz44=on&fz223=on&af=on&ca=on&pc=on&pa=on&placingWayList=&okpd2Ids=&okpd2IdsCodes=&npaHidden=&restrictionsToPurchase44=&publishDateFrom=&publishDateTo=&applSubmissionCloseDateFrom=&applSubmissionCloseDateTo=&priceFromGeneral=&priceFromGWS=&priceFromUnitGWS=&priceToGeneral=&priceToGWS=&priceToUnitGWS=&currencyIdGeneral=-1&customerIdOrg=&agencyIdOrg=";
            var data = GetPageContent(requestUrl);
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(data);
            var fzList = htmlDoc.DocumentNode.SelectNodes("//div[@class='registry-entry__header-top__title text-truncate']");
            var titles = htmlDoc.DocumentNode.SelectNodes("//div[@class='registry-entry__body-value']");
            var customers = htmlDoc.DocumentNode.SelectNodes("//div[@class='registry-entry__body-href']/a");
            var prices = htmlDoc.DocumentNode.SelectNodes("//div[@class='price-block__value']");
            var allocationDates = htmlDoc.DocumentNode.SelectNodes("//div[@class='data-block mt-auto']//div[@class='col-6'][1]/div[@class='data-block__value']");
            var updatedDates = htmlDoc.DocumentNode.SelectNodes("//div[@class='data-block mt-auto']//div[@class='col-6'][2]/div[@class='data-block__value']");
            var purchaseTypes = htmlDoc.DocumentNode.SelectNodes("//div[@class='registry-entry__header-top__title text-truncate']");
            var purchaseNumbers = htmlDoc.DocumentNode.SelectNodes("//div[@class='registry-entry__header-mid__number']");
            var purchaseStatuses = htmlDoc.DocumentNode.SelectNodes("//div[@class='registry-entry__header-mid__title']");

            var purchaseDataList = new List<PurchaseData>();
            for (var i=0; i<recordsPerPage; i++)
            {
                purchaseDataList.Add(new PurchaseData
                {
                    Title = titles[i].InnerText.Trim(),
                    Customer = customers[i].InnerText.Trim(),
                    Price = prices[i].InnerText.Split(' ')[0],
                    AllocationDate = allocationDates[i].InnerText,
                    UpdatedDate = updatedDates[i].InnerText,
                    PartitionFz = fzList[i].InnerText.Split('\n')[1].Trim(),
                    PurchaseType = purchaseTypes[i].InnerText.Split('\n')[2].Trim(),
                    PurchaseNumber = purchaseNumbers[i].InnerText.Trim(),
                    PurchaseStatus = purchaseStatuses[i].InnerText.Trim()
                });
            }

            return purchaseDataList;
        }
    }
}

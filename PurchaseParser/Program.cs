using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseParser
{
    class Program
    {
        static void Main(string[] args)
        {
            //IList<string> data = new List<string>();
            var searchResultUrl = "https://zakupki.gov.ru/epz/order/extendedsearch/results.html";
            var htmlDoc = new HtmlDocument();
            
            for (var i = 1; i <= 10; i++)
            {
                string reqParams = $"?searchString=&morphology=on&search-filter=%D0%94%D0%B0%D1%82%D0%B5+%D1%80%D0%B0%D0%B7%D0%BC%D0%B5%D1%89%D0%B5%D0%BD%D0%B8%D1%8F&pageNumber={i}&sortDirection=false&recordsPerPage=_10&showLotsInfoHidden=false&savedSearchSettingsIdHidden=&sortBy=UPDATE_DATE&fz44=on&fz223=on&af=on&ca=on&pc=on&pa=on&placingWayList=&okpd2Ids=&okpd2IdsCodes=&npaHidden=&restrictionsToPurchase44=&publishDateFrom=&publishDateTo=&applSubmissionCloseDateFrom=&applSubmissionCloseDateTo=&priceFromGeneral=&priceFromGWS=&priceFromUnitGWS=&priceToGeneral=&priceToGWS=&priceToUnitGWS=&currencyIdGeneral=-1&customerIdOrg=&agencyIdOrg=";
                var data = GetPageContent(searchResultUrl + reqParams);
                htmlDoc.LoadHtml(data);
                var nodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='registry-entry__body-value']");
            }
        }

        public static string GetPageContent(string url)
        {           
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.163 Safari/537.36";
            request.KeepAlive = true;
            request.Host = "zakupki.gov.ru";
            request.Timeout = 60000;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode != HttpStatusCode.OK) throw new Exception($"Status code: {response.StatusCode}");
            
            var receiveStream = response.GetResponseStream();
            var readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
            var content = readStream.ReadToEnd();
            response.Close();
            readStream.Close();
            
            return content;
        }
    }
}

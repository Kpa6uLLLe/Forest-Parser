using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;
using Forest_Parser.DB;
using System.Reflection;

namespace Forest_Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            
            while (true)
            {
                int totalDeals = GetTotalInit();
                DBLayer dBLayer = new DBLayer();
                int dealsSavedCounter = 0, dealsCounter = 0, pagesToParse = totalDeals / Consts.DealsOnPage + 1;
                List<PageJSON> pages = new List<PageJSON>();
                for (int i = 0; i < pagesToParse; i++)
                {
                    PageJSON page = GetPageByNumber(i);
                    if(page != null)
                    pages.Add(page);
                    Console.WriteLine($"Done with page #{i+1}/{pagesToParse}");
                }
                foreach (PageJSON page in pages)
                {
                    foreach (Content deal in page.data.searchReportWoodDeal.content)
                    {
                        dBLayer.AddValuesToCommand(deal,',');
                        dealsCounter++;
                        if(dealsCounter>=Consts.DealsToSave || (dealsCounter>=totalDeals%Consts.DealsToSave) && (dealsCounter == totalDeals - dealsSavedCounter))
                        {
                            dBLayer.AddValuesToCommand(page.data.searchReportWoodDeal.content[page.data.searchReportWoodDeal.content.Length - 1], ';');
                            dBLayer.ExecuteCommand();
                            dealsSavedCounter += dealsCounter;
                            Console.WriteLine($"{dealsSavedCounter}/{totalDeals}");
                            dealsCounter = 0;
                        }
                    }
                    
                }
                Console.WriteLine("Parsing completed");
                pages.Clear();
                Thread.Sleep(Consts.ParsingDelay);
            }
        }
        static int GetTotalInit()
        {

            var postRequest = new PostRequest("https://www.lesegais.ru/open-area/graphql");
            /*
            var proxy = new WebProxy("http://localhost:8888", true);
            postRequest.Proxy = proxy;
            //Testing via Fiddler
            */
            postRequest.Data = ($"{{\"query\":\"query SearchReportWoodDealCount($size: Int!, $number: Int!, $filter: Filter, $orders: [Order!]) {{\\n searchReportWoodDeal(filter: $filter, pageable: {{ number: $number, size: $size}}, orders: $orders) {{\\n total\\n number\\n size\\n overallBuyerVolume\\n overallSellerVolume\\n __typename\\n  }}\\n}}\\n\",\"variables\":{{\"size\":20,\"number\":0,\"filter\":null}},\"operationName\":\"SearchReportWoodDealCount\"}}");
            postRequest.Accept = "*/*";
            postRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.5060.114 Safari/537.36 Edg/103.0.1264.62";
            postRequest.ContentType = "application/json";
            postRequest.Referer = "https://www.lesegais.ru/open-area/deal";
            postRequest.Host = "www.lesegais.ru";
            System.Net.ServicePointManager.Expect100Continue = false;
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                ["sec-ch-ua"] = "\" Not; A Brand\";v=\"99\", \"Microsoft Edge\";v=\"103\", \"Chromium\";v=\"103\"",
                ["sec-ch-ua-mobile"] = "?0",
                ["sec-ch-ua-platform"] = "\"Windows\"",
                ["Sec-Fetch-Site"] = "same-origin",
                ["Sec-Fetch-Mode"] = "cors",
                ["Sec-Fetch-Dest"] = "empty",
                ["Accept-Encoding"] = "gzip, deflate",
                ["Accept-Language"] = " ru,en;q=0.9,en-GB;q=0.8,en-US;q=0.7",
                ["Origin"] = "https://www.lesegais.ru",
                ["Cache-Control"] = "no-cache"

            };
            foreach (var pair in headers)
            {
                postRequest.Headers.Add(pair.Key, pair.Value);
            }

            postRequest.Run();
            var response = postRequest.Response;
            int start = response.IndexOf("\"total\":")+8;
            int length = response.Substring(start).IndexOf(",");
            var str = JsonConvert.DeserializeObject(response);
            return Convert.ToInt32(response.Substring(start,length));
        }
        static PageJSON GetPageByNumber(int pageNumber)
        {

            
            var postRequest = new PostRequest("https://www.lesegais.ru/open-area/graphql");
           /*
            var proxy = new WebProxy("http://localhost:8888", true);
            postRequest.Proxy = proxy;
            //Testing via Fiddler
            */
            postRequest.Data = ($"{{\"query\":\"query SearchReportWoodDeal($size: Int!, $number: Int!, $filter: Filter, $orders: [Order!]) {{\\n searchReportWoodDeal(filter: $filter, pageable: {{ number: $number, size: $size}}, orders: $orders) {{\\n content {{\\n sellerName\\n sellerInn\\n buyerName\\n buyerInn\\n woodVolumeBuyer\\n woodVolumeSeller\\n dealDate\\n dealNumber\\n}}\\n}}\\n}}\\n\",\"variables\":{{\"size\":{Consts.DealsOnPage},\"number\":{pageNumber},\"filter\":null,\"orders\":null}},\"operationName\":\"SearchReportWoodDeal\"}}");
            postRequest.Accept = "*/*";
            postRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.5060.114 Safari/537.36 Edg/103.0.1264.62";
            postRequest.ContentType = "application/json";
            postRequest.Referer = "https://www.lesegais.ru/open-area/deal";
            postRequest.Host = "www.lesegais.ru";
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                ["sec-ch-ua"] = "\" Not; A Brand\";v=\"99\", \"Microsoft Edge\";v=\"103\", \"Chromium\";v=\"103\"",
                ["sec-ch-ua-mobile"] = "?0",
                ["sec-ch-ua-platform"] = "\"Windows\"",
                ["Sec-Fetch-Site"] = "same-origin",
                ["Sec-Fetch-Mode"] = "cors",
                ["Sec-Fetch-Dest"] = "empty",
                ["Accept-Encoding"] = "gzip, deflate",
                ["Accept-Language"] = " ru,en;q=0.9,en-GB;q=0.8,en-US;q=0.7",
                ["Origin"] = "https://www.lesegais.ru",
                ["Cache-Control"] = "no-cache"

            };
            foreach (var pair in headers)
            {
                postRequest.Headers.Add(pair.Key, pair.Value);
            }

            postRequest.Run();
            try
            {
                return JsonConvert.DeserializeObject<PageJSON>(postRequest.Response);
            }
            catch
            {
                Console.WriteLine($"Parsing of page#{pageNumber+1} failed");
                return new PageJSON();
            }
        }
    }
}
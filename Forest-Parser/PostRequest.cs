using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.IO;
using System.Threading;

namespace Forest_Parser
{
    public class PostRequest
    {
        HttpWebRequest _request;
        string _adress;

        public Dictionary<string, string> Headers { get; set; }
        public string Response { get; set; }

        public string Referer { get; set; }

        public string UserAgent { get; set; }

        public string Host { get; set; }
        public string ContentType { get; set; }
        public string Accept { get; set; }
        public string Data { get; set; }

        public IWebProxy Proxy { get; set; }


        public PostRequest(string adress)
        {
            _adress = adress;
            Headers = new Dictionary<string, string>();
            Proxy = WebProxy.GetDefaultProxy();
        }
        public void Run()
        {
            _request = (HttpWebRequest)WebRequest.Create(_adress);
            _request.Method = "POST";
            _request.Proxy = Proxy;
            _request.Accept = Accept;
            _request.Host = Host;
            _request.ContentType = ContentType;
            _request.UserAgent = UserAgent;
            _request.Referer = Referer;
            _request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            foreach (var pair in Headers)
            {
                _request.Headers.Add(pair.Key, pair.Value);
            }
            try
            {
                byte[] sentData = Encoding.UTF8.GetBytes(Data);
                _request.ContentLength = sentData.Length;
                Stream sendStream;
                sendStream = _request.GetRequestStream();
                sendStream.Write(sentData, 0, sentData.Length);
                sendStream.Close();
                HttpWebResponse response = (HttpWebResponse)_request.GetResponse();
                var stream = response.GetResponseStream();
                if (stream != null) Response = new StreamReader(stream, Encoding.UTF8).ReadToEnd().ToString();
                stream.Close();
            }
            catch(Exception ex)
            {
            }
        }

    }
}

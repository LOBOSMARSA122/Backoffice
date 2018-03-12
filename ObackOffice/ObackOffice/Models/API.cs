using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Net;
using System.IO;
using System.Configuration;
using Newtonsoft.Json;
using System.Globalization;

namespace ObackOffice.Models
{
    enum HttpVerb
    {
        GET,
        POST,
        PUT,
        DELETE
    }

    public class Api
    {
        private string apiUrl = ConfigurationManager.AppSettings["ObackOfficeApiUrl"];

        public string ApiUrl
        {
            get { return apiUrl; }
            set { apiUrl = value; }
        }

        public string AccessToken { get; set; }

        public Api() : this(null) { }

        public Api(string token)
        {
            AccessToken = token;
            ServicePointManager.ServerCertificateValidationCallback += (s, c, c2, e) => { return true; };
        }

        public string ConvertToJson(object value)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(value);
        }

        #region GET
        public T Get<T>(string relativePath)
        {
            string json = Call(relativePath, HttpVerb.GET, null);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public T Get<T>(string relativePath, Dictionary<string, string> args)
        {
            string json = Call(relativePath, HttpVerb.GET, args);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public byte[] GetDownloadStream(string relativePath, Dictionary<string, string> args)
        {
            return CallStream(relativePath, HttpVerb.GET, args);
        }

        public dynamic GetDynamic(string relativePath, Dictionary<string, string> args)
        {
            string json = Call(relativePath, HttpVerb.GET, args);
            dynamic obj = null;
            try
            {
                obj = Newtonsoft.Json.Linq.JObject.Parse(json);
            }
            catch
            {
                obj = Newtonsoft.Json.Linq.JArray.Parse(json);
            }
            return obj;
        }
        #endregion

        #region POST
        public T Post<T>(string relativePath, Dictionary<string, string> args)
        {
            string json = Call(relativePath, HttpVerb.POST, args);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public string PostUploadStream(string relativePath, byte[] arr)
        {
            return CallStream(relativePath, HttpVerb.POST, arr);
        }

        public byte[] PostDownloadStream(string relativePath, Dictionary<string, string> args)
        {
            return CallStream(relativePath, HttpVerb.POST, args);
        }
        #endregion

        #region DELETE
        public string Delete(string relativePath)
        {
            return Call(relativePath, HttpVerb.DELETE, null);
        }
        #endregion

        #region PUT
        public T PUT<T>(string relativePath, Dictionary<string, string> args)
        {
            string json = Call(relativePath, HttpVerb.PUT, args);
            return JsonConvert.DeserializeObject<T>(json);
        }
        #endregion

        #region PRIVATE
        private string Call(string relativePath, HttpVerb httpVerb, Dictionary<string, string> args)
        {

            Uri baseURL = new Uri(apiUrl);
            Uri url = new Uri(baseURL, relativePath);
            if (args == null)
            {
                args = new Dictionary<string, string>();
            }
            //if (!string.IsNullOrEmpty(AccessToken))
            //{
            //    args["Token"] = AccessToken;
            //}
            string obj = MakeRequest(url, httpVerb, args);
            return obj;
        }

        private byte[] CallStream(string relativePath, HttpVerb httpVerb, Dictionary<string, string> args)
        {
            Uri baseURL = new Uri(apiUrl);
            Uri url = new Uri(baseURL, relativePath);
            if (args == null)
            {
                args = new Dictionary<string, string>();
            }
            //if (!string.IsNullOrEmpty(AccessToken))
            //{
            //    args["Token"] = AccessToken;
            //}
            byte[] obj = StreamFrom(url, httpVerb, args);
            return obj;
        }

        private string CallStream(string relativePath, HttpVerb httpVerb, byte[] arr)
        {
            Uri baseURL = new Uri(apiUrl);
            Uri url = new Uri(baseURL, relativePath);
            string obj = MakeRequestStream(url, httpVerb, arr);
            return obj;
        }

        private string MakeRequest(Uri url, HttpVerb httpVerb, Dictionary<string, string> args)
        {
            if (args != null && args.Keys.Count > 0 && httpVerb == HttpVerb.GET)
            {
                url = new Uri(url.ToString() + EncodeDictionary(args, true));
            }

            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

            request.Method = httpVerb.ToString();

            if (httpVerb == HttpVerb.POST || httpVerb == HttpVerb.PUT)
            {
                string postData = EncodeDictionary(args, false);

                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] postDataBytes = encoding.GetBytes(postData);

                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postDataBytes.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(postDataBytes, 0, postDataBytes.Length);
                requestStream.Close();
            }

            try
            {
                using (HttpWebResponse response
                        = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader
                        = new StreamReader(response.GetResponseStream());

                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private byte[] StreamFrom(Uri url, HttpVerb httpVerb, Dictionary<string, string> args)
        {
            if (args != null && args.Keys.Count > 0 && httpVerb == HttpVerb.GET)
            {
                url = new Uri(url.ToString() + EncodeDictionary(args, true));
            }

            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.AllowWriteStreamBuffering = false;
            request.Method = httpVerb.ToString();

            if (httpVerb == HttpVerb.POST)
            {
                string postData = EncodeDictionary(args, false);

                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] postDataBytes = encoding.GetBytes(postData);

                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postDataBytes.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(postDataBytes, 0, postDataBytes.Length);
                requestStream.Close();
            }

            try
            {
                byte[] result = null;
                byte[] buffer = new byte[1024];

                using (HttpWebResponse response
                        = request.GetResponse() as HttpWebResponse)
                {
                    using (Stream requestStream = response.GetResponseStream())
                    {
                        int dataLenght = (int)response.ContentLength;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            while (result == null)
                            {
                                int bytesRead = requestStream.Read(buffer, 0, buffer.Length);
                                if (bytesRead == 0)
                                {
                                    result = ms.ToArray();
                                }
                                else
                                {
                                    ms.Write(buffer, 0, bytesRead);
                                }
                            }
                        }
                    }
                }
                return result;

            }
            catch
            {
                throw;
            }
        }

        private string MakeRequestStream(Uri url, HttpVerb httpVerb, byte[] arr)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

            request.Method = httpVerb.ToString();

            if (httpVerb == HttpVerb.POST)
            {
                request.ContentType = "octect/stream";
                request.ContentLength = arr.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(arr, 0, arr.Length);
                //requestStream.Close();
            }

            try
            {
                using (HttpWebResponse response
                        = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader
                        = new StreamReader(response.GetResponseStream());

                    return reader.ReadToEnd();
                }
            }
            catch
            {
                throw;
            }
        }


        private string EncodeDictionary(Dictionary<string, string> dict, bool questionMark)
        {
            StringBuilder sb = new StringBuilder();
            if (questionMark)
            {
                sb.Append("?");
            }
            foreach (KeyValuePair<string, string> kvp in dict)
            {
                sb.Append(HttpUtility.UrlEncode(kvp.Key));
                sb.Append("=");
                sb.Append(HttpUtility.UrlEncode(kvp.Value));
                sb.Append("&");
            }
            if(dict.Count > 0)
                sb.Remove(sb.Length - 1, 1); // Remove last &

            return sb.ToString();
        }
        #endregion
    }
}
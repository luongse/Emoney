using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace OneSignalNotification.Helper
{
    internal class OneSignalHelper
    {
        public static string SendPushToServer(string pushServerUrl, object pushData, ref int responseCode)
        {
            string responseContent = null,
                htmlResult;

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(pushServerUrl);

                if (pushServerUrl.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                }

                var pushDataJson = JsonConvert.SerializeObject(pushData);
                var byteArray = Encoding.UTF8.GetBytes(pushDataJson);

                request.KeepAlive = true;
                request.Method = "POST";
                request.ContentType = "application/json; charset=utf-8";
                request.ContentLength = byteArray.Length;

                var writer = request.GetRequestStream();
                writer.Write(byteArray, 0, byteArray.Length);
                writer.Close();

                var response = (HttpWebResponse)request.GetResponse();

                var responseStream = response.GetResponseStream();
                if (responseStream != null)
                {
                    using (var reader = new StreamReader(responseStream))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                responseCode = (int)((HttpWebResponse)ex.Response).StatusCode;
                using (var response = ex.Response)
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream != null)
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                responseContent = reader.ReadToEnd();
                            }
                        }
                        else
                        {
                            responseContent = ex.ToString();
                        }
                    }
                }
            }
            finally
            {
                htmlResult = responseContent;
            }

            return htmlResult;
        }
    }
}

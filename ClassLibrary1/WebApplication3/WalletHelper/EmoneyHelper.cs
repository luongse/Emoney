using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace WebApplication3.WalletHelper
{
    public class EmoneyHelper
    {
        #region

        public const string EMONEY_API_URL = "https://mg.emoney.com.kh:8686/v2";
        public const string EMONEY_API_URL_TRANSFER = "/transactions";
        public const string EMONEY_APP_OS_NAME = "Android";
        public const int EMONEY_APP_OS_VERSION = 33;
        public const string EMONEY_PHONE_NAME = "sdk_gphone_x86_64";
        public const string EMONEY_PHONE_DEVICE_ID = "333c2dc4-674b-49c7-a8a5-c6c07c4daef1";
        public const string EMONEY_VERSION_APP = "3.6.6";
        public const string EMONEY_APP_LOCALE = "en";

        //Dùng cho quét giao dịch

        public class EmoneyHeaderRequest
        {
            public string HttpMethod { get; set; }
            public string EmoneyInfo { get; set; }
            public string EmoneyLanguage { get; set; }

            public string Authorization { get; set; }

            public string ContentType { get; set; }

            public Dictionary<string, string> AdditionHeaders { get; set; }
        }

        public class GetEmoneyTransactionBodyRequest
        {
            public string fromDate { get; set; }
            public int groupId { get; set; }
            public int page { get; set; }
            public string toDate { get; set; }
        }

        public class GetEmoneyTransactionResponse
        {
            public int status { get; set; }
            public string code { get; set; }
            public string message { get; set; }
            public long? transId { get; set; }
            public bool requireOtp { get; set; }
            public int expiredOtp { get; set; }
            public int currency { get; set; }
            public double? balance { get; set; }
            public long transTime { get; set; }

            public long? categories { get; set; }

            public TransactionResponse transactions { get; set; }

            public bool success { get; set; }
        }

        public class TransactionResponse
        {
            public int totalRecords { get; set; }
            public int totalPages { get; set; }
            public int currentPage { get; set; }
            public int pageSize { get; set; }

            public List<TransactionModelResponse> results { get; set; }
        }

        public class TransactionModelResponse
        {
            public long transId { get; set; }
            public long transDate { get; set; }
            public Dictionary<string,string> transTypeName { get; set; }
            public int currency { get; set; }
            public string currencyCode { get; set; }
            public string currencyName { get; set; }
            public double  amount { get; set; }
            public double fee { get; set; }
            public double? discount { get; set; }
            public string transMsisdn { get; set; }
            public string transCustName { get; set; }
            public string benMsisdn { get; set; }
            public string benCustName { get; set; }
            public string fullContent { get; set; }
            public string transDirection { get; set; }
            public string shortContent { get; set; }
            public string serviceCode { get; set; }
            public string transDirectionScreen { get; set; }
            public string groupCode { get; set; }
            public string featureName { get; set; }
            public string bankAccountNumber { get; set; }
            public string bankAccountName { get; set; }
            public string bankName { get; set; }
            public string bankTransType { get; set; }
            public string partnerReferId { get; set; }
            public string trxHash { get; set; }
            public string refNo { get; set; }

        }


        public static string SendRequestToURL(string apiUrl, string formBody, EmoneyHeaderRequest emoneyHeaderRequest)
        {
            string responseCode;
            var resultOutput = string.Empty;
            try
            {

                var request = (HttpWebRequest)WebRequest.Create(apiUrl);

                request.KeepAlive = false;
                request.Method = emoneyHeaderRequest.HttpMethod;
                request.ContentType = emoneyHeaderRequest.ContentType;
                request.Headers.Add("e-info", emoneyHeaderRequest.EmoneyInfo);
                request.Headers.Add("e-language", emoneyHeaderRequest.EmoneyLanguage);
                request.Headers.Add("Authorization", emoneyHeaderRequest.Authorization);


                if (!string.IsNullOrEmpty(formBody))
                {
                    var encoding = new UTF8Encoding();
                    var data = encoding.GetBytes(formBody);
                    request.ContentLength = data.Length;

                    var writer = request.GetRequestStream();
                    writer.Write(data, 0, data.Length);
                    writer.Close();
                }

                var response = (HttpWebResponse)request.GetResponse();
                responseCode = response.StatusCode.ToString();
                var responseStream = response.GetResponseStream();
                if (responseStream != null)
                {
                    using (var reader = new StreamReader(responseStream))
                    {
                        resultOutput = reader.ReadToEnd();
                    }
                }
            }
            catch (WebException e)
            {
                if (e.Response != null)
                {
                    responseCode = ((HttpWebResponse)e.Response).StatusCode.ToString();
                    using (var stream = e.Response.GetResponseStream())
                    {
                        if (stream != null)
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                resultOutput = reader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                responseCode = HttpStatusCode.InternalServerError.ToString();

                resultOutput = ex.ToString();
            }


            return resultOutput;

        }

        #endregion


    }
}
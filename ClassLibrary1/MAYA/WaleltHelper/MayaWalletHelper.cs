using MyUtility.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace MAYA.WaleltHelper
{
    public class MayaWalletHelper
    {
        private const string MAYA_API_URL = "https://api.paymaya.com/client/";
        public const string MAYA_APP_LANGUAGE = "en";
        public const string MAYA_HEADER_CLIENT_OS_NAME_KEY = "client_os_name";
        public const string MAYA_HEADER_X_REQUEST_TOKEN_KEY = "x-request-token";
        public const string MAYA_HEADER_SESSION_ID_KEY = "x-session_id-token";
        public const string MAYA_HEADER_CLIENT_APP_VERSION_KEY= "client_app_version";
        public const string MAYA_HEADER_CLIENT_SOURCE_KEY= "source";
        public const string MAYA_HEADER_CLIENT_X_REQUEST_TIMESTAMP_KEY= "x-request-timestamp";
        public const string MAYA_HEADER_CLIENT_CID_KEY = "cid";
        public const string MAYA_HEADER_CLIENT_TOKEN_KEY = "token";
        public const string MAYA_APP_HEADER_AUTHORIZATION_KEY = "Authorization";
        public const string MAYA_APP_HEADER_USER_AGENT_KEY = "User-Agent";
        public const string MAYA_APP_HEADER_X_ENCODED_USER_AGENT_KEY = "x-encoded-user-agent";
        public const string MAYA_APP_HEADER_REQUEST_REFERENCE_NO_KEY = "Request-Reference-No";

        private const string MAYA_API_BALANCE_URL = "/v1/accounts/balance";

        // Thời gian close connect sau khi thực hiện xong request (tính bằng giây)
        private const int CONNECTION_LEASE_TIMEOUT = 0;

        private const string MAYA_APP_VERSION = "2.96.0";
        private const string MAYA_APP_VERSION_CODE = "2960";

        private const string MAYA_DEVICE_BRAND = "samsung";
        private const string MAYA_DEVICE_NAME = "SM-G965N";
        private const string MAYA_DEVICE_PLATFORM_NAME = "Android";
        private const string MAYA_DEVICE_PLATFORM_OS_VERSION = "7.1.2";

        #region commomn
        private static WMayaWalletInfoModel GetWalletInfo(string jsonEnvInfo, string walletId, string deviceId)
        {
            if (string.IsNullOrEmpty(jsonEnvInfo))
            {
                if (string.IsNullOrEmpty(walletId))
                    throw new ArgumentNullException(nameof(walletId));

                if (string.IsNullOrEmpty(deviceId))
                    throw new ArgumentNullException(nameof(deviceId));

                return new WEmoneyWalletInfoModel
                {
                    WalletId = walletId,

                    DeviceId = deviceId,
                    DevicePlatformName = MAYA_DEVICE_PLATFORM_NAME,
                    DeviceBrandName = MAYA_DEVICE_BRAND,
                    DeviceName = MAYA_DEVICE_NAME,
                    DevicePlatformVersion = MAYA_DEVICE_PLATFORM_OS_VERSION,

                    AppVersion = MAYA_APP_VERSION,
                    AppVersionCode = MAYA_APP_VERSION_CODE,
                    AppLanguage = MAYA_APP_LANGUAGE,
                };
            }

            var walletInfo = JsonConvert.DeserializeObject<WMayaWalletInfoModel>(jsonEnvInfo);
            return walletInfo;
        }


        private static string SendGetRequestJsonSimple(string apiUrl, string formBody, string bindingIpRequest, ref string responseCode,
            MayaHeaderRequest headers, string contentType = "application/json; charset=utf-8")
        {
            var resultOutput = string.Empty;
            var htmlResult = string.Empty;
            var stepInfo = "SendGetRequestJsonSimple";

            try
            {
                stepInfo += "-1";
                ServicePointManager.UseNagleAlgorithm = true;
                if (apiUrl.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.CheckCertificateRevocationList = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                }

                if (!string.IsNullOrEmpty(bindingIpRequest))
                {
                    stepInfo += "-2";

                    var servicePoint = ServicePointManager.FindServicePoint(new Uri(apiUrl));

                    stepInfo += "-3";
                    servicePoint.BindIPEndPointDelegate = (sp, remoteEndPoint, retryCount) =>
                    {
                        // return null, computer will auto select network adapter
                        if (retryCount > 3)
                        {
                            stepInfo += "-3.1";
                            //ogger.CommonLogger.PaymentLogger.DefaultLogger("WMayaHelper -- SendGetRequestJsonSimple -- Binding failed -- Url: {0} -- bindingIpRequest: {1}",
                            //   apiUrl, bindingIpRequest);
                            return null;
                        }

                        var ipAddress = IPAddress.Parse(bindingIpRequest);
                        return new IPEndPoint(ipAddress, 0);
                    };

                    // https://docs.microsoft.com/en-us/dotnet/api/system.net.servicepoint.connectionleasetimeout?view=netframework-4.5#system-net-servicepoint-connectionleasetimeout
                    servicePoint.ConnectionLeaseTimeout = CONNECTION_LEASE_TIMEOUT * 1000; // sau CONNECTION_LEASE_TIMEOUT giây connect sẽ close
                    //servicePoint.ConnectionLeaseTimeout = 0; // force close các connection đang active
                }

                var request = (HttpWebRequest)WebRequest.Create(apiUrl);

                request.KeepAlive = false;
                request.Method = "GET";
                request.Accept = "application/json";

                switch (headers.MayaTokenType)
                {
                    case MayaTokenType.Bearer:
                        request.Headers.Add("Authorization", "Bearer " + headers.AuthorizeToken);
                        break;
                    default:
                        request.Headers.Add("Authorization", headers.AuthorizeToken);
                        break;

                }

                request.Headers.Add(MAYA_HEADER_CLIENT_TOKEN_KEY, headers.Token);
                request.Headers.Add(MAYA_APP_HEADER_USER_AGENT_KEY, headers.UserAgent);
                request.Headers.Add(MAYA_APP_HEADER_X_ENCODED_USER_AGENT_KEY, headers.XEncodeUserAgent);
                request.Headers.Add(MAYA_APP_HEADER_REQUEST_REFERENCE_NO_KEY, headers.RequestReferenceNo);

                if (headers.AdditionHeaders != null && headers.AdditionHeaders.Any())
                {
                    foreach (var headerKey in headers.AdditionHeaders)
                    {
                        request.Headers.Add(headerKey.Key, headerKey.Value);
                    }
                }

                if (!string.IsNullOrEmpty(formBody))
                {
                    var encoding = new UTF8Encoding();
                    var data = encoding.GetBytes(formBody);

                    request.ContentType = contentType;
                    request.ContentLength = data.Length;

                    var writer = request.GetRequestStream();
                    writer.Write(data, 0, data.Length);
                    writer.Close();
                }

                if (string.IsNullOrEmpty(bindingIpRequest))
                    // Logger.CommonLogger.PaymentLogger.DebugFormat("WMayaHelper -- SendPOSTRequestJsonSimple -- Url: {0} -- bindingIpRequest null", apiUrl);

                    stepInfo += "-4";
                var response = (HttpWebResponse)request.GetResponse();
                stepInfo += "-4.1";

                responseCode = response.StatusCode.Value().ToString();
                var responseStream = response.GetResponseStream();
                stepInfo += "-4.2";

                if (responseStream != null)
                {
                    stepInfo += "-4.3";
                    using (var reader = new StreamReader(responseStream))
                    {
                        resultOutput = reader.ReadToEnd();
                    }
                }
            }
            catch (WebException e)
            {
                responseCode = "500";
                //  Logger.CommonLogger.DefaultLogger.ErrorFormat("WMayaHelper -- SendGETRequestJsonSimple -- Url: {0} -- ex: {1}", apiUrl, e.InnerException != null ? e.InnerException.ToString() : e.Message);
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
                // Logger.CommonLogger.DefaultLogger.ErrorFormat("WMayaHelper -- SendGETRequestJsonSimple -- Url: {0} -- ex: {1}", apiUrl, resultOutput);
            }
            catch (Exception ex)
            {
                responseCode = HttpStatusCode.InternalServerError.ToString();

                resultOutput = ex + " -- stepInfo: " + stepInfo;
                // Logger.CommonLogger.DefaultLogger.ErrorFormat("WMayaHelper -- SendGETRequestJsonSimple -- Url: {0} -- ex: {1}", apiUrl, resultOutput);
            }
            finally
            {
                htmlResult = resultOutput;
            }

            return htmlResult;

        }

       
     
        private static decimal ConvertAmount(string netAmount)
        {
            if (string.IsNullOrEmpty(netAmount))
                return 0;

            try
            {
                return decimal.Parse(netAmount);
            }
            catch
            {
                return 0;
            }
        }

        #endregion


        #region model

        #region wallet info

        public class WMayaWalletInfoModel
        {
            [JsonProperty("wallet_id")]
            public string WalletId { get; set; }


            [JsonProperty("app_language")]
            public string AppLanguage { get; set; }

            [JsonProperty("app_version_string")]
            public string AppVersion { get; set; }

            [JsonProperty("app_version_code")]
            public string AppVersionCode { get; set; }


            [JsonProperty("device_id")]
            public string DeviceId { get; set; }

            [JsonProperty("device_platform_name")]
            public string DevicePlatformName { get; set; }

            [JsonProperty("device_platform_os_version")]
            public string DevicePlatformVersion { get; set; }

            [JsonProperty("device_brand_name")]

            public string DeviceBrandName { get; set; }

            [JsonProperty("device_name")]
            public string DeviceName { get; set; }
        }

        #endregion

        #region MAYA Header Request
        public class MayaHeaderRequest
        {
            [JsonProperty("token")]
            public string Token { get; set; }

            [JsonProperty("authorized_token")]
            public string AuthorizeToken { get; set; }

            [JsonProperty("maya_token_type")]
            public MayaTokenType MayaTokenType { get; set; }

            [JsonProperty("user_agent")]
            public string UserAgent { get; set; }

            [JsonProperty("x_encode_user_agent")]
            public string XEncodeUserAgent { get; set; }

            [JsonProperty("request_reference_no")]
            public string RequestReferenceNo { get; set; }


            [JsonProperty("additon_headers")]
            public Dictionary<string, string> AdditionHeaders { get; set; }

        }

        #endregion



        #region enum

        public enum MayaTokenType
        {
            None,
            Basic,
            Bearer
        }
        #endregion




    }
}
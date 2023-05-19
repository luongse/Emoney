using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace WebApplication3.WalletHelper
{
    public class EmoneyWalletHelper
    {
        private const string EMONEY_API_URL = "https://mg.emoney.com.kh:8686/v2";

        public const string EMONEY_APP_LANGUAGE = "en";
        public const string EMONEY_APP_HEADER_INFO_KEY = "e-info";
        public const string EMONEY_APP_HEADER_LANGUAGE_KEY = "e-language";
        public const string EMONEY_APP_HEADER_AUTHORIZATION_KEY = "Authorization";

        private const string EMONEY_API_TRANSFER_URL = "/transactions";
        private const string EMONEY_API_BALANCE_URL = "/account/bal";

        // Thời gian close connect sau khi thực hiện xong request (tính bằng giây)
        private const int CONNECTION_LEASE_TIMEOUT = 0;

        private const string EMONEY_APP_VERSION = "3.6.6";
        private const string EMONEY_APP_VERSION_CODE = "366";

        private const string EMONEY_DEVICE_BRAND = "samsung";
        private const string EMONEY_DEVICE_NAME = "SM-G965N";
        private const string EMONEY_DEVICE_PLATFORM_NAME = "Android";
        private const string EMONEY_DEVICE_PLATFORM_OS_VERSION = "7.1.2";

        #region private

        #region common

        private static WEmoneyWalletInfoModel GetWalletInfo(string jsonEnvInfo, string walletId, string deviceId)
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
                    DevicePlatformName = EMONEY_DEVICE_PLATFORM_NAME,
                    DeviceBrandName = EMONEY_DEVICE_BRAND,
                    DeviceName = EMONEY_DEVICE_NAME,
                    DevicePlatformVersion = EMONEY_DEVICE_PLATFORM_OS_VERSION,

                    AppVersion = EMONEY_APP_VERSION,
                    AppVersionCode = EMONEY_APP_VERSION_CODE,
                    AppLanguage = EMONEY_APP_LANGUAGE,
                };
            }

            var walletInfo = JsonConvert.DeserializeObject<WEmoneyWalletInfoModel>(jsonEnvInfo);
            return walletInfo;
        }

        private static string GetEInfoHeader(WEmoneyWalletInfoModel walletInfo, string deviceId)
        {
            if (walletInfo == null)
                throw new ArgumentNullException(nameof(walletInfo));

            var forceDeviceId = deviceId;
            if (string.IsNullOrEmpty(deviceId))
                forceDeviceId = walletInfo.DeviceId;

            return string.Format("{0};{1};{2};1;{3};{4}",
                walletInfo.DevicePlatformName, walletInfo.DevicePlatformVersion, walletInfo.DeviceName, forceDeviceId, walletInfo.AppVersion);
        }

        private static string GetEInfoHeader(string jsonEnvInfo, string walletId, string deviceId)
        {
            if (string.IsNullOrEmpty(jsonEnvInfo))
                throw new ArgumentNullException(nameof(jsonEnvInfo));

            var walletInfo = GetWalletInfo(jsonEnvInfo, walletId, deviceId);
            return GetEInfoHeader(walletInfo, deviceId);
        }

        private static Dictionary<string, string> GetCommonHeader(string jsonEnvInfo, string walletId, string deviceId, string token)
        {
            if (string.IsNullOrEmpty(jsonEnvInfo))
                throw new ArgumentNullException(nameof(jsonEnvInfo));

            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException(nameof(token));

            var walletInfo = GetWalletInfo(jsonEnvInfo, walletId, deviceId);
            if (walletInfo == null)
                throw new ArgumentNullException(nameof(walletInfo));

            return new Dictionary<string, string>
            {
                {EMONEY_APP_HEADER_INFO_KEY, GetEInfoHeader(walletInfo, deviceId)},
                {EMONEY_APP_HEADER_LANGUAGE_KEY, walletInfo.AppLanguage},
                {EMONEY_APP_HEADER_AUTHORIZATION_KEY, token},
            };
        }

        private static string SendPOSTRequestJsonSimple(string apiUrl, string formBody, string bindingIpRequest, ref string responseCode,
            Dictionary<string, string> headers, string contentType = "application/json; charset=utf-8")
        {
            var resultOutput = string.Empty;
            var htmlResult = string.Empty;
            var stepInfo = "SendPOSTRequestJsonSimple";

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
                           // Logger.CommonLogger.PaymentLogger.DebugFormat("WEmoneyHelper -- SendPOSTRequestJsonSimple -- Binding failed -- Url: {0} -- bindingIpRequest: {1}",
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
                request.Method = "POST";
                request.Accept = "application/json";

                if (headers != null && headers.Any())
                {
                    foreach (var headerKey in headers.Keys)
                    {
                        request.Headers.Add(headerKey, headers[headerKey]);
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
                   // Logger.CommonLogger.PaymentLogger.DebugFormat("WEmoneyHelper -- SendPOSTRequestJsonSimple -- Url: {0} -- bindingIpRequest null", apiUrl);

                stepInfo += "-4";
                var response = (HttpWebResponse)request.GetResponse();
                stepInfo += "-4.1";

                responseCode = response.StatusCode.ToString();
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
              //  Logger.CommonLogger.DefaultLogger.ErrorFormat("WEmoneyHelper -- SendPOSTRequestJsonSimple -- Url: {0} -- ex: {1}", apiUrl, e.InnerException != null ? e.InnerException.ToString() : e.Message);
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
               // Logger.CommonLogger.DefaultLogger.ErrorFormat("WEmoneyHelper -- SendPOSTRequestJsonSimple -- Url: {0} -- ex: {1}", apiUrl, resultOutput);
            }
            catch (Exception ex)
            {
                responseCode = HttpStatusCode.InternalServerError.ToString();

                resultOutput = ex + " -- stepInfo: " + stepInfo;
               // Logger.CommonLogger.DefaultLogger.ErrorFormat("WEmoneyHelper -- SendPOSTRequestJsonSimple -- Url: {0} -- ex: {1}", apiUrl, resultOutput);
            }
            finally
            {
                htmlResult = resultOutput;
            }

            return htmlResult;

        }

        #endregion


        #region balance

        private static bool BalanceWithId(string walletId, string currency, string accessToken, string bindingIpRequest, string walletDeviceId, string jsonEnvInfo,
            ref string message, ref string errorCode, ref decimal balance, ref decimal pendingBalance)
        {
            try
            {
                var headers = GetCommonHeader(jsonEnvInfo, walletId, walletDeviceId, accessToken);
                var requestUrl = string.Format("{0}{1}", EMONEY_API_URL, EMONEY_API_BALANCE_URL);

                const int sleepTime = 1000;
                System.Threading.Thread.Sleep(sleepTime);
                var resultDetail = SendPOSTRequestJsonSimple(requestUrl, "", bindingIpRequest, ref errorCode, headers, "application/json; charset=utf-8");

                //Logger.CommonLogger.PaymentLogger.DebugFormat("WEmoneyHelper -- BalanceWithId -- wingSenderID: {0} -- Url: {1} -- Result: {2}",
                  //  walletId, requestUrl, resultDetail);

                message = resultDetail;

                if (string.IsNullOrEmpty(resultDetail))
                    return false;

                var balanceInfo = JsonConvert.DeserializeObject<WEmoneyBalanceResponseModel>(resultDetail);
                if (balanceInfo == null)
                    return false;

                balance = currency.ToUpper() == "KHR" ? balanceInfo.KhrBalance.GetValueOrDefault(0) : balanceInfo.UsdBalance.GetValueOrDefault(0);
                pendingBalance = 0M;
                return true;
            }
            catch (Exception exp)
            {
              //  Logger.CommonLogger.PaymentLogger.DebugFormat("WEmoneyHelper -- BalanceWithId -- wingSenderID: {0} -- exp: {1}",
                 //   walletId, exp);
                return false;
            }
        }

        #endregion

        #endregion


        #region login

        #endregion


        #region balance
        /*
        public static WalletBalanceWithIdModel GetWalletBalanceWithAccessToken(string walletId, string currency, string accessToken, string bindingIpRequest, string walletDeviceId, string jsonEnvInfo)
        {
            string message = "", errorCode = "";
            var balance = 0M;
            var pendingBalance = 0M;

            if (string.IsNullOrEmpty(walletId) || string.IsNullOrEmpty(walletDeviceId) || string.IsNullOrEmpty(jsonEnvInfo) || string.IsNullOrEmpty(accessToken))
                return null;

            var result = BalanceWithId(walletId, currency, accessToken, bindingIpRequest, walletDeviceId, jsonEnvInfo, ref message, ref errorCode, ref balance, ref pendingBalance);
            if (!result) return null;

            var balanceInfo = new WalletBalanceWithIdModel
            {
                StatusCode = WHelperStatusCode.Success,
                Balance = balance,
                BlockAmount = pendingBalance,
                WalletId = walletId,
            };

            return balanceInfo;
        }
        */
        #endregion


        #region detail

        #endregion


        #region transaction
        /*
          public static List<TransactionModelResponse> GetWalletTransactionWithAccessToken(string walletId, DateTime dateFrom, int groupId, int page, DateTime dateTo, string accessToken, string bindingIpRequest, string walletDeviceId, string jsonEnvInfo)
    {
        string message = "", errorCode = "" ;
        List<TransactionModelResponse> listTransactionModelResponse = new List<TransactionModelResponse>();
        if (string.IsNullOrEmpty(walletId) || string.IsNullOrEmpty(walletDeviceId) || string.IsNullOrEmpty(jsonEnvInfo) || string.IsNullOrEmpty(accessToken))
            return null;

        //DateTime? resultDateTo = !(dateTo.HasValue) ? DateTime.Now : dateTo;
        //DateTime? resultDateFrom = !(dateFrom.HasValue) ? resultDateTo.AddDays(-7) : dateFrom;

        //if (!dateFrom.HasValue)
        //{
        //    resultDateFrom = resultDateTo.AddDays(-7);
        //}


        var result = GetListWalletTransactionsWithId(walletId, dateFrom, groupId, page, dateTo, accessToken, bindingIpRequest, walletDeviceId, jsonEnvInfo, ref message,  ref errorCode, ref  listTransactionModelResponse);

    }
         */


        private static bool GetListWalletTransactionsWithId(string walletId, DateTime dateFrom, int groupId, int page, DateTime dateTo, string accessToken, string bindingIpRequest, string walletDeviceId, string jsonEnvInfo, ref string message, ref string errorCode, ref List<TransactionModelResponse> listTransactionModelResponse)
        {
            if (page < 1) page = 1;
            try
            {
                var headers = GetCommonHeader(jsonEnvInfo, walletId, walletDeviceId, accessToken);
                var requestUrl = string.Format("{0}{1}", EMONEY_API_URL, EMONEY_API_TRANSFER_URL);
                var bodyRequest = new GetEmoneyTransactionBodyRequest
                {
                    FromDate = dateFrom.ToString(),
                    GroupId = groupId,
                    Page = page,
                    ToDate = dateTo.ToString(),
                };
                string body = JsonConvert.SerializeObject(bodyRequest);

                const int sleepTime = 1000;
                System.Threading.Thread.Sleep(sleepTime);
                var resultDetail = SendPOSTRequestJsonSimple(requestUrl, body, bindingIpRequest, ref errorCode, headers, "application/json; charset=utf-8");

                //Logger.CommonLogger.PaymentLogger.DebugFormat("WEmoneyHelper -- Transactions -- wingSenderID: {0} -- Url: {1} -- Result: {2}",
                //  walletId, requestUrl, resultDetail);

                message = resultDetail;

                if (string.IsNullOrEmpty(resultDetail))
                    return false;

                var transactions = JsonConvert.DeserializeObject<TransactionResponse>(resultDetail);
                if (transactions == null || transactions.Results == null || !(transactions.Results.Any()))
                    return false;

                listTransactionModelResponse = transactions.Results;
                return true;
            }
            catch (Exception exp)
            {
                //  Logger.CommonLogger.PaymentLogger.DebugFormat("WEmoneyHelper -- Transactions -- wingSenderID: {0} -- exp: {1}",
                //   walletId, exp);
                return false;
            }
        }
        #endregion


        #region transfer

        #endregion
    }



    #region model

    #region wallet info

    public class WEmoneyWalletInfoModel
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


    #region base

    #region response

    public class WEmoneyBaseResponse
    {
        [JsonProperty("status")]
        public int StatusCode { get; set; }

        [JsonProperty("code")]
        public string ErrorCode { get; set; }

        [JsonProperty("message")]
        public string ErrorMessage { get; set; }

        [JsonProperty("transId")]
        public string TransactionId { get; set; }

        [JsonProperty("requireOtp")]
        public int RequireOtp { get; set; }

        [JsonProperty("expiredOtp")]
        public int ExpiredOtp { get; set; }

        [JsonProperty("currency")]
        public int Currency { get; set; }

        [JsonProperty("transTime")]
        public long TransactionTime { get; set; }

        [JsonProperty("success")]
        public bool IsSuccess { get; set; }
    }

    #endregion

    #endregion


    #region balance

    #region request

    #endregion


    #region response

    public class WEmoneyBalanceResponseModel : WEmoneyBaseResponse
    {
        /*
        {
          "status": 0,
          "code": "MSG_SUCCESS",
          "message": "Success",
          "transId": null,
          "requireOtp": 0,
          "expiredOtp": 90,
          "currency": 0,
          "balance": null,
          "transTime": 1684211343628,
          "usdBalance": "5.00",
          "khrBalance": "0",
          "success": true
        }
        */

        [JsonProperty("balance")]
        public object BalanceInfo { get; set; }

        [JsonProperty("usdBalance")]
        public decimal? UsdBalance { get; set; }

        [JsonProperty("khrBalance")]
        public decimal? KhrBalance { get; set; }
    }

    #endregion

    #endregion


    #region Quét giao dich

    #region request
    
    public class GetEmoneyTransactionBodyRequest
    {
        [JsonProperty("fromDate")]
        public string FromDate { get; set; }

        [JsonProperty("groupId")]
        public int GroupId { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("toDate")]
        public string ToDate { get; set; }
    }
    #endregion

    #region response

 
    public class GetEmoneyTransactionResponse : WEmoneyBaseResponse
    {
        [JsonProperty("categories")]

        public long? Categories { get; set; }

        [JsonProperty("transactions")]
        public TransactionResponse Transactions { get; set; }

    }

    public class TransactionResponse
    {
        [JsonProperty("totalRecords")]

        public int TotalRecords { get; set; }

        [JsonProperty("TotalPages")]
        public int totalPages { get; set; }

       
        [JsonProperty("currentPage")]
        public int CurrentPage { get; set; }

        [JsonProperty("pageSize")]
        public int PageSize { get; set; }

        [JsonProperty("results")]
        public List<TransactionModelResponse> Results { get; set; }
    }

    public class TransactionModelResponse
    {
        [JsonProperty("transId")]
        public long TransId { get; set; }

        [JsonProperty("transDate")]
        public long TransDate { get; set; }

        [JsonProperty("transTypeName")]
        public Dictionary<string, string> TransTypeName { get; set; }

        [JsonProperty("currency")]
        public int Currency { get; set; }

        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonProperty("currencyName")]
        public string CurrencyName { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("fee")]
        public double Fee { get; set; }

        [JsonProperty("discount")]
        public double? Discount { get; set; }

        [JsonProperty("transMsisdn")]
        public string TransMsisdn { get; set; }

        [JsonProperty("transMsisdn")]
        public string TransCustName { get; set; }

        [JsonProperty("benMsisdn")]
        public string BenMsisdn { get; set; }

        [JsonProperty("benCustName")]
        public string BenCustName { get; set; }

        [JsonProperty("fullContent")]
        public string FullContent { get; set; }

        [JsonProperty("transDirection")]
        public string TransDirection { get; set; }

        [JsonProperty("shortContent")]
        public string ShortContent { get; set; }

        [JsonProperty("serviceCode")]
        public string ServiceCode { get; set; }

        [JsonProperty("transDirectionScreen")]
        public string TransDirectionScreen { get; set; }

        [JsonProperty("groupCode")]
        public string GroupCode { get; set; }

        [JsonProperty("featureName")]
        public string FeatureName { get; set; }

        [JsonProperty("bankAccountNumber")]
        public string BankAccountNumber { get; set; }

        [JsonProperty("bankAccountName")]
        public string BankAccountName { get; set; }

        [JsonProperty("bankName")]
        public string BankName { get; set; }

        [JsonProperty("bankTransType")]
        public string BankTransType { get; set; }

        [JsonProperty("partnerReferId")]
        public string PartnerReferId { get; set; }

        [JsonProperty("trxHash")]
        public string TrxHash { get; set; }

        [JsonProperty("refNo")]
        public string RefNo { get; set; }

    }
    #endregion

    #endregion

    #endregion
}
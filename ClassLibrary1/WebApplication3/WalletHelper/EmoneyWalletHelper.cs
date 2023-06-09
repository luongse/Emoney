﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using BusinessObject.WalletBusiness.Enums;
using MyUtility.Extensions;
using Newtonsoft.Json;
using WebApplication3.Models;

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
        private const string EMONEY_API_SENDMONEYINFO = "/transfer/info";
        private const string EMONEY_API_CONFIRMSENDMONEYINFO = "/transfer";

        private const string EMONEY_API_LOGIN_URL = "/auth/login";
        private const string EMONEY_API_GET_OTP_URL = "/auth/otp/1/";


        // Thời gian close connect sau khi thực hiện xong request (tính bằng giây)
        private const int CONNECTION_LEASE_TIMEOUT = 0;

        private const string EMONEY_APP_VERSION = "3.6.6";
        private const string EMONEY_APP_VERSION_CODE = "366";

        private const string EMONEY_DEVICE_BRAND = "samsung";
        private const string EMONEY_DEVICE_NAME = "SM-G965N";
        private const string EMONEY_DEVICE_PLATFORM_NAME = "Android";
        private const string EMONEY_DEVICE_PLATFORM_OS_VERSION = "7.1.2";

        private const string EMONEY_TRANSTYPE_DEPOSIT = "DEPOSIT";
        private const string EMONEY_TRANSTYPE_WITHDRAW = "WITHDRAW";

        private const int EMONEY_REQUIRE_OTP = 1;



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

        private static string SendGETRequestJsonSimple(string apiUrl, string formBody, string bindingIpRequest, ref string responseCode,
            Dictionary<string, string> headers, string contentType = "application/json; charset=utf-8")
        {
            var resultOutput = string.Empty;
            var htmlResult = string.Empty;
            var stepInfo = "SendGETRequestJsonSimple";

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
                            // Logger.CommonLogger.PaymentLogger.DebugFormat("WEmoneyHelper -- SendGETRequestJsonSimple -- Binding failed -- Url: {0} -- bindingIpRequest: {1}",
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
                    // Logger.CommonLogger.PaymentLogger.DebugFormat("WEmoneyHelper -- SendGETRequestJsonSimple -- Url: {0} -- bindingIpRequest null", apiUrl);

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
                //  Logger.CommonLogger.DefaultLogger.ErrorFormat("WEmoneyHelper -- SendGETRequestJsonSimple -- Url: {0} -- ex: {1}", apiUrl, e.InnerException != null ? e.InnerException.ToString() : e.Message);
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

        public static string ParseETransType(string eTransType)
        {
            switch (eTransType)
            {
                case "in":
                    return EMONEY_TRANSTYPE_DEPOSIT;

                case "out":
                    return EMONEY_TRANSTYPE_WITHDRAW;


                default:
                    return eTransType;
            }
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
        private static string ConvertWalletFormat(string walletId)
        {
            if (string.IsNullOrEmpty(walletId))
                return string.Empty;

            walletId = walletId.Trim();
            if (string.IsNullOrEmpty(walletId))
                return string.Empty;

            try
            {
                var regex = new Regex(@"(^\+?855)");
                return regex.IsMatch(walletId) ? regex.Replace(walletId, "0") : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        private static TransactionDetailModel ConvertToTransactionDetailModel(WEmoneyWalletInfoModel wEmoneyWalletInfoModel, TransactionDetailDtoResponse transactionResponse)
        {
            if (transactionResponse == null) return null;
            if (wEmoneyWalletInfoModel == null) return null;

            var walletId = string.IsNullOrEmpty(transactionResponse.TransMsisdn)
                ? wEmoneyWalletInfoModel.WalletId
                : transactionResponse.TransMsisdn;

            if (walletId.StartsWith("+855"))
                walletId = "0" + walletId.Substring(4);

            if (walletId.StartsWith("855"))
                walletId = "0" + walletId.Substring(3);

            var walletName = string.IsNullOrEmpty(transactionResponse.TransCustName) ? "" : transactionResponse.TransCustName.Trim();


            var destWalletName = string.IsNullOrEmpty(transactionResponse.BenCustName) ? "" : transactionResponse.BenCustName.Trim();
            var destWalletId = !string.IsNullOrEmpty(transactionResponse.BenMsisdn)
                ? transactionResponse.BenMsisdn.Trim()
                : wEmoneyWalletInfoModel.WalletId;

            if (destWalletId.StartsWith("+855"))
                destWalletId = "0" + destWalletId.Substring(4);

            if (destWalletId.StartsWith("855"))
                destWalletId = "0" + destWalletId.Substring(3);

            var currencyCode = string.IsNullOrEmpty(transactionResponse.CurrencyCode) ? "" : transactionResponse.CurrencyCode;
            var description = string.IsNullOrEmpty(transactionResponse.FullContent) ? "" : transactionResponse.FullContent;
            var jsonData = JsonConvert.SerializeObject(transactionResponse);
            var transType = ParseETransType(transactionResponse.TransDirection);

            TransactionDetailModel dataSuccess = new TransactionDetailModel();
            switch (transType)
            {
                case EMONEY_TRANSTYPE_DEPOSIT:
                    dataSuccess = new TransactionDetailModel
                    {
                        TransactionId = transactionResponse.TransId,
                        TransTime = transactionResponse.TransDate,
                        WalletId = walletId,
                        WalletAccountName = walletName,
                        DepositAmount = ConvertAmount(transactionResponse.Amount.ToString(CultureInfo.InvariantCulture)),
                        DestWalletId = destWalletId,
                        DestWalletAccountName = destWalletName,
                        TransactionType = transType,
                        CurrencyCode = currencyCode,
                        Description = description,
                        JsonData = jsonData
                    };
                    break;
                case EMONEY_TRANSTYPE_WITHDRAW:
                    dataSuccess = new TransactionDetailModel
                    {
                        TransactionId = transactionResponse.TransId,
                        TransTime = transactionResponse.TransDate,
                        WalletId = walletId,
                        WalletAccountName = walletName,
                        DepositAmount = ConvertAmount(transactionResponse.Amount.ToString(CultureInfo.InvariantCulture)),
                        DestWalletId = destWalletId,
                        DestWalletAccountName = destWalletName,
                        TransactionType = transType,
                        CurrencyCode = currencyCode,
                        Description = description,
                        JsonData = jsonData

                    };
                    break;

                default:
                    dataSuccess = new TransactionDetailModel
                    {
                        TransactionId = transactionResponse.TransId,
                        TransTime = transactionResponse.TransDate,
                        WalletAccountName = walletName,
                        DepositAmount = ConvertAmount(transactionResponse.Amount.ToString(CultureInfo.InvariantCulture)),
                        DestWalletId = destWalletId,
                        DestWalletAccountName = destWalletName,
                        TransactionType = transType,
                        CurrencyCode = currencyCode,
                        Description = description,
                        JsonData = jsonData
                    };
                    break;

            }



            return dataSuccess;

        }
        private static List<TransactionDetailModel> ConvertToTransactionDetailModel(WEmoneyWalletInfoModel wEmoneyWalletInfoModel, List<TransactionDetailDtoResponse> emoneyTransactions)
        {
            if (emoneyTransactions == null || !emoneyTransactions.Any())
                return null;

            if (wEmoneyWalletInfoModel == null)
                return null;

            return emoneyTransactions.Select(g => ConvertToTransactionDetailModel(wEmoneyWalletInfoModel, g)).Where(t => t != null).ToList();
        }
        private static TransferEMoneyResponseModel ConvertToTransferEMoneyResponseModel(GetBillInfoEmoneyResponse getBillInfoEmoneyResponse)
        {
            if (getBillInfoEmoneyResponse == null) return null;
            if (string.IsNullOrEmpty(getBillInfoEmoneyResponse.TransactionId))
            {
                return null;
            }

            var amount = ConvertAmount(getBillInfoEmoneyResponse.Amount);
            var fee = ConvertAmount(getBillInfoEmoneyResponse.Fee);
            var totalAmount = ConvertAmount(getBillInfoEmoneyResponse.TotalAmount);
            var balance = ConvertAmount(getBillInfoEmoneyResponse.Balance);

            
            var walletId = string.IsNullOrEmpty(getBillInfoEmoneyResponse.SenderMsisdn)
                 ? ""
                 : getBillInfoEmoneyResponse.SenderMsisdn;

            if (walletId.StartsWith("+855"))
                walletId = "0" + walletId.Substring(4);

            if (walletId.StartsWith("855"))
                walletId = "0" + walletId.Substring(3);

            var destWalletId = !string.IsNullOrEmpty(getBillInfoEmoneyResponse.ReceiverMsisdn)
                ? getBillInfoEmoneyResponse.ReceiverMsisdn.Trim()
                : "";

            if (destWalletId.StartsWith("+855"))
                destWalletId = "0" + destWalletId.Substring(4);

            if (destWalletId.StartsWith("855"))
                destWalletId = "0" + destWalletId.Substring(3);

            var destWalletName = string.IsNullOrEmpty(getBillInfoEmoneyResponse.ReceiverName) ? "" : getBillInfoEmoneyResponse.ReceiverName;

            var dataSuccess = new TransferEMoneyResponseModel
            {
                StatusCode = WHelperStatusCode.Success.Value(),
                TransactionId = getBillInfoEmoneyResponse.TransactionId,
                TransactionTime = getBillInfoEmoneyResponse.TransactionTime,
                Balance = balance,
                Amount = amount,
                Fee = fee,
                TotalAmount = totalAmount,
                Currency = getBillInfoEmoneyResponse.Currency,
                WalletId = walletId,
                DesWalletId = destWalletId,
                DestWalletName = destWalletName
            };
            return dataSuccess;
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

        #region transactions
        private static bool GetListWalletTransactionsWithId(string walletId, string dateFrom, int groupId, int page, string dateTo, string accessToken, string bindingIpRequest, string walletDeviceId, string jsonEnvInfo, ref string message, ref string errorCode, ref List<TransactionDetailModel> transactionDetailModels)
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
                var responseCode = string.Empty;
                const int sleepTime = 1000;
                System.Threading.Thread.Sleep(sleepTime);
                var resultDetail = SendPOSTRequestJsonSimple(requestUrl, body, bindingIpRequest, ref responseCode, headers, "application/json; charset=utf-8");

                //Logger.CommonLogger.PaymentLogger.DebugFormat("WEmoneyHelper -- GetListWalletTransactionsWithId -- wingSenderID: {0} -- Url: {1} -- Result: {2}",
                //  walletId, requestUrl, resultDetail);


                if (string.IsNullOrEmpty(resultDetail))
                    return false;

                if (!string.IsNullOrEmpty(responseCode) && responseCode != "200")
                {
                    message = "response code is not OK";
                    return false;
                }

                var transactions = JsonConvert.DeserializeObject<GetEmoneyTransactionResponse>(resultDetail);
                if (transactions == null)
                    return false;

                message = "Success - ResponseString: " + resultDetail;
                transactionDetailModels = new List<TransactionDetailModel>();
                var walletInfoModel = GetWalletInfo(jsonEnvInfo, walletId, walletDeviceId);
                if (walletInfoModel == null)
                {
                    message = "Cannot Get Wallet Info -ListTransactions ";
                    return false;
                }
                if (transactions.Transactions.Results != null && transactions.Transactions.Results.Any())
                    transactionDetailModels = ConvertToTransactionDetailModel(walletInfoModel, transactions.Transactions.Results);

                return true;
            }
            catch (Exception exp)
            {
                //  Logger.CommonLogger.PaymentLogger.DebugFormat("WEmoneyHelper -- GetListWalletTransactionsWithId -- wingSenderID: {0} -- exp: {1}",
                //   walletId, exp);
                return false;
            }
        }

        #endregion
        #region transfer 
        private static bool CheckoutInfoSendingMoneyResponse(string walletId, string amount, string content, int currency, int option, string pin, string destWalletId, string accessToken, string bindingIpRequest, string walletDeviceId, string jsonEnvInfo, ref string message, ref string errorCode, ref TransferEMoneyResponseModel transferEMoneyResponseModel, ref  WHelperDetailStatusCode wHelperDetailStatusCode, ref string detailStatusText)
        {
            try
            {
              
                var headers = GetCommonHeader(jsonEnvInfo, walletId, walletDeviceId, accessToken);
                var requestUrl = string.Format("{0}{1}", EMONEY_API_URL, EMONEY_API_SENDMONEYINFO);
                var amountTransfer = ConvertAmount(amount);


                var bodyRequest = new GetBillInfoEmoneyBodyRequest
                {
                    Amount = amountTransfer.ToString(),
                    Content = content,
                    Currency = currency,
                    Pin = pin,
                    Option = option,
                    ReceiverMsisdn = destWalletId
                };
                string body = JsonConvert.SerializeObject(bodyRequest);
                var responseCode = string.Empty;
                const int sleepTime = 1000;
                System.Threading.Thread.Sleep(sleepTime);
                var resultDetail = SendPOSTRequestJsonSimple(requestUrl, body, bindingIpRequest, ref responseCode, headers, "application/json; charset=utf-8");
                detailStatusText = resultDetail;
                //Logger.CommonLogger.PaymentLogger.DebugFormat("WEmoneyHelper -- CheckoutInfoSendingMoneyResponse -- wingSenderID: {0} -- Url: {1} -- Result: {2}",
                //  walletId, requestUrl, resultDetail);


                if (string.IsNullOrEmpty(resultDetail))
                {
                    wHelperDetailStatusCode = WHelperDetailStatusCode.EmoneyCheckoutInfoEmptyResponse;
                    return false;
                }
                if (!string.IsNullOrEmpty(responseCode) && responseCode != "200")
                {
                    wHelperDetailStatusCode = WHelperDetailStatusCode.EmoneyCheckoutInfoHttpStatusNotOK;
                    message = "response code is not OK";
                    return false;
                }

                var billInfoResponse = JsonConvert.DeserializeObject<GetBillInfoEmoneyResponse>(resultDetail);
                if (billInfoResponse == null)
                {
                    wHelperDetailStatusCode = WHelperDetailStatusCode.EmoneyCheckoutInfoParseDataNull;
                    return false;

                }
                transferEMoneyResponseModel = new TransferEMoneyResponseModel();
                transferEMoneyResponseModel = ConvertToTransferEMoneyResponseModel(billInfoResponse);
                if(transferEMoneyResponseModel == null)
                {
                    wHelperDetailStatusCode = WHelperDetailStatusCode.EmoneyCheckoutInfoConvertDataNull;
                    return false;
                }
                wHelperDetailStatusCode = WHelperDetailStatusCode.Success;

                return true;
            }
            catch (Exception exp)
            {
                //  Logger.CommonLogger.PaymentLogger.DebugFormat("WEmoneyHelper -- CheckoutInfoSendingMoneyResponse -- wingSenderID: {0} -- exp: {1}",
                //   walletId, exp);
                wHelperDetailStatusCode = WHelperDetailStatusCode.WalletExceptionError;
                detailStatusText = exp.ToString();
                return false;
            }
        }

        private static bool ConfirmSendingEmoneyWallet(string walletId, string transId, string accessToken, string bindingIpRequest, string walletDeviceId, string jsonEnvInfo, ref string message, ref string errorCode, ref TransferEMoneyResponseModel confirmTransferEMoneyResponseModel, ref WHelperDetailStatusCode wHelperDetailStatusCode, ref string detailStatusText)
        {
            try
            {                
                var headers = GetCommonHeader(jsonEnvInfo, walletId, walletDeviceId, accessToken);
                var requestUrl = string.Format("{0}{1}", EMONEY_API_URL, EMONEY_API_CONFIRMSENDMONEYINFO);
                if (string.IsNullOrEmpty(transId))
                {
                    message = "transId is null.";
                    return false;

                }
                var bodyRequest = new ConfirmSendingEmoneyWalletRequest
                {
                    TransactionId = transId
                };
                string body = JsonConvert.SerializeObject(bodyRequest);
                var responseCode = string.Empty;
                const int sleepTime = 1000;
                System.Threading.Thread.Sleep(sleepTime);
                var resultDetail = SendPOSTRequestJsonSimple(requestUrl, body, bindingIpRequest, ref responseCode, headers, "application/json; charset=utf-8");
                detailStatusText = resultDetail;

                //Logger.CommonLogger.PaymentLogger.DebugFormat("WEmoneyHelper -- ConfirmSendingEmoneyWallet -- wingSenderID: {0} -- Url: {1} -- Result: {2}",
                //  walletId, requestUrl, resultDetail);


                if (string.IsNullOrEmpty(resultDetail))
                {
                    wHelperDetailStatusCode = WHelperDetailStatusCode.EmoneyConfirmSendingEmptyResponse;
                    return false;

                }

                if (!string.IsNullOrEmpty(responseCode) && responseCode != "200")
                {
                    wHelperDetailStatusCode = WHelperDetailStatusCode.EmoneyConfirmSendingHttpStatusNotOK;
                    message = "response code is not OK";
                    return false;
                }

                var confirmSendingMoneyResponse = JsonConvert.DeserializeObject<GetBillInfoEmoneyResponse>(resultDetail);
                if (confirmSendingMoneyResponse == null)
                {
                    wHelperDetailStatusCode = WHelperDetailStatusCode.EmoneyConfirmSendingParseDataNull;
                    return false;

                }
                confirmTransferEMoneyResponseModel = new TransferEMoneyResponseModel();
                confirmTransferEMoneyResponseModel = ConvertToTransferEMoneyResponseModel(confirmSendingMoneyResponse);
                if (confirmTransferEMoneyResponseModel == null)
                {
                    wHelperDetailStatusCode = WHelperDetailStatusCode.EmoneyConfirmSendingConvertDataNull;
                    return false;
                }

                wHelperDetailStatusCode = WHelperDetailStatusCode.Success;
                return true;
            }
            catch (Exception exp)
            {
                //  Logger.CommonLogger.PaymentLogger.DebugFormat("WEmoneyHelper -- ConfirmSendingEmoneyWallet -- wingSenderID: {0} -- exp: {1}",
                //   walletId, exp);
                wHelperDetailStatusCode = WHelperDetailStatusCode.WalletExceptionError;
                detailStatusText = exp.ToString();
                return false;
            }
        }

        #endregion
        #endregion


        #region login
        private static WEmoneyLoginResponseModel ConvertToWEmoneyLoginResponseModel(GetAccountInfoLoginResponse getAccountInfoLoginResponse)
        {
            if (getAccountInfoLoginResponse == null || getAccountInfoLoginResponse.UserInfo == null)
                return null;
            var walletId = getAccountInfoLoginResponse.UserInfo.WalletId;

            if (string.IsNullOrEmpty(walletId))
                return null;

            if (walletId.StartsWith("+855"))
                walletId = "0" + walletId.Substring(4);

            if (walletId.StartsWith("855"))
                walletId = "0" + walletId.Substring(3);

            var walletName = string.IsNullOrEmpty(getAccountInfoLoginResponse.UserInfo.WalletName) ? "" : getAccountInfoLoginResponse.UserInfo.WalletName;
            var idAccountNumber = string.IsNullOrEmpty(getAccountInfoLoginResponse.UserInfo.IdNumber) ? "" : getAccountInfoLoginResponse.UserInfo.IdNumber;
            var birthDay = string.IsNullOrEmpty(getAccountInfoLoginResponse.UserInfo.BirthDay) ? "" : getAccountInfoLoginResponse.UserInfo.BirthDay;
            var accessToken = string.IsNullOrEmpty(getAccountInfoLoginResponse.AccessToken) ? "" : getAccountInfoLoginResponse.AccessToken;

            var loginResponseModel = new WEmoneyLoginResponseModel
            {
                WalletId = walletId,
                WalletName = walletName,
                IdAccountNumber = idAccountNumber,
                Gender = getAccountInfoLoginResponse.UserInfo.Gender.GetValueOrDefault(0),
                BirthDay = birthDay,
                AccessToken = accessToken
            };

            return loginResponseModel;
        }

        private static bool LoginWithId(string walletId, string pin, string otp, string accessToken, string bindingIpRequest, string walletDeviceId, string jsonEnvInfo, ref string errorCode, ref string message, ref GetAccountInfoLoginResponse getAccountInfoLoginResponse)
        {
            try
            {
                var headers = GetCommonHeader(jsonEnvInfo, walletId, walletDeviceId, accessToken);
                var requestUrl = string.Format("{0}{1}", EMONEY_API_URL, EMONEY_API_LOGIN_URL);

                const int sleepTime = 1000;
                System.Threading.Thread.Sleep(sleepTime);

                var bodyRequest = new LoginBodyRequest
                {
                    WalletId = walletId,
                    Pin = pin,
                    Otp = otp
                };
                string body = JsonConvert.SerializeObject(bodyRequest);
                var resultDetail = SendPOSTRequestJsonSimple(requestUrl, body, bindingIpRequest, ref errorCode, headers, "application/json; charset=utf-8");

                //Logger.CommonLogger.PaymentLogger.DebugFormat("WEmoneyHelper -- LoginWithId -- wingSenderID: {0} -- Url: {1} -- Result: {2}",
                //  walletId, requestUrl, resultDetail);

                message = resultDetail;

                if (string.IsNullOrEmpty(resultDetail))
                    return false;

                var loginAccountInfoResult = JsonConvert.DeserializeObject<GetAccountInfoLoginResponse>(resultDetail);
                if (loginAccountInfoResult == null)
                    return false;

                if (loginAccountInfoResult.RequireOtp == EMONEY_REQUIRE_OTP || loginAccountInfoResult.UserInfo == null)
                {
                    message = "---LoginWithId--- Required Otp: " + walletId;
                    return false;
                }

                getAccountInfoLoginResponse = loginAccountInfoResult;

                return true;
            }
            catch (Exception exp)
            {
                //  Logger.CommonLogger.PaymentLogger.DebugFormat("WEmoneyHelper -- LoginWithId -- wingSenderID: {0} -- exp: {1}",
                //   walletId, exp);
                return false;
            }
        }
        /*
        private static bool LoginWithOTP(string walletId, string pin,string otp, string accessToken, string bindingIpRequest, string walletDeviceId, string jsonEnvInfo, ref string errorCode, ref string message, ref GetAccountInfoLoginResponse getAccountInfoLoginResponse)
        {
            try
            {
                var headers = GetCommonHeader(jsonEnvInfo, walletId, walletDeviceId, accessToken);
                var requestUrl = string.Format("{0}{1}", EMONEY_API_URL, EMONEY_API_LOGIN_URL);

                const int sleepTime = 1000;
                System.Threading.Thread.Sleep(sleepTime);

                var bodyRequest = new LoginBodyRequest
                {
                    WalletId = walletId,
                    Pin = pin,
                    Otp= otp
                };
                string body = JsonConvert.SerializeObject(bodyRequest);
                var resultDetail = SendPOSTRequestJsonSimple(requestUrl, body, bindingIpRequest, ref errorCode, headers, "application/json; charset=utf-8");

                //Logger.CommonLogger.PaymentLogger.DebugFormat("WEmoneyHelper -- LoginWithId -- wingSenderID: {0} -- Url: {1} -- Result: {2}",
                //  walletId, requestUrl, resultDetail);

                message = resultDetail;

                if (string.IsNullOrEmpty(resultDetail))
                    return false;

                var loginAccountInfoResult = JsonConvert.DeserializeObject<GetAccountInfoLoginResponse>(resultDetail);
                if (loginAccountInfoResult == null || loginAccountInfoResult.UserInfo == null )
                    return false;

                getAccountInfoLoginResponse = loginAccountInfoResult;
                return true;               
            }
            catch (Exception exp)
            {
                //  Logger.CommonLogger.PaymentLogger.DebugFormat("WEmoneyHelper -- LoginWithId -- wingSenderID: {0} -- exp: {1}",
                //   walletId, exp);
                return false;
            }
        }
       */
        private static bool EmoneyGetOtp(string walletId, string accessToken, string bindingIpRequest, string walletDeviceId, string jsonEnvInfo,
            ref string message, ref string errorCode, ref WEmoneyBaseResponse wEmoneyBaseResponse)
        {
            try
            {
                if (string.IsNullOrEmpty(walletId)) return false;
                if (walletId.StartsWith("+855"))
                    walletId = "0" + walletId.Substring(4);

                if (walletId.StartsWith("855"))
                    walletId = "0" + walletId.Substring(3);

                var headers = GetCommonHeader(jsonEnvInfo, walletId, walletDeviceId, accessToken);
                var requestUrl = string.Format("{0}{1}{2}", EMONEY_API_URL, EMONEY_API_GET_OTP_URL, walletId);

                const int sleepTime = 1000;
                System.Threading.Thread.Sleep(sleepTime);

                var resultDetail = SendGETRequestJsonSimple(requestUrl, "", bindingIpRequest, ref errorCode, headers, "application/json; charset=utf-8");

                //Logger.CommonLogger.PaymentLogger.DebugFormat("WEmoneyHelper -- EmoneyGetOtp -- wingSenderID: {0} -- Url: {1} -- Result: {2}",
                //  walletId, requestUrl, resultDetail);

                message = resultDetail;

                if (string.IsNullOrEmpty(resultDetail))
                    return false;

                if (!string.IsNullOrEmpty(errorCode) && errorCode != "200")
                {
                    message = "response code is not OK";
                    return false;
                }

                var getOtpResults = JsonConvert.DeserializeObject<WEmoneyBaseResponse>(resultDetail);
                if (getOtpResults == null || !getOtpResults.IsSuccess)
                    return false;
                wEmoneyBaseResponse = getOtpResults;
                return true;
            }
            catch (Exception exp)
            {
                //  Logger.CommonLogger.PaymentLogger.DebugFormat("WEmoneyHelper -- EmoneyGetOtp -- wingSenderID: {0} -- exp: {1}",
                //   walletId, exp);
                return false;
            }
        }
        #endregion


        #region balance

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

        #endregion


        #region detail

        #endregion


        #region transaction

        public static List<TransactionDetailModel> GetWalletTransactionWithAccessToken(string walletId, DateTime dateFrom, int groupId, int page, DateTime dateTo, string accessToken, string bindingIpRequest, string walletDeviceId, string jsonEnvInfo)
        {
            string message = "", errorCode = "";
            if (string.IsNullOrEmpty(walletId) || string.IsNullOrEmpty(walletDeviceId) || string.IsNullOrEmpty(jsonEnvInfo) || string.IsNullOrEmpty(accessToken) ||
                dateFrom > dateTo)
                return null;

            var dateFromString = dateFrom.ToString("dd/MM/yyyy");
            var dateToString = dateTo.ToString("dd/MM/yyyy");
            var listTransactionDetailModels = new List<TransactionDetailModel>();
            var result = GetListWalletTransactionsWithId(walletId, dateFromString, groupId, page, dateToString, accessToken, bindingIpRequest, walletDeviceId, jsonEnvInfo, ref message, ref errorCode, ref listTransactionDetailModels);

            if (!result || listTransactionDetailModels == null || !listTransactionDetailModels.Any()) return null;

            return listTransactionDetailModels;

        }


        public static List<TransactionDetailModel> GetWalletTransactionPageToPageWithAccessToken(string walletId, DateTime dateFrom, int groupId, int pageIndex, int maxPage, DateTime dateTo, string lastBankTransId, string accessToken, string bindingIpRequest, string walletDeviceId, string jsonEnvInfo)
        {
            string message = "", errorCode = "";
            if (string.IsNullOrEmpty(walletId) || string.IsNullOrEmpty(walletDeviceId) || string.IsNullOrEmpty(jsonEnvInfo) || string.IsNullOrEmpty(accessToken) ||
                dateFrom > dateTo)
                return null;

            var dateFromString = dateFrom.ToString("dd/MM/yyyy");
            var dateToString = dateTo.ToString("dd/MM/yyyy");
            if (pageIndex < 1) pageIndex = 1;
            if (maxPage < 1) maxPage = 1;
            var listTransactionDetailModels = new List<TransactionDetailModel>();

            try
            {

                const int sleepTime = 1000;
                for (; pageIndex<=maxPage; pageIndex++)
                {
                    if (pageIndex > 1)
                        System.Threading.Thread.Sleep(sleepTime);

                    var listTransactionDetailResponse = new List<TransactionDetailModel>();
                    var getListTransactionWithPageResults = GetListWalletTransactionsWithId(walletId, dateFromString, groupId, pageIndex, dateToString, accessToken, bindingIpRequest, walletDeviceId, jsonEnvInfo, ref message, ref errorCode, ref listTransactionDetailResponse);


                    if (!getListTransactionWithPageResults || listTransactionDetailResponse == null || !listTransactionDetailResponse.Any())
                        break;

                    if (listTransactionDetailModels == null)
                        listTransactionDetailModels = new List<TransactionDetailModel>();
                    listTransactionDetailModels.AddRange(listTransactionDetailResponse);

                    if (!string.IsNullOrEmpty(lastBankTransId))
                    {
                        // Nếu tồn tại last tid thì dừng không quét nữa
                        var transInfo = listTransactionDetailResponse.FirstOrDefault(t => t.TransactionId.ToString() == lastBankTransId);
                        if (transInfo != null)
                            break;
                    }
                }

                if (listTransactionDetailModels == null || !listTransactionDetailModels.Any()) return null;

                return listTransactionDetailModels;
            }
            catch(Exception ex)
            {
                //  Logger.CommonLogger.PaymentLogger.DebugFormat("WEmoneyHelper -- GetWalletTransactionPageToPageWithAccessToken -- wingSenderID: {0} -- exp: {1}",
                //   walletId, exp);
                return null;
            }
         
            

        }



        #endregion


        #region transfer
        public static TransferEMoneyResponseModel SendingMoneyEMoneyWallet(string walletId, string amount, string content, int currency, int option, string pin, string destWalletId, string accessToken, string bindingIpRequest, string walletDeviceId, string jsonEnvInfo,ref string message, ref WHelperDetailStatusCode detailStatusCode, ref string detailStatusText)
        {
          
            try
            {
                string errorCode = "";                               
                if (string.IsNullOrEmpty(walletId) || string.IsNullOrEmpty(destWalletId) || string.IsNullOrEmpty(walletDeviceId) || string.IsNullOrEmpty(jsonEnvInfo) || string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(amount) )
                {

                    detailStatusCode = WHelperDetailStatusCode.InputDataInvalid;
                    message = "walletId || destWalletId || amount || walletDeviceId  || jsonEnvInfo || accessToken  null or empty ";
                    return null;
                }
                if(walletId == destWalletId)
                {
                    message = "WalletId and DestWalletId is the same ";
                    detailStatusCode = WHelperDetailStatusCode.InputDataInvalid;
                    return null;
                }
               
                TransferEMoneyResponseModel transferEMoneyResponseModel = new TransferEMoneyResponseModel();
              
                var checkoutInfoSendingMoneyResult = CheckoutInfoSendingMoneyResponse(walletId, amount, content, currency, option, pin, destWalletId, accessToken, bindingIpRequest, walletDeviceId, jsonEnvInfo, ref message, ref errorCode, ref transferEMoneyResponseModel, ref detailStatusCode, ref  detailStatusText);
                if (!checkoutInfoSendingMoneyResult) return null;

                if (string.IsNullOrEmpty(transferEMoneyResponseModel.TransactionId))
                {

                    detailStatusCode = WHelperDetailStatusCode.CheckoutInfoGenTransIdEmpty;
                    message = "Do not generate TransactionID --CheckoutInfoSendingMoneyResponse -- wingSenderID :  " + walletId;
                    return null;
                }
                var transId = transferEMoneyResponseModel.TransactionId;
                var confirmSendingMoneyResult = ConfirmSendingEmoneyWallet(walletId, transId, accessToken, bindingIpRequest, walletDeviceId, jsonEnvInfo, ref message, ref errorCode, ref transferEMoneyResponseModel, ref  detailStatusCode, ref  detailStatusText);

                if (!confirmSendingMoneyResult)
                {
                    message = "Confirm Sending Money Response is null  ";
                    

                    return null;
                }

                return transferEMoneyResponseModel;
            }
            catch (Exception exp)
            {
                //Logger.CommonLogger.PaymentLogger.DebugFormat("WEmoneyHelper -- SendingMoneyEMoneyWallet -- wingSenderID: {0} -- exp: {1}",
                // walletId, exp);
                detailStatusCode = WHelperDetailStatusCode.WalletExceptionError;
                detailStatusText = exp.ToString();
                return null;
            }
        }
        #endregion

        #region login 
        public static WEmoneyLoginResponseModel LoginEmoneyWalletWithId(string walletId, string pin, string otp,  string accessToken, string bindingIpRequest, string walletDeviceId, string jsonEnvInfo)
        {
            try
            {
                string message = "", errorCode = "";

                if (string.IsNullOrEmpty(walletId) || string.IsNullOrEmpty(walletDeviceId) || string.IsNullOrEmpty(jsonEnvInfo) || string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(pin))
                    return null;
                GetAccountInfoLoginResponse getAccountInfoLoginDetailResponse = new GetAccountInfoLoginResponse();
                var loginResponseResult = LoginWithId(walletId, pin, otp, accessToken, bindingIpRequest, walletDeviceId, jsonEnvInfo, ref errorCode, ref message, ref getAccountInfoLoginDetailResponse);

                if (!loginResponseResult)
                    return null;
                var loginResponseModel = ConvertToWEmoneyLoginResponseModel(getAccountInfoLoginDetailResponse);
                if (loginResponseModel == null)
                    return null;


                return loginResponseModel;


            }
            catch (Exception exp)
            {
                //Logger.CommonLogger.PaymentLogger.DebugFormat("WEmoneyHelper -- LoginEmoneyWalletWithId -- wingSenderID: {0} -- exp: {1}",
                // walletId, exp);
                return null;
            }
        }

        public static WEmoneyBaseResponse RequestOtp(string walletId, string accessToken, string bindingIpRequest, string walletDeviceId, string jsonEnvInfo)
        {
            try
            {
                string message = "", errorCode = "";

                if (string.IsNullOrEmpty(walletId) || string.IsNullOrEmpty(walletDeviceId) || string.IsNullOrEmpty(jsonEnvInfo) || string.IsNullOrEmpty(accessToken))
                    return null;
                WEmoneyBaseResponse requestOtpResponse = new WEmoneyBaseResponse();
                var requestOtpResult = EmoneyGetOtp(walletId, accessToken, bindingIpRequest, walletDeviceId, jsonEnvInfo, ref errorCode, ref message, ref requestOtpResponse);

                if (!requestOtpResult)
                    return null;
              
                if (requestOtpResponse == null)
                    return null;

                if (!requestOtpResponse.IsSuccess)
                    return null;

                return requestOtpResponse;


            }
            catch (Exception exp)
            {
                //Logger.CommonLogger.PaymentLogger.DebugFormat("WEmoneyHelper -- RequestOtp -- wingSenderID: {0} -- exp: {1}",
                // walletId, exp);
                return null;
            }
        }

        

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

        [JsonProperty("balance")]
        public string Balance { get; set; }

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


    #region Transaction History

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
        public TransactionsDetailResponse Transactions { get; set; }

    }

    public class TransactionsDetailResponse
    {
        [JsonProperty("totalRecords")]

        public int TotalRecords { get; set; }

        [JsonProperty("totalPages")]
        public int TotalPages { get; set; }


        [JsonProperty("currentPage")]
        public int CurrentPage { get; set; }

        [JsonProperty("pageSize")]
        public int PageSize { get; set; }

        [JsonProperty("results")]
        public List<TransactionDetailDtoResponse> Results { get; set; }
    }

    public class TransactionDetailDtoResponse
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

        [JsonProperty("transCustName")]
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
    public class TransactionDetailModel
    {
        /// <summary>
        /// ID Ví nạp/gởi
        /// </summary>
        [JsonProperty("wallet_id")]
        public string WalletId { get; set; }

        /// <summary>
        /// Tên tài khoản Ví nạp/gởi
        /// </summary>
        [JsonProperty("wallet_name")]
        public string WalletAccountName { get; set; }

        /// <summary>
        /// ID Ví nhận
        /// </summary> 
        [JsonProperty("dest_wallet_id")]
        public string DestWalletId { get; set; }

        /// <summary>
        /// Tên tài khoản Ví nhận
        /// </summary>
        [JsonProperty("dest_wallet_name")]
        public string DestWalletAccountName { get; set; }

        /// <summary>
        /// Mệnh giá nạp
        /// </summary>
        [JsonProperty("amount")]
        public decimal DepositAmount { get; set; }


        /// <summary>
        /// Là nạp hay rút (Deposit, Withdraw, ...)
        /// </summary>
        [JsonProperty("trans_type")]
        public string TransactionType { get; set; }


        /// <summary>
        /// Mã giao dịch
        /// </summary>
        [JsonProperty("trans_id")]
        public long TransactionId { get; set; }

        /// <summary>
        /// Thời gian giao dịch
        /// </summary>
        [JsonProperty("trans_date")]
        public long TransTime { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        /// 
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Chuỗi json từ Emoney
        /// </summary>
        [JsonProperty("json_data")]
        public string JsonData { get; set; }

        /// <summary>
        /// Đơn vị tiền tệ
        /// </summary>

        [JsonProperty("currency_code")]
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Thời gian tạo ở DB hệ thống
        /// </summary>

        [JsonProperty("create_date")]
        public DateTime CreateDate { get; set; }
    }




    #endregion

    #endregion

    #region transfer

    #region request
    public class GetBillInfoEmoneyBodyRequest
    {
        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("currency")]
        public int Currency { get; set; }

        [JsonProperty("option")]
        public int Option { get; set; }

        [JsonProperty("pin")]
        public string Pin { get; set; }

        [JsonProperty("receiverMsisdn")]
        public string ReceiverMsisdn { get; set; }
    }

    public class ConfirmSendingEmoneyWalletRequest
    {
        [JsonProperty("transId")]
        public string TransactionId { get; set; }
    }
    #endregion

    #region response
    public class GetBillInfoEmoneyResponse : WEmoneyBaseResponse
    {
        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("fee")]
        public string Fee { get; set; }

        [JsonProperty("totalAmount")]
        public string TotalAmount { get; set; }

        [JsonProperty("commission")]
        public string Commission { get; set; }

        [JsonProperty("senderMsisdn")]
        public string SenderMsisdn { get; set; }

        [JsonProperty("receiverMsisdn")]
        public string ReceiverMsisdn { get; set; }

        [JsonProperty("receiverName")]
        public string ReceiverName { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("privateCode")]
        public string PrivateCode { get; set; }
    }

    public class TransferEMoneyResponseModel
    {
        [JsonProperty("status")]
        public int StatusCode { get; set; }

        [JsonProperty("transaction_id")]
        public string TransactionId { get; set; }

        [JsonProperty("currency")]
        public int Currency { get; set; }

        [JsonProperty("balance")]
        public decimal Balance { get; set; }

        [JsonProperty("transaction_time")]
        public long TransactionTime { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("fee")]
        public decimal Fee { get; set; }

        [JsonProperty("total_amount")]
        public decimal TotalAmount { get; set; }

        [JsonProperty("wallet_id")]
        public string WalletId { get; set; }

        [JsonProperty("dest_wallet_id")]
        public string DesWalletId { get; set; }

        [JsonProperty("dest_wallet_name")]
        public string DestWalletName { get; set; }

    }

    #endregion

    #endregion

    #region Login
    #region Request
    public class LoginBodyRequest
    {
        [JsonProperty("msisdn")]
        public string  WalletId { get; set; }

        [JsonProperty("pin")]
        public string  Pin { get; set; }

        [JsonProperty("otp", NullValueHandling = NullValueHandling.Ignore)]       
        public string  Otp { get; set; }
    }
    #endregion

    #region Response
    public class GetAccountInfoLoginResponse : WEmoneyBaseResponse
    {
        [JsonProperty("notificationTopics")]
        public List<string> NotificationTopic { get; set; }

        [JsonProperty("userInfo")]
        public GetAccountInfoLoginDetailResponse UserInfo { get; set; }

        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        [JsonProperty("verifyOwner")]
        public int VerifyOwner { get; set; }
    }
    public class GetAccountInfoLoginDetailResponse
    {

        [JsonProperty("msisdn")]
        public string WalletId { get; set; }

        [JsonProperty("name")]
        public string WalletName { get; set; }

        [JsonProperty("gender")]
        public int? Gender { get; set; }

        [JsonProperty("accountType")]
        public int AccountType { get; set; }

        [JsonProperty("accountStatus")]
        public int AccountStatus { get; set; }

        [JsonProperty("idType")]
        public string IdType { get; set; }

        [JsonProperty("idNumber")]
        public string IdNumber { get; set; }

        [JsonProperty("idIssueDate")]
        public string IdIssueDate { get; set; }

        [JsonProperty("idIssuePlace")]
        public string IdIssuePlace { get; set; }

        [JsonProperty("usdBalance")]
        public string USDBalance { get; set; }

        [JsonProperty("khrBalance")]
        public string KHRBalance { get; set; }

        [JsonProperty("otpType")]
        public int OtpType { get; set; }

        [JsonProperty("accountApprovalStatus")]
        public int AccountApprovalStatus { get; set; }

        [JsonProperty("birthday")]
        public string BirthDay { get; set; }
    }
    public class WEmoneyLoginResponseModel
    {


        [JsonProperty("wallet_id")]
        public string WalletId { get; set; }

        [JsonProperty("wallet_name")]
        public string WalletName { get; set; }

        [JsonProperty("gender")]
        public int Gender { get; set; }

        [JsonProperty("id_account_number")]
        public string IdAccountNumber { get; set; }

        [JsonProperty("birth_day")]
        public string BirthDay { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

    }

    #endregion

    #endregion


    #endregion
}
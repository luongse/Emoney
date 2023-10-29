
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;

namespace WebApplication3.WalletHelper
{
    
        public static class EnumExtension
        {
            public static T ToEnum<T>(this object enumString)
            {
                return (T)Enum.Parse(typeof(T), enumString.ToString());
            }
        }
    public class EmoneyDoBlockWallet
    {
       // public const string EMONEY_APP_LANGUAGE = "en";
        public const string EMONEY_APP_HEADER_INFO_KEY = "e-info";
        public const string EMONEY_APP_HEADER_LANGUAGE_KEY = "e-language";
        public const string EMONEY_APP_HEADER_AUTHORIZATION_KEY = "Authorization";

        public const string EMONEY_ACCOUNT_NOT_FOUND_MESSAGE = "Account not found";
        public const string EMONEY_ACCOUNT_NOT_FOUND_CODE = "ERR_ACCOUNT_NOT_FOUND";

        public const string EMONEY_LOGIN_INVALID_OTP_MESSAGE = "Login OTP invalid";
        public const string EMONEY_LOGIN_INVALID_OTP_CODE = "ERR_LOGIN_OTP_INVALID";

        public const string EMONEY_LOGIN_OVER_LIMIT_INVALID_OTP_MESSAGE = "requested OTP beyond the allowed limit";
        public const string EMONEY_LOGIN_OVER_LIMIT_INVALID_OTP_CODE = "ERR_RESEND_OTP_OVER_LIMITED";

        public const string EMONEY_LOGIN_INFORMATION_INVALID__MESSAGE = "Login information invalid";
        public const string EMONEY_LOGIN_INFORMATION_INVALID_CODE = "ERR_LOGIN_INFORMATION_INVALID";

        public const string EMONEY_LOGIN_ERR_LOGIN_CHANGE_DEVICE_REQUIRED_OTP_MESSAGE = "please confirm by OTP to complete";
        public const string EMONEY_LOGIN_ERR_LOGIN_CHANGE_DEVICE_REQUIRED_OTP_CODE = "ERR_LOGIN_CHANGE_DEVICE_REQUIRED_OTP";

        public const string EMONEY_LOGIN_ACCOUNT_BLOCK_MESSAGE = "locked";
        public const string EMONEY_LOGIN__ACCOUNT_BLOCK_CODE = "ERR_LOGIN_ACCOUNT_WAS_LOCKED";

       



        public const string EMONEY_DO_BLOCK_STATUS_DETAIL_SUCCESS = "Account has been blocked.";
        public const string EMONEY_DO_BLOCK_STATUS_DETAIL_FAIL = "Account block fail.";
        public const string EMONEY_DO_BLOCK_STATUS_DETAIL_NOT_REGISTER = "Phone number do not register";
        public const string EMONEY_DO_BLOCK_STATUS_DETAIL_NOT_CORRECT_INFOMATION = "Input Phone number is not correc!";

        public const string EMONEY_API_URL = "https://mg.emoney.com.kh:8686/v2";
        public const string EMONEY_APP_OS_NAME = "Android";
        public const int EMONEY_APP_OS_VERSION = 33;
        public const string EMONEY_PHONE_NAME = "sdk_gphone_x86_64";
        public const string EMONEY_API_LOGIN_URL = "/auth/login";
        public const string EMONEY_API_LOGIN_GET_OTP_URL = "/auth/otp/1";
        public const string EMONEY_PHONE_DEVICE_ID = "39415d3e-9ff8-4298-a9de-677d1044540f";
        public const string contentType = "application/json; charset=UTF-8";
        public const string EMONEY_VERSION_APP = "3.6.6";
        public const string eLanguage = "en";

        public static List<DoBlockWalletResult> DoBlock(List<string> phoneNumbers,  bool isWindowConsole)
        {
           
            //var deviceID = new Guid().ToString();
            //string emoneyInfo = EMONEY_APP_OS_NAME + ";" + EMONEY_APP_OS_VERSION + ";" + EMONEY_PHONE_NAME + ";" + "1" + ";" + deviceID + ";" + EMONEY_VERSION_APP;
            var listFinalResult = new List<DoBlockWalletResult>();
            foreach (var phone in phoneNumbers)
            {

                #region Old
                //Request Login Lần đầu tiên
                //var requestUrlLogin = string.Format("{0}{1}", EMONEY_API_URL, EMONEY_API_LOGIN_URL);

                //var bodyRequest = new LoginBodyRequest
                //{
                //    WalletId = phone,
                //    Pin = "111111"
                //};
                //string body = JsonConvert.SerializeObject(bodyRequest);
                //var byteContent = new StringContent(body, Encoding.UTF8, "application/json");
                //var token = "P5puUi8ZLN/dL46/Bt/ZP74Dc09u2fJEzQmnoAgbgw3Jg6T4B4hJfxcNwKgYQLJdq0kjU2eQWsD2Krjm3fGWp41SaqBtRyoFDloRTK3ra5qfcS9aX1DPoorECdctQ/XRNvt/PssYXLsHTCK7s7xNJ8oTAAMXHVhVZuVBsGY8Q2FDZ4A4gvyGrDT8WS2QkbFlNL/hTFCvleKW0K5E+OZuWxiIWn1ILfYQD3xGl1yRagsTZhiTxYunfCfQ90GyY8/hpQ5E+PW0n8Esm9nClyYZvcT7K1oa/tPaPC8PoWArXNzUWtDr49TZF6PADVtQRaRunfRBGJ/o2M6FNuHchWlnfw==";
                //using (var client = new HttpClient())
                //{
                //    var header = new Dictionary<string, string>
                //    {
                //        {EMONEY_APP_HEADER_INFO_KEY, emoneyInfo},
                //        {EMONEY_APP_HEADER_LANGUAGE_KEY, eLanguage},
                //        {EMONEY_APP_HEADER_AUTHORIZATION_KEY, token},
                //        {"Content-Type", contentType}
                //    };

                //    //Thêm header
                //    foreach (var headerKey in header.Keys)
                //    {
                //        client.DefaultRequestHeaders.TryAddWithoutValidation(headerKey, header[headerKey]);
                //    }

                //    client.BaseAddress = new Uri(requestUrlLogin);
                //    var response = client.PostAsync(requestUrlLogin, byteContent).Result;
                //    //Result login lần đầu tiên
                //    var firstLoginResult = JsonConvert.DeserializeObject<WEmoneyBaseResponse>(response.Content.ReadAsStringAsync().Result);


                //    //Check số điện thoại hợp lệ
                //    if (!firstLoginResult.IsSuccess && firstLoginResult.ErrorCode.ToUpper().Contains(EMONEY_LOGIN_INFORMATION_INVALID_CODE) || firstLoginResult.ErrorMessage.ToUpper().Contains(EMONEY_LOGIN_INFORMATION_INVALID__MESSAGE.ToUpper()))
                //    {
                //        listFinalResult.Add(new DoBlockWalletResult
                //        {
                //            PhoneNumber = phone,
                //            IsBlock = false,
                //            DetailStatus = EMONEY_DO_BLOCK_STATUS_DETAIL_NOT_REGISTER
                //        });
                //        continue;
                //    }
                //    //Check tài khoản ví có tồn tại không
                //    if (!firstLoginResult.IsSuccess && firstLoginResult.ErrorCode.ToUpper().Contains(EMONEY_ACCOUNT_NOT_FOUND_CODE) || firstLoginResult.ErrorMessage.ToUpper().Contains(EMONEY_ACCOUNT_NOT_FOUND_MESSAGE.ToUpper()))
                //    {
                //        listFinalResult.Add(new DoBlockWalletResult
                //        {
                //            PhoneNumber = phone,
                //            IsBlock = false,
                //            DetailStatus = EMONEY_DO_BLOCK_STATUS_DETAIL_NOT_REGISTER
                //        });
                //        continue;
                //    }

                //startFirstTestLogin:
                //    //Check tài khoản login bị fail lần đầu 
                //    if (!firstLoginResult.IsSuccess && firstLoginResult.ErrorCode.ToUpper().Contains(EMONEY_LOGIN_INVALID_OTP_CODE) || firstLoginResult.ErrorMessage.ToUpper().Contains(EMONEY_LOGIN_INVALID_OTP_MESSAGE.ToUpper()))
                //    {
                //        //Gọi get Otp 
                //        var getOtpUrl = EMONEY_API_URL + EMONEY_API_LOGIN_GET_OTP_URL + "/" + phone;

                //        var getOtpFirstTimeResult = GetApiAsync<object>(getOtpUrl, header);

                //        //Xong gọi lại login, nhưng có kèm otp param
                //        for (int i = 0; i <= 3; i++)
                //        {
                //            var bodyRequestLoginAgain = new LoginBodyRequest
                //            {
                //                WalletId = phone,
                //                Pin = "111111",
                //                Otp = "1111"
                //            };

                //            var myContent = JsonConvert.SerializeObject(bodyRequestLoginAgain);
                //            var byteContentLoginAgain = new StringContent(myContent, Encoding.UTF8, "application/json");
                //            var loginAgainResult = PostApiAsync<object>(requestUrlLogin, byteContentLoginAgain, header);


                //            if (!loginAgainResult.IsSuccess && loginAgainResult.ErrorCode.ToUpper().Contains(EMONEY_LOGIN_INVALID_OTP_CODE) || loginAgainResult.ErrorMessage.ToUpper().Contains(EMONEY_LOGIN_INVALID_OTP_MESSAGE.ToUpper()))
                //            {
                //                var getOtpContinuosResult = GetApiAsync<object>(getOtpUrl, header);

                //            }
                //            else if (!loginAgainResult.IsSuccess && loginAgainResult.ErrorCode.ToUpper().Contains(EMONEY_LOGIN_OVER_LIMIT_INVALID_OTP_CODE) || loginAgainResult.ErrorMessage.ToUpper().Contains(EMONEY_LOGIN_OVER_LIMIT_INVALID_OTP_MESSAGE.ToUpper()))
                //            {
                //                const int sleepTimeRequestOtp = 130000;
                //                System.Threading.Thread.Sleep(sleepTimeRequestOtp);
                //                var getOtpContinuosResult = GetApiAsync<object>(getOtpUrl, header);
                //            }
                //            else
                //            {
                //                if (!loginAgainResult.IsSuccess && loginAgainResult.ErrorCode.ToUpper().Contains(EMONEY_LOGIN__ACCOUNT_BLOCK_CODE) || loginAgainResult.ErrorMessage.ToUpper().Contains(EMONEY_LOGIN_ACCOUNT_BLOCK_MESSAGE.ToUpper()))
                //                {
                //                    continue;
                //                }
                //            }

                //        }
                //        var continueLogin = PostApiAsync<object>(requestUrlLogin, byteContent, header);

                //        var loginInFinalStepResult = WEmoneyEnumLoginFail.ERR_LOGIN_OTP_INVALID;

                //        if (Enum.TryParse(continueLogin.ErrorCode, out loginInFinalStepResult))
                //        {
                //            switch (loginInFinalStepResult)
                //            {

                //                case WEmoneyEnumLoginFail.ERR_LOGIN_OTP_INVALID:
                //                    // Xử lý khi OTP không hợp lệ
                //                    listFinalResult.Add(new DoBlockWalletResult
                //                    {
                //                        PhoneNumber = phone,
                //                        IsBlock = false,
                //                        DetailStatus = EMONEY_LOGIN_INVALID_OTP_CODE
                //                    });
                //                    break;
                //                case WEmoneyEnumLoginFail.ERR_RESEND_OTP_OVER_LIMITED:
                //                    // Xử lý khi vượt quá số lần gửi lại OTP cho phép
                //                    listFinalResult.Add(new DoBlockWalletResult
                //                    {
                //                        PhoneNumber = phone,
                //                        IsBlock = false,
                //                        DetailStatus = EMONEY_LOGIN_OVER_LIMIT_INVALID_OTP_CODE
                //                    });
                //                    break;
                //                case WEmoneyEnumLoginFail.ERR_LOGIN_ACCOUNT_WAS_LOCKED:
                //                    listFinalResult.Add(new DoBlockWalletResult
                //                    {
                //                        PhoneNumber = phone,
                //                        IsBlock = true,
                //                        DetailStatus = EMONEY_LOGIN__ACCOUNT_BLOCK_CODE
                //                    });
                //                    break;
                //                default:
                //                    listFinalResult.Add(new DoBlockWalletResult
                //                    {
                //                        PhoneNumber = phone,
                //                        IsBlock = false,
                //                        DetailStatus = EMONEY_LOGIN_INVALID_OTP_CODE
                //                    });
                //                    break;
                //            }
                //        }
                //        else
                //        {
                //            listFinalResult.Add(new DoBlockWalletResult
                //            {
                //                PhoneNumber = phone,
                //                IsBlock = false,
                //                DetailStatus = continueLogin.ErrorMessage
                //            });
                //            break;
                //        }



                //    }
                //    else if (!firstLoginResult.IsSuccess && firstLoginResult.ErrorCode.ToUpper().Contains(EMONEY_LOGIN__ACCOUNT_BLOCK_CODE) || firstLoginResult.ErrorMessage.ToUpper().Contains(EMONEY_LOGIN_ACCOUNT_BLOCK_MESSAGE.ToUpper()))
                //    {
                //        continue;

                //    }
                //    else
                //    {
                //        goto startFirstTestLogin;
                //    }

                //}


                #endregion

                #region New 
                var deviceId = Guid.NewGuid().ToString();

                var doBlockEmoneyWalletResult = DoBlockSingleWalletEmoney(phone, deviceId, isWindowConsole);

                if(doBlockEmoneyWalletResult == null)
                {
                    doBlockEmoneyWalletResult = new DoBlockWalletResult
                    {
                        PhoneNumber = phone,
                        IsBlock = false,
                        DetailStatus = "Do Block Fail."
                    
                    };
                }

                listFinalResult.Add(doBlockEmoneyWalletResult);
                #endregion

            }
            return listFinalResult;
        }

        public static DoBlockWalletResult DoBlockSingleWalletEmoney(string phone, string deviceId, bool isWindowConsole)
        {

            if (string.IsNullOrEmpty(deviceId))
            {
                deviceId = new Guid().ToString();
            } 
            string emoneyInfo = EMONEY_APP_OS_NAME + ";" + EMONEY_APP_OS_VERSION + ";" + EMONEY_PHONE_NAME + ";" + "1" + ";" + deviceId + ";" + EMONEY_VERSION_APP;

            var requestUrlLogin = string.Format("{0}{1}", EMONEY_API_URL, EMONEY_API_LOGIN_URL);

            var bodyRequest = new LoginBodyRequest
            {
                WalletId = phone,
                Pin = "111111"
            };

            var fileName = "PhoneCheck/" + phone  + ".txt";

            var path = GetFilePath(fileName, isWindowConsole);
            var folder = GetFolderContainFilePath(fileName);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string start = "Start DoBlockSingleWalletEmoney: " + phone + "-" + DateTime.Now + Environment.NewLine;
            File.AppendAllText(path, start);


            string body = JsonConvert.SerializeObject(bodyRequest);
            var byteContent = new StringContent(body, Encoding.UTF8, "application/json");
            var token = "P5puUi8ZLN/dL46/Bt/ZP74Dc09u2fJEzQmnoAgbgw3Jg6T4B4hJfxcNwKgYQLJdq0kjU2eQWsD2Krjm3fGWp41SaqBtRyoFDloRTK3ra5qfcS9aX1DPoorECdctQ/XRNvt/PssYXLsHTCK7s7xNJ8oTAAMXHVhVZuVBsGY8Q2FDZ4A4gvyGrDT8WS2QkbFlNL/hTFCvleKW0K5E+OZuWxiIWn1ILfYQD3xGl1yRagsTZhiTxYunfCfQ90GyY8/hpQ5E+PW0n8Esm9nClyYZvcT7K1oa/tPaPC8PoWArXNzUWtDr49TZF6PADVtQRaRunfRBGJ/o2M6FNuHchWlnfw==";
            using (var client = new HttpClient())
            {
                var header = new Dictionary<string, string>
                    {
                        {EMONEY_APP_HEADER_INFO_KEY, emoneyInfo},
                        {EMONEY_APP_HEADER_LANGUAGE_KEY, eLanguage},
                        {EMONEY_APP_HEADER_AUTHORIZATION_KEY, token},
                        {"Content-Type", contentType}
                    };

                //Thêm header
                foreach (var headerKey in header.Keys)
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation(headerKey, header[headerKey]);
                }

                client.BaseAddress = new Uri(requestUrlLogin);
                //Ghi file log request
                string step1TxtRequest = "Firtst Call Login Request : " + body.ToString() + Environment.NewLine;
                File.AppendAllText(path, step1TxtRequest);
                var response = client.PostAsync(requestUrlLogin, byteContent).Result;
                //Result login lần đầu tiên
                var firstLoginResult = JsonConvert.DeserializeObject<WEmoneyBaseResponse>(response.Content.ReadAsStringAsync().Result);


                string step1TxtResponse = "Firtst Call Login Response : " + JsonConvert.SerializeObject(firstLoginResult) + Environment.NewLine;
                File.AppendAllText(path, step1TxtResponse);

                //Check số điện thoại hợp lệ
                if (!firstLoginResult.IsSuccess && firstLoginResult.ErrorCode.ToUpper().Contains(EMONEY_LOGIN_INFORMATION_INVALID_CODE) || firstLoginResult.ErrorMessage.ToUpper().Contains(EMONEY_LOGIN_INFORMATION_INVALID__MESSAGE.ToUpper()))
                {
                    return new DoBlockWalletResult
                    {
                        PhoneNumber = phone,
                        IsBlock = false,
                        DetailStatus = EMONEY_DO_BLOCK_STATUS_DETAIL_NOT_REGISTER
                    };
                }
                //Check tài khoản ví có tồn tại không
                else if (!firstLoginResult.IsSuccess && firstLoginResult.ErrorCode.ToUpper().Contains(EMONEY_ACCOUNT_NOT_FOUND_CODE) || firstLoginResult.ErrorMessage.ToUpper().Contains(EMONEY_ACCOUNT_NOT_FOUND_MESSAGE.ToUpper()))
                {
                    return new DoBlockWalletResult
                    {
                        PhoneNumber = phone,
                        IsBlock = false,
                        DetailStatus = EMONEY_DO_BLOCK_STATUS_DETAIL_NOT_REGISTER
                    };
                }

                if (!firstLoginResult.IsSuccess && firstLoginResult.ErrorCode.ToUpper().Contains(EMONEY_LOGIN_ERR_LOGIN_CHANGE_DEVICE_REQUIRED_OTP_CODE.ToUpper()) || firstLoginResult.ErrorMessage.ToUpper().Contains(EMONEY_LOGIN_ERR_LOGIN_CHANGE_DEVICE_REQUIRED_OTP_MESSAGE.ToUpper()))
                {
                    //Gọi get Otp 
                    var getOtpUrl = EMONEY_API_URL + EMONEY_API_LOGIN_GET_OTP_URL + "/" + phone;

                    string getOtpUrlRequestTxt = "Case Required Otp Change Device Request  : " + getOtpUrl + Environment.NewLine;
                    File.AppendAllText(path, getOtpUrlRequestTxt);
                    var getOtpFirstTimeResult = GetApiAsync<object>(getOtpUrl, header);

                    string getOtpUrlResponseTxt = "Case Required Otp Change Device Response  : " + JsonConvert.SerializeObject(getOtpFirstTimeResult) + Environment.NewLine;
                    File.AppendAllText(path, getOtpUrlRequestTxt);

                    var sleepTimeChangeDevice = 3000;
                    System.Threading.Thread.Sleep(sleepTimeChangeDevice);
                    var callItSelfAgain = DoBlockSingleWalletEmoney(phone, deviceId, isWindowConsole);
                    return callItSelfAgain;
                }


                //Check tài khoản login bị fail lần đầu 
                else if (!firstLoginResult.IsSuccess && firstLoginResult.ErrorCode.ToUpper().Contains(EMONEY_LOGIN_INVALID_OTP_CODE) || firstLoginResult.ErrorMessage.ToUpper().Contains(EMONEY_LOGIN_INVALID_OTP_MESSAGE.ToUpper()))
                {
                    //Gọi get Otp 
                    var getOtpUrl = EMONEY_API_URL + EMONEY_API_LOGIN_GET_OTP_URL + "/" + phone;

                    string getOtpUrlRequestTxt = "Case First Time Invalid Otp  Request  : " + getOtpUrl + Environment.NewLine;
                    File.AppendAllText(path, getOtpUrlRequestTxt);

                    var getOtpFirstTimeResult = GetApiAsync<object>(getOtpUrl, header);

                    string getOtpUrlResponseTxt = "Case First Time Invalid Otp Response  : " + JsonConvert.SerializeObject(getOtpFirstTimeResult) + Environment.NewLine;
                    File.AppendAllText(path, getOtpUrlRequestTxt);

                    //Xong gọi lại login, nhưng có kèm otp param
                    for (int i = 0; i <= 3; i++)
                    {
                        var bodyRequestLoginAgain = new LoginBodyRequest
                        {
                            WalletId = phone,
                            Pin = "111111",
                            Otp = "1111"
                        };

                        var myContent = JsonConvert.SerializeObject(bodyRequestLoginAgain);
                        var byteContentLoginAgain = new StringContent(myContent, Encoding.UTF8, "application/json");

                        string loginWithOtpRequestTxt = "Login With Otp at the " + i + " time Request: " + myContent + Environment.NewLine;
                        File.AppendAllText(path, loginWithOtpRequestTxt);

                        var loginAgainResult = PostApiAsync<object>(requestUrlLogin, byteContentLoginAgain, header);

                        string loginWithOtpResponseTxt = "Login With Otp at the " + i + " time Response: " + JsonConvert.SerializeObject(loginAgainResult) + Environment.NewLine;
                        File.AppendAllText(path, loginWithOtpResponseTxt);


                        if (!loginAgainResult.IsSuccess && loginAgainResult.ErrorCode.ToUpper().Contains(EMONEY_LOGIN_INVALID_OTP_CODE) || loginAgainResult.ErrorMessage.ToUpper().Contains(EMONEY_LOGIN_INVALID_OTP_MESSAGE.ToUpper()))
                        {
                            var timeLogin = i + 1;
                            string getOtpContinuosRequestTxt = "Call Get Otp at the " + timeLogin + " Request  : " + getOtpUrl + Environment.NewLine;
                            File.AppendAllText(path, getOtpContinuosRequestTxt);

                            var getOtpContinuosResult = GetApiAsync<object>(getOtpUrl, header);

                            string getOtpContinuosResponseTxt = "Call Get Otp at the " + timeLogin +  "Response: " + JsonConvert.SerializeObject(getOtpContinuosResult) + Environment.NewLine;
                            File.AppendAllText(path, getOtpContinuosResponseTxt);

                        }
                        else if (!loginAgainResult.IsSuccess && loginAgainResult.ErrorCode.ToUpper().Contains(EMONEY_LOGIN_OVER_LIMIT_INVALID_OTP_CODE) || loginAgainResult.ErrorMessage.ToUpper().Contains(EMONEY_LOGIN_OVER_LIMIT_INVALID_OTP_MESSAGE.ToUpper()))
                        {

                           

                            const int sleepTimeRequestOtp = 130000;
                            System.Threading.Thread.Sleep(sleepTimeRequestOtp);
                            var timeLogin = i + 1;
                            var getOtpContinuosResult = GetApiAsync<object>(getOtpUrl, header);
                            string loginResponseTxt = "Case Over Litmit Login- Msg: " + loginAgainResult.ErrorMessage + Environment.NewLine;
                            loginResponseTxt += "Call Get Otp at the " + timeLogin + "Response: " + JsonConvert.SerializeObject(getOtpContinuosResult) + Environment.NewLine;

                            File.AppendAllText(path, loginResponseTxt);


                        }
                        else
                        {
                            if (!loginAgainResult.IsSuccess && loginAgainResult.ErrorCode.ToUpper().Contains(EMONEY_LOGIN__ACCOUNT_BLOCK_CODE) || loginAgainResult.ErrorMessage.ToUpper().Contains(EMONEY_LOGIN_ACCOUNT_BLOCK_MESSAGE.ToUpper()))
                            {
                                string loginResponseTxt = "Account Has been block-Msg"  + loginAgainResult.ErrorMessage + Environment.NewLine;
                                File.AppendAllText(path, loginResponseTxt);
                                continue;
                            }
                        }

                    }

                    var bodyRequestLoginFinal = new LoginBodyRequest
                    {
                        WalletId = phone,
                        Pin = "111111",
                        Otp = "1111"
                    };

                    var myContentLastLogin = JsonConvert.SerializeObject(bodyRequestLoginFinal);

                    string finalCallLoginTxtRequest = "Final Call Login Request : " + myContentLastLogin + Environment.NewLine;
                    File.AppendAllText(path, finalCallLoginTxtRequest);

                    var byteContentLastLoginAgain = new StringContent(myContentLastLogin, Encoding.UTF8, "application/json");

                    var continueLogin = PostApiAsync<object>(requestUrlLogin, byteContentLastLoginAgain, header);

                    string finalCallLoginResponse = "Final Call Login Response : " + JsonConvert.SerializeObject(firstLoginResult) + Environment.NewLine;
                    File.AppendAllText(path, finalCallLoginResponse);

                    var loginInFinalStepResult = WEmoneyEnumLoginFail.ERR_LOGIN_OTP_INVALID;

                    if (Enum.TryParse(continueLogin.ErrorCode, out loginInFinalStepResult))
                    {
                        switch (loginInFinalStepResult)
                        {

                            case WEmoneyEnumLoginFail.ERR_LOGIN_OTP_INVALID:
                                // Xử lý khi OTP không hợp lệ
                                return new DoBlockWalletResult
                                {
                                    PhoneNumber = phone,
                                    IsBlock = false,
                                    DetailStatus = EMONEY_LOGIN_INVALID_OTP_CODE
                                };

                            case WEmoneyEnumLoginFail.ERR_RESEND_OTP_OVER_LIMITED:
                                // Xử lý khi vượt quá số lần gửi lại OTP cho phép
                                return new DoBlockWalletResult
                                {
                                    PhoneNumber = phone,
                                    IsBlock = false,
                                    DetailStatus = EMONEY_LOGIN_OVER_LIMIT_INVALID_OTP_CODE
                                };
                            case WEmoneyEnumLoginFail.ERR_LOGIN_ACCOUNT_WAS_LOCKED:
                                return new DoBlockWalletResult
                                {
                                    PhoneNumber = phone,
                                    IsBlock = true,
                                    DetailStatus = EMONEY_LOGIN__ACCOUNT_BLOCK_CODE
                                };

                            default:
                                return new DoBlockWalletResult
                                {
                                    PhoneNumber = phone,
                                    IsBlock = false,
                                    DetailStatus = EMONEY_LOGIN_INVALID_OTP_CODE
                                };
                        }
                    }
                    else
                    {
                        return new DoBlockWalletResult
                        {
                            PhoneNumber = phone,
                            IsBlock = false,
                            DetailStatus = continueLogin.ErrorMessage
                        };
                    }



                }
                else if (!firstLoginResult.IsSuccess && firstLoginResult.ErrorCode.ToUpper().Contains(EMONEY_LOGIN__ACCOUNT_BLOCK_CODE) || firstLoginResult.ErrorMessage.ToUpper().Contains(EMONEY_LOGIN_ACCOUNT_BLOCK_MESSAGE.ToUpper()))
                {

                    return new DoBlockWalletResult
                    {
                        PhoneNumber = phone,
                        IsBlock = true,
                        DetailStatus = EMONEY_LOGIN__ACCOUNT_BLOCK_CODE
                    };

                }
                else
                {
                    return new DoBlockWalletResult
                    {
                        PhoneNumber = phone,
                        IsBlock = false,
                        DetailStatus = firstLoginResult.ErrorMessage
                    };
                }

            }


        }

        private static string GetFilePath(string relativePathtoFile, bool isWindowConsole, bool isWriteDebug = false)
        {
            const string startWith = "~/";
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            //if (isWriteDebug)
            //    Logger.CommonLogger.DefaultLogger.Debug("GetFilePath ==> baseDirectory: " + baseDirectory);

            if (isWindowConsole && !string.IsNullOrEmpty(baseDirectory))
                baseDirectory = "";

            var path = Path.Combine(baseDirectory, relativePathtoFile);
            if (relativePathtoFile.StartsWith(startWith))
                path = relativePathtoFile.Replace(startWith, baseDirectory);

            //if (isWriteDebug)
            //    Logger.CommonLogger.DefaultLogger.Debug("GetFilePath ==> path: " + path);
            return path;
        }

        private static string GetFolderContainFilePath(string relativePathtoFile, bool isWindowConsole = false)
        {
            var path = GetFilePath(relativePathtoFile, isWindowConsole);

            path = path.Replace("/", "\\");
            var lastIndexOfSplash = path.LastIndexOf("\\", StringComparison.OrdinalIgnoreCase);
            return lastIndexOfSplash < 0 ? string.Empty : path.Substring(0, lastIndexOfSplash);
        }



        private static WEmoneyBaseResponse PostApiAsync<T>(string requestUrl, HttpContent data, Dictionary<string, string> header)
        {
            using (var client = new HttpClient())
            {
                //Thêm header
                foreach (var headerKey in header.Keys)
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation(headerKey, header[headerKey]);
                }
                client.BaseAddress = new Uri(requestUrl);
                var response = client.PostAsync(requestUrl, data).Result;

                if (!response.IsSuccessStatusCode) return null;

                var result = JsonConvert.DeserializeObject<WEmoneyBaseResponse>(response.Content.ReadAsStringAsync().Result);
                return result;
            }
        }

        private static WEmoneyBaseResponse GetApiAsync<T>(string requestUrl, Dictionary<string, string> header)
        {
            using (var client = new HttpClient())
            {
                //Thêm header
                foreach (var headerKey in header.Keys)
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation(headerKey, header[headerKey]);
                }
                client.BaseAddress = new Uri(requestUrl);
                var response = client.GetAsync(requestUrl).Result;

                if (!response.IsSuccessStatusCode) return null;

                var result = JsonConvert.DeserializeObject<WEmoneyBaseResponse>(response.Content.ReadAsStringAsync().Result);
                return result;
            }
        }
        public enum WEmoneyEnumLoginFail
        {
            ERR_ACCOUNT_NOT_FOUND,
            ERR_LOGIN_OTP_INVALID,
            ERR_RESEND_OTP_OVER_LIMITED,
            ERR_LOGIN_ACCOUNT_WAS_LOCKED,
            ERR_LOGIN_INFORMATION_INVALID,
            ERR_LOGIN_CHANGE_DEVICE_REQUIRED_OTP

        }
        public class DoBlockWalletResult
        {
            public string PhoneNumber { get; set; }
            public bool IsBlock { get; set; }
            public string DetailStatus { get; set; }
        }

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

        public class LoginBodyRequest
        {
            [JsonProperty("msisdn")]
            public string WalletId { get; set; }

            [JsonProperty("pin")]
            public string Pin { get; set; }

            [JsonProperty("otp", NullValueHandling = NullValueHandling.Ignore)]
            public string Otp { get; set; }
        }




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



    }
}
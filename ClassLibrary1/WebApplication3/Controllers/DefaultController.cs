using BusinessObject.WalletBusiness.Enums;
using Newtonsoft.Json;
using ProductsApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using WebApplication3.Models;
using WebApplication3.WalletHelper;
using static WebApplication3.WalletHelper.EmoneyHelper;

namespace ProductsApp.Controllers
{
    public class ProductsController : ApiController
    {
        Product[] products = new Product[]
        {
            new Product { Id = 1, Name = "Tomato Soup", Category = "Groceries", Price = 1 },
            new Product { Id = 2, Name = "Yo-yo", Category = "Toys", Price = 3.75M },
            new Product { Id = 3, Name = "Hammer", Category = "Hardware", Price = 16.99M }
        };

        

        public IEnumerable<Product> GetAllProducts()
        {
            return products;
        }

        public IHttpActionResult GetProduct(int id)
        {
            var product = products.FirstOrDefault((p) => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        //public IHttpActionResult ThemProduct(Product p)
        //{
        //    // var product = products.FirstOrDefault((p) => p.Id == id);
        //    //if (product == null)
        //    //{
        //    //    return NotFound();
        //    //}
        //    List<Product> list = products.ToList<Product>();
        //    list.Add(p);
        //    return Ok(list);
        //}

        [HttpDelete]
        public IHttpActionResult DeleteProduct()
        {
            var phoneNumbers = new List<String> { "016790292", "0969063693" };

            var blockResult = EmoneyDoBlockWallet.DoBlock(phoneNumbers, false);


            return Ok(blockResult);

        }



        [HttpPost]
        public IHttpActionResult SendRequestGetEmoneyData(ApiTopUpMoney requestModel)
        {
            //public IHttpActionResult SendRequestGetEmoneyData(ApiExchangeMoney requestModel)
            //{
            //    var EMONEY_API_URL = EmoneyHelper.EMONEY_API_URL;
            //    var EMONEY_API_URL_TRANSFER = EmoneyHelper.EMONEY_API_URL_TRANSFER;
            //    var EMONEY_API_URL_BALANCE = EmoneyHelper.EMONEY_API_URL_BALANCE;
            //    var EMONEY_API_URL_SENDMONEYINFO = EmoneyHelper.EMONEY_API_URL_SENDMONEYINFO;
            //    var EMONEY_API_URL_CONFIRMSENDMONEYINFO = EmoneyHelper.EMONEY_API_URL_CONFIRMSENDMONEYINFO;

            //    var EMONEY_APP_OS_NAME = EmoneyHelper.EMONEY_APP_OS_NAME;
            //    var EMONEY_APP_OS_VERSION = EmoneyHelper.EMONEY_APP_OS_VERSION;
            //    var EMONEY_PHONE_NAME = EmoneyHelper.EMONEY_PHONE_NAME;
            //    var EMONEY_PHONE_DEVICE_ID = requestModel.DeviceId; // EmoneyHelper.EMONEY_PHONE_DEVICE_ID;
            //    var EMONEY_VERSION_APP = EmoneyHelper.EMONEY_VERSION_APP;
            //    var EMONEY_APP_LOCALE = EmoneyHelper.EMONEY_APP_LOCALE;


            //    var contentType = "application/json; charset=UTF-8";
            //    string urlTransfer = EMONEY_API_URL + EMONEY_API_URL_TRANSFER;
            //    string urlGetBalance = EMONEY_API_URL + EMONEY_API_URL_BALANCE;
            //    string urlSendMoneyInfo = EMONEY_API_URL + EMONEY_API_URL_SENDMONEYINFO;
            //    string urlConfirmSendMoneyInfo = EMONEY_API_URL + EMONEY_API_URL_CONFIRMSENDMONEYINFO;

            //    string emoneyInfo = EMONEY_APP_OS_NAME + ";" + EMONEY_APP_OS_VERSION + ";" + EMONEY_PHONE_NAME + ";" + "1" + ";" + EMONEY_PHONE_DEVICE_ID + ";" + EMONEY_VERSION_APP;
            //    string eLanguage = "en";



            //    /*
            //   var json = "{\"wallet_id\":\"0969059611\",\"app_language\":\"en\",\"app_version_string\":\"3.6.6\",\"app_version_code\":\"3.6.6\",\"device_id\":\"79459a8e-92aa-465e-adb1-e9cccf4c0466\",\"device_platform_name\":\"Android\",\"device_platform_os_version\":\"13\",\"device_brand_name\":\"Pixel\",\"device_name\":\"sdk_gphone_x86_64\"}";
            //   var result = EmoneyWalletHelper.LoginEmoneyWalletWithId(requestModel.WalletId, requestModel.Pin, requestModel.Otp, requestModel.Token, "", requestModel.DeviceId, json);
            //   if(result == null)
            //   {
            //       var getOtp  = EmoneyWalletHelper.RequestOtp(requestModel.WalletId, requestModel.Token, "", requestModel.DeviceId, json);

            //   }
            //   return Ok(result);
            //   */

            //    #region Quet giao dich

            //    //DateTime resultDateFrom, resultDateTo;
            //    //DateTime.TryParseExact("09/05/2023", "dd'/'MM'/'yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out resultDateFrom);
            //    //DateTime.TryParseExact("16/05/2023", "dd'/'MM'/'yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out resultDateTo);
            //    //var json = "{\"wallet_id\":\"0969059611\",\"app_language\":\"en\",\"app_version_string\":\"3.6.6\",\"app_version_code\":\"3.6.6\",\"device_id\":\"17b93d0b-30bc-4663-9839-8906308640e6\",\"device_platform_name\":\"Android\",\"device_platform_os_version\":\"13\",\"device_brand_name\":\"Pixel\",\"device_name\":\"sdk_gphone_x86_64\"}";
            //    //var result  = EmoneyWalletHelper.GetWalletTransactionWithAccessToken(requestModel.WalletId, resultDateFrom, -1,1, resultDateTo,requestModel.Token,"",requestModel.DeviceId,json);
            //    //return Ok(result);







            //    #endregion



            //    #region Lấy Balance
            //    /*
            //    var json = "{\"wallet_id\":\"0969059611\",\"app_language\":\"en\",\"app_version_string\":\"3.6.6\",\"app_version_code\":\"3.6.6\",\"device_id\":\"8f2b2494-5332-4966-9882-ae4e06ee6558\",\"device_platform_name\":\"Android\",\"device_platform_os_version\":\"13\",\"device_brand_name\":\"Pixel\",\"device_name\":\"sdk_gphone_x86_64\"}";
            //    var result = EmoneyWalletHelper.GetWalletBalanceWithAccessToken(requestModel.WalletId,"USD", requestModel.Token, "", requestModel.DeviceId, json);
            //    return Ok(result);

            //     */
            //    #endregion

            #region chuyển tiền                  

            var json = "{\"wallet_id\":\"0969059611\",\"app_language\":\"en\",\"app_version_string\":\"3.6.6\",\"app_version_code\":\"3.6.6\",\"device_id\":\"0a9b4809-950d-4ceb-99e4-b001a1b61260\",\"device_platform_name\":\"Android\",\"device_platform_os_version\":\"13\",\"device_brand_name\":\"Pixel\",\"device_name\":\"sdk_gphone_x86_64\"}";
        var statuscode = WHelperDetailStatusCode.NotRun;
        var a = "";
        var m = "";
        var result = EmoneyWalletHelper.TopUpMoneyEMoneyWallet(requestModel.WalletId, requestModel.Amount,  requestModel.Currency,  requestModel.Pin, requestModel.PhoneNumber, requestModel.Token, "", requestModel.DeviceId, json, ref m, ref statuscode, ref a);
        return Ok( " a "+a  +"statuscode: " + statuscode   +"  m  "+ m);

        #endregion


        //    #region login 
        //    /*
        //    var json = "{\"wallet_id\":\"0969059611\",\"app_language\":\"en\",\"app_version_string\":\"3.6.6\",\"app_version_code\":\"3.6.6\",\"device_id\":\"79459a8e-92aa-465e-adb1-e9cccf4c0466\",\"device_platform_name\":\"Android\",\"device_platform_os_version\":\"13\",\"device_brand_name\":\"Pixel\",\"device_name\":\"sdk_gphone_x86_64\"}";
        //    var result = EmoneyWalletHelper.LoginEmoneyWalletWithId(requestModel.WalletId, requestModel.Pin, requestModel.Otp, requestModel.Token, "", requestModel.DeviceId, json);
        //    if(result == null)
        //    {
        //        var getOtp  = EmoneyWalletHelper.RequestOtp(requestModel.WalletId, requestModel.Token, "", requestModel.DeviceId, json);

        //    }
        //    return Ok(result);
        //    */
        //    #endregion

        //    #region đổi tiền


        //    //return Ok(" a " + a + "statuscode: " + statuscode + "  m  " + m + "result:  " +result );
        //    #endregion

        }
    }
}


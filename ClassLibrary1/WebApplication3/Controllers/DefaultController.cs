﻿using Newtonsoft.Json;
using ProductsApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
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
        public IHttpActionResult DeleteProduct(int c)
        {
            // var product = products.FirstOrDefault((p) => p.Id == id);
            //if (product == null)
            //{
            //    return NotFound();
            //}
            List<Product> list = products.ToList<Product>();
            Product d = list.SingleOrDefault(o => o.Id == c);
            list.Remove(d);
            return Ok(list);
        }



        [HttpPost]
        public IHttpActionResult SendRequestGetEmoneyData(RequestModel requestModel)
        {
            var EMONEY_API_URL = EmoneyHelper.EMONEY_API_URL;
            var EMONEY_API_URL_TRANSFER = EmoneyHelper.EMONEY_API_URL_TRANSFER;
            var EMONEY_APP_OS_NAME = EmoneyHelper.EMONEY_APP_OS_NAME;
            var EMONEY_APP_OS_VERSION = EmoneyHelper.EMONEY_APP_OS_VERSION;
            var EMONEY_PHONE_NAME = EmoneyHelper.EMONEY_PHONE_NAME;
            var EMONEY_PHONE_DEVICE_ID = requestModel.DeviceId; // EmoneyHelper.EMONEY_PHONE_DEVICE_ID;
            var EMONEY_VERSION_APP = EmoneyHelper.EMONEY_VERSION_APP;
            var EMONEY_APP_LOCALE = EmoneyHelper.EMONEY_APP_LOCALE;

            var httpMethod = "POST";
            var contentType = "application/json; charset=UTF-8";
            string urlTransfer = EMONEY_API_URL + EMONEY_API_URL_TRANSFER;

            string emoneyInfo = EMONEY_APP_OS_NAME + ";" + EMONEY_APP_OS_VERSION + ";" + EMONEY_PHONE_NAME + ";" + "1" + ";" + EMONEY_PHONE_DEVICE_ID + ";" + EMONEY_VERSION_APP;
            string eLanguage = "en";
            var requestHeader = new EmoneyHeaderRequest
            {
                ContentType = contentType,
                EmoneyInfo = emoneyInfo,
                EmoneyLanguage = eLanguage,
                Authorization = requestModel.Token,
                HttpMethod = httpMethod
            };
            //DateTime resultDateFrom, resultDateTo;
            //DateTime.TryParseExact("09/05/2023", "dd'/'MM'/'yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out resultDateFrom);
            //DateTime.TryParseExact("16/05/2023", "dd'/'MM'/'yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out resultDateTo);

            DateTime resultDateTo = DateTime.Now;
            DateTime resultDateFrom = resultDateTo.AddDays(-6);
            var bodyRequest = new GetEmoneyTransactionBodyRequest
            {
                fromDate = resultDateFrom.ToString("dd/MM/yyyy"),
                groupId = -1,
                page = 1,
                toDate = resultDateTo.ToString("dd/MM/yyyy")
            };
            string body = JsonConvert.SerializeObject(bodyRequest);
            var result = EmoneyHelper.SendRequestToURL(urlTransfer, body, requestHeader);

            return Ok(result);

        }
    }
}

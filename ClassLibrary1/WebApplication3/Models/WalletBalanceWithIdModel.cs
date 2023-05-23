using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication3.WalletEnum;

namespace WebApplication3.Models
{
    public class WalletBalanceWithIdModel
    {
        public WHelperStatusCode StatusCode { get; set; }
        public decimal Balance { get; set; }
        public decimal BlockAmount { get; set; }
        public string WalletId { get; set; }
    }
}
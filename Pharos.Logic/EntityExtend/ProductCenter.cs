﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    public class ProductCenter
    {
        public short Source{get;set;}
        public string Barcode {get;set;}
        public string BrandTitle {get;set;}
        public string CategoryTitle {get;set;}
        public string BrandClassTitle { get; set; }
        public string SubUnit {get;set;}
        public string Size{get;set;}
         public decimal SysPrice {get;set;}
        public string  Title{get;set;}
        public string CompanyIds { get; set; }
        public short Expiry { get; set; }
        public short? ExpiryUnit { get; set; }
    }
}
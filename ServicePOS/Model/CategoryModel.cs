﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePOS.Model
{
    public class CategoryModel
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string CategoryNameSort { get; set; }
        public int Status { get; set; }
        public string Color { get; set; }
        public string ProductColor { get; set; }
        public string Note { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public int CatalogueID { get; set; }

    }
}

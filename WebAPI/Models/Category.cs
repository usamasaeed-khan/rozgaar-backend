using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class Category
    {

        public Category(int v1, string v2, byte[] v3)
        {
            this.Category_ID = v1;
            this.CategoryName = v2;
            this.CategoryImage = v3;
        }

        public int Category_ID { get; set; }
        public string CategoryName { get; set; }
        public byte[] CategoryImage { get; set; }
    }
}
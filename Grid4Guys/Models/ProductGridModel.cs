using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Grid4Guys.Models
{
    public class ProductGridModel
    {
        public int NumericPageCount;
        public ProductGridModel()
        {
            // Define any default values here...
            this.PageSize = 10;
            this.NumericPageCount = 10;
            this.OmitDiscontinued = false;
        }
        public IEnumerable<Products> Products { get; set; }

        //排序
        public string SortBy { get; set; }
        public bool SortAscending { get; set; }
        public string SortExpression
        {
            get
            {
                return this.SortAscending ? this.SortBy + " asc" : this.SortBy + " desc";
            }
        }

        //分页
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalRecordCount { get; set; }
        public int TotalPageCount {
            get
            {
                //return Math.Max(this.TotalRecordCount / this.PageSize, 1);
                return (int)Math.Ceiling((double)this.TotalRecordCount / this.PageSize);
            }
        }

        //筛选
        public int? CategoryId { get; set; }
        public decimal MinPrice { get; set; }
        public bool OmitDiscontinued { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Grid4Guys.Models;
using System.Data.Linq;
using System.Linq.Dynamic;
namespace Grid4Guys.Controllers
{
    public class ProductsController : Controller
    {
        public northwndEntities db = new northwndEntities();
        // GET: /Products/
        public ActionResult Index()
        {
            //var model = this.DataContext.Products;
            var model = new northwndEntities();
            return View(model);
        }
        public ActionResult SortAble(string sortBy = "ProductName", bool ascending = true)
        {
            var db = new northwndEntities();
            var model = new ProductGridModel()
            {
                SortBy = sortBy,
                SortAscending = ascending
            };
            //use dynamic linq
            model.Products = db.Products.OrderBy(model.SortExpression);
            //model.Products = from m in DataContext.Products
            //                 orderby model.SortExpression
            //                 select m;
            return View(model);
        }
        //分页
        public ActionResult Paged(int page = 1, int pageSize = 10)
        {
            var model = new ProductGridModel
            {
                PageIndex = page,
                PageSize = pageSize
            };
            //总页数
            model.TotalRecordCount = db.Products.Count();
            //获得当前页的数据
            model.Products = db.Products.OrderBy(p => p.ProductID).Skip((model.PageIndex - 1) * model.PageSize).Take(model.PageSize);
            return View(model);
        }
        //筛选
        public ActionResult Filterable(int? categoryId = null, decimal minPrice = 0M, bool? omitDiscontinued = null)
        {
            var model = new ProductGridModel
            {
                CategoryId = categoryId,
                MinPrice = minPrice,
                OmitDiscontinued = omitDiscontinued.HasValue ? omitDiscontinued.Value : false,
                CategoryList = db.Categories.OrderBy(c => c.CategoryName)
                .Select(c =>
                new SelectListItem
                {
                    Text = c.CategoryName,
                    //Value = c.CategoryID.ToString()//LINQ to Entities 不识别方法“System.String ToString()”，因此该方法无法转换为存储表达式。
                    Value = System.Data.Entity.SqlServer.SqlFunctions.StringConvert((decimal)c.CategoryID)
                })
            };
            var query = db.Products.AsQueryable();
            if (categoryId != null)
                query = query.Where(c => c.CategoryID == categoryId.Value);
            if (minPrice > 0M)
                query = query.Where(p => p.UnitPrice >= minPrice);
            if (omitDiscontinued != null && omitDiscontinued.Value == true)
                query = query.Where(o => o.Discontinued == false);
            model.Products = query;
            return View(model);
        }
        // 分页和排序
        public ActionResult SortAndPage(string sortBy = "ProductName", bool ascending = true, int page = 1, int pageSize = 5)
        {
            var model = new ProductGridModel
            {
                SortBy = sortBy,
                SortAscending = ascending,
                PageIndex = page,
                PageSize = pageSize
            };
            model.TotalRecordCount = db.Products.Count();
            model.Products = db.Products.OrderBy(model.SortExpression)
                .Skip((model.PageIndex - 1) * model.PageSize)
                .Take(model.PageSize);
            return View(model);
        }
        //排序分页筛选
        public ActionResult SortPageAndFilter(string sortBy = "ProductName", bool ascending = true, int page = 1, int pageSize = 3, int? categoryId = null, decimal minPrice = 0M, bool? omitDiscontinued = null)
        {
            var model = new ProductGridModel
            {
                SortBy = sortBy,
                SortAscending = ascending,
                PageIndex = page,
                PageSize = pageSize,
                CategoryId = categoryId,
                MinPrice = minPrice,
                OmitDiscontinued = omitDiscontinued.HasValue ? omitDiscontinued.Value : false,
                CategoryList = db.Categories.OrderBy(c => c.CategoryName).
                Select(c => new SelectListItem
                {
                    Text = c.CategoryName,
                    Value = System.Data.Entity.SqlServer.SqlFunctions.StringConvert((decimal)c.CategoryID)
                })
            };
            // Filter the results
            var filteredResults = db.Products.AsQueryable();
            if (categoryId != null)
                filteredResults = filteredResults.Where(p => p.CategoryID == categoryId.Value);
            if (minPrice > 0M)
                filteredResults = filteredResults.Where(p => p.UnitPrice >= minPrice);
            if (omitDiscontinued != null && omitDiscontinued.Value == true)
                filteredResults = filteredResults.Where(p => p.Discontinued == false);
            // Determine the total number of FILTERED products being paged through (needed to compute PageCount)
            model.TotalRecordCount = filteredResults.Count();
            // Get the current page of sorted, filtered products
            model.Products = filteredResults.OrderBy(model.SortExpression)
                .Skip((model.PageIndex - 1) * model.PageSize)
                .Take(model.PageSize);
            return View(model);
        }
    }
}
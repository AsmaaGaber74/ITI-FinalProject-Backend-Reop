﻿using Jumia.Application.Services;
using Jumia.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
namespace Jumia.Mvc.Controllers
{
    [Authorize]

    public class AdminController : Controller
    {
        private readonly IOrderService orderService;
        private readonly IConfiguration _configuration;

        public AdminController(IOrderService orderService, IConfiguration configuration) 
        {
            this.orderService = orderService;
            _configuration = configuration;

        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> DisplayOrders()
        {
            var ordersDto = await orderService.GetAllOrdersAsync();
            return View(ordersDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> UpdateStatus(int orderId, string status)
        {
            await orderService.UpdateOrderStatusAsync(orderId, status);
            return RedirectToAction("DisplayOrders","Admin");
        }
        [HttpPost]
        public JsonResult DashBoardcount()
        {
            try
            {
                List<string[]> DashBoardcount = new List<string[]>();
                string connectionString = _configuration.GetConnectionString("Db");
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT BrandNameEn, COUNT(*) AS NumberOfProducts FROM dbo.Products GROUP BY BrandNameEn", con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string[] data = new string[2];
                        data[0] = reader["BrandNameEn"].ToString();
                        data[1] = reader["NumberOfProducts"].ToString();
                        DashBoardcount.Add(data);
                    }
                    reader.Close();
                }
                return Json(DashBoardcount);
            }
            catch (Exception ex)
            {
                return Json(new { Error = ex.Message });
            }
        }
        [HttpPost]

        public JsonResult DashBoardreview()
        {
            try
            {
                List<string[]> DashBoardreview = new List<string[]>();
                string connectionString = _configuration.GetConnectionString("Db");
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT  p.NameEn, AVG(r.Rating) AS AverageRating FROM  dbo.Products p JOIN reviews r ON p.Id = r.ProductID GROUP BY p.Id, p.NameEn ORDER BY AVG(r.Rating) DESC", con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string[] data = new string[2];
                        data[0] = reader["NameEn"].ToString();
                        data[1] = reader["AverageRating"].ToString();
                        DashBoardreview.Add(data);
                    }

                    reader.Close();
                }
                return Json(DashBoardreview);
            }
            catch (Exception ex)
            {
                return Json(new { Error = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult ProductCountDashbord()
        {
            try
            {
                int productCount = 0;
                string connectionString = _configuration.GetConnectionString("Db");
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) AS ProductCount FROM dbo.Products;", con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {

                        productCount = Convert.ToInt32(reader["ProductCount"]);
                    }
                    reader.Close();
                }
                return Json(new { ProductCount = productCount });
            }
            catch (Exception ex)
            {
                return Json(new { Error = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult OrderDashbord()
        {
            try
            {
                int totalOrders = 0;
                string connectionString = _configuration.GetConnectionString("Db");
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) AS TotalOrders FROM [dbo].[Orders]", con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        totalOrders = Convert.ToInt32(reader["TotalOrders"]);
                    }
                    reader.Close();
                }
                return Json(new { TotalOrders = totalOrders });
            }
            catch (Exception ex)
            {
                return Json(new { Error = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult PaymentDashbord()
        {
            try
            {
                decimal totalpayment = 0;
                string connectionString = _configuration.GetConnectionString("Db");
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT SUM(TotalPrice) AS TotalPaymentForAllProducts FROM [dbo].[orderProducts]", con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        totalpayment = Convert.ToDecimal(reader["TotalPaymentForAllProducts"]);
                    }
                    reader.Close();
                }
                return Json(new { TotalPayment = totalpayment });
            }
            catch (Exception ex)
            {
                return Json(new { Error = ex.Message });
            }
        }


        [HttpPost]
        public JsonResult UserDashbord()
        {
            try
            {
                int totalusers = 0;
                string connectionString = _configuration.GetConnectionString("Db");
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) AS NumberOfUsers FROM [dbo].[AspNetUsers]", con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        totalusers = Convert.ToInt32(reader["NumberOfUsers"]);
                    }
                    reader.Close();
                }
                return Json(new { TotalUsers = totalusers });
            }
            catch (Exception ex)
            {
                return Json(new { Error = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult SellesbyYear()
        {
            var orderData = new List<object>();

            try
            {
                string connectionString = _configuration.GetConnectionString("Db");
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT DATEPART(YEAR, DatePlaced) AS Year, COUNT(*) AS NumberOfOrders FROM [dbo].[Orders] GROUP BY DATEPART(YEAR, DatePlaced) ORDER BY Year", con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var year = Convert.ToInt32(reader["Year"]);
                        var numberOfOrders = Convert.ToInt32(reader["NumberOfOrders"]);
                        orderData.Add(new object[] { year.ToString(), numberOfOrders });
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                return Json(new { Error = ex.Message });
            }

            return Json(orderData);
        }

        [HttpPost]
        public JsonResult GetTopSellers()
        {
            var topSellers = new List<object>();

            try
            {
                string connectionString = _configuration.GetConnectionString("Db");
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT TOP (3) p.NameEn, SUM(op.Quantity) AS TotalQuantitySold FROM [dbo].[products] p JOIN [dbo].[orderProducts] op ON p.Id = op.ProductId GROUP BY p.Id, p.NameEn ORDER BY TotalQuantitySold DESC", con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var name = reader["NameEn"].ToString();
                        var totalQuantitySold = Convert.ToInt32(reader["TotalQuantitySold"]);
                        topSellers.Add(new { Name = name, TotalQuantitySold = totalQuantitySold });
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                return Json(new { Error = ex.Message });
            }

            return Json(topSellers);
        }
    }

}



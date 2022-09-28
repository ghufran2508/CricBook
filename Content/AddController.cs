using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Content
{
    public class AddController : Controller
    {
        // GET: Add
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult test(Test test)
        {
            string conStr = @"server=DESKTOP-GVRTHCT\SQLEXPRESS01; database=db_test; Integrated Security = SSPI";
            SqlConnection con = new SqlConnection(conStr);
            try
            {
                con.Open();
                string query = "INSERT INTO [tbl_Student]([Name],[Fname],[Email],[Mobile],[Description]) VALUES ('" + test.Name + "','" + test.Fname + "','" + test.Email + "','" + test.Mobile + "','" + test.Description + "')";
                SqlCommand cmd=new SqlCommand(query, con);
                var result=cmd.ExecuteNonQuery();
                cmd.Dispose();
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if(con.State==ConnectionState.Open)
                    con.Close();
            }
          

        }
    }
}
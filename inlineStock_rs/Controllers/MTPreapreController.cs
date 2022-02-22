using inlineStock_rs.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace inlineStock_rs.Controllers
{
    public class MTPreapreController : Controller
    {
        MATERIAL_STOCKEntities1 db = new MATERIAL_STOCKEntities1(); // declare goaldbal Database

        // GET: MTPreapre
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult test_pivote()
        {
            return View();
        }


        public ActionResult material_pre_pareration_view()
        {
            pivote_table_prepareration_type();
            pivote_table_prepareration_material_name();
            return View();
        }
        public ActionResult ajax_get_prepare_type()
        {
            var data = db.sp_material_prepareration_type().ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        // ==== pivote_table_prepareration_type ==== //
        public ActionResult pivote_table_prepareration_type()
        {
            string conString = ConfigurationManager.ConnectionStrings["dbinlineEntities2"].ConnectionString;
            //string conString = @"Server=43.72.1.3;UID=iecommon;PASSWORD=iecommon1234;database=MATERIAL_STOCK";
            string connetionString = null;
            SqlConnection connection;
            SqlDataAdapter adapter;
            SqlCommand command = new SqlCommand();
            DataSet dataset = new DataSet();

            connetionString = conString;
            connection = new SqlConnection(connetionString);

            connection.Open();
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_material_prepareration_type";

            adapter = new SqlDataAdapter(command);
            adapter.Fill(dataset);
            connection.Close();

            DataTable table = new DataTable();

            table = dataset.Tables[0];

            ViewData["PivotDataTable_type"] = table;

            return View();
        }

        //public ActionResult pivote_table_prepareration_type_test()
        //{
        //    string conString = ConfigurationManager.ConnectionStrings["dbinlineEntities2"].ConnectionString;
        //    //string conString = @"Server=43.72.1.3;UID=iecommon;PASSWORD=iecommon1234;database=MATERIAL_STOCK";
        //    string connetionString = null;
        //    SqlConnection connection;
        //    SqlDataAdapter adapter;
        //    SqlCommand command = new SqlCommand();
        //    DataSet dataset = new DataSet();

        //    connetionString = conString;
        //    connection = new SqlConnection(connetionString);

        //    connection.Open();
        //    command.Connection = connection;
        //    command.CommandType = CommandType.StoredProcedure;
        //    command.CommandText = "sp_material_prepareration_type";

        //    adapter = new SqlDataAdapter(command);
        //    adapter.Fill(dataset);
        //    connection.Close();

        //    DataTable table = new DataTable();

        //    table = dataset.Tables[0];

        //    ViewData["PivotDataTable"] = table;

        //    return View();
        //}

        // ==== pivote_table_prepareration_name ==== //
        public ActionResult pivote_table_prepareration_material_name()
        {
            string conString = ConfigurationManager.ConnectionStrings["dbinlineEntities2"].ConnectionString;
            //string conString = @"Server=43.72.1.3;UID=iecommon;PASSWORD=iecommon1234;database=MATERIAL_STOCK";

            string connetionString = null;
            SqlConnection connection;
            SqlDataAdapter adapter;
            SqlCommand command = new SqlCommand();
            DataSet dataset = new DataSet();

            connetionString = conString;
            connection = new SqlConnection(connetionString);

            connection.Open();
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_material_prepareration_material_name";

            adapter = new SqlDataAdapter(command);
            adapter.Fill(dataset);
            connection.Close();

            DataTable table = new DataTable();

            table = dataset.Tables[0];

            ViewData["PivotDataTable_materialname_all"] = table;

            return View();
        }

        public JsonResult get_json_material_name_all()
        {
            string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["dbinlineEntities2"].ConnectionString;
            SqlConnection cnn = new SqlConnection(cnnString);
            SqlCommand cmd = new SqlCommand();
            DataSet dataset = new DataSet();
            DataTable dt = new DataTable();
            cmd.Connection = cnn;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "sp_material_prepareration_material_name";
            cnn.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dataset);
            cnn.Close();
            dt = dataset.Tables[0];
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            cnn.Close();
            //var data = rows.ToList().OrderByDescending(dict => dict["Material_Group"]);
            //return Json(new { data = rows }, JsonRequestBehavior.AllowGet);

            return Json(rows, JsonRequestBehavior.AllowGet);
        }

    }
}
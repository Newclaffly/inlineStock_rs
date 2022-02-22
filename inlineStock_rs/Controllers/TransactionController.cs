using inlineStock_rs.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace inlineStock_rs.Controllers
{
    public class TransactionController : Controller
    {
        MATERIAL_STOCKEntities1 db = new MATERIAL_STOCKEntities1(); // declare goaldbal Database

        // GET: Transaction
        public ActionResult transaction_overview()
        {
            return View();
        }

        public ActionResult Transaction()
        {
            return View();
        }

        public ActionResult transaction_issue()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Ajax_get_transaction_issue(string material_code)
        {
            var param1 = new SqlParameter();
            param1.ParameterName = "@material_code";
            param1.SqlDbType = SqlDbType.VarChar;
            param1.SqlValue = material_code;

            //var param2 = new SqlParameter();
            //param2.ParameterName = "@start_date";
            //param2.SqlDbType = SqlDbType.VarChar;
            //param2.SqlValue = start_date;

            //var param3 = new SqlParameter();
            //param3.ParameterName = "@end_date";
            //param3.SqlDbType = SqlDbType.VarChar;
            //param3.SqlValue = end_date;

            var data = db.Database.SqlQuery<sp_issue_history_Result>("exec sp_issue_history @material_code", param1).ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Ajax_get_transaction(string param_name)
        {
                var data =  db.tb_Ntransaction.Where(x => x.FLAG_MAT == param_name).OrderByDescending(x => x.UPDATE_DATE);
                return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteTransaction(tb_Ntransaction delete_value)
        {
            string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["dbinlineEntities2"].ConnectionString;
            SqlConnection cnn = new SqlConnection(cnnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "sp_update_delete_transaction";
            cmd.Parameters.AddWithValue("@MAT_ID", delete_value.MAT_ID);
            cmd.Parameters.AddWithValue("@FLAG_MAT", delete_value.FLAG_MAT);
            cmd.Parameters.AddWithValue("@MATCODE", delete_value.MATCODE);
            cmd.Parameters.AddWithValue("@ITEM_NUMBER", delete_value.ITEM_NUMBER);
            cmd.Parameters.AddWithValue("@ISSUE_WH", delete_value.ISSUE_WH);
            cmd.Parameters.AddWithValue("@COST_CENTER", delete_value.COST_CENTER);
            cmd.Parameters.AddWithValue("@USAGE", delete_value.USAGE);
            cmd.Parameters.AddWithValue("@PURPOSE_DETAIL", delete_value.PURPOSE_DETAIL);
            cnn.Open();
            object o = cmd.ExecuteNonQuery();
            cnn.Close();
            return new EmptyResult();
        }

        public ActionResult vw_request_stats()
        {
            return View();
        }

        public ActionResult ajax_get_request_inventory()
        {
            var data = db.V_REQUEST_MAT.OrderByDescending(x => x.CREATE_DATE);
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Ajax_get_transaction_assy(string param_name)
        {
            var data = db.sp_display_transaction_detail()
                         .Where(x => x.FLAG_MAT == "ASSY")
                         .ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult Ajax_get_transaction_assy(string param_name)
        //{
        //    var param1 = new SqlParameter();
        //    param1.ParameterName = "@flag_mat";
        //    param1.SqlDbType = SqlDbType.VarChar;
        //    param1.SqlValue = param_name;

        //    var data = db.Database.SqlQuery<sp_display_transaction_detail>("exec sp_display_transaction_detail @flag_mat", param1).ToList();
        //    return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult transaction_axis()
        {
            return View();
        }

        //public ActionResult ajax_get_request_axis()
        //{
        //    //var data = db.sp_Inline_axis_usage_transaction().to
        //    var data = db.sp_Inline_auto_transaction_axis().OrderByDescending(x => x.CREATE_DATE);
        //    return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult ajax_get_request_axis(string param_start_date, string param_end_date)
        {
            var param1 = new SqlParameter();
            param1.ParameterName = "@start_date";
            param1.SqlDbType = SqlDbType.VarChar;
            param1.SqlValue = param_start_date;

            var param2 = new SqlParameter();
            param2.ParameterName = "@end_date";
            param2.SqlDbType = SqlDbType.VarChar;
            param2.SqlValue = param_end_date;
            var data = db.Database.SqlQuery<sp_Inline_auto_transaction_axis_Result>("exec sp_Inline_axis_usage_transaction @start_date,@end_date", param1, param2).ToList();
            var jsonResult = Json(new { data = data }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

            //return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

    }
}
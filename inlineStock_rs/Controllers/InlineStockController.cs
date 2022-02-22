using inlineStock_rs.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace inlineStock_rs.Controllers
{
    public class InlineStockController : Controller
    {
        MATERIAL_STOCKEntities1 db = new MATERIAL_STOCKEntities1(); // declare goalbal Database

        // GET: InlineStock

        public ActionResult inline_home()
        {
            return View();
        }
        public ActionResult overview_records()
        {
            return View();
        }
        
        // DICING
        public ActionResult Oparate_inline_dicing()
        {
            Ajax_Get();
            return View();
        }

        public ActionResult Ajax_Get()
        {
            var data = db.sp_master_table().ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Ajax_show()
        {
            var data = db.tb_NEvent.ToList().OrderByDescending(x => x.COST_CENTER);
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CU(tb_NEvent edit_value)
        {
            string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["dbinlineEntities2"].ConnectionString;
            SqlConnection cnn = new SqlConnection(cnnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "sp_add_usage";
            cmd.Parameters.AddWithValue("@BIZ_NAME", edit_value.BIZ_NAME);
            cmd.Parameters.AddWithValue("@MATCODE", edit_value.MATCODE);
            cmd.Parameters.AddWithValue("@ITEM_NUMBER", edit_value.ITEM_NUMBER);
            cmd.Parameters.AddWithValue("@MATNAME", edit_value.MATNAME);
            cmd.Parameters.AddWithValue("@MATGROUP", edit_value.MATGROUP);
            cmd.Parameters.AddWithValue("@COST_CENTER", edit_value.COST_CENTER);
            cmd.Parameters.AddWithValue("@FLAG_MAT", edit_value.FLAG_MAT);
            cmd.Parameters.AddWithValue("@ISSUE_WH", edit_value.ISSUE_WH);
            cmd.Parameters.AddWithValue("@STOCK_WH", edit_value.STOCK_WH);
            cmd.Parameters.AddWithValue("@USAGE", edit_value.USAGE);
            cmd.Parameters.AddWithValue("@REMAIN_INLINE", edit_value.REMAIN_INLINE);
            cmd.Parameters.AddWithValue("@UPDATE_DATE", edit_value.UPDATE_DATE);
            cmd.Parameters.AddWithValue("@PURPOSE_DETAIL", edit_value.PURPOSE_DETAIL);
            cmd.Parameters.AddWithValue("@UPDATE_BY", edit_value.UPDATE_BY);
            cnn.Open();
            object o = cmd.ExecuteNonQuery();
            cnn.Close();
            return new EmptyResult();
        }
        
        // ASSY
        public ActionResult Oparate_inline_assy()
        {
            return View();
        }

        public ActionResult ajax_get_assy()
        {
            var data = db.sp_master_table().ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ajax_show_assy()
        {
            var data = db.tb_NEvent_ASSY.ToList()
                    .Where(a => a.MATNAME != "-")
                    .OrderByDescending(x => x.MATGROUP);
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        // ASSY tablet
        public ActionResult Oparate_inline_assy_tablet()
        {
           // ajax_get_assy();
            return View();
        }

        public ActionResult Oparate_inline_assy_menu_tablet()
        {
            return View();
        }

        //JUNB 
        public ActionResult Oparate_inline_junb_view()
        {
            ajax_get_junb();
            return View();
        }

        public ActionResult Oparate_inline_junb()
        {
            //ajax_show_junb_master();
            ajax_get_junb();
            return View();
        }

        public ActionResult ajax_get_junb()
        {
            var data = db.sp_master_table().ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ajax_show_junb()
        {
            var data = db.sp_display_JUNB().ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ajax_show_junb_master()
        {
            var data = db.sp_display_junb_main().ToList().OrderByDescending(x => x.MATGROUP);
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public ActionResult update_usage_junb(tb_NEvent_JUNBS edit_value)
        //{
        //    string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["dbinlineEntities2"].ConnectionString;
        //    SqlConnection cnn = new SqlConnection(cnnString);
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.Connection = cnn;
        //    cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //    cmd.CommandText = "sp_add_usage_junb";
        //    cmd.Parameters.AddWithValue("@BIZ_NAME", edit_value.BIZ_NAME);
        //    cmd.Parameters.AddWithValue("@MATCODE", edit_value.MATCODE);
        //    cmd.Parameters.AddWithValue("@ITEM_NUMBER", edit_value.ITEM_NUMBER);
        //    cmd.Parameters.AddWithValue("@MATNAME", edit_value.MATNAME);
        //    cmd.Parameters.AddWithValue("@MATGROUP", edit_value.MATGROUP);
        //    //cmd.Parameters.AddWithValue("@COST_CENTER", edit_value.COST_CENTER);
        //    cmd.Parameters.AddWithValue("@FLAG_MAT", edit_value.FLAG_MAT);
        //    cmd.Parameters.AddWithValue("@ISSUE_WH", edit_value.ISSUE_WH);
        //    cmd.Parameters.AddWithValue("@STOCK_WH", edit_value.STOCK_WH);
        //    cmd.Parameters.AddWithValue("@USAGE", edit_value.USAGE);
        //    cmd.Parameters.AddWithValue("@REMAIN_INLINE", edit_value.REMAIN_INLINE);
        //    cmd.Parameters.AddWithValue("@UPDATE_DATE", edit_value.UPDATE_DATE);
        //    cmd.Parameters.AddWithValue("@PURPOSE_DETAIL", edit_value.PURPOSE_DETAIL);
        //    cmd.Parameters.AddWithValue("@UPDATE_BY", edit_value.UPDATE_BY);
        //    cnn.Open();
        //    object o = cmd.ExecuteNonQuery();
        //    cnn.Close();
        //    return new EmptyResult();
        //}

        // CSAT
        public ActionResult Oparate_inline_csat_view()
        {
            ajax_get_csat();
            return View();
        }

        public ActionResult Oparate_inline_csat()
        {
            ajax_get_csat();
            return View();
        }

        public ActionResult ajax_get_csat()
        {
            var data = db.sp_master_table().ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ajax_show_csat()
        {
            var data = db.sp_display_CSAT().ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public ActionResult update_usage_csat(tb_NEvent_CSAT edit_value)
        //{
        //    string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["dbinlineEntities2"].ConnectionString;
        //    SqlConnection cnn = new SqlConnection(cnnString);
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.Connection = cnn;
        //    cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //    cmd.CommandText = "sp_add_usage_csat";
        //    cmd.Parameters.AddWithValue("@BIZ_NAME", edit_value.BIZ_NAME);
        //    cmd.Parameters.AddWithValue("@MATCODE", edit_value.MATCODE);
        //    cmd.Parameters.AddWithValue("@ITEM_NUMBER", edit_value.ITEM_NUMBER);
        //    cmd.Parameters.AddWithValue("@MATNAME", edit_value.MATNAME);
        //    cmd.Parameters.AddWithValue("@MATGROUP", edit_value.MATGROUP);
        //    cmd.Parameters.AddWithValue("@COST_CENTER", edit_value.COST_CENTER);
        //    cmd.Parameters.AddWithValue("@FLAG_MAT", edit_value.FLAG_MAT);
        //    cmd.Parameters.AddWithValue("@ISSUE_WH", edit_value.ISSUE_WH);
        //    cmd.Parameters.AddWithValue("@STOCK_WH", edit_value.STOCK_WH);
        //    cmd.Parameters.AddWithValue("@USAGE", edit_value.USAGE);
        //    cmd.Parameters.AddWithValue("@REMAIN_INLINE", edit_value.REMAIN_INLINE);
        //    cmd.Parameters.AddWithValue("@UPDATE_DATE", edit_value.UPDATE_DATE);
        //    cmd.Parameters.AddWithValue("@PURPOSE_DETAIL", edit_value.PURPOSE_DETAIL);
        //    cmd.Parameters.AddWithValue("@UPDATE_BY", edit_value.UPDATE_BY);
        //    cnn.Open();
        //    object o = cmd.ExecuteNonQuery();
        //    cnn.Close();
        //    return new EmptyResult();
        //}


        // CG
        public ActionResult Oparate_inline_cg()
        {
            ajax_get_master();
            return View();
        }

        public ActionResult ajax_get_master()
        {
            var data = db.sp_master_table().ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ajax_show_cg()
        {
            var data = db.tb_NEvent_CG.ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public ActionResult update_usage_cg(tb_NEvent_CG edit_value)
        //{
        //    string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["dbinlineEntities2"].ConnectionString;
        //    SqlConnection cnn = new SqlConnection(cnnString);
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.Connection = cnn;
        //    cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //    cmd.CommandText = "sp_add_usage_cg";
        //    cmd.Parameters.AddWithValue("@BIZ_NAME", edit_value.BIZ_NAME);
        //    cmd.Parameters.AddWithValue("@MATCODE", edit_value.MATCODE);
        //    cmd.Parameters.AddWithValue("@ITEM_NUMBER", edit_value.ITEM_NUMBER);
        //    cmd.Parameters.AddWithValue("@MATNAME", edit_value.MATNAME);
        //    cmd.Parameters.AddWithValue("@MATGROUP", edit_value.MATGROUP);
        //    cmd.Parameters.AddWithValue("@COST_CENTER", edit_value.COST_CENTER);
        //    cmd.Parameters.AddWithValue("@FLAG_MAT", edit_value.FLAG_MAT);
        //    cmd.Parameters.AddWithValue("@ISSUE_WH", edit_value.ISSUE_WH);
        //    cmd.Parameters.AddWithValue("@STOCK_WH", edit_value.STOCK_WH);
        //    cmd.Parameters.AddWithValue("@USAGE", edit_value.USAGE);
        //    cmd.Parameters.AddWithValue("@REMAIN_INLINE", edit_value.REMAIN_INLINE);
        //    cmd.Parameters.AddWithValue("@UPDATE_DATE", edit_value.UPDATE_DATE);
        //    cmd.Parameters.AddWithValue("@PURPOSE_DETAIL", edit_value.PURPOSE_DETAIL);
        //    cmd.Parameters.AddWithValue("@UPDATE_BY", edit_value.UPDATE_BY);
        //    cnn.Open();
        //    object o = cmd.ExecuteNonQuery();
        //    cnn.Close();
        //    return new EmptyResult();
        //}

        public ActionResult Oparate_inline_cg_view()
        {
            ajax_show_cg();
            return View();
        }

        //M-OLED
        public ActionResult Oparate_inline_moled_view()
        {
            //ajax_get_moled();
            return View();
        }
        public ActionResult Oparate_inline_moled()
        {
            //ajax_get_moled();
            return View();
        }

        //public ActionResult ajax_get_moled()
        //{
        //    var data = db.sp_master_table().ToList();
        //    return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult ajax_show_moled()
        {
            var data = db.sp_display_moled().ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public ActionResult update_usage_moled(tb_NEvent_M_OLED edit_value)
        //{
        //    string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["dbinlineEntities2"].ConnectionString;
        //    SqlConnection cnn = new SqlConnection(cnnString);
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.Connection = cnn;
        //    cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //    cmd.CommandText = "sp_add_usage_junb";
        //    cmd.Parameters.AddWithValue("@BIZ_NAME", edit_value.BIZ_NAME);
        //    cmd.Parameters.AddWithValue("@MATCODE", edit_value.MATCODE);
        //    cmd.Parameters.AddWithValue("@ITEM_NUMBER", edit_value.ITEM_NUMBER);
        //    cmd.Parameters.AddWithValue("@MATNAME", edit_value.MATNAME);
        //    cmd.Parameters.AddWithValue("@MATGROUP", edit_value.MATGROUP);
        //    cmd.Parameters.AddWithValue("@COST_CENTER", edit_value.COST_CENTER);
        //    cmd.Parameters.AddWithValue("@FLAG_MAT", edit_value.FLAG_MAT);
        //    cmd.Parameters.AddWithValue("@ISSUE_WH", edit_value.ISSUE_WH);
        //    cmd.Parameters.AddWithValue("@STOCK_WH", edit_value.STOCK_WH);
        //    cmd.Parameters.AddWithValue("@USAGE", edit_value.USAGE);
        //    cmd.Parameters.AddWithValue("@REMAIN_INLINE", edit_value.REMAIN_INLINE);
        //    cmd.Parameters.AddWithValue("@UPDATE_DATE", edit_value.UPDATE_DATE);
        //    cmd.Parameters.AddWithValue("@PURPOSE_DETAIL", edit_value.PURPOSE_DETAIL);
        //    cmd.Parameters.AddWithValue("@UPDATE_BY", edit_value.UPDATE_BY);
        //    cnn.Open();
        //    object o = cmd.ExecuteNonQuery();
        //    cnn.Close();
        //    return new EmptyResult();
        //}

        [HttpPost]
        public ActionResult record_reason(tb_log_reason edit_value)
        {
            string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["dbinlineEntities2"].ConnectionString;
            SqlConnection cnn = new SqlConnection(cnnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "sp_insert_reason_log";
            cmd.Parameters.AddWithValue("@MATCODE", edit_value.MATCODE);
            cmd.Parameters.AddWithValue("@ITEM_NUMBER", edit_value.ITEM_NUMBER);
            cmd.Parameters.AddWithValue("@BIZ", edit_value.BIZ);
            cmd.Parameters.AddWithValue("@PROCESS", edit_value.PROCESS);
            cmd.Parameters.AddWithValue("@COST_CENTER", edit_value.COST_CENTER);
            cmd.Parameters.AddWithValue("@REASON", edit_value.REASON);
            cmd.Parameters.AddWithValue("@CREATE_DATE", edit_value.CREATE_DATE);
            cmd.Parameters.AddWithValue("@CREATE_BY", edit_value.CREATE_BY);
            cnn.Open();
            object o = cmd.ExecuteNonQuery();
            cnn.Close();
            return new EmptyResult();
        }


        public ActionResult Oparate_inline_htps_view()
        {
            return View();
        }
        public ActionResult Oparate_inline_htps()
        {
            return View();
        }

        public ActionResult ajax_show_htps()
        {
            var data = db.sp_display_htps().ToList().OrderByDescending(x => x.MATGROUP);
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

    }
}
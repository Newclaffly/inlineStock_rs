using inlineStock_rs.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace inlineStock_rs.Controllers
{
    public class ReportController : Controller
    {
        MATERIAL_STOCKEntities1 db = new MATERIAL_STOCKEntities1(); // declare goaldbal Database

        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult report_monthly()
        {
            return View();
        }

        public ActionResult snap_inline()
        {
            return View();
        }

        public JsonResult ajax_monthly_data(string process)
        {
            var param1 = new SqlParameter();
            param1.ParameterName = "@process";
            param1.SqlDbType = SqlDbType.VarChar;
            param1.SqlValue = process;
            var data = db.Database.SqlQuery<sp_export_monthly_Result>("exec sp_export_monthly @process", param1).ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult Export_excel_mc(Model_Search edit_value)
        {
            string strConnString = @"Server=43.72.1.3;UID=iecommon;PASSWORD=iecommon1234;database=MATERIAL_STOCK";
            var objConn = new SqlConnection(strConnString);
            //var dtAdapter = new SqlDataAdapter();
            var dt = new DataTable();

            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("sp_export_monthly", objConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@process", edit_value.process);
            adapter.SelectCommand = cmd;
            adapter.Fill(dt);

            string fileName = "Inline_stock.xls";
            FileInfo template = new FileInfo(Server.MapPath("~/Content/Inline_stock.xlsx"));
            ExcelPackage package = new ExcelPackage(template);
            FileContentResult data;
            var workbook = package.Workbook;

            int startRows = 3;
            //*** Sheet 1
            var worksheet2 = workbook.Worksheets["Sheet1"];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                worksheet2.Cells["A" + (i + startRows)].Value = dt.Rows[i]["FLAG_MAT"].ToString();
                worksheet2.Cells["B" + (i + startRows)].Value = dt.Rows[i]["ITEM_NUMBER"].ToString();
                worksheet2.Cells["C" + (i + startRows)].Value = dt.Rows[i]["MATNAME"].ToString();
                worksheet2.Cells["D" + (i + startRows)].Value = dt.Rows[i]["MATGROUP"]?.ToString();
                worksheet2.Cells["E" + (i + startRows)].Value = dt.Rows[i]["COST_CENTER"]?.ToString();
                worksheet2.Cells["F" + (i + startRows)].Value = dt.Rows[i]["REMAIN_INLINE"].ToString();
                worksheet2.Cells["G" + (i + startRows)].Value = dt.Rows[i]["UNIT"].ToString();
            }

            using (MemoryStream stream = new MemoryStream())
            {
                package.SaveAs(stream);
                var bytesdata = File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                data = bytesdata;
            }
            return Json(data, JsonRequestBehavior.AllowGet); //returning bytes of file data as json object
        } // end function export

        [HttpPost]
        public ActionResult Export_excel_miscell()
        {
            string strConnString = @"Server=43.72.1.3;UID=iecommon;PASSWORD=iecommon1234;database=MATERIAL_STOCK";
            var objConn = new SqlConnection(strConnString);
            //var dtAdapter = new SqlDataAdapter();
            var dt = new DataTable();

            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("sp_export_miscell", objConn);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@process", edit_value.process);
            adapter.SelectCommand = cmd;
            adapter.Fill(dt);

            string fileName = "Inline_stock.xls";
            FileInfo template = new FileInfo(Server.MapPath("~/Content/Template_inline_miscell.xlsx"));
            ExcelPackage package = new ExcelPackage(template);
            FileContentResult data;
            var workbook = package.Workbook;

            int startRows = 3;
            //*** Sheet 1
            var worksheet2 = workbook.Worksheets["Sheet1"];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                worksheet2.Cells["A" + (i + startRows)].Value = dt.Rows[i]["BIZ"].ToString();
                worksheet2.Cells["B" + (i + startRows)].Value = dt.Rows[i]["CAT_TYPE"].ToString();
                worksheet2.Cells["C" + (i + startRows)].Value = dt.Rows[i]["PROCESS"].ToString();
                worksheet2.Cells["D" + (i + startRows)].Value = dt.Rows[i]["MATNAME"]?.ToString();
                worksheet2.Cells["E" + (i + startRows)].Value = dt.Rows[i]["COST_CENTER"]?.ToString();
                worksheet2.Cells["F" + (i + startRows)].Value = dt.Rows[i]["ITEM_NUMBER"].ToString();
                worksheet2.Cells["G" + (i + startRows)].Value = dt.Rows[i]["GAP"].ToString();
                worksheet2.Cells["H" + (i + startRows)].Value = dt.Rows[i]["GAP_AMOUNTH"].ToString();
                worksheet2.Cells["I" + (i + startRows)].Value = dt.Rows[i]["GAP_QTY"].ToString();
                worksheet2.Cells["J" + (i + startRows)].Value = dt.Rows[i]["MONTHLY_ISSUE"].ToString();
                worksheet2.Cells["K" + (i + startRows)].Value = dt.Rows[i]["ACTUAL_ISSUE"].ToString();
                worksheet2.Cells["L" + (i + startRows)].Value = dt.Rows[i]["DAILY_ISSUE"].ToString();
                worksheet2.Cells["M" + (i + startRows)].Value = dt.Rows[i]["GAP_DAILY"].ToString();
                worksheet2.Cells["N" + (i + startRows)].Value = dt.Rows[i]["GAP_AMOUNT_DAILY"].ToString();
                worksheet2.Cells["O" + (i + startRows)].Value = dt.Rows[i]["GAP_QTY_DAILY"].ToString();

            }

            using (MemoryStream stream = new MemoryStream())
            {
                package.SaveAs(stream);
                var bytesdata = File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                data = bytesdata;
            }
            return Json(data, JsonRequestBehavior.AllowGet); //returning bytes of file data as json object
        } // end function export

        [HttpPost]
        public ActionResult insert_schedule(tb_Master_cut_off schedule_value)
        {
            string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["dbinlineEntities2"].ConnectionString;
            SqlConnection cnn = new SqlConnection(cnnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "sp_insert_schedule_cutoff";
            cmd.Parameters.AddWithValue("@SC_DATE", schedule_value.SC_DATE);
            cmd.Parameters.AddWithValue("@SC_CREATE_BY", schedule_value.SC_CREATE_BY);
            cnn.Open();
            object o = cmd.ExecuteNonQuery();
            cnn.Close();
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult update_schedule(tb_Master_cut_off schedule_edit_value)
        {
            string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["dbinlineEntities2"].ConnectionString;
            SqlConnection cnn = new SqlConnection(cnnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "sp_update_schedule_cutoff";
            cmd.Parameters.AddWithValue("@ID_SC", schedule_edit_value.ID_SC);
            cmd.Parameters.AddWithValue("@SC_DATE", schedule_edit_value.SC_DATE);
            cmd.Parameters.AddWithValue("@SC_UPDATE_BY", schedule_edit_value.SC_UPDATE_BY);
            cnn.Open();
            object o = cmd.ExecuteNonQuery();
            cnn.Close();
            return new EmptyResult();
        }


        public ActionResult material_issue_daily_report()
        {
            return View();
        }

        public ActionResult ajax_material_issue_daily_report()
        {
            var data = db.tb_information_issue_daily_report.ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult miscell_schedule()
        {
            return View();
        }

        public ActionResult ajax_miscell_schedule()
        {
            var data = db.tb_Master_cut_off
                            .Select(x => new
                            {
                                x.ID_SC,
                                x.SC_DATE,
                                //x.SC_SELECT_DAY,
                                //x.SC_SELECT_MONTH_VALUE,
                                //x.SC_SELECT_MONTH,
                                //x.SC_SELECT_MONTH_FULL,
                                //x.SC_SELECT_YEAR,
                                x.SC_CREATE_DATE,
                                x.SC_CREATE_BY,
                                x.SC_UPDATE_BY,
                                x.SC_UPDATE_DATE
                                //DATE_QUERY = x.SC_SELECT_DAY + " " + x.SC_SELECT_MONTH_FULL + " ' " + x.SC_SELECT_YEAR
                            })
                            .ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ajax_search_biz()
        {
            var data = db.sp_check_master_all_biz()
                         .Where(x => x.BIZ != null)
                         .Select(x => x.BIZ)
                         .Distinct()
                         .ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ajax_search_cat_type(string param_name)
        {
            var data = db.sp_check_master_all_biz()
                         .Where(x => x.CAT_TYPE != null && x.BIZ == param_name)
                         .Select(x => x.CAT_TYPE)
                         .Distinct()
                         .ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ajax_search_matgroup(string param_name)
        {
            var data = db.sp_check_master_all_biz()
                         .Where(x => x.MATGROUP != null && x.BIZ == param_name)
                         .Select(x => x.MATGROUP)
                         .Distinct()
                         .ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ajax_search_process(string param_name)
        {
            var data = db.sp_check_master_all_biz()
                         .Where(x => x.PROCESS != null && x.BIZ == param_name)
                         .Select(x => x.PROCESS)
                         .Distinct()
                         .ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ajax_search_cc(string param_name)
        {
            var data = db.sp_check_master_all_biz()
                         .Where(x => x.COST_CENTER != null && x.BIZ == param_name)
                         .Select(x => x.COST_CENTER)
                         .Distinct()
                         .ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult ajax_miscell_summary_table(string biz, string category)
        //{
        //    var param1 = new SqlParameter();
        //    param1.ParameterName = "@BIZ";
        //    param1.SqlDbType = SqlDbType.VarChar;
        //    param1.SqlValue = biz;

        //    var param2 = new SqlParameter();
        //    param2.ParameterName = "@CAT_TYPE";
        //    param2.SqlDbType = SqlDbType.VarChar;
        //    param2.SqlValue = category;

        //    var data = db.Database.SqlQuery<sp_chart_miscell_report_fillter_Result>("exec sp_chart_miscell_report_fillter @BIZ,@CAT_TYPE", param1, param2).ToList();
        //    var jsonResult = Json(new { data = data }, JsonRequestBehavior.AllowGet);
        //    jsonResult.MaxJsonLength = int.MaxValue;
        //    return jsonResult;
        //}

        [HttpPost]
        public JsonResult ajax_miscell_summary_table(string biz)
        {
            var param1 = new SqlParameter();
            param1.ParameterName = "@BIZ";
            param1.SqlDbType = SqlDbType.VarChar;
            param1.SqlValue = biz;

            //var param2 = new SqlParameter();
            //param2.ParameterName = "@CAT_TYPE";
            //param2.SqlDbType = SqlDbType.VarChar;
            //param2.SqlValue = category;

            var data = db.Database.SqlQuery<sp_chart_miscell_report_fillter_Result>("exec sp_chart_miscell_report_fillter @BIZ", param1).ToList();
            var jsonResult = Json(new { data = data }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }


        //[HttpPost]
        //public ActionResult ajax_miscell_summary_table(string biz, string matgroup)
        //{
        //    var data = db.sp_chart_miscell_report_fillter()
        //        .Where(x => x.BIZ == biz).ToList();

        //    return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public ActionResult ajax_miscell_summary_to_chart()
        //{
        //    var data = db.sp_chart_miscell_report().ToList();
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult ajax_miscell_summary_to_chart(string biz)
        {
            var param1 = new SqlParameter();
            param1.ParameterName = "@BIZ";
            param1.SqlDbType = SqlDbType.VarChar;
            param1.SqlValue = biz;

            //var param2 = new SqlParameter();
            //param2.ParameterName = "@CAT_TYPE";
            //param2.SqlDbType = SqlDbType.VarChar;
            //param2.SqlValue = category;

            var data = db.Database.SqlQuery<sp_chart_miscell_report_fillter_Result>("exec sp_chart_miscell_report_fillter @BIZ", param1).ToList();
            var jsonResult = Json(data, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        //[HttpGet]
        //public JsonResult PopulationChart()
        //{
        //    var data = db.sp_chart_issue_daily_report().ToList();
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult invtory_check_record()
        {
            return View();
        }

        public ActionResult ajax_get_invtory_check_record()
        {
            var data = db.sp_record_check_inv_report_f1().ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ajax_get_invtory_check_record_ffp()
        {
            var data = db.sp_record_check_inv_report_f2().ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult record_reason_inv_check(tb_NEvent_ASSY object_value)
        {
            string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["dbinlineEntities2"].ConnectionString;
            SqlConnection cnn = new SqlConnection(cnnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "sp_update_inv_check";
            cmd.Parameters.AddWithValue("@ITEM_NUMBER", object_value.ITEM_NUMBER);
            cmd.Parameters.AddWithValue("@ADJUST_VALUE", object_value.ADJUST_VALUE);
            cmd.Parameters.AddWithValue("@ADJUST_REASON", object_value.ADJUST_REASON);
            cnn.Open();
            object o = cmd.ExecuteNonQuery();
            cnn.Close();
            return new EmptyResult();
        }


        [HttpPost]
        public ActionResult export_excel_inv_check()
        {
            string strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["dbinlineEntities2"].ConnectionString;
            var objConn = new SqlConnection(strConnString);
            //var dtAdapter = new SqlDataAdapter();
            var dt = new DataTable();
            var dt_sheet2 = new DataTable();

            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("sp_record_check_inv_report_f1", objConn);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@process", edit_value.process);
            adapter.SelectCommand = cmd;
            adapter.Fill(dt);

            SqlCommand cmd_sheet2 = new SqlCommand("sp_record_check_inv_report_f2", objConn);
            cmd_sheet2.CommandType = CommandType.StoredProcedure;
            //cmd_sheet2.Parameters.AddWithValue("@process", edit_value.process);
            adapter.SelectCommand = cmd_sheet2;
            adapter.Fill(dt_sheet2);

            string fileName = "Template_inv_check_assy.xls";
            FileInfo template = new FileInfo(Server.MapPath("~/Content/Template_inv_check_assy.xlsx"));
            ExcelPackage package = new ExcelPackage(template);
            FileContentResult data;
            var workbook = package.Workbook;

            int startRows = 4;
            //*** Sheet 1
            var worksheet2 = workbook.Worksheets["Sheet1"];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                worksheet2.Cells["B" + (i + startRows)].Value = dt.Rows[i]["ITEM_NUMBER"].ToString();
                worksheet2.Cells["C" + (i + startRows)].Value = dt.Rows[i]["MATNAME"].ToString();
                worksheet2.Cells["D" + (i + startRows)].Value = dt.Rows[i]["ON_HAND"]?.ToString();
                worksheet2.Cells["E" + (i + startRows)].Value = dt.Rows[i]["TOTAL"]?.ToString();
                worksheet2.Cells["F" + (i + startRows)].Value = dt.Rows[i]["REMAIN_INLINE"].ToString();
                worksheet2.Cells["G" + (i + startRows)].Value = dt.Rows[i]["DIFF"].ToString();
                worksheet2.Cells["H" + (i + startRows)].Value = dt.Rows[i]["DIFF2"].ToString();
                worksheet2.Cells["I" + (i + startRows)].Value = dt.Rows[i]["ADJUST_DETAIL"].ToString();
             
            }

            var worksheet_sheet2 = workbook.Worksheets["Sheet2"];
            for (int i = 0; i < dt_sheet2.Rows.Count; i++)
            {
                worksheet_sheet2.Cells["B" + (i + startRows)].Value = dt_sheet2.Rows[i]["ITEM_NUMBER"].ToString();
                worksheet_sheet2.Cells["C" + (i + startRows)].Value = dt_sheet2.Rows[i]["MATNAME"].ToString();
                worksheet_sheet2.Cells["D" + (i + startRows)].Value = dt_sheet2.Rows[i]["ON_HAND"]?.ToString();
                worksheet_sheet2.Cells["E" + (i + startRows)].Value = dt_sheet2.Rows[i]["TOTAL"]?.ToString();
                worksheet_sheet2.Cells["F" + (i + startRows)].Value = dt_sheet2.Rows[i]["REMAIN_INLINE"].ToString();
                worksheet_sheet2.Cells["G" + (i + startRows)].Value = dt_sheet2.Rows[i]["DIFF"].ToString();
                worksheet_sheet2.Cells["H" + (i + startRows)].Value = dt_sheet2.Rows[i]["DIFF2"].ToString();
                worksheet_sheet2.Cells["I" + (i + startRows)].Value = dt_sheet2.Rows[i]["ADJUST_DETAIL"].ToString();
            }

            using (MemoryStream stream = new MemoryStream()) 
            {
                package.SaveAs(stream);
                var bytesdata = File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                data = bytesdata;
            } 

            return Json(data, JsonRequestBehavior.AllowGet); //returning bytes of file data as json object
        } // end function export

        public ActionResult dashboard_summary()
        {
            return View();
        }

    }
}





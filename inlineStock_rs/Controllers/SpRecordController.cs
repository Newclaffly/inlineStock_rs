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
    public class SpRecordController : Controller
    {
        MATERIAL_STOCKEntities1 db = new MATERIAL_STOCKEntities1(); // declare goaldbal Database

        // VIEW : View

        // IP
        public ActionResult record_assy_interposer()
        {
            return View();
        }

        // RESIN
        public ActionResult record_assy_resin()
        {
            return View();
        }

        // B-FRAME
        public ActionResult record_assy_b_frame()
        {
            return View();
        }

        // SEAL GLASS
        public ActionResult record_assy_seal_glass()
        {
            return View();
        }

        // CAPILLARY
        public ActionResult record_assy_capillary()
        {
            return View();
        }

        // GOLD WIRE
        public ActionResult record_assy_gold_wire()
        {
            return View();
        }

        // FUNCTION : COMMON OVERDIDE METHOD

        [HttpPost]
        public ActionResult ajax_get_data_check(string matname)
        {
            var param1 = new SqlParameter();
            param1.ParameterName = "@matname";
            param1.SqlDbType = SqlDbType.VarChar;
            param1.SqlValue = matname;
            var data = db.Database.SqlQuery<sp_record_assy_fillter_Result>("exec sp_record_assy_fillter @matname", param1).ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        //ajax_check_matname_goldwire_by_material_code
        public ActionResult ajax_get_data_check_goldwire_mtcode(string matcode)
        {
            var data = db.tb_NEvent_ASSY
                      .Where(x => x.MATCODE == matcode)
                      .ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        //Gold wire master
        public ActionResult ajax_master_gold_wire()
        {
            var data = db.tb_NEvent_ASSY
                         .Where(x => x.MATGROUP == "GOLD WIRE")
                         .Distinct()
                         .ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult update_usage_assy(tb_NEvent_ASSY edit_value)
        {
            string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["dbinlineEntities2"].ConnectionString;
            SqlConnection cnn = new SqlConnection(cnnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "sp_add_usage_assy";
            cmd.Parameters.AddWithValue("@BIZ_NAME", edit_value.BIZ_NAME);
            cmd.Parameters.AddWithValue("@MATCODE", edit_value.MATCODE);
            cmd.Parameters.AddWithValue("@ITEM_NUMBER", edit_value.ITEM_NUMBER);
            cmd.Parameters.AddWithValue("@MATNAME", edit_value.MATNAME);
            cmd.Parameters.AddWithValue("@MATGROUP", edit_value.MATGROUP);
            cmd.Parameters.AddWithValue("@STOCK_WH", edit_value.STOCK_WH);
            //cmd.Parameters.AddWithValue("@COST_CENTER", edit_value.COST_CENTER);
            cmd.Parameters.AddWithValue("@FLAG_MAT", edit_value.FLAG_MAT);
            cmd.Parameters.AddWithValue("@ISSUE_WH", edit_value.ISSUE_WH);
            cmd.Parameters.AddWithValue("@USAGE", edit_value.USAGE);
            cmd.Parameters.AddWithValue("@REMAIN_INLINE", edit_value.REMAIN_INLINE);
            cmd.Parameters.AddWithValue("@UPDATE_DATE", edit_value.UPDATE_DATE);
            cmd.Parameters.AddWithValue("@PURPOSE_DETAIL", edit_value.PURPOSE_DETAIL);
            cmd.Parameters.AddWithValue("@UPDATE_BY", edit_value.UPDATE_BY);
            cmd.Parameters.AddWithValue("@MC_ID", edit_value.MC_ID);

            cnn.Open();
            object o = cmd.ExecuteNonQuery();
            cnn.Close();
            return new EmptyResult();
        }

        public ActionResult ajax_master_mac_id()
        {
            var data = db.tb_Master_MAC_D
                         .Where(x => x.LOC_NAME == "ASSY")
                         .Select(x => x.MAC_ID)
                         .Distinct()
                         .ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
        // FUNCTION : PURPOSE 

        // IP
        public ActionResult ajax_master_purpose_ip()
        {
            var data = db.tb_Master_Purpose
                         .Where(x => x.Purpose_flag == "COMMON" || x.Purpose_flag == "SPECIAL")
                         .Select(x => x.Purpose_detail)
                         .Distinct()
                         .ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        // B-FRAME
        public ActionResult ajax_master_purpose_b_frame()
        {
            var data = db.tb_Master_Purpose
                         .Where(x => x.Purpose_flag == "COMMON" || x.Purpose_flag == "SPECIAL")
                         .Select(x => x.Purpose_detail)
                         .Distinct()
                         .ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        // RESIN
        public ActionResult ajax_master_purpose_resin()
        {
            var data = db.tb_Master_Purpose
                         .Where(x => x.Purpose_flag == "COMMON" || x.Purpose_flag == "RESIN")
                         .Select(x => x.Purpose_detail)
                         .Distinct()
                         .ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        // SEAL_GLASS
        public ActionResult ajax_master_purpose_seal_glass()
        {
            var data = db.tb_Master_Purpose
                         .Where(x => x.Purpose_flag == "COMMON" || x.Purpose_flag == "SPECIAL")
                         .Select(x => x.Purpose_detail)
                         .Distinct()
                         .ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        // CAPILLARY
        public ActionResult ajax_master_purpose_capillary()
        {
            var data = db.tb_Master_Purpose
                         .Where(x => x.Purpose_flag == "COMMON")
                         .Select(x => x.Purpose_detail)
                         .Distinct()
                         .ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        // GOLD_WIRE
        //public ActionResult ajax_master_purpose_gold_wire()
        //{
        //    var data = db.tb_Master_Purpose
        //                 .Where(x => x.Purpose_flag == "GOLD_WIRE")
        //                 .OrderByDescending(x => x.Purpose_ID)
        //                 .Select(x => x.Purpose_detail)
        //                 .Distinct()
        //                 .ToList();
        //    return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        //}

        //  FUNCTION : AUTO COMPLETED 

        // IP
        public JsonResult ajax_master_auto_completed_ip(string term)
        {
            var data = db.tb_NEvent_ASSY
                         .Where(x => x.MATGROUP == "CERAMIC PACKAGE" || x.MATGROUP == "INTERPOSER")
                         .Where(x => x.MATNAME.ToLower().Contains(term.ToLower()))
                         .Select(x => x.MATNAME)
                         .Distinct();
            return Json(data, JsonRequestBehavior.AllowGet);

            //return Json(data.ToList(), JsonRequestBehavior.AllowGet);
            //return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        // B-FRAME
        public JsonResult ajax_master_auto_completed_b_frmae(string term)
        {
            var data = db.tb_NEvent_ASSY
                         .Where(x => x.MATGROUP == "B FRAME")
                         .Where(x => x.MATNAME.ToLower().Contains(term.ToLower()))
                         .Select(x => x.MATNAME)
                         .Distinct();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        // RESIN
        public JsonResult ajax_master_auto_completed_resin(string term)
        {
            var data = db.tb_NEvent_ASSY
                         .Where(x => x.MATGROUP == "EPOXY RESIN" || x.MATGROUP == "DIE BOND PASTE" || x.MATGROUP == "SEALING RESIN" || x.MATGROUP == "UV CURABLE EPOXY RESIN" || x.MATGROUP == "DIE BOND ADHESIVE")
                         .Where(x => x.MATNAME.ToLower().Contains(term.ToLower()))
                         .Select(x => x.MATNAME)
                         .Distinct();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        // SEAL GLASS
        public JsonResult ajax_master_auto_completed_seal_glass(string term)
        {
            var data = db.tb_NEvent_ASSY
                         .Where(x => x.MATGROUP == "SEAL GLASS")
                         .Where(x => x.MATNAME.ToLower().Contains(term.ToLower()))
                         .Select(x => x.MATNAME)
                         .Distinct();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        // CAPILLARY
        public JsonResult ajax_master_auto_completed_capillary(string term)
        {
            var data = db.tb_NEvent_ASSY
                         .Where(x => x.MATGROUP == "CAPILLARY")
                         .Where(x => x.MATNAME.ToLower().Contains(term.ToLower()))
                         .Select(x => x.MATNAME)
                         .Distinct();
            return Json(data, JsonRequestBehavior.AllowGet);
         
        }

        // SEAL GLASS
        public JsonResult ajax_master_auto_completed_gold_wire(string term)
        {
            var data = db.tb_NEvent_ASSY
                         .Where(x => x.MATGROUP == "GOLD WIRE")
                         .Where(x => x.MATNAME.ToLower().Contains(term.ToLower()))
                         .Select(x => x.MATNAME)
                         .Distinct();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}
using inlineStock_rs.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace inlineStock_rs.Controllers
{
    public class ManagementsController : Controller
    {
        MATERIAL_STOCKEntities1 db = new MATERIAL_STOCKEntities1(); // declare goaldbal Database

        // GET: Managements
        public ActionResult managements_overview()
        {
            return View();
        }

        //dicing
        [HttpGet]
        public ActionResult managements_adjust_actual_dicing()
        {
            List<tb_NEvent> model = new List<tb_NEvent>();
            using (MATERIAL_STOCKEntities1 dc = new MATERIAL_STOCKEntities1())
            {
                model = dc.tb_NEvent.ToList();
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult managements_adjust_actual_dicing(List<tb_NEvent> list)
        {
            if (ModelState.IsValid)
            {
                using (MATERIAL_STOCKEntities1 dc = new MATERIAL_STOCKEntities1())
                {
                    foreach (var i in list)
                    {
                        var c = dc.tb_NEvent.Where(a => a.MATCODE.Equals(i.MATCODE)).FirstOrDefault();
                        if (c != null)
                        {
                            c.REMAIN_INLINE = i.REMAIN_INLINE;
                            c.PURPOSE_DETAIL = "RESET";

                        }
                    }
                    dc.SaveChanges();
                }
                ViewBag.Message = "Successfully Updated.";
                return View(list);
            }
            else
            {
                ViewBag.Message = "Failed ! Please try again.";
                return View(list);
            }
        }

        //CG
        public ActionResult managements_adjust_actual_cg()
        {
            return View();
        }

        public ActionResult ajax_get_managements_adjust_actual_cg()
        {
            var data = db.tb_NEvent_CG.ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult update_adjust_end_month_cg(tb_NEvent_CG edit_value)
        {
            string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["dbinlineEntities2"].ConnectionString;
            SqlConnection cnn = new SqlConnection(cnnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "sp_adjust_eomonth";
            cmd.Parameters.AddWithValue("@MATCODE", edit_value.MATCODE);
            cmd.Parameters.AddWithValue("@ITEM_NUMBER", edit_value.ITEM_NUMBER);
            cmd.Parameters.AddWithValue("@COST_CENTER", edit_value.COST_CENTER);
            cmd.Parameters.AddWithValue("@REMAIN_INLINE", edit_value.REMAIN_INLINE);
            cmd.Parameters.AddWithValue("@FLAG_MAT", edit_value.FLAG_MAT);
            cmd.Parameters.AddWithValue("@UPDATE_BY", edit_value.UPDATE_BY);

            cnn.Open();
            object o = cmd.ExecuteNonQuery();
            cnn.Close();
            return new EmptyResult();
        }

        //ASSY
        public ActionResult managements_adjust_actual_assy()
        {
            return View();
        }

        public ActionResult ajax_get_managements_adjust_actual_assy()
        {
            var data = db.tb_NEvent_ASSY.ToList()
                     .Where(a => a.MATNAME != "-")
                    .OrderByDescending(x => x.MATGROUP); ;
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        //JUNB
        public ActionResult managements_adjust_actual_junb()
        {
            return View();
        }

        public ActionResult ajax_get_managements_adjust_actual_junb()
        {
            var data = db.sp_display_junb_main().ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
      
        //CSAT
        public ActionResult managements_adjust_actual_csat()
        {
            return View();
        }

        public ActionResult ajax_get_managements_adjust_actual_csat()
        {
            var data = db.sp_display_CSAT().ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        //M-OLED
        public ActionResult managements_adjust_actual_moled()
        {
            return View();
        }

        public ActionResult ajax_get_managements_adjust_actual_moled()
        {
            var data = db.sp_display_moled().ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        // HTPS
        public ActionResult managements_adjust_actual_htps()
        {
            return View();
        }

        public ActionResult ajax_get_managements_adjust_actual_htps()
        {
            //var data = db.tb_NEvent_HTPS.ToList();
            //var data = (from a in db.tb_NEvent_HTPS
            //              leftjoin b in db.tb_formula_factor 
            //            on new { p1=a.MATCODE , p2=a.ITEM_NUMBER } equals new {p1=b.MATCODE, p2=b.ITEM_NUMBER }                        
            //            select new { a.MATGROUP, a.MATCODE, a.ITEM_NUMBER, a.MATNAME, a.REMAIN_INLINE, b.UNIT }).Distinct();

            // var data = from u in db.tb_NEvent_HTPS
            //            join p in db.tb_formula_factor.Where(x => x.FLAG_MAT == "HTPS")
            //    on u.MATCODE equals p.MATCODE
            //into temp
            //            from j in temp.DefaultIfEmpty()
            //            select new
            //            {
            //                BIZ_NAME = u.BIZ_NAME,

            //                FLAG_MAT = u.FLAG_MAT,

            //                MATGROUP = u.MATGROUP,
            //                MATCODE = u.MATCODE,
            //                ITEM_NUMBER = u.ITEM_NUMBER,
            //                MATNAME = u.MATNAME,
            //                REMAIN_INLINE = u.REMAIN_INLINE,
            //                UNIT = j.UNIT ?? u.UNIT
            //            };
            var data = db.sp_display_adjust_htps().ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Upload_excel_taget_junb_csat()
        {
            return View();
        }

        // Upload Excel target data
        public ActionResult clear_information_target()
        {
            db.Database.ExecuteSqlCommand("DELETE FROM tb_temp_junb_csat_target DBCC CHECKIDENT('tb_temp_junb_csat_target', RESEED, 0)"); // reset identity in sqlserver
            string message = "Welcome";
            return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    
        [HttpPost]
        public ActionResult Upload_excel_target(FormCollection formCollection, string valueINeeds)
        {
            try
            {
                var tb_temp_junb_csat_target_LIST = new List<tb_temp_junb_csat_target>();

                if (Request != null)
                {
                    HttpPostedFileBase file = Request.Files["UploadedFile_junb_csat"];
                    if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                    {
                        string fileName = file.FileName;
                        string fileContentType = file.ContentType;
                        byte[] fileBytes = new byte[file.ContentLength];
                        var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = package.Workbook.Worksheets["For Adj & Upload"];
                            var noOfCol = workSheet.Dimension.End.Column;
                            //var noOfRow = workSheet.Dimension.End.Row;
                            var noOfRow = workSheet.Cells.Where(cell => !string.IsNullOrEmpty(cell.Value?.ToString() ?? string.Empty)).LastOrDefault().End.Row;

                            for (int rowIterator = 5; rowIterator <= noOfRow; rowIterator++)
                            {
                                // tager information
                                var data_temp_junb_csat_target = new tb_temp_junb_csat_target();


                                //data_temp_junb_csat_target.PROCESS = workSheet.Cells[rowIterator, 3]?.Value?.ToString();//IS CG M-OLED
                                //data_temp_junb_csat_target.ITEM_NUMBER = workSheet.Cells[rowIterator, 7]?.Value?.ToString();//IS CG M-OLED
                                //data_temp_junb_csat_target.MATERIAL_TYPE = workSheet.Cells[rowIterator, 2]?.Value?.ToString();//IS CG M-OLED
                                //data_temp_junb_csat_target.COST_CENTER = workSheet.Cells[rowIterator, 6]?.Value?.ToString();//IS CG M-OLED
                                //data_temp_junb_csat_target.ISSUE_TARGET = workSheet.Cells[rowIterator, 36]?.Value?.ToString();//IS CG M-OLED

                                //data_temp_junb_csat_target.ITEM_NUMBER = workSheet.Cells[rowIterator, 6]?.Value?.ToString(); //LSI
                                //data_temp_junb_csat_target.COST_CENTER = workSheet.Cells[rowIterator, 3]?.Value?.ToString(); // LSI
                                //data_temp_junb_csat_target.ISSUE_TARGET = workSheet.Cells[rowIterator, 37]?.Value?.ToString(); //LSI

                                data_temp_junb_csat_target.PROCESS = workSheet.Cells[rowIterator, 4]?.Value?.ToString();//HTPS
                                data_temp_junb_csat_target.ITEM_NUMBER = workSheet.Cells[rowIterator, 6]?.Value?.ToString();//HTPS
                                data_temp_junb_csat_target.MATERIAL_TYPE = workSheet.Cells[rowIterator, 3]?.Value?.ToString();//HTPS
                                data_temp_junb_csat_target.COST_CENTER = workSheet.Cells[rowIterator, 7]?.Value?.ToString();//HTPS
                                data_temp_junb_csat_target.ISSUE_TARGET = workSheet.Cells[rowIterator, 35]?.Value?.ToString();//HTPS


                                DateTime now = DateTime.Now;
                                data_temp_junb_csat_target.UPDATE_DATE = now;
                                data_temp_junb_csat_target.UPDATE_BY = "SYSTEM";
                                data_temp_junb_csat_target.FLAG_MAT = valueINeeds;

                                tb_temp_junb_csat_target_LIST.Add(data_temp_junb_csat_target);
                                //    }
                                //}//end check if model null or not null
                            }//end loop for read data in excel
                        }//end using package
                    }//end if file null
                }// end if Request

                using (MATERIAL_STOCKEntities1 excelImportDBEntities = new MATERIAL_STOCKEntities1())
                {
                    foreach (var item in tb_temp_junb_csat_target_LIST)
                    {
                        excelImportDBEntities.tb_temp_junb_csat_target.Add(item);
                    }
                    excelImportDBEntities.SaveChanges();
                }
            }//end try
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return View("managements_overview");
        }//end method

        // Upload Excel obe
        public ActionResult clear_information_obe()
        {
            db.Database.ExecuteSqlCommand("DELETE FROM tb_temp_obe DBCC CHECKIDENT('tb_temp_obe', RESEED, 0)"); // reset identity in sqlserver
            string message = "Welcome";
            return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public ActionResult Upload_excel_obe(FormCollection formCollection, string valueINeeds)
        {
            try
            {
                var tb_temp_obe_LIST = new List<tb_temp_obe>();

                if (Request != null)
                {
                    HttpPostedFileBase file = Request.Files["UploadedFile_obe"];
                    if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                    {
                        string fileName = file.FileName;
                        string fileContentType = file.ContentType;
                        byte[] fileBytes = new byte[file.ContentLength];
                        var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = package.Workbook.Worksheets["DI-33_MFG-R1-MATERIAL_WORKBENCH"];
                            var noOfCol = workSheet.Dimension.End.Column;
                            //var noOfRow = workSheet.Dimension.End.Row;
                            var noOfRow = workSheet.Cells.Where(cell => !string.IsNullOrEmpty(cell.Value?.ToString() ?? string.Empty)).LastOrDefault().End.Row;

                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                var data_tb_temp_obe = new tb_temp_obe();
                                data_tb_temp_obe.SUB = workSheet.Cells[rowIterator, 3]?.Value?.ToString();
                                data_tb_temp_obe.MATNAME = workSheet.Cells[rowIterator, 4]?.Value?.ToString();
                                data_tb_temp_obe.ITEM_NUMBER = workSheet.Cells[rowIterator, 5]?.Value?.ToString();
                                data_tb_temp_obe.ON_HAND = workSheet.Cells[rowIterator, 7]?.Value?.ToString();
                                DateTime now = DateTime.Now;
                                data_tb_temp_obe.CREATE_DATE = now;
                                data_tb_temp_obe.UPDATE_BY = "SYSTEM";
                                tb_temp_obe_LIST.Add(data_tb_temp_obe);
                            
                            }//end loop for read data in excel
                        }//end using package
                    }//end if file null
                }// end if Request

                using (MATERIAL_STOCKEntities1 excelImportDBEntities = new MATERIAL_STOCKEntities1())
                {
                    foreach (var item in tb_temp_obe_LIST)
                    {
                        excelImportDBEntities.tb_temp_obe.Add(item);
                    }
                    excelImportDBEntities.SaveChanges();
                }
            }//end try
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return View("managements_overview");
        }//end method

        // Upload std cost data
        [HttpPost]
        public ActionResult Upload_std_cost(FormCollection formCollection, string valueINeeds)
        {
            try
            {
                var tb_temp_std_cost_LIST = new List<tb_std_cost_upload>();

                if (Request != null)
                {
                    HttpPostedFileBase file = Request.Files["UploadedFile_std_cost"];
                    if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                    {
                        string fileName = file.FileName;
                        string fileContentType = file.ContentType;
                        byte[] fileBytes = new byte[file.ContentLength];
                        var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = package.Workbook.Worksheets["Sheet1"];
                            var noOfCol = workSheet.Dimension.End.Column;
                            //var noOfRow = workSheet.Dimension.End.Row;
                            var noOfRow = workSheet.Cells.Where(cell => !string.IsNullOrEmpty(cell.Value?.ToString() ?? string.Empty)).LastOrDefault().End.Row;

                            for (int rowIterator = 6; rowIterator <= noOfRow; rowIterator++)
                            {
                                // target information
                                var data_temp_std_cost = new tb_std_cost_upload();

                                data_temp_std_cost.ITEM_NUMBER = workSheet.Cells[rowIterator, 1]?.Value?.ToString();
                                //data_temp_std_cost.DESCRIPTION = workSheet.Cells[rowIterator, 2]?.Value?.ToString();
                                data_temp_std_cost.CATEGORY = workSheet.Cells[rowIterator, 3]?.Value?.ToString();
                                data_temp_std_cost.TERM = workSheet.Cells[rowIterator, 4]?.Value?.ToString();
                                data_temp_std_cost.UNIT_PRICE_CURRENCY = workSheet.Cells[rowIterator, 5]?.Value?.ToString();
                                data_temp_std_cost.CURRENCY = workSheet.Cells[rowIterator, 6]?.Value?.ToString();
                                data_temp_std_cost.UNIT_PRICE_TH = workSheet.Cells[rowIterator, 7]?.Value?.ToString();
                                data_temp_std_cost.FREIGHT = workSheet.Cells[rowIterator, 8]?.Value?.ToString();
                                data_temp_std_cost.INCURANCE = workSheet.Cells[rowIterator, 9]?.Value?.ToString();
                                data_temp_std_cost.OVER_HEAD = workSheet.Cells[rowIterator, 10]?.Value?.ToString();
                                data_temp_std_cost.ITEM_COST_TH = workSheet.Cells[rowIterator, 11]?.Value?.ToString();
                                data_temp_std_cost.BIZ = workSheet.Cells[rowIterator, 12]?.Value?.ToString();
                                data_temp_std_cost.MAT_GROUP = workSheet.Cells[rowIterator, 14]?.Value?.ToString();
                                //data_temp_std_cost.S_NAME = workSheet.Cells[rowIterator, 17]?.Value?.ToString();
                                //data_temp_std_cost.M_NAME = workSheet.Cells[rowIterator, 18]?.Value?.ToString();
                                DateTime now = DateTime.Now;
                                data_temp_std_cost.UPDATE_DATE = now;
                                data_temp_std_cost.UPDATE_BY = "THATPHON CHUTINAN (SDT)";
                                tb_temp_std_cost_LIST.Add(data_temp_std_cost);
                                //    }
                                //}//end check if model null or not null
                            }//end loop for read data in excel
                        }//end using package
                    }//end if file null
                }// end if Request

                using (MATERIAL_STOCKEntities1 excelImportDBEntities = new MATERIAL_STOCKEntities1())
                {
                    foreach (var item in tb_temp_std_cost_LIST)
                    {
                        excelImportDBEntities.tb_std_cost_upload.Add(item);
                    }
                    excelImportDBEntities.SaveChanges();
                }
            }//end try
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return View("managements_overview");
        }//end method

        // Upload master_issues data
        [HttpPost]
        public ActionResult Upload_master(FormCollection formCollection, string valueINeeds)
        {
            try
            {
                var tb_temp_master_daily_report_LIST = new List<tb_master_issue_daily>();

                if (Request != null)
                {
                    HttpPostedFileBase file = Request.Files["UploadedFile_master_issue"];
                    if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                    {
                        string fileName = file.FileName;
                        string fileContentType = file.ContentType;
                        byte[] fileBytes = new byte[file.ContentLength];
                        var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            //var currentSheet = package.Workbook.Worksheets;
                            //var workSheet = package.Workbook.Worksheets["21RBGT"];
                            //var noOfCol = workSheet.Dimension.End.Column;
                            ////var noOfRow = workSheet.Dimension.End.Row;
                            //var noOfRow = workSheet.Cells.Where(cell => !string.IsNullOrEmpty(cell.Value?.ToString() ?? string.Empty)).LastOrDefault().End.Row;

                            //for (int rowIterator = 6; rowIterator <= noOfRow; rowIterator++)
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfCol = workSheet.Dimension.End.Column;
                            //var noOfRow = workSheet.Dimension.End.Row;
                            var noOfRow = workSheet.Cells.Where(cell => !string.IsNullOrEmpty(cell.Value?.ToString() ?? string.Empty)).LastOrDefault().End.Row;

                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                var data_temp_issue_daily = new tb_master_issue_daily();
                                data_temp_issue_daily.ITEM_NUMBER = workSheet.Cells[rowIterator, 1]?.Value?.ToString();
                                data_temp_issue_daily.PROCESS = workSheet.Cells[rowIterator, 2]?.Value?.ToString();
                                data_temp_issue_daily.COST_CENTER = workSheet.Cells[rowIterator, 4]?.Value?.ToString();
                                data_temp_issue_daily.BIZ = workSheet.Cells[rowIterator, 5]?.Value?.ToString();

                                tb_temp_master_daily_report_LIST.Add(data_temp_issue_daily);
                            }//end loop for read data in excel
                        }//end using package
                    }//end if file null
                }// end if Request

                using (MATERIAL_STOCKEntities1 excelImportDBEntities = new MATERIAL_STOCKEntities1())
                {
                    foreach (var item in tb_temp_master_daily_report_LIST)
                    {
                        excelImportDBEntities.tb_master_issue_daily.Add(item);
                    }
                    excelImportDBEntities.SaveChanges();
                }
            }//end try
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return View("managements_overview");
        }//end method

        //Upload Excel temp data for IS : ASSY
       [HttpPost]
        public ActionResult Upload_excel_data_assy(FormCollection formCollection, string valueINeeds)
        {
            //db.Database.ExecuteSqlCommand("DELETE FROM tb_temp_data_assy DBCC CHECKIDENT('tb_temp_data_assy', RESEED, 0)"); // reset identity in sqlserver

            try
            {
                var tb_temp_data_assy_LIST = new List<tb_temp_data_assy>(); // Declare variable List for temp data

                if (Request != null) 
                {
                    HttpPostedFileBase file = Request.Files["UploadedFile_data_assy"]; // Browse file location
                    if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                    {
                        string fileName = file.FileName;
                        string fileContentType = file.ContentType;
                        byte[] fileBytes = new byte[file.ContentLength];
                        var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Declare License nonComerical
                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = package.Workbook.Worksheets["Format 1"]; // Follow "Sheet1" in excel
                            var noOfCol = workSheet.Dimension.End.Column;
                            //var noOfRow = workSheet.Dimension.End.Row;
                            var noOfRow = workSheet.Cells.Where(cell => !string.IsNullOrEmpty(cell.Value?.ToString() ?? string.Empty)).LastOrDefault().End.Row; // Change to where last row empty

                            for (int rowIterator = 4; rowIterator <= noOfRow; rowIterator++)
                            {
                                var data_tb_temp_data_assy_LIST = new tb_temp_data_assy(); // Declare data for overdide variable
                            
                                //data_tb_temp_data_assy_LIST.MATGROUP = workSheet.Cells[rowIterator, 2]?.Value?.ToString();
                                data_tb_temp_data_assy_LIST.ITEM_NUMBER = workSheet.Cells[rowIterator, 2]?.Value?.ToString();
                                data_tb_temp_data_assy_LIST.MATNAME = workSheet.Cells[rowIterator, 3]?.Value?.ToString();
                                //data_tb_temp_data_assy_LIST.CAT_TYPE = workSheet.Cells[rowIterator, 4]?.Value?.ToString();
                                //data_tb_temp_data_assy_LIST.UNIT = workSheet.Cells[rowIterator, 5]?.Value?.ToString();
                                //data_tb_temp_data_assy_LIST.QAH_SCT_QA = workSheet.Cells[rowIterator, 10]?.Value?.ToString();
                                //data_tb_temp_data_assy_LIST.DRT_HLD_SO_QA = workSheet.Cells[rowIterator, 11]?.Value?.ToString();
                                //data_tb_temp_data_assy_LIST.PM_2G_MFG = workSheet.Cells[rowIterator, 12]?.Value?.ToString();
                                data_tb_temp_data_assy_LIST.DRT_2G2_FL_MFG = workSheet.Cells[rowIterator, 6]?.Value?.ToString();
                                //data_tb_temp_data_assy_LIST.FLOOR_ACT = workSheet.Cells[rowIterator, 14]?.Value?.ToString();
                                //data_tb_temp_data_assy_LIST.PARENT_MAT = workSheet.Cells[rowIterator, 15]?.Value?.ToString();
                                DateTime now = DateTime.Now;
                                data_tb_temp_data_assy_LIST.UPDATE_DATE = now;
                                data_tb_temp_data_assy_LIST.UPDATE_BY = "THATPHON CHUTINAN (SDT)";
                                data_tb_temp_data_assy_LIST.FLAG_MAT = "ASSY";
                                tb_temp_data_assy_LIST.Add(data_tb_temp_data_assy_LIST); // Add data to List
                               
                            }//end loop for read data in excel
                        }//end using package
                    }//end if file null
                }// end if Request

                using (MATERIAL_STOCKEntities1 excelImportDBEntities = new MATERIAL_STOCKEntities1())
                {
                    foreach (var item in tb_temp_data_assy_LIST)
                    {
                        excelImportDBEntities.tb_temp_data_assy.Add(item);
                    }
                    excelImportDBEntities.SaveChanges();
                }
            }//end try
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return View("managements_overview");
        }//end method
       
        public ActionResult reset_identity()
        {
            var data = db.spReset_Identity();
            return View();
        }
       
        public ActionResult managements_control_factor()
        {
            return View();
        }
       
        public ActionResult information_control_factor(string permis)
        {
            var data = db.tb_formula_factor.Where(x=> x.FLAG_MAT == permis);
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult control_factor_edit_update(tb_formula_factor edit_value_factor, string PROCEDURE)
        {
            string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["dbinlineEntities2"].ConnectionString;
            SqlConnection cnn = new SqlConnection(cnnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "sp_controls_convert_factor_unit";
            cmd.Parameters.AddWithValue("@MATCODE", edit_value_factor.MATCODE);
            cmd.Parameters.AddWithValue("@ITEM_NUMBER", edit_value_factor.ITEM_NUMBER);
            cmd.Parameters.AddWithValue("@FLAG_MAT", edit_value_factor.FLAG_MAT);
            cmd.Parameters.AddWithValue("@FACTOR", edit_value_factor.FACTOR);
            cmd.Parameters.AddWithValue("@UNIT", edit_value_factor.UNIT);
            cmd.Parameters.AddWithValue("@PROCEDURE", PROCEDURE);
            cmd.Parameters.AddWithValue("@UPDATE_BY", edit_value_factor.UPDATE_BY);

            cnn.Open();
            object o = cmd.ExecuteNonQuery();
            cnn.Close();
            return new EmptyResult();
        }


        // adjust end of month

        // adjust junb (IS)
        [HttpPost]
        public ActionResult update_adjust_end_month_junb(tb_NEvent_JUNBS edit_value)
        {
            string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["dbinlineEntities2"].ConnectionString;
            SqlConnection cnn = new SqlConnection(cnnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "sp_adjust_eomonth";
            cmd.Parameters.AddWithValue("@MATCODE", edit_value.MATCODE);
            cmd.Parameters.AddWithValue("@ITEM_NUMBER", edit_value.ITEM_NUMBER);
            cmd.Parameters.AddWithValue("@COST_CENTER", edit_value.COST_CENTER);
            cmd.Parameters.AddWithValue("@REMAIN_INLINE", edit_value.REMAIN_INLINE);
            cmd.Parameters.AddWithValue("@FLAG_MAT", edit_value.FLAG_MAT);
            cmd.Parameters.AddWithValue("@UPDATE_BY", edit_value.UPDATE_BY);

            cnn.Open();
            object o = cmd.ExecuteNonQuery();
            cnn.Close();
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult update_adjust_end_month_csat(tb_NEvent_CSAT edit_value)
        {
            string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["dbinlineEntities2"].ConnectionString;
            SqlConnection cnn = new SqlConnection(cnnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "sp_adjust_eomonth";
            cmd.Parameters.AddWithValue("@MATCODE", edit_value.MATCODE);
            cmd.Parameters.AddWithValue("@ITEM_NUMBER", edit_value.ITEM_NUMBER);
            cmd.Parameters.AddWithValue("@COST_CENTER", edit_value.COST_CENTER);
            cmd.Parameters.AddWithValue("@REMAIN_INLINE", edit_value.REMAIN_INLINE);
            cmd.Parameters.AddWithValue("@FLAG_MAT", edit_value.FLAG_MAT);
            cmd.Parameters.AddWithValue("@UPDATE_BY", edit_value.UPDATE_BY);

            cnn.Open();
            object o = cmd.ExecuteNonQuery();
            cnn.Close();
            return new EmptyResult();
        }

        // adjust ASSY (IS)
        [HttpPost]
        public ActionResult update_adjust_end_month_ASSY(tb_NEvent_ASSY edit_value)
        {
            string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["dbinlineEntities2"].ConnectionString;
            SqlConnection cnn = new SqlConnection(cnnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "sp_adjust_eomonth_problem";
            cmd.Parameters.AddWithValue("@MATCODE", edit_value.MATCODE);
            cmd.Parameters.AddWithValue("@ITEM_NUMBER", edit_value.ITEM_NUMBER);
            cmd.Parameters.AddWithValue("@COST_CENTER", edit_value.COST_CENTER);
            cmd.Parameters.AddWithValue("@REMAIN_INLINE", edit_value.REMAIN_INLINE);
            cmd.Parameters.AddWithValue("@FLAG_MAT", edit_value.FLAG_MAT);
            cmd.Parameters.AddWithValue("@UPDATE_BY", edit_value.UPDATE_BY);
            cmd.Parameters.AddWithValue("@REMAIN_INLINE_PROBLEM", edit_value.REMAIN_INLINE_PROBLEM);
            cmd.Parameters.AddWithValue("@COMMENT_AJDSUST_EOMONTH", edit_value.COMMENT_AJDSUST_EOMONTH);

            cnn.Open();
            object o = cmd.ExecuteNonQuery();
            cnn.Close();
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult update_adjust_end_month_moled(tb_NEvent_M_OLED edit_value)
        {
            string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["dbinlineEntities2"].ConnectionString;
            SqlConnection cnn = new SqlConnection(cnnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "sp_adjust_eomonth";
            cmd.Parameters.AddWithValue("@MATCODE", edit_value.MATCODE);
            cmd.Parameters.AddWithValue("@ITEM_NUMBER", edit_value.ITEM_NUMBER);
            cmd.Parameters.AddWithValue("@COST_CENTER", edit_value.COST_CENTER);
            cmd.Parameters.AddWithValue("@REMAIN_INLINE", edit_value.REMAIN_INLINE);
            cmd.Parameters.AddWithValue("@FLAG_MAT", edit_value.FLAG_MAT);
            cmd.Parameters.AddWithValue("@UPDATE_BY", edit_value.UPDATE_BY);

            cnn.Open();
            object o = cmd.ExecuteNonQuery();
            cnn.Close();
            return new EmptyResult();
        }


        [HttpPost]
        public ActionResult update_adjust_end_month_htps(tb_NEvent_HTPS edit_value)
        {
            string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["dbinlineEntities2"].ConnectionString;
            SqlConnection cnn = new SqlConnection(cnnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "sp_adjust_eomonth";
            cmd.Parameters.AddWithValue("@MATCODE", edit_value.MATCODE);
            cmd.Parameters.AddWithValue("@ITEM_NUMBER", edit_value.ITEM_NUMBER);
            cmd.Parameters.AddWithValue("@COST_CENTER", edit_value.COST_CENTER);
            cmd.Parameters.AddWithValue("@REMAIN_INLINE", edit_value.REMAIN_INLINE);
            cmd.Parameters.AddWithValue("@FLAG_MAT", edit_value.FLAG_MAT);
            cmd.Parameters.AddWithValue("@UPDATE_BY", edit_value.UPDATE_BY);

            cnn.Open();
            object o = cmd.ExecuteNonQuery();
            cnn.Close();
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult freez_inline_stock_htps()
        {
            string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["dbinlineEntities2"].ConnectionString;
            SqlConnection cnn = new SqlConnection(cnnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "sp_inline_snap_eomonth";
            cmd.Parameters.AddWithValue("@FLAG_MAT", "HTPS");
            cnn.Open();
            object o = cmd.ExecuteNonQuery();
            cnn.Close();
            return new EmptyResult();
            //string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["dbinlineEntities2"].ConnectionString;
            //SqlConnection cnn = new SqlConnection(cnnString);
            //SqlCommand cmd = new SqlCommand();
            //cmd.Connection = cnn;
            //cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //cmd.CommandText = "sp_inline_snap_eomonth";
            //cnn.Open();
            //object o = cmd.ExecuteNonQuery();
            //cnn.Close();
            //return new EmptyResult();
        }

        [HttpPost]
        public ActionResult export_excel_sanp()
        {
            string strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["dbinlineEntities2"].ConnectionString;
            var objConn = new SqlConnection(strConnString);
            var dt = new DataTable();

            SqlDataAdapter adapter = new SqlDataAdapter();
            //SqlCommand cmd = new SqlCommand("sp_inline_snap_eomonth", objConn);
            SqlCommand cmd = new SqlCommand("SELECT BIZ_NAME,PROCESS,MATCODE,ITEM_NUMBER,COST_CENTER,REMAIN_INLINE,UPDATE_DATE,LAST_SNAP FROM tb_SanpInline WHERE BIZ_NAME = 'HTPS' ", objConn);
            cmd.CommandType = CommandType.Text;
            adapter.SelectCommand = cmd;
            adapter.Fill(dt);

            string fileName = "Template_snap_inline.xls";
            FileInfo template = new FileInfo(Server.MapPath("~/Content/Template_snap_inline.xlsx"));
            ExcelPackage package = new ExcelPackage(template);
            FileContentResult data;
            var workbook = package.Workbook;

            int startRows = 3;
            //*** Sheet 1
            var worksheet2 = workbook.Worksheets["SNAP"];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                worksheet2.Cells["A" + (i + startRows)].Value = dt.Rows[i]["BIZ_NAME"].ToString();
                worksheet2.Cells["B" + (i + startRows)].Value = dt.Rows[i]["PROCESS"].ToString();
                worksheet2.Cells["C" + (i + startRows)].Value = dt.Rows[i]["MATCODE"].ToString();
                worksheet2.Cells["D" + (i + startRows)].Value = dt.Rows[i]["ITEM_NUMBER"].ToString();
                worksheet2.Cells["E" + (i + startRows)].Value = dt.Rows[i]["COST_CENTER"]?.ToString();
                worksheet2.Cells["F" + (i + startRows)].Value = dt.Rows[i]["REMAIN_INLINE"].ToString();
                worksheet2.Cells["G" + (i + startRows)].Value = dt.Rows[i]["UPDATE_DATE"].ToString();
                worksheet2.Cells["H" + (i + startRows)].Value = dt.Rows[i]["LAST_SNAP"].ToString();

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
        public ActionResult freez_inline_stock_assy()
        {
            string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["dbinlineEntities2"].ConnectionString;
            SqlConnection cnn = new SqlConnection(cnnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "sp_inline_snap_eomonth";
            cmd.Parameters.AddWithValue("@FLAG_MAT","ASSY");
            cnn.Open();
            object o = cmd.ExecuteNonQuery();
            cnn.Close();
            return new EmptyResult();
            //string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["dbinlineEntities2"].ConnectionString;
            //SqlConnection cnn = new SqlConnection(cnnString);
            //SqlCommand cmd = new SqlCommand();
            //cmd.Connection = cnn;
            //cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //cmd.CommandText = "sp_inline_snap_eomonth";
            //cnn.Open();
            //object o = cmd.ExecuteNonQuery();
            //cnn.Close();
            //return new EmptyResult();
        }

        [HttpPost]
        public ActionResult export_excel_sanp_assy()
        {
            string strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["dbinlineEntities2"].ConnectionString;
            var objConn = new SqlConnection(strConnString);
            var dt = new DataTable();

            SqlDataAdapter adapter = new SqlDataAdapter();
            //SqlCommand cmd = new SqlCommand("sp_inline_snap_eomonth", objConn);
            SqlCommand cmd = new SqlCommand("SELECT BIZ_NAME,PROCESS,MATCODE,ITEM_NUMBER,COST_CENTER,REMAIN_INLINE,UPDATE_DATE,LAST_SNAP FROM tb_SanpInline WHERE PROCESS = 'ASSY' ", objConn);
            cmd.CommandType = CommandType.Text;
            adapter.SelectCommand = cmd;
            adapter.Fill(dt);

            string fileName = "Template_snap_inline.xls";
            FileInfo template = new FileInfo(Server.MapPath("~/Content/Template_snap_inline.xlsx"));
            ExcelPackage package = new ExcelPackage(template);
            FileContentResult data;
            var workbook = package.Workbook;

            int startRows = 3;
            //*** Sheet 1
            var worksheet2 = workbook.Worksheets["SNAP"];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                worksheet2.Cells["A" + (i + startRows)].Value = dt.Rows[i]["BIZ_NAME"].ToString();
                worksheet2.Cells["B" + (i + startRows)].Value = dt.Rows[i]["PROCESS"].ToString();
                worksheet2.Cells["C" + (i + startRows)].Value = dt.Rows[i]["MATCODE"].ToString();
                worksheet2.Cells["D" + (i + startRows)].Value = dt.Rows[i]["ITEM_NUMBER"].ToString();
                worksheet2.Cells["E" + (i + startRows)].Value = dt.Rows[i]["COST_CENTER"]?.ToString();
                worksheet2.Cells["F" + (i + startRows)].Value = dt.Rows[i]["REMAIN_INLINE"].ToString();
                worksheet2.Cells["G" + (i + startRows)].Value = dt.Rows[i]["UPDATE_DATE"].ToString();
                worksheet2.Cells["H" + (i + startRows)].Value = dt.Rows[i]["LAST_SNAP"].ToString();

            }

            using (MemoryStream stream = new MemoryStream())
            {
                package.SaveAs(stream);
                var bytesdata = File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                data = bytesdata;
            }
            return Json(data, JsonRequestBehavior.AllowGet); //returning bytes of file data as json object
        } // end function export




    }// END
}
using inlineStock_rs.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace inlineStock_rs.Controllers
{
    public class AutomailController : Controller
    {
        MATERIAL_STOCKEntities1 db = new MATERIAL_STOCKEntities1(); // declare goaldbal Database

        // GET: Automail
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test_Email_assy()
        {
            var today = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            var result = db.tb_NEvent_ASSY.SqlQuery("Select * from tb_NEvent_ASSY").ToList<tb_NEvent_ASSY>();

            //List<sp_display_JUNB_Result> result = db.Database.SqlQuery<sp_display_JUNB_Result>("sp_display_JUNB").ToList();
            //var result = db.sp_master_table().ToList()
            //List<tb_NEvent_ASSY> list = db.tb_NEvent_ASSY().ToList();


            ////* Convert list to Datatable
            DataTable dt = new DataTable();
            PropertyInfo[] Props = typeof(tb_NEvent_ASSY).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                dt.Columns.Add(prop.Name);
            }
            foreach (tb_NEvent_ASSY e in result)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(e, null);
                }
                dt.Rows.Add(values);
            }

            //Fetching Email Body Text from Email Template File.  
            string FilePath = "D:\\mit\\Material\\InlineStock\\Views\\Automail\\vw_template_automail_assy.cshtml"; // path ที่วางไฟล์ template
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();

            var table = "";
            foreach (DataRow row in dt.Rows)
            {
                double remain = Convert.ToDouble(row["REMAIN_INLINE"]);
                if (remain < 0)
                {
                    table += "<tr><td>" + row["BIZ_NAME"] + "</td>";
                    table += "<td>" + row["FLAG_MAT"] + "</td>";
                    table += "<td>" + row["MATGROUP"] + "</td>";
                    table += "<td>" + row["MATCODE"] + "</td>";
                    table += "<td>" + row["MATNAME"] + "</td>";
                    table += "<td>" + row["COST_CENTER"] + "</td>";
                    table += "<td style='background:red; color='white';>" + row["REMAIN_INLINE"] + "</td></tr>";
                }
                else
                {
                    table += "<tr><td>" + row["BIZ_NAME"] + "</td>";
                    table += "<td>" + row["FLAG_MAT"] + "</td>";
                    table += "<td>" + row["MATGROUP"] + "</td>";
                    table += "<td>" + row["MATCODE"] + "</td>";
                    table += "<td>" + row["MATNAME"] + "</td>";
                    table += "<td>" + row["COST_CENTER"] + "</td>";
                    table += "<td>" + row["REMAIN_INLINE"] + "</td></tr>";
                }
            }
            // Critical
            var table_critical = "";
            var total_critical_count = 0;
            foreach (DataRow row in dt.Rows)
            {
                double remain = Convert.ToDouble(row["REMAIN_INLINE"]);
                if (remain < 0)
                {
                    total_critical_count = total_critical_count + 1;
                    table_critical += "<tr><td>" + row["BIZ_NAME"] + "</td>";
                    table_critical += "<td>" + row["FLAG_MAT"] + "</td>";
                    table_critical += "<td>" + row["MATGROUP"] + "</td>";
                    table_critical += "<td>" + row["MATCODE"] + "</td>";
                    table_critical += "<td>" + row["MATNAME"] + "</td>";
                    table_critical += "<td>" + row["COST_CENTER"] + "</td>";
                    table_critical += "<td style='background:red; color='white';>" + row["REMAIN_INLINE"] + "</td></tr>";
                }
            }
         
            /// }
            int total = dt.Rows.Count;
            int total_critical = dt.Rows.Count; //Critical

            //Repalce []
            MailText = MailText.Replace("[table]", table);
            MailText = MailText.Replace("[total]", total.ToString());

            //Critical
            MailText = MailText.Replace("[table_critical]", table_critical);
            MailText = MailText.Replace("[total_critical]", total_critical_count.ToString());

            var message = new MailMessage();
            message.To.Add(new MailAddress("Thatphon.Chutinan@ap.sony.com"));
            //IS
            //message.To.Add(new MailAddress("Nuttawat.Thitikamol@ap.sony.com")); // พี่บอย
            //message.To.Add(new MailAddress("Jarinya.Kommanee@ap.sony.com")); // แป้ง
            //message.To.Add(new MailAddress("Waranya.Meepetch@ap.sony.com")); // พี่ผึ้ง
            //message.To.Add(new MailAddress("kamonnat.phurat@ap.sony.com")); // พี่มินตรา
            //message.To.Add(new MailAddress("Montira.Lamai @ap.sony.com")); // น้องนิว

            message.From = new MailAddress("Thatphon.Chutinan@ap.sony.com", ""); //ผู้ส่ง
            message.Subject = "[IS-ASSY INLINE STOCK SYSTEM] REMAIN INLINE of " + today + " ";
            message.Body = MailText;   // ยัด Html ทั้งหมดลง Body
            message.IsBodyHtml = true; //ตั้งค่าให้ body ใน email แปล Tag html ได้
          
            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential();
                smtp.Credentials = credential;
                smtp.Host = "43.72.1.2";
                smtp.Port = 25;
                smtp.EnableSsl = false;
                smtp.Send(message);
            }
            return null;
        }

        public ActionResult Test_Email_junb()
        {
            var today = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            var result = db.tb_NEvent_JUNBS.SqlQuery("Select * from tb_NEvent_JUNBS").ToList<tb_NEvent_JUNBS>();

            //List<sp_display_JUNB_Result> result = db.Database.SqlQuery<sp_display_JUNB_Result>("sp_display_JUNB").ToList();
            //var result = db.sp_master_table().ToList()
            //List<tb_NEvent_JUNBS> list = db.tb_NEvent_JUNBS().ToList();


            ////* Convert list to Datatable
            DataTable dt = new DataTable();
            PropertyInfo[] Props = typeof(tb_NEvent_JUNBS).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                dt.Columns.Add(prop.Name);
            }
            foreach (tb_NEvent_JUNBS e in result)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(e, null);
                }
                dt.Rows.Add(values);
            }

            //Fetching Email Body Text from Email Template File.  
            string FilePath = "D:\\mit\\Material\\InlineStock\\Views\\Automail\\vw_template_automail_assy.cshtml"; // path ที่วางไฟล์ template
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();

            var table = "";
            foreach (DataRow row in dt.Rows)
            {
                double remain = Convert.ToDouble(row["REMAIN_INLINE"]);
                if (remain < 0)
                {
                    table += "<tr><td>" + row["BIZ_NAME"] + "</td>";
                    table += "<td>" + row["FLAG_MAT"] + "</td>";
                    table += "<td>" + row["MATGROUP"] + "</td>";
                    table += "<td>" + row["MATCODE"] + "</td>";
                    table += "<td>" + row["MATNAME"] + "</td>";
                    table += "<td>" + row["COST_CENTER"] + "</td>";
                    table += "<td style='background:red; color='white';>" + row["REMAIN_INLINE"] + "</td></tr>";
                }
                else
                {
                    table += "<tr><td>" + row["BIZ_NAME"] + "</td>";
                    table += "<td>" + row["FLAG_MAT"] + "</td>";
                    table += "<td>" + row["MATGROUP"] + "</td>";
                    table += "<td>" + row["MATCODE"] + "</td>";
                    table += "<td>" + row["MATNAME"] + "</td>";
                    table += "<td>" + row["COST_CENTER"] + "</td>";
                    table += "<td>" + row["REMAIN_INLINE"] + "</td></tr>";
                }
            }
            // Critical
            var table_critical = "";
            var total_critical_count = 0;
            foreach (DataRow row in dt.Rows)
            {
                double remain = Convert.ToDouble(row["REMAIN_INLINE"]);
                if (remain < 0)
                {
                    total_critical_count = total_critical_count + 1;
                    table_critical += "<tr><td>" + row["BIZ_NAME"] + "</td>";
                    table_critical += "<td>" + row["FLAG_MAT"] + "</td>";
                    table_critical += "<td>" + row["MATGROUP"] + "</td>";
                    table_critical += "<td>" + row["MATCODE"] + "</td>";
                    table_critical += "<td>" + row["MATNAME"] + "</td>";
                    table_critical += "<td>" + row["COST_CENTER"] + "</td>";
                    table_critical += "<td style='background:red; color='white';>" + row["REMAIN_INLINE"] + "</td></tr>";
                }
            }

            /// }
            int total = dt.Rows.Count;
            int total_critical = dt.Rows.Count; //Critical

            //Repalce []
            MailText = MailText.Replace("[table]", table);
            MailText = MailText.Replace("[total]", total.ToString());

            //Critical
            MailText = MailText.Replace("[table_critical]", table_critical);
            MailText = MailText.Replace("[total_critical]", total_critical_count.ToString());

            var message = new MailMessage();
            message.To.Add(new MailAddress("Thatphon.Chutinan@ap.sony.com"));
            //IS
            //message.To.Add(new MailAddress("Nuttawat.Thitikamol@ap.sony.com")); // พี่บอย
            //message.To.Add(new MailAddress("Jarinya.Kommanee@ap.sony.com")); // แป้ง
            //message.To.Add(new MailAddress("Waranya.Meepetch@ap.sony.com")); // พี่ผึ้ง
            //message.To.Add(new MailAddress("kamonnat.phurat@ap.sony.com")); // พี่มินตรา
            //message.To.Add(new MailAddress("Montira.Lamai @ap.sony.com")); // น้องนิว

            message.From = new MailAddress("Thatphon.Chutinan@ap.sony.com", ""); //ผู้ส่ง
            message.Subject = "[IS-JUNB INLINE STOCK SYSTEM] REMAIN INLINE of " + today + " ";
            message.Body = MailText;   // ยัด Html ทั้งหมดลง Body
            message.IsBodyHtml = true; //ตั้งค่าให้ body ใน email แปล Tag html ได้

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential();
                smtp.Credentials = credential;
                smtp.Host = "43.72.1.2";
                smtp.Port = 25;
                smtp.EnableSsl = false;
                smtp.Send(message);
            }
            return null;
        }

        public ActionResult Test_Email_csat()
        {
            var today = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            var result = db.tb_NEvent_CSAT.SqlQuery("Select * from tb_NEvent_CSAT").ToList<tb_NEvent_CSAT>();

            //List<sp_display_JUNB_Result> result = db.Database.SqlQuery<sp_display_JUNB_Result>("sp_display_JUNB").ToList();
            //var result = db.sp_master_table().ToList()
            //List<tb_NEvent_JUNBS> list = db.tb_NEvent_JUNBS().ToList();


            ////* Convert list to Datatable
            DataTable dt = new DataTable();
            PropertyInfo[] Props = typeof(tb_NEvent_CSAT).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                dt.Columns.Add(prop.Name);
            }
            foreach (tb_NEvent_CSAT e in result)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(e, null);
                }
                dt.Rows.Add(values);
            }

            //Fetching Email Body Text from Email Template File.  
            string FilePath = "D:\\mit\\Material\\InlineStock\\Views\\Automail\\vw_template_automail_assy.cshtml"; // path ที่วางไฟล์ template
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();

            var table = "";
            foreach (DataRow row in dt.Rows)
            {
                double remain = Convert.ToDouble(row["REMAIN_INLINE"]);
                if (remain < 0)
                {
                    table += "<tr><td>" + row["BIZ_NAME"] + "</td>";
                    table += "<td>" + row["FLAG_MAT"] + "</td>";
                    table += "<td>" + row["MATGROUP"] + "</td>";
                    table += "<td>" + row["MATCODE"] + "</td>";
                    table += "<td>" + row["MATNAME"] + "</td>";
                    table += "<td>" + row["COST_CENTER"] + "</td>";
                    table += "<td style='background:red; color='white';>" + row["REMAIN_INLINE"] + "</td></tr>";
                }
                else
                {
                    table += "<tr><td>" + row["BIZ_NAME"] + "</td>";
                    table += "<td>" + row["FLAG_MAT"] + "</td>";
                    table += "<td>" + row["MATGROUP"] + "</td>";
                    table += "<td>" + row["MATCODE"] + "</td>";
                    table += "<td>" + row["MATNAME"] + "</td>";
                    table += "<td>" + row["COST_CENTER"] + "</td>";
                    table += "<td>" + row["REMAIN_INLINE"] + "</td></tr>";
                }
            }
            // Critical
            var table_critical = "";
            var total_critical_count = 0;
            foreach (DataRow row in dt.Rows)
            {
                double remain = Convert.ToDouble(row["REMAIN_INLINE"]);
                if (remain < 0)
                {
                    total_critical_count = total_critical_count + 1;
                    table_critical += "<tr><td>" + row["BIZ_NAME"] + "</td>";
                    table_critical += "<td>" + row["FLAG_MAT"] + "</td>";
                    table_critical += "<td>" + row["MATGROUP"] + "</td>";
                    table_critical += "<td>" + row["MATCODE"] + "</td>";
                    table_critical += "<td>" + row["MATNAME"] + "</td>";
                    table_critical += "<td>" + row["COST_CENTER"] + "</td>";
                    table_critical += "<td style='background:red; color='white';>" + row["REMAIN_INLINE"] + "</td></tr>";
                }
            }

            /// }
            int total = dt.Rows.Count;
            int total_critical = dt.Rows.Count; //Critical

            //Repalce []
            MailText = MailText.Replace("[table]", table);
            MailText = MailText.Replace("[total]", total.ToString());

            //Critical
            MailText = MailText.Replace("[table_critical]", table_critical);
            MailText = MailText.Replace("[total_critical]", total_critical_count.ToString());

            var message = new MailMessage();
            message.To.Add(new MailAddress("Thatphon.Chutinan@ap.sony.com"));
            //IS
            //message.To.Add(new MailAddress("Nuttawat.Thitikamol@ap.sony.com")); // พี่บอย
            //message.To.Add(new MailAddress("Jarinya.Kommanee@ap.sony.com")); // แป้ง
            //message.To.Add(new MailAddress("Waranya.Meepetch@ap.sony.com")); // พี่ผึ้ง
            //message.To.Add(new MailAddress("kamonnat.phurat@ap.sony.com")); // พี่มินตรา
            //message.To.Add(new MailAddress("Montira.Lamai @ap.sony.com")); // น้องนิว

            message.From = new MailAddress("Thatphon.Chutinan@ap.sony.com", ""); //ผู้ส่ง
            message.Subject = "[IS-CSAT INLINE STOCK SYSTEM] REMAIN INLINE of " + today + " ";
            message.Body = MailText;   // ยัด Html ทั้งหมดลง Body
            message.IsBodyHtml = true; //ตั้งค่าให้ body ใน email แปล Tag html ได้

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential();
                smtp.Credentials = credential;
                smtp.Host = "43.72.1.2";
                smtp.Port = 25;
                smtp.EnableSsl = false;
                smtp.Send(message);
            }
            return null;
        }

        public ActionResult Test_Email_dicing()
        {
            var today = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            var result = db.tb_NEvent.SqlQuery("Select * from tb_NEvent").ToList<tb_NEvent>();

            //List<sp_display_JUNB_Result> result = db.Database.SqlQuery<sp_display_JUNB_Result>("sp_display_JUNB").ToList();
            //var result = db.sp_master_table().ToList()
            //List<tb_NEvent_JUNBS> list = db.tb_NEvent_JUNBS().ToList();


            ////* Convert list to Datatable
            DataTable dt = new DataTable();
            PropertyInfo[] Props = typeof(tb_NEvent).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                dt.Columns.Add(prop.Name);
            }
            foreach (tb_NEvent e in result)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(e, null);
                }
                dt.Rows.Add(values);
            }

            //Fetching Email Body Text from Email Template File.  
            string FilePath = "D:\\mit\\Material\\InlineStock\\Views\\Automail\\vw_template_automail_assy.cshtml"; // path ที่วางไฟล์ template
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();

            var table = "";
            foreach (DataRow row in dt.Rows)
            {
                double remain = Convert.ToDouble(row["REMAIN_INLINE"]);
                if (remain < 0)
                {
                    table += "<tr><td>" + row["BIZ_NAME"] + "</td>";
                    table += "<td>" + row["FLAG_MAT"] + "</td>";
                    table += "<td>" + row["MATGROUP"] + "</td>";
                    table += "<td>" + row["MATCODE"] + "</td>";
                    table += "<td>" + row["MATNAME"] + "</td>";
                    table += "<td>" + row["COST_CENTER"] + "</td>";
                    table += "<td style='background:red; color='white';>" + row["REMAIN_INLINE"] + "</td></tr>";
                }
                else
                {
                    table += "<tr><td>" + row["BIZ_NAME"] + "</td>";
                    table += "<td>" + row["FLAG_MAT"] + "</td>";
                    table += "<td>" + row["MATGROUP"] + "</td>";
                    table += "<td>" + row["MATCODE"] + "</td>";
                    table += "<td>" + row["MATNAME"] + "</td>";
                    table += "<td>" + row["COST_CENTER"] + "</td>";
                    table += "<td>" + row["REMAIN_INLINE"] + "</td></tr>";
                }
            }
            // Critical
            var table_critical = "";
            var total_critical_count = 0;
            foreach (DataRow row in dt.Rows)
            {
                double remain = Convert.ToDouble(row["REMAIN_INLINE"]);
                if (remain < 0)
                {
                    total_critical_count = total_critical_count + 1;
                    table_critical += "<tr><td>" + row["BIZ_NAME"] + "</td>";
                    table_critical += "<td>" + row["FLAG_MAT"] + "</td>";
                    table_critical += "<td>" + row["MATGROUP"] + "</td>";
                    table_critical += "<td>" + row["MATCODE"] + "</td>";
                    table_critical += "<td>" + row["MATNAME"] + "</td>";
                    table_critical += "<td>" + row["COST_CENTER"] + "</td>";
                    table_critical += "<td style='background:red; color='white';>" + row["REMAIN_INLINE"] + "</td></tr>";
                }
            }

            /// }
            int total = dt.Rows.Count;
            int total_critical = dt.Rows.Count; //Critical

            //Repalce []
            MailText = MailText.Replace("[table]", table);
            MailText = MailText.Replace("[total]", total.ToString());

            //Critical
            MailText = MailText.Replace("[table_critical]", table_critical);
            MailText = MailText.Replace("[total_critical]", total_critical_count.ToString());

            var message = new MailMessage();
            message.To.Add(new MailAddress("Thatphon.Chutinan@ap.sony.com"));
            //IS
            //message.To.Add(new MailAddress("Nuttawat.Thitikamol@ap.sony.com")); // พี่บอย
            //message.To.Add(new MailAddress("Jarinya.Kommanee@ap.sony.com")); // แป้ง
            //message.To.Add(new MailAddress("Waranya.Meepetch@ap.sony.com")); // พี่ผึ้ง
            //message.To.Add(new MailAddress("kamonnat.phurat@ap.sony.com")); // พี่มินตรา
            //message.To.Add(new MailAddress("Montira.Lamai @ap.sony.com")); // น้องนิว

            message.From = new MailAddress("Thatphon.Chutinan@ap.sony.com", ""); //ผู้ส่ง
            message.Subject = "[IS-DICING INLINE STOCK SYSTEM] REMAIN INLINE of " + today + " ";
            message.Body = MailText;   // ยัด Html ทั้งหมดลง Body
            message.IsBodyHtml = true; //ตั้งค่าให้ body ใน email แปล Tag html ได้

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential();
                smtp.Credentials = credential;
                smtp.Host = "43.72.1.2";
                smtp.Port = 25;
                smtp.EnableSsl = false;
                smtp.Send(message);
            }
            return null;
        }

        public ActionResult Test_Email_moled()
        {
            var today = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            var result = db.tb_NEvent_M_OLED.SqlQuery("Select * from tb_NEvent_M_OLED").ToList<tb_NEvent_M_OLED>();

            MemoryStream ms;
            string strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["dbinlineEntities2"].ConnectionString;
            var objConn = new SqlConnection(strConnString);
            var dt_attached = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("sp_export_miscell", objConn);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@process", edit_value.process);
            adapter.SelectCommand = cmd;
            adapter.Fill(dt_attached);

            //string fileNames_attached = "Inline_stock.xls";
            FileInfo template = new FileInfo(Server.MapPath("~/Content/Template_inline_miscell.xlsx"));
            ExcelPackage package = new ExcelPackage(template);
            //FileContentResult data;
            var workbook = package.Workbook;

            int startRows = 3;
            //*** Sheet 1
            var worksheet2 = workbook.Worksheets["Sheet1"];
            //ExcelWorksheet worksheet2 = package.Workbook.Worksheets.Add("Sheet 1");


            for (int i = 0; i < dt_attached.Rows.Count; i++)
            {
                worksheet2.Cells["A" + (i + startRows)].Value = dt_attached.Rows[i]["BIZ"].ToString();
                worksheet2.Cells["B" + (i + startRows)].Value = dt_attached.Rows[i]["CAT_TYPE"].ToString();
                worksheet2.Cells["C" + (i + startRows)].Value = dt_attached.Rows[i]["PROCESS"].ToString();
                worksheet2.Cells["D" + (i + startRows)].Value = dt_attached.Rows[i]["MATNAME"]?.ToString();
                worksheet2.Cells["E" + (i + startRows)].Value = dt_attached.Rows[i]["COST_CENTER"]?.ToString();
                worksheet2.Cells["F" + (i + startRows)].Value = dt_attached.Rows[i]["ITEM_NUMBER"].ToString();
                worksheet2.Cells["G" + (i + startRows)].Value = dt_attached.Rows[i]["GAP"].ToString();
                worksheet2.Cells["H" + (i + startRows)].Value = dt_attached.Rows[i]["GAP_AMOUNTH"].ToString();
                worksheet2.Cells["I" + (i + startRows)].Value = dt_attached.Rows[i]["GAP_QTY"].ToString();
                worksheet2.Cells["J" + (i + startRows)].Value = dt_attached.Rows[i]["MONTHLY_ISSUE"].ToString();
                worksheet2.Cells["K" + (i + startRows)].Value = dt_attached.Rows[i]["ACTUAL_ISSUE"].ToString();
                worksheet2.Cells["L" + (i + startRows)].Value = dt_attached.Rows[i]["DAILY_ISSUE"].ToString();
                worksheet2.Cells["M" + (i + startRows)].Value = dt_attached.Rows[i]["GAP_DAILY"].ToString();
                worksheet2.Cells["N" + (i + startRows)].Value = dt_attached.Rows[i]["GAP_AMOUNT_DAILY"].ToString();
                worksheet2.Cells["O" + (i + startRows)].Value = dt_attached.Rows[i]["GAP_QTY_DAILY"].ToString();

            }
            //save the excel to the stream
            ms = new MemoryStream(package.GetAsByteArray());


            ////* Convert list to Datatable
            DataTable dt = new DataTable();
            PropertyInfo[] Props = typeof(tb_NEvent_M_OLED).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                dt.Columns.Add(prop.Name);
            }
            foreach (tb_NEvent_M_OLED e in result)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(e, null);
                }
                dt.Rows.Add(values);
            }

            //Fetching Email Body Text from Email Template File.  
            string FilePath = "D:\\mit\\Material\\InlineStock\\Views\\Automail\\vw_template_automail_assy.cshtml"; // path ที่วางไฟล์ template
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();

            var table = "";
            foreach (DataRow row in dt.Rows)
            {
                double remain = Convert.ToDouble(row["REMAIN_INLINE"]);
                if (remain < 0)
                {
                    table += "<tr><td>" + row["BIZ_NAME"] + "</td>";
                    table += "<td>" + row["FLAG_MAT"] + "</td>";
                    table += "<td>" + row["MATGROUP"] + "</td>";
                    table += "<td>" + row["MATCODE"] + "</td>";
                    table += "<td>" + row["MATNAME"] + "</td>";
                    table += "<td>" + row["COST_CENTER"] + "</td>";
                    table += "<td style='background:red; color='white';>" + row["REMAIN_INLINE"] + "</td></tr>";
                }
                else
                {
                    table += "<tr><td>" + row["BIZ_NAME"] + "</td>";
                    table += "<td>" + row["FLAG_MAT"] + "</td>";
                    table += "<td>" + row["MATGROUP"] + "</td>";
                    table += "<td>" + row["MATCODE"] + "</td>";
                    table += "<td>" + row["MATNAME"] + "</td>";
                    table += "<td>" + row["COST_CENTER"] + "</td>";
                    table += "<td>" + row["REMAIN_INLINE"] + "</td></tr>";
                }
            }
            // Critical
            var table_critical = "";
            var total_critical_count = 0;
            foreach (DataRow row in dt.Rows)
            {
                double remain = Convert.ToDouble(row["REMAIN_INLINE"]);
                if (remain < 0)
                {
                    total_critical_count = total_critical_count + 1;
                    table_critical += "<tr><td>" + row["BIZ_NAME"] + "</td>";
                    table_critical += "<td>" + row["FLAG_MAT"] + "</td>";
                    table_critical += "<td>" + row["MATGROUP"] + "</td>";
                    table_critical += "<td>" + row["MATCODE"] + "</td>";
                    table_critical += "<td>" + row["MATNAME"] + "</td>";
                    table_critical += "<td>" + row["COST_CENTER"] + "</td>";
                    table_critical += "<td style='background:red; color='white';>" + row["REMAIN_INLINE"] + "</td></tr>";
                }
            }

            /// }
            int total = dt.Rows.Count;
            int total_critical = dt.Rows.Count; //Critical

            //Repalce []
            MailText = MailText.Replace("[table]", table);
            MailText = MailText.Replace("[total]", total.ToString());

            //Critical
            MailText = MailText.Replace("[table_critical]", table_critical);
            MailText = MailText.Replace("[total_critical]", total_critical_count.ToString());

            var message = new MailMessage();
            message.To.Add(new MailAddress("Thatphon.Chutinan@ap.sony.com"));
            //IS
            //message.To.Add(new MailAddress("Nuttawat.Thitikamol@ap.sony.com")); // พี่บอย
            //message.To.Add(new MailAddress("Jarinya.Kommanee@ap.sony.com")); // แป้ง
            //message.To.Add(new MailAddress("Waranya.Meepetch@ap.sony.com")); // พี่ผึ้ง
            //message.To.Add(new MailAddress("kamonnat.phurat@ap.sony.com")); // พี่มินตรา
            //message.To.Add(new MailAddress("Montira.Lamai @ap.sony.com")); // น้องนิว

            message.From = new MailAddress("Thatphon.Chutinan@ap.sony.com", ""); //ผู้ส่ง
            message.Subject = "[DD-M-OLED INLINE STOCK SYSTEM] REMAIN INLINE of " + today + " ";
            message.Body = MailText;   // ยัด Html ทั้งหมดลง Body
            message.IsBodyHtml = true; //ตั้งค่าให้ body ใน email แปล Tag html ได้
            message.Attachments.Add(new Attachment(ms, "Inline_stock_MOLED" + today + " .xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"));

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential();
                smtp.Credentials = credential;
                smtp.Host = "43.72.1.2";
                smtp.Port = 25;
                smtp.EnableSsl = false;
                smtp.Send(message);
            }
            return null;
        }

        public ActionResult vw_template_automail_assy()
        {
            return View();
        }

        public ActionResult vw_template_automail_moled()
        {
            return View();
        }

        //Miscell
        public ActionResult vw_template_automail_miscell()
        {
            return View();
        }
        public ActionResult Ajax_Get()
        {
            var data = db.vw_miscell_summary_email_is.ToList();
            //var data = db.vw_miscell_summary_email_other
            //                .Where(x => x.BIZ == "LSI")
            //                 .ToList();

            //var data = db.tb_information_issue_daily_report.ToList()
            //            .Select(x => new { x.BIZ, x.PROCESS, x.MATCODE, x.ITEM_NUMBER, x.MATNAME, x.MATGROUP, x.COST_CENTER, x.CAT_TYPE, x.GAP_AMOUNTH })
            //           //.Where(x => x.BIZ == "IS" && Convert.ToInt32(x.GAP_AMOUNTH) > 50000);int.TryParse(n, out temp2) ? temp2 : 0
            //           .Where(x => x.BIZ == "IS" && Convert.ToInt32(x.GAP_AMOUNTH) > 50000);

            //var result_lsi = db.tb_information_issue_daily_report.SqlQuery("SELECT [BIZ],[PROCESS],[MATCODE],[ITEM_NUMBER],[MATNAME],[MATGROUP],[COST_CENTER],[CAT_TYPE],[GAP_QTY],[GAP_AMOUNTH] FROM [MATERIAL_STOCK].[dbo].[tb_information_issue_daily_report] WHERE BIZ = 'IS' AND GAP_AMOUNTH > 50000").ToList();

            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Test_Email_alert_miscell()
        {
            var today = DateTime.Now.ToString("yyyy-MM-dd");
            var date_miscell_attached = DateTime.Now.ToString("yyyy-MM");


            MemoryStream ms;

            string strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["dbinlineEntities2"].ConnectionString;
            var objConn = new SqlConnection(strConnString);
            var dt_attached = new DataTable();

            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("sp_export_miscell", objConn);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@process", edit_value.process);
            adapter.SelectCommand = cmd;
            adapter.Fill(dt_attached);

            //string fileNames_attached = "Inline_stock.xls";
            FileInfo template = new FileInfo(Server.MapPath("~/Content/Template_inline_miscell.xlsx"));
            ExcelPackage package = new ExcelPackage(template);
            //FileContentResult data;
            var workbook = package.Workbook;

            int startRows = 3;
            //*** Sheet 1
            var worksheet2 = workbook.Worksheets["Sheet1"];
            //ExcelWorksheet worksheet2 = package.Workbook.Worksheets.Add("Sheet 1");


            for (int i = 0; i < dt_attached.Rows.Count; i++)
            {
                worksheet2.Cells["A" + (i + startRows)].Value = dt_attached.Rows[i]["BIZ"].ToString();
                worksheet2.Cells["B" + (i + startRows)].Value = dt_attached.Rows[i]["CAT_TYPE"].ToString();
                worksheet2.Cells["C" + (i + startRows)].Value = dt_attached.Rows[i]["PROCESS"].ToString();
                worksheet2.Cells["D" + (i + startRows)].Value = dt_attached.Rows[i]["MATNAME"]?.ToString();
                worksheet2.Cells["E" + (i + startRows)].Value = dt_attached.Rows[i]["COST_CENTER"]?.ToString();
                worksheet2.Cells["F" + (i + startRows)].Value = dt_attached.Rows[i]["ITEM_NUMBER"].ToString();
                worksheet2.Cells["G" + (i + startRows)].Value = dt_attached.Rows[i]["GAP"].ToString();
                worksheet2.Cells["H" + (i + startRows)].Value = dt_attached.Rows[i]["GAP_AMOUNTH"].ToString();
                worksheet2.Cells["I" + (i + startRows)].Value = dt_attached.Rows[i]["GAP_QTY"].ToString();
                worksheet2.Cells["J" + (i + startRows)].Value = dt_attached.Rows[i]["MONTHLY_ISSUE"].ToString();
                worksheet2.Cells["K" + (i + startRows)].Value = dt_attached.Rows[i]["ACTUAL_ISSUE"].ToString();
                worksheet2.Cells["L" + (i + startRows)].Value = dt_attached.Rows[i]["DAILY_ISSUE"].ToString();
                worksheet2.Cells["M" + (i + startRows)].Value = dt_attached.Rows[i]["GAP_DAILY"].ToString();
                worksheet2.Cells["N" + (i + startRows)].Value = dt_attached.Rows[i]["GAP_AMOUNT_DAILY"].ToString();
                worksheet2.Cells["O" + (i + startRows)].Value = dt_attached.Rows[i]["GAP_QTY_DAILY"].ToString();

            }
            //save the excel to the stream
            ms = new MemoryStream(package.GetAsByteArray());


            var result_lsi = db.vw_miscell_summary_email_other
                            .Where(x => x.BIZ == "LSI")
                            .ToList();

            var result_is = db.vw_miscell_summary_email_is.ToList();

            var result_htps = db.vw_miscell_summary_email_other
                           .Where(x => x.BIZ == "HTPS")
                           .ToList();

            var result_moled = db.vw_miscell_summary_email_other
                           .Where(x => x.BIZ == "M-OLED")
                           .ToList();

            var result_ffp = db.vw_miscell_summary_email_other
                        .Where(x => x.BIZ == "FFP")
                        .ToList();

            //* Convert list to Datatable LSI
            DataTable dt = new DataTable();
            PropertyInfo[] Props = typeof(vw_miscell_summary_email_other).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                dt.Columns.Add(prop.Name);
            }
            foreach (vw_miscell_summary_email_other e in result_lsi)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(e, null);
                }
                dt.Rows.Add(values);
            }

            ////* Convert list to Datatable IS
            DataTable dt_is = new DataTable();
            PropertyInfo[] Props_is = typeof(vw_miscell_summary_email_is).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props_is)
            {
                dt_is.Columns.Add(prop.Name);
            }

            foreach (vw_miscell_summary_email_is e in result_is)
            {
                var values = new object[Props_is.Length];
                for (int i = 0; i < Props_is.Length; i++)
                {
                    values[i] = Props_is[i].GetValue(e, null);
                }
                dt_is.Rows.Add(values);
            }

            ////* Convert list to Datatable HTPS
            DataTable dt_htps = new DataTable();
            PropertyInfo[] Props_hpts = typeof(vw_miscell_summary_email_other).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props_hpts)
            {
                dt_htps.Columns.Add(prop.Name);
            }

            foreach (vw_miscell_summary_email_other e in result_htps)
            {
                var values = new object[Props_hpts.Length];
                for (int i = 0; i < Props_hpts.Length; i++)
                {
                    values[i] = Props_hpts[i].GetValue(e, null);
                }
                dt_htps.Rows.Add(values);
            }

            ////* Convert list to Datatable M-OLED
            DataTable dt_moled = new DataTable();
            PropertyInfo[] Props_moled = typeof(vw_miscell_summary_email_other).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props_moled)
            {
                dt_moled.Columns.Add(prop.Name);
            }

            foreach (vw_miscell_summary_email_other e in result_moled)
            {
                var values = new object[Props_moled.Length];
                for (int i = 0; i < Props_moled.Length; i++)
                {
                    values[i] = Props_moled[i].GetValue(e, null);
                }
                dt_moled.Rows.Add(values);
            }

            ////* Convert list to Datatable BMS
            DataTable dt_bms = new DataTable();
            PropertyInfo[] Props_bms = typeof(vw_miscell_summary_email_other).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props_bms)
            {
                dt_bms.Columns.Add(prop.Name);
            }

            foreach (vw_miscell_summary_email_other e in result_moled)
            {
                var values = new object[Props_bms.Length];
                for (int i = 0; i < Props_bms.Length; i++)
                {
                    values[i] = Props_bms[i].GetValue(e, null);
                }
                dt_bms.Rows.Add(values);
            }

            ////* Convert list to Datatable FFP
            DataTable dt_ffp = new DataTable();
            PropertyInfo[] Props_ffp = typeof(vw_miscell_summary_email_other).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props_ffp)
            {
                dt_ffp.Columns.Add(prop.Name);
            }

            foreach (vw_miscell_summary_email_other e in result_ffp)
            {
                var values = new object[Props_ffp.Length];
                for (int i = 0; i < Props_ffp.Length; i++)
                {
                    values[i] = Props_ffp[i].GetValue(e, null);
                }
                dt_ffp.Rows.Add(values);
            }


            //Fetching Email Body Text from Email Template File.  
            string FilePath = "D:\\mit\\Material\\InlineStock\\Views\\Automail\\vw_template_automail_miscell.cshtml"; // path ที่วางไฟล์ template
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();

            // Critical LSI
            var table_critical_lsi = "";
            var total_critical_count_lsi = 0;
            foreach (DataRow row in dt.Rows)
            {
                if (dt.Rows.Count > 0)
                {                
                    string GAP_AMOUNT_LSI = row["CAT_TYPE"].ToString();
                     if (GAP_AMOUNT_LSI != "-")
                     {
                        total_critical_count_lsi = total_critical_count_lsi + 1;
                        table_critical_lsi += "<tr><td>" + row["CAT_TYPE"] + "</td>";
                        table_critical_lsi += "<td>" + row["MATGROUP"] + "</td>";
                        table_critical_lsi += "<td>" + row["PROCESS"] + "</td>";
                        table_critical_lsi += "<td>" + row["MATNAME"] + "</td>";
                        table_critical_lsi += "<td>" + row["COST_CENTER"] + "</td>";
                        table_critical_lsi += "<td>" + row["ITEM_NUMBER"] + "</td>";
                        table_critical_lsi += "<td style='background:red; color='white';>" + row["GAP_AMOUNTH"] + "</td></tr>";
                    }
                    else if (GAP_AMOUNT_LSI == "-")
                    {
                        total_critical_count_lsi = total_critical_count_lsi + 1;
                        table_critical_lsi += "<tr><td>" + row["CAT_TYPE"] + "</td>";
                        table_critical_lsi += "<td>" + row["MATGROUP"] + "</td>";
                        table_critical_lsi += "<td>" + row["PROCESS"] + "</td>";
                        table_critical_lsi += "<td>" + row["MATNAME"] + "</td>";
                        table_critical_lsi += "<td>" + row["COST_CENTER"] + "</td>";
                        table_critical_lsi += "<td>" + row["ITEM_NUMBER"] + "</td>";
                        table_critical_lsi += "<td style='background:red; color='white';>" + row["GAP_AMOUNTH"] + "</td></tr>";
                    }
                }
                else if(dt.Rows == null)
                {
                    total_critical_count_lsi = total_critical_count_lsi + 1;
                    table_critical_lsi += "<tr><td>No information</td>";
                    table_critical_lsi += "<td>No information</td>";
                    table_critical_lsi += "<td>No information</td>";
                    table_critical_lsi += "<td>No information</td>";
                    table_critical_lsi += "<td>No information</td>";
                    table_critical_lsi += "<td>No information</td>";
                    table_critical_lsi += "<td>No information</td>";
                }
            }

           //Critical_LSI
            //int total = dt.Rows.Count;
            MailText = MailText.Replace("[table_critical_lsi]", table_critical_lsi);
            MailText = MailText.Replace("[total_critical_lsi]", total_critical_count_lsi.ToString());

            // Critical IS
            var table_critical_is = "";
            var total_critical_count_is = 0;
            foreach (DataRow row_is in dt_is.Rows)
            {
                double GAP_AMOUNT_IS = Convert.ToDouble(row_is["GAP_AMOUNTH"]);
                if (dt_is.Rows.Count > 0)
                {
                    if (GAP_AMOUNT_IS > 0)
                    {
                        total_critical_count_is = total_critical_count_is + 1;
                        table_critical_is += "<tr><td>" + row_is["CAT_TYPE"] + "</td>";
                        table_critical_is += "<td>" + row_is["MATGROUP"] + "</td>";
                        table_critical_is += "<td>" + row_is["PROCESS"] + "</td>";
                        table_critical_is += "<td>" + row_is["MATNAME"] + "</td>";
                        table_critical_is += "<td>" + row_is["COST_CENTER"] + "</td>";
                        table_critical_is += "<td>" + row_is["ITEM_NUMBER"] + "</td>";
                        table_critical_is += "<td style='background:red; color='white';>" + row_is["GAP_AMOUNTH"] + "</td></tr>";
                    }
                }
                else if (dt_is.Rows.Count <= 0)
                {
                    total_critical_count_is = total_critical_count_is + 1;
                    table_critical_is += "<tr><td>No information</td>";
                    table_critical_is += "<td>No information</td>";
                    table_critical_is += "<td>No information</td>";
                    table_critical_is += "<td>No information</td>";
                    table_critical_is += "<td>No information</td>";
                    table_critical_is += "<td>No information</td>";
                    table_critical_is += "<td>No information</td>";
                }

            }

            //Critical_IS
            //int total = dt.Rows.Count;
            MailText = MailText.Replace("[table_critical_is]", table_critical_is);
            MailText = MailText.Replace("[total_critical_is]", total_critical_count_is.ToString());

            // Critical HTPS
            var table_critical_htps = "";
            var total_critical_count_htps = 0;
            foreach (DataRow row_htps in dt_htps.Rows)
            {
                if (dt_htps.Rows.Count > 0)
                {
                    string GAP_AMOUNT_HTPS = row_htps["CAT_TYPE"].ToString();
                    if (GAP_AMOUNT_HTPS != "-")
                    {
                        total_critical_count_htps = total_critical_count_htps + 1;
                        table_critical_htps += "<tr><td>" + row_htps["CAT_TYPE"] + "</td>";
                        table_critical_htps += "<td>" + row_htps["MATGROUP"] + "</td>";
                        table_critical_htps += "<td>" + row_htps["PROCESS"] + "</td>";
                        table_critical_htps += "<td>" + row_htps["MATNAME"] + "</td>";
                        table_critical_htps += "<td>" + row_htps["COST_CENTER"] + "</td>";
                        table_critical_htps += "<td>" + row_htps["ITEM_NUMBER"] + "</td>";
                        table_critical_htps += "<td style='background:red; color='white';>" + row_htps["GAP_AMOUNTH"] + "</td></tr>";
                    }
                    else if (GAP_AMOUNT_HTPS == "-")
                    {
                        total_critical_count_htps = total_critical_count_htps + 1;
                        table_critical_htps += "<tr><td>" + row_htps["CAT_TYPE"] + "</td>";
                        table_critical_htps += "<td>" + row_htps["MATGROUP"] + "</td>";
                        table_critical_htps += "<td>" + row_htps["PROCESS"] + "</td>";
                        table_critical_htps += "<td>" + row_htps["MATNAME"] + "</td>";
                        table_critical_htps += "<td>" + row_htps["COST_CENTER"] + "</td>";
                        table_critical_htps += "<td>" + row_htps["ITEM_NUMBER"] + "</td>";
                        table_critical_htps += "<td style='background:red; color='white';>" + row_htps["GAP_AMOUNTH"] + "</td></tr>";
                    }
                }
                else if (dt_htps.Rows == null)
                {
                    total_critical_count_htps = total_critical_count_htps + 1;
                    table_critical_htps += "<tr><td>No information</td>";
                    table_critical_htps += "<td>No information</td>";
                    table_critical_htps += "<td>No information</td>";
                    table_critical_htps += "<td>No information</td>";
                    table_critical_htps += "<td>No information</td>";
                    table_critical_htps += "<td>No information</td>";
                    table_critical_htps += "<td>No information</td>";
                }

            }

            //Critical_HTPS
            //int total = dt.Rows.Count;
            MailText = MailText.Replace("[table_critical_htps]", table_critical_htps);
            MailText = MailText.Replace("[total_critical_htps]", total_critical_count_htps.ToString());

            // Critical MOLED
            var table_critical_moled= "";
            var total_critical_count_moled = 0;
            foreach (DataRow row_moled in dt_moled.Rows)
            {
                if (dt_moled.Rows.Count > 0)
                {
                    string GAP_AMOUNT_MOLED = row_moled["CAT_TYPE"].ToString();
                    if (GAP_AMOUNT_MOLED != "-")
                    {
                        total_critical_count_moled = total_critical_count_moled + 1;
                        table_critical_moled += "<tr><td>" + row_moled["CAT_TYPE"] + "</td>";
                        table_critical_moled += "<td>" + row_moled["MATGROUP"] + "</td>";
                        table_critical_moled += "<td>" + row_moled["PROCESS"] + "</td>";
                        table_critical_moled += "<td>" + row_moled["MATNAME"] + "</td>";
                        table_critical_moled += "<td>" + row_moled["COST_CENTER"] + "</td>";
                        table_critical_moled += "<td>" + row_moled["ITEM_NUMBER"] + "</td>";
                        table_critical_moled += "<td style='background:red; color='white';>" + row_moled["GAP_AMOUNTH"] + "</td></tr>";
                    }
                    else if (GAP_AMOUNT_MOLED == "-")
                    {
                        total_critical_count_moled = total_critical_count_moled + 1;
                        table_critical_moled += "<tr><td>" + row_moled["CAT_TYPE"] + "</td>";
                        table_critical_moled += "<td>" + row_moled["MATGROUP"] + "</td>";
                        table_critical_moled += "<td>" + row_moled["PROCESS"] + "</td>";
                        table_critical_moled += "<td>" + row_moled["MATNAME"] + "</td>";
                        table_critical_moled += "<td>" + row_moled["COST_CENTER"] + "</td>";
                        table_critical_moled += "<td>" + row_moled["ITEM_NUMBER"] + "</td>";
                        table_critical_moled += "<td style='background:red; color='white';>" + row_moled["GAP_AMOUNTH"] + "</td></tr>";
                    }
                }
                else if (dt_moled.Rows == null)
                {
                    total_critical_count_moled = total_critical_count_htps + 1;
                    table_critical_moled += "<tr><td>No information</td>";
                    table_critical_moled += "<td>No information</td>";
                    table_critical_moled += "<td>No information</td>";
                    table_critical_moled += "<td>No information</td>";
                    table_critical_moled += "<td>No information</td>";
                    table_critical_moled += "<td>No information</td>";
                    table_critical_moled += "<td>No information</td>";
                }

            }

            //Critical_MOLED
            //int total = dt.Rows.Count;
            MailText = MailText.Replace("[table_critical_moled]", table_critical_moled);
            MailText = MailText.Replace("[total_critical_moled]", total_critical_count_moled.ToString());


            // Critical BMS
            var table_critical_bms = "";
            var total_critical_count_bms = 0;
            foreach (DataRow row_bms in dt_bms.Rows)
            {
                if (dt_bms.Rows.Count > 0)
                {
                    string GAP_AMOUNT_BMS = row_bms["CAT_TYPE"].ToString();
                    if (GAP_AMOUNT_BMS != "-")
                    {
                        total_critical_count_bms = total_critical_count_bms + 1;
                        table_critical_bms += "<tr><td>" + row_bms["CAT_TYPE"] + "</td>";
                        table_critical_bms += "<td>" + row_bms["MATGROUP"] + "</td>";
                        table_critical_bms += "<td>" + row_bms["PROCESS"] + "</td>";
                        table_critical_bms += "<td>" + row_bms["MATNAME"] + "</td>";
                        table_critical_bms += "<td>" + row_bms["COST_CENTER"] + "</td>";
                        table_critical_bms += "<td>" + row_bms["ITEM_NUMBER"] + "</td>";
                        table_critical_bms += "<td style='background:red; color='white';>" + row_bms["GAP_AMOUNTH"] + "</td></tr>";
                    }
                    else if (GAP_AMOUNT_BMS == "-")
                    {
                        total_critical_count_bms = total_critical_count_bms + 1;
                        table_critical_bms += "<tr><td>" + row_bms["CAT_TYPE"] + "</td>";
                        table_critical_bms += "<td>" + row_bms["MATGROUP"] + "</td>";
                        table_critical_bms += "<td>" + row_bms["PROCESS"] + "</td>";
                        table_critical_bms += "<td>" + row_bms["MATNAME"] + "</td>";
                        table_critical_bms += "<td>" + row_bms["COST_CENTER"] + "</td>";
                        table_critical_bms += "<td>" + row_bms["ITEM_NUMBER"] + "</td>";
                        table_critical_bms += "<td style='background:red; color='white';>" + row_bms["GAP_AMOUNTH"] + "</td></tr>";
                    }
                }
                else if (dt_bms.Rows == null)
                {
                    total_critical_count_bms = total_critical_count_bms + 1;
                    table_critical_bms += "<tr><td>No information</td>";
                    table_critical_bms += "<td>No information</td>";
                    table_critical_bms += "<td>No information</td>";
                    table_critical_bms += "<td>No information</td>";
                    table_critical_bms += "<td>No information</td>";
                    table_critical_bms += "<td>No information</td>";
                    table_critical_bms += "<td>No information</td>";
                }

            }

            //Critical_BMS
            //int total = dt.Rows.Count;
            MailText = MailText.Replace("[table_critical_bms]", table_critical_bms);
            MailText = MailText.Replace("[total_critical_bms]", total_critical_count_bms.ToString());

            // Critical FFP
            var table_critical_ffp = "";
            var total_critical_count_ffp = 0;
            foreach (DataRow row_ffp in dt_ffp.Rows)
            {
                if (dt_ffp.Rows.Count > 0)
                {
                    string GAP_AMOUNT_FFP = row_ffp["CAT_TYPE"].ToString();
                    if (GAP_AMOUNT_FFP != "-")
                    {
                        total_critical_count_ffp = total_critical_count_ffp + 1;
                        table_critical_ffp += "<tr><td>" + row_ffp["CAT_TYPE"] + "</td>";
                        table_critical_ffp += "<td>" + row_ffp["MATGROUP"] + "</td>";
                        table_critical_ffp += "<td>" + row_ffp["PROCESS"] + "</td>";
                        table_critical_ffp += "<td>" + row_ffp["MATNAME"] + "</td>";
                        table_critical_ffp += "<td>" + row_ffp["COST_CENTER"] + "</td>";
                        table_critical_ffp += "<td>" + row_ffp["ITEM_NUMBER"] + "</td>";
                        table_critical_ffp += "<td style='background:red; color='white';>" + row_ffp["GAP_AMOUNTH"] + "</td></tr>";
                    }
                    else if (GAP_AMOUNT_FFP == "-")
                    {
                        total_critical_count_ffp = total_critical_count_ffp + 1;
                        table_critical_ffp += "<tr><td>" + row_ffp["CAT_TYPE"] + "</td>";
                        table_critical_ffp += "<td>" + row_ffp["MATGROUP"] + "</td>";
                        table_critical_ffp += "<td>" + row_ffp["PROCESS"] + "</td>";
                        table_critical_ffp += "<td>" + row_ffp["MATNAME"] + "</td>";
                        table_critical_ffp += "<td>" + row_ffp["COST_CENTER"] + "</td>";
                        table_critical_ffp += "<td>" + row_ffp["ITEM_NUMBER"] + "</td>";
                        table_critical_ffp += "<td style='background:red; color='white';>" + row_ffp["GAP_AMOUNTH"] + "</td></tr>";
                    }
                }
                else if (dt_ffp.Rows == null)
                {
                    total_critical_count_ffp = total_critical_count_ffp + 1;
                    table_critical_ffp += "<tr><td>No information</td>";
                    table_critical_ffp += "<td>No information</td>";
                    table_critical_ffp += "<td>No information</td>";
                    table_critical_ffp += "<td>No information</td>";
                    table_critical_ffp += "<td>No information</td>";
                    table_critical_ffp += "<td>No information</td>";
                    table_critical_ffp += "<td>No information</td>";
                }

            }

            //Critical_FFP
            //int total = dt.Rows.Count;
            MailText = MailText.Replace("[table_critical_ffp]", table_critical_ffp);
            MailText = MailText.Replace("[total_critical_ffp]", total_critical_count_ffp.ToString());

            var date_month = DateTime.Now.ToString("MMMyy");

            var message = new MailMessage();
            message.To.Add(new MailAddress("Thatphon.Chutinan@ap.sony.com"));
            message.From = new MailAddress("Thatphon.Chutinan@ap.sony.com", ""); //ผู้ส่ง
            message.Subject = "[MATERIAL ISSUE DAILY REPORT] of " + date_month + " Actual";
            message.Body = MailText;   // ยัด Html ทั้งหมดลง Body
            message.IsBodyHtml = true; //ตั้งค่าให้ body ใน email แปล Tag html ได้
            message.Attachments.Add(new Attachment(ms, "Miscell_" + date_miscell_attached + " .xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"));

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential();
                smtp.Credentials = credential;
                smtp.Host = "43.72.1.2";
                smtp.Port = 25;
                smtp.EnableSsl = false;
                smtp.Send(message);
            }
            return null;
            ms.Dispose();


        }



    }
}
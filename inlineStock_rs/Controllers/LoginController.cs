using inlineStock_rs.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace inlineStock_rs.Controllers
{
    public class LoginController : Controller
    {
        MATERIAL_STOCKEntities1 db = new MATERIAL_STOCKEntities1(); // declare goaldbal Database

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Login_axis()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Tb_member_Inline objUser)
        {
            if (ModelState.IsValid)
            {
                using (MATERIAL_STOCKEntities1 db = new MATERIAL_STOCKEntities1())
                {
                    var obj = db.Tb_member_Inline.Where(a => a.GOALBAL_ID.Equals(objUser.GOALBAL_ID) && a.PASSWORD.Equals(objUser.PASSWORD)).FirstOrDefault();

                    if (obj != null)
                    {
                        Session["GOALBAL_ID"] = obj.GOALBAL_ID.ToString();
                        Session["PASSWORD"] = obj.PASSWORD.ToString();
                        Session["NAME_ENG"] = obj.NAME_ENG.ToString();
                        Session["SURENAME_ENG"] = obj.SURENAME_ENG.ToString();
                        Session["POSITION"] = obj.POSITION.ToString();
                        Session["PERMISSION"] = obj.PERMISSION.ToString();
                        Session["Level"] = obj.Level.ToString();

                        return RedirectToAction("Oparate_inline_route");
                        //return Json(new { data = obj }, JsonRequestBehavior.AllowGet);

                    }

                    else
                    {
                        ViewBag.Message = "";
                        return View();
                    }
                }
            }
            return View(objUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login_axis(Model_Search axis)
        {
            if (ModelState.IsValid)
            {
                using (MATERIAL_STOCKEntities1 db = new MATERIAL_STOCKEntities1())
                {
                    var obj_axis = db.sp_login_axis().Where(a => a.EMP_ID.Equals(axis.emp_id) && a.PASSWORD.Equals(axis.password_axis)).FirstOrDefault();
                    //var obj_axis = db.sp_login_axis().Where(a => a.EMP_ID.Equals(axis.emp_id)).FirstOrDefault();
                    if (obj_axis != null)
                    {
                        Session["GOALBAL_ID"] = obj_axis.EMP_ID.ToString();
                        Session["PASSWORD"] = obj_axis.PASSWORD.ToString();
                        Session["NAME_ENG"] = obj_axis.EMP_NAME.ToString();
                        Session["SURENAME_ENG"] = obj_axis.EMP_NAME.ToString();
                        Session["POSITION"] = obj_axis.DEPARTMENT_CODE.ToString();
                        Session["PERMISSION"] = "AXIS";
                        Session["Level"] = "1";
                        return RedirectToAction("overview_records", "InlineStock");
                        //return Json(new { data = obj_axis }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        ViewBag.Message = "";
                        return View();
                    }
                }
            }
            return View(axis);
        }


        public ActionResult Oparate_inline_route()
        {
            if (Session["GOALBAL_ID"] != null && Session["PERMISSION"].ToString() == "Dicing")
            {
                return RedirectToAction("Oparate_inline_dicing", "InlineStock");
            }
            else if (Session["GOALBAL_ID"] != null && Session["PERMISSION"].ToString() == "ASSY")
            {
                return RedirectToAction("Oparate_inline_assy_menu_tablet", "InlineStock");
            }
            else if (Session["GOALBAL_ID"] != null && Session["PERMISSION"].ToString() == "JUNB")
            {
                return RedirectToAction("Oparate_inline_junb", "InlineStock");
            }
            else if (Session["GOALBAL_ID"] != null && Session["PERMISSION"].ToString() == "CSAT")
            {
                return RedirectToAction("Oparate_inline_csat", "InlineStock");
            }
            else if (Session["GOALBAL_ID"] != null && Session["PERMISSION"].ToString() == "HTPS")
            {
                return RedirectToAction("Oparate_inline_htps", "InlineStock");
            }
            else if (Session["GOALBAL_ID"] != null && Session["PERMISSION"].ToString() == "MOLED")
            {
                return RedirectToAction("Oparate_inline_moled", "InlineStock");
            }
            else if (Session["GOALBAL_ID"] != null && Session["PERMISSION"].ToString() == "CG")
            {
                return RedirectToAction("Oparate_inline_cg", "InlineStock");
            }
            else if (Session["GOALBAL_ID"] != null && Session["PERMISSION"].ToString() == "AXIS")
            {
                return RedirectToAction("overview_records", "InlineStock");
            }
            else
            {
                return View("Login");
            }
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Login");
        }

        //public ActionResult axis_login()
        //{
        //    var data = db.sp_login_axis().ToList().Where(x => x.EMP_ID == "206385");

        //    var s = Session["GOALBAL_ID"] = db.sp_login_axis().ToList().Where(x => x.EMP_ID == "206385");
        //    return RedirectToAction("Oparate_inline_junb_view", "InlineStock");
        //}

        //[HttpPost]
        //public ActionResult axis_login(Model_Search login)
        //{
        //    string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["dbinlineEntities2"].ConnectionString;
        //    SqlConnection cnn = new SqlConnection(cnnString);
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.Connection = cnn;
        //    cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //    cmd.CommandText = "sp_login_axis";
        //    cmd.Parameters.AddWithValue("@AXISID", login.emp_id);
        //    cnn.Open();
        //    object o = cmd.ExecuteNonQuery();
        //    cnn.Close();


        //    return new EmptyResult();
        // }


    }
}
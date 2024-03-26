using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JainamDemo.Models;
using System.Data;
using System.Collections;
using System.IO;

namespace JainamDemo.Controllers
{
    public class HomeController : Controller
    {
        SqlConnection cn = new SqlConnection();
        string dbname = "";

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Branch()
        {

            return View();
        }
        public class Dblist
        {
            public string dbname { get; set; }
        }
        private List<BranchModel> GetBranchlist()
        {
            try
            {
                List<BranchModel> branchs = new List<BranchModel>();
                DataTable dt = Returntable("select * from Branch_Master where Active=1", "Demo1");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    BranchModel branch = new BranchModel();
                    branch.Branch_Code = dt.Rows[i]["BranchCode"].ToString();
                    branch.Branch_Name = dt.Rows[i]["BranchName"].ToString();
                    branch.Branch_Addr = dt.Rows[i]["BranchAddress"].ToString();
                    branch.Branch_Type = dt.Rows[i]["BranchType"].ToString();
                    branch.Branch_InitDate = (DateTime)dt.Rows[i]["Branchdate"];
                    branch.Branch_Logo = dt.Rows[i]["Branchlogo"].ToString();
                    branch.Branch_Db = "Demo1";
                    branchs.Add(branch);
                }

                DataTable dts = Returntable("select * from Branch_Master where Active=1", "Demo2");

                for (int iw = 0; iw < dts.Rows.Count; iw++)
                {
                    BranchModel branch = new BranchModel();
                    branch.Branch_Code = dts.Rows[iw]["BranchCode"].ToString();
                    branch.Branch_Name = dts.Rows[iw]["BranchName"].ToString();
                    branch.Branch_Addr = dts.Rows[iw]["BranchAddress"].ToString();
                    branch.Branch_Type = dts.Rows[iw]["BranchType"].ToString();
                    branch.Branch_InitDate = (DateTime)dts.Rows[iw]["Branchdate"];
                    branch.Branch_Logo = dts.Rows[iw]["Branchlogo"].ToString();
                    branch.Branch_Db = "Demo2";
                    branchs.Add(branch);
                }

                return branchs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult Edit(int ID, string branch_db)
        {

            DataTable dt = Returntable("select * from Branch_Master where Active=1 and BranchCode=" + ID + "", branch_db);
            List<BranchModel> branches = new List<BranchModel>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                BranchModel branch = new BranchModel();
                branch.Branch_Code = dt.Rows[i]["BranchCode"].ToString();
                branch.Branch_Name = dt.Rows[i]["BranchName"].ToString();
                branch.Branch_Addr = dt.Rows[i]["BranchAddress"].ToString();
                branch.Branch_Type = dt.Rows[i]["BranchType"].ToString();
                branch.Branch_InitDate = (DateTime)dt.Rows[i]["Branchdate"];
                branch.Branch_Logo = dt.Rows[i]["Branchlogo"].ToString();
                branch.Branch_Db = branch_db;
                branch.IsActive = true;
                branches.Add(branch);
            }
            return View("~/Views/Home/Branch.cshtml", branches);
        }


        public ActionResult BranchList()
        {
            List<BranchModel> Branchlist = GetBranchlist();
            ViewBag.Branchlist = Branchlist;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        public JsonResult Save(FormCollection formData)
        {
            Resultpass<object> result = new Resultpass<object>();
            try
            {
                var BranchCode = Request.Form["BranchCode"];
                var BranchName = Request.Form["BranchName"];
                var BranchAddress = Request.Form["BranchAddress"];
                var BranchType = Request.Form["BranchType"];
                var Active = Request.Form["Active"];
                var BranchDb = Request.Form["BranchDb"];
                var Branchdate = Request.Form["Branchdate"];
                string BranclogoImagePath = null;
                // Retrieve files
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    BranclogoImagePath = BranchCode + "_" + BranchDb + ".png";
                    HttpPostedFileBase file = Request.Files[i];
                    SaveFile(file, BranclogoImagePath);
                    // Process each file as needed
                }
                if (BranclogoImagePath == null)
                {
                    BranclogoImagePath = Request.Form["modifiedSrc"];
                }
                dbname = BranchDb;
                Hashtable ht = new Hashtable();
                ht.Add("BranchCode", BranchCode);
                ht.Add("BranchName", BranchName);
                ht.Add("BranchAddress", BranchAddress);
                ht.Add("BranchType", BranchType);
                ht.Add("Branchdate", Branchdate);
                ht.Add("Branchlogo", BranclogoImagePath);
                ht.Add("Active", Active);
                excute("USP_SavebranchMaster", ht);
                result.opstatus = true;
                result.opmessage = "0";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
                result.opstatus = false;
                result.opmessage = "0";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        private void SaveFile(HttpPostedFileBase file, string fileName)
        {
            if (file != null && file.ContentLength > 0)
            {
                // Example: Save file to server
                var path = Path.Combine(Server.MapPath("~/images"), fileName);
                file.SaveAs(path);
            }
        }
        public class Resultpass<T>
        {
            public bool opstatus { get; set; }
            public string opmessage { get; set; }
        }

        public Int32 excute(string sql, Hashtable param)
        {
            cn.ConnectionString = "Data Source=DESKTOP-38OCLA6;Initial Catalog=" + dbname + ";Integrated Security=True";
            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (DictionaryEntry entry in param)
            {
                cmd.Parameters.AddWithValue("@" + entry.Key, entry.Value);
            }
            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
            return 0;
        }
        public DataTable Returntable(string sql, string dbnamelist)
        {
            DataTable dt = null;
            cn.ConnectionString = "Data Source=DESKTOP-38OCLA6;Initial Catalog=" + dbnamelist + ";Integrated Security=True";
            SqlDataAdapter adp = new SqlDataAdapter(sql, cn);
            cn.Open();
            DataSet ds = new DataSet();
            adp.Fill(ds);
            cn.Close();
            dt = ds.Tables[0];
            return dt;
        }
        [HttpPost]
        public JsonResult Deletebranch(string id,string BranchDb)
        {
            dbname = BranchDb;
            Hashtable ht = new Hashtable();
            ht.Add("BranchCode", id);
            excute("USP_DeletebranchMaster", ht);

            return Json("Delete", JsonRequestBehavior.AllowGet);
        }
    }
}
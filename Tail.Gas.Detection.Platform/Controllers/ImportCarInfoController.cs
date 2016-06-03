using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Tail.Gas.Detection.Platform.Authorize;
using System.IO;

namespace Tail.Gas.Detection.Platform.Controllers
{
    public class ImportCarInfoController : BaseController
    {
        //
        // GET: /ImportCarInfo/
        public readonly static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [AuthorizeFilterAttribute]
        public ActionResult Index()
        {
            if ("管理员".Equals(System.Web.HttpContext.Current.Session["usertype"]))
            {
                return View();
            }
            else
            {
                return new RedirectResult(ApplicationPath + "/home/forbidden");
            }
            
            
        }

        [HttpPost]
        public string UpFile()
        {
            JObject joResult = new JObject();
            string filepath = "";
            try
            {
               var file = Request.Files[0];
               filepath = System.AppDomain.CurrentDomain.BaseDirectory + Guid.NewGuid() + ".xls";
               file.SaveAs(filepath);
               string connString = "server = (local); uid = sa; pwd = sa; database = cardb";
               TransferData(filepath, "CarInfo", connString);
               joResult["result"] = 0;
            }
            catch (Exception ex)
            {
               logger.Error(ex);
               joResult["result"] = 1;
            }
            if (System.IO.File.Exists(filepath))
            {
                System.IO.File.Delete(filepath);
            }
            return joResult.ToString();
        }

        public void TransferData(string excelFile, string sheetName, string connectionString)
        {
            DataSet ds = new DataSet();
            try
            {
                //获取全部数据     
                string strConn = "Provider = Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + excelFile + ";" + "Extended Properties = Excel 8.0;";
                OleDbConnection conn = new OleDbConnection(strConn);
                conn.Open();
                string strExcel = "";
                OleDbDataAdapter myCommand = null;
                strExcel = string.Format("select * from [{0}$]", "Sheet1");
                myCommand = new OleDbDataAdapter(strExcel, strConn);
                myCommand.Fill(ds, sheetName);
                conn.Close();
                DataTable dt = ds.Tables[0];
                using (SqlBulkCopy sqlRevdBulkCopy = new SqlBulkCopy(connectionString))//引用SqlBulkCopy  
                    {
                        sqlRevdBulkCopy.DestinationTableName = sheetName;//数据库中对应的表名  

                        sqlRevdBulkCopy.NotifyAfter = dt.Rows.Count;//有几行数据  
                        //指定源列和目标列之间的对应关系
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            sqlRevdBulkCopy.ColumnMappings.Add(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName);
                        }
                        sqlRevdBulkCopy.WriteToServer(dt);//数据导入数据库  

                        sqlRevdBulkCopy.Close();//关闭连接  
                    }  

            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw ex;
            }
        }
      
    }
}

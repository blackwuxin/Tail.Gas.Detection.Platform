using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using Tail.Gas.Detection.Platform.Models;

namespace Tail.Gas.Detection.Platform.Dao
{
    public class CarStatusInfoDao
    {
        public readonly static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
        public static IList<CarStatusInfo> GetAll()
        {
            using (cardbEntities cardb = new cardbEntities())
            {
                 IList<CarStatusInfo> carstatusinfos = cardb.CarStatusInfo.ToList<CarStatusInfo>();
                 return carstatusinfos;
           }
           
        }

        public static void InsertCarStatusInfo(CarStatusInfo carstatusinfo)
        {
            using (cardbEntities cardb = new cardbEntities())
            {
                cardb.CarStatusInfo.Add(carstatusinfo);
                cardb.SaveChanges();
            }
        }

        public static void GetNormalListByPage(string CarNo,string Category, string Belong,  ref JArray messagelist)
        {

            try
            {
                string sql = "select CarNo,Color,Category,Belong,TemperatureBefore,SensorNum,SystemStatus,Data_LastChangeTime from ";
                string table = @"select b.CarNo,a.Color,a.Category,a.Belong,b.TemperatureBefore,b.SensorNum,b.SystemStatus,b.Data_LastChangeTime
                          from carinfo a,carstatusinfo b
                          where b.SystemStatus = 0 and a.no = b.carno ";
                string sqlwhere = "";
                if (!string.IsNullOrEmpty(Category))
                {
                    sqlwhere += "and a.Category = '" + Category + "'";
                }
                if (!string.IsNullOrEmpty(Belong))
                {
                    sqlwhere += "and a.Belong ='" + Belong + "'";
                }
                if (!string.IsNullOrEmpty(CarNo))
                {
                    sqlwhere += "and a.no ='" + CarNo + "'";
                }

                string excutesql = sql + "(" + table + sqlwhere + ") c where not exists(select 1 from (" + table + sqlwhere + ") d where CarNo = c.CarNo and Data_LastChangeTime > c.Data_LastChangeTime)";

                using (cardbEntities cardb = new cardbEntities())
                {
                    //var query =from a in cardb.CarInfo
                    //             join ar in cardb.CarStatusInfo
                    //             on a.NO equals ar.CarNo
                    //             where ar.SystemStatus == 0
                    //             where a.Category.Contains(Category)
                    //             where a.Belong.Contains(Belong)
                    //             select new CarStatus
                    //             {
                    //                 CarNo = a.NO,
                    //                 Color = a.Color,
                    //                 Category = a.Category,
                    //                 Belong = a.Belong,
                    //                 TemperatureBefore = ar.TemperatureBefore,
                    //                 SensorNum = ar.SensorNum,
                    //                 SystemStatus = ar.SystemStatus,
                    //                 Data_LastChangeTime = ar.Data_LastChangeTime
                    //             });
                    var query = cardb.Database.SqlQuery<CarStatus>(excutesql);

                    foreach (var item in query)
                    {
                        JObject joRow = new JObject();
                        joRow["CarNo"] = item.CarNo;
                        joRow["Color"] = item.Color;
                        joRow["Category"] = item.Category;
                        joRow["Belong"] = item.Belong;
                        joRow["TemperatureBefore"] = item.TemperatureBefore;
                        joRow["SensorNum"] = item.SensorNum;
                        joRow["SystemStatus"] = item.SystemStatus;
                        joRow["Data_LastChangeTime"] = item.Data_LastChangeTime;
                        messagelist.Add(joRow);
                    }
                }
                logger.Info("GetNormalListByPage Success");
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            
        }

        public static void GetErrorListByPage(string CarNo,string Category, string Belong, ref JArray messagelist)
        {

            try
            {
                string sql = "select CarNo,Color,Category,Belong,TemperatureBefore,SensorNum,SystemStatus,Data_LastChangeTime from ";
                string table = @"select b.CarNo,a.Color,a.Category,a.Belong,b.TemperatureBefore,b.SensorNum,b.SystemStatus,b.Data_LastChangeTime
                          from carinfo a,carstatusinfo b
                          where b.SystemStatus != 0 and a.no = b.carno ";
                string sqlwhere = "";
                if (!string.IsNullOrEmpty(Category))
                {
                    sqlwhere += "and a.Category = '" + Category + "'";
                }
                if (!string.IsNullOrEmpty(Belong))
                {
                    sqlwhere += "and a.Belong ='" + Belong + "'";
                }
                if (!string.IsNullOrEmpty(CarNo))
                {
                    sqlwhere += "and a.no ='" + CarNo + "'";
                }

                string excutesql = sql + "(" + table + sqlwhere + ") c where not exists(select 1 from (" + table + sqlwhere + ") d where CarNo = c.CarNo and Data_LastChangeTime > c.Data_LastChangeTime)";

                using (cardbEntities cardb = new cardbEntities())
                {
                    //var query =from a in cardb.CarInfo
                    //             join ar in cardb.CarStatusInfo
                    //             on a.NO equals ar.CarNo
                    //             where ar.SystemStatus == 0
                    //             where a.Category.Contains(Category)
                    //             where a.Belong.Contains(Belong)
                    //             select new CarStatus
                    //             {
                    //                 CarNo = a.NO,
                    //                 Color = a.Color,
                    //                 Category = a.Category,
                    //                 Belong = a.Belong,
                    //                 TemperatureBefore = ar.TemperatureBefore,
                    //                 SensorNum = ar.SensorNum,
                    //                 SystemStatus = ar.SystemStatus,
                    //                 Data_LastChangeTime = ar.Data_LastChangeTime
                    //             });
                    var query = cardb.Database.SqlQuery<CarStatus>(excutesql);

                    foreach (var item in query)
                    {
                        JObject joRow = new JObject();
                        joRow["CarNo"] = item.CarNo;
                        joRow["Color"] = item.Color;
                        joRow["Category"] = item.Category;
                        joRow["Belong"] = item.Belong;
                        joRow["TemperatureBefore"] = item.TemperatureBefore;
                        joRow["SensorNum"] = item.SensorNum;
                        joRow["SystemStatus"] = item.SystemStatus;
                        joRow["Data_LastChangeTime"] = item.Data_LastChangeTime;
                        messagelist.Add(joRow);
                    }
                }
                logger.Info("GetErrorListByPage Success");
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

        }
       

        public class CarStatus
        {
            public string CarNo{get;set;}
            public string Color { get; set; }
            public string Category { get; set; }
            public string Belong { get; set; }
            public decimal? TemperatureBefore { get; set; }
            public decimal? SensorNum { get; set; }
            public int? SystemStatus { get; set; }
            public DateTime? Data_LastChangeTime { get; set; }
        }
    }
}
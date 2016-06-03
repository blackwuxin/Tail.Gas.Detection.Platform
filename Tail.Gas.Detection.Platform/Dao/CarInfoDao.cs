using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tail.Gas.Detection.Platform.Models;

namespace Tail.Gas.Detection.Platform.Dao
{
    public class CarInfoDao
    {
        public readonly static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static IList<CarInfo> GetAlltest()
        {
            cardbEntities cardb = new cardbEntities();
            IList<CarInfo> carinfos = cardb.CarInfo.ToList<CarInfo>();
            return carinfos;
            
        }

        public static void InsertCarInfo(CarInfo carinfo)
        {
            using (cardbEntities cardb = new cardbEntities())
            {
                cardb.CarInfo.Add(carinfo);
                cardb.SaveChanges();
            }
        }

        public static CarInfo GetCardInfoByCarNo(string carno)
        {
            try
            {
                using (cardbEntities cardb = new cardbEntities())
                {
                    return cardb.CarInfo.FirstOrDefault(x => x.NO == carno);
                }
                logger.Info("GetCardInfoByCarNo Success");
            }
            catch (Exception ex)
            {
                logger.Error("GetCardInfoByCarNo",ex);
                return null;
            }
           
        }
    }
}
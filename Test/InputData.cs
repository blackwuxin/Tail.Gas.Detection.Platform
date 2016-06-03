using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class InputData
    {
        public readonly static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static cardbEntities cardb = new cardbEntities();
        public string InputData2(string data)
        {
            try
            {
                cardbEntities cardb = new cardbEntities();
                InputCarStatusInfo iputcarstatusinfo = new InputCarStatusInfo();
                iputcarstatusinfo.cardstatusinfo = data;
                iputcarstatusinfo.DataChange_LastTime = DateTime.Now;
                cardb.InputCarStatusInfo.Add(iputcarstatusinfo);
                cardb.SaveChanges();
                return "success";
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return "error";
            }
        }

        public string InputCheckData(string data)
        {
            try
            {

                if (string.IsNullOrEmpty(data) || (data.Length != 84))
                {
                    return "error";
                }
                if (!("55").Equals(data.Substring(0, 2)))
                {
                    return "error";
                }
                if (!"AA".Equals(data.Substring(2, 2)))
                {
                    return "error";
                }
                InputData2(data);
                logger.Info("data:" + data);

                CarStatusInfo carstatus = new CarStatusInfo();
                carstatus.ProvinceNo = CheckProvience(data.Substring(10, 2));
                carstatus.RegionNo = Chr(Convert.ToInt32(data.Substring(12, 2), 16));
                carstatus.CarNo = carstatus.ProvinceNo + carstatus.RegionNo + Chr(Convert.ToInt32(data.Substring(14, 2), 16)) + Chr(Convert.ToInt32(data.Substring(16, 2), 16)) + Chr(Convert.ToInt32(data.Substring(18, 2), 16))
                    + Chr(Convert.ToInt32(data.Substring(20, 2), 16)) + Chr(Convert.ToInt32(data.Substring(22, 2), 16));
                carstatus.Color = CheckColor(data.Substring(24, 2));
                carstatus.Speed = Convert.ToInt32(data.Substring(26, 2), 16);
                carstatus.PositionXDegree = Convert.ToInt32(data.Substring(38, 2), 16);
                carstatus.PositionXM = Convert.ToInt32(data.Substring(40, 2), 16);
                carstatus.PositionXS = Convert.ToInt32(data.Substring(42, 2), 16) * 256 + Convert.ToInt32(data.Substring(44, 2), 16);
                carstatus.PositionYDegree = Convert.ToInt32(data.Substring(46, 2), 16);
                carstatus.PositionYM = Convert.ToInt32(data.Substring(48, 2), 16);
                carstatus.PositionYS = Convert.ToInt32(data.Substring(50, 2), 16) * 256 + Convert.ToInt32(data.Substring(52, 2), 16);
                carstatus.TemperatureBefore = Convert.ToInt32(data.Substring(54, 2), 16) * 256 + Convert.ToInt32(data.Substring(56, 2), 16);
                carstatus.TemperatureAfter = Convert.ToInt32(data.Substring(58, 2), 16) * 256 + Convert.ToInt32(data.Substring(60, 2), 16);
                carstatus.SensorNum = Convert.ToDecimal(Convert.ToInt32(data.Substring(62, 2), 16) / 10.0);
                carstatus.LiquidHeight = Convert.ToInt32(data.Substring(64, 2), 16) == 255? 0:Convert.ToInt32(data.Substring(64, 2), 16);
                carstatus.SystemStatus = Convert.ToInt32(data.Substring(68, 2), 16);
                carstatus.Data_CreateTime = DateTime.Now;
                carstatus.Data_LastChangeTime = DateTime.Now;

                cardb.CarStatusInfo.Add(carstatus);
                cardb.SaveChanges();

 
                return "success";
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return "error";
            }

        }

        public static string Chr(int asciiCode)
        {
            if (asciiCode >= 0 && asciiCode <= 255)
            {
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                byte[] byteArray = new byte[] { (byte)asciiCode };
                string strCharacter = asciiEncoding.GetString(byteArray);
                return (strCharacter);
            }
            else
            {
                throw new Exception("ASCII Code is not valid.");
            }
        }
        public static string CheckColor(string color)
        {
            if ("00".Equals(color))
            {
                return "蓝";
            }
            else if ("01".Equals(color))
            {
                return "黄";
            }
            else if ("02".Equals(color))
            {
                return "黑";
            }
            else if ("04".Equals(color))
            {
                return "白";
            }
            else
            {
                return "无";
            }

        }

        public static string CheckProvience(string no)
        {
            CarNoProvince carnoprovince = cardb.CarNoProvince.Where(a => a.NO == no).FirstOrDefault();
            if (carnoprovince != null)
            {
                return carnoprovince.CarNo;
            }
            return "";

        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Timers;
using System.Web;
using Tail.Gas.Detection.Platform.Models;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;

namespace Tail.Gas.Detection.Platform.Bussiness
{
    public class monitor
    {
        public readonly static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static Timer timer = new Timer();
        public static void begin()
        {
            try
            {
                timer.Elapsed += new ElapsedEventHandler(Monitor);
                timer.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["intervaltime"]);
                timer.Start();
                timer.Enabled = true;
            }
            catch(Exception ex)
            {
                logger.Error(ex);
            }
           
        }
        public static void Monitor(object source, ElapsedEventArgs e)
        {
            try
            {
                using (cardbEntities cardb = new cardbEntities())
                {
                    string excutesql = @" select ttt.* from (select *,ROW_NUMBER() over(partition by no order by Data_LastChangeTime desc) rn from 
                                          (select a.no ,a.ProductModel,a.UserInfo,a.Telphone,b.TemperatureBefore,b.SensorNum,b.Data_LastChangeTime
                                         from carinfo a ,carstatusinfo b 
                                         where 
											a.no
                                         in(
                                         select carno from (select t.* from (select *,ROW_NUMBER() over(partition by carno order by Data_LastChangeTime desc) rn from CarStatusInfo) t 
                                         where rn<=2
                                         and SystemStatus !=0)d
                                         group by d.CarNo 
                                         having count(*)>1) and a.no =b.CarNo) tt) ttt
										 where rn <=1";
                    var query = cardb.Database.SqlQuery<EmailInfo>(excutesql);

                    if (query.Count() > 0)
                    {
                        string body = "";
                        string title = "";
                        foreach (EmailInfo item in query)
                        {

                            body = String.Format("车牌号：{0}的{1}设备出现异常情况，温度：{2}摄氏度，压差：{3}KPA，请尽快联系车主：{4}，联系电话：{5}。   <br>机动车尾气排放监控平台 <br>当前时间：{6}", item.no, item.ProductModel, item.TemperatureBefore, item.SensorNum, item.UserInfo, item.Telphone,DateTime.Now.ToString("yyyy年MM月dd日HH时mm分"));
                            title = "设备运行异常告警信息-" + item.no;
                            sendMail(title,body);
                            if (ConfigurationManager.AppSettings["istest"] != "true")
                            {
                                sendSMS(item.no, item.Telphone);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
           
        }
        public static string sendMail(string title,string body)
        {
            try
            {
                string[] emailtos = ConfigurationManager.AppSettings["emailto"].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(ConfigurationManager.AppSettings["emailaccount"], "");
                foreach (var s in emailtos)
                {
                    mail.To.Add(new MailAddress(s, ""));
                }
                mail.Subject = title;
                mail.Body = body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["emailsmtp"], int.Parse(ConfigurationManager.AppSettings["smtpport"]));
                smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["emailaccount"], ConfigurationManager.AppSettings["emailpassword"]);
                smtp.Send(mail);
                logger.Info("sendemail data:" + mail);
                return "success";
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return ex.ToString();
            }
        }

        public static void sendSMS(string carno,string telphone)
        {
            try
            {
                ITopClient client = new DefaultTopClient("http://gw.api.taobao.com/router/rest", ConfigurationManager.AppSettings["appkey"], ConfigurationManager.AppSettings["secret"]);
                AlibabaAliqinFcSmsNumSendRequest req = new AlibabaAliqinFcSmsNumSendRequest();
                req.Extend = "123456".ToString();
                req.SmsType = "normal".ToString();
                req.SmsFreeSignName = "警告信息".ToString();
                req.SmsParam = "{'plate':'" + carno + "'}";
                req.RecNum = ((ConfigurationManager.AppSettings["mobile"] == "" ? "" :  ConfigurationManager.AppSettings["mobile"]) + (telphone==""?"":","+telphone)).ToString();
                req.SmsTemplateCode = "SMS_10310253".ToString();
                AlibabaAliqinFcSmsNumSendResponse rsp = client.Execute(req);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

        }

        
    }
    public class EmailInfo
    {
       public string no {get;set;}
       public string ProductModel { get; set; }
       public string UserInfo { get; set; }
       public string Telphone { get; set; }
       public decimal TemperatureBefore { get; set; }
       public decimal SensorNum { get; set; }
       public Int64 rn { get; set; }

    }
}
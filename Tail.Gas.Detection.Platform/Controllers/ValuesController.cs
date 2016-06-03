using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;
using Tail.Gas.Detection.Platform.Models;

namespace Tail.Gas.Detection.Platform.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
           
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return sendMail();
            //return "value";


            
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
            cardbEntities cardb = new cardbEntities();
            InputCarStatusInfo iputcarstatusinfo = new InputCarStatusInfo();
            iputcarstatusinfo.cardstatusinfo = value;
            cardb.InputCarStatusInfo.Add(iputcarstatusinfo);
            cardb.SaveChanges();
        }


        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
        string sendMail()
        {
            try
            {
                string[] emailtos = ConfigurationManager.AppSettings["emailto"].Split(new char[]{';'},StringSplitOptions.RemoveEmptyEntries);
                
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(ConfigurationManager.AppSettings["emailaccount"], "");
                foreach (var s in emailtos)
                {
                    mail.To.Add(new MailAddress(s,""));
                }
                mail.Subject = "异常告警通知";
                mail.Body = "设备状态异常\r\n当前时间<br>签名";
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["emailsmtp"], int.Parse(ConfigurationManager.AppSettings["smtpport"]));
                smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["emailaccount"], ConfigurationManager.AppSettings["emailpassword"]);
                smtp.Send(mail);
                return "success";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        void insertData(string str)
        {
            str = "3031";
            byte[] byteArray = System.Text.Encoding.Default.GetBytes(str);
            if(byteArray[0] ==0x30){

            }

           
        }
    }
}
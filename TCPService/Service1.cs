using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Configuration;
namespace TCPService
{
    public partial class Service1 : ServiceBase
    {
        Thread newThread;
        //TCP Listner for Socket  
        public TcpListener tcpServer;
        //Creating a Network Stream  
        public NetworkStream ns;
        //Creating a Stream reader  
        public StreamReader sr;
        //Creating a Stream Writer  
        public StreamWriter sw;
        //Creating a Socket  
        public Socket sc;
        public string clientReq;
        public readonly static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static InputData inputdata = new InputData();
        public static System.Timers.Timer timer = new System.Timers.Timer();
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            tcpServer = new TcpListener(new IPEndPoint(IPAddress.Any, 8889));
            newThread = new Thread(new ThreadStart(ThreadFunction));
            newThread.IsBackground = true;
            newThread.Start();

            beginChangeData();
        }

        protected override void OnStop()
        {
            newThread.Abort(); 
        }
        /// <summary>
        /// For Listening Socket.
        /// </summary>
        public void ThreadFunction()
        {
            tcpServer.Start(2000);
            while (true)
            {
                sc = tcpServer.AcceptSocket();
                try
                {
                    if (sc.Connected)
                    {
                        try
                        {
                            var clientReq = Receive(sc, 60000);
                            //ns = new NetworkStream(sc);
                            //sr = new StreamReader(ns);
                            //sw = new StreamWriter(ns);
                            //clientReq = sr.ReadLine();
                            Console.WriteLine(clientReq);
                            logger.Info(clientReq);
                            sc.Send(new byte[] { 0x55, 0xAA, 0x03, 0x03 });
                            // DestroySocket(sc);
                            string result = inputdata.InputCheckData(clientReq);
                            logger.Info(result);
                        }
                      
                        catch (Exception ex)
                        {
                            logger.Error(ex);
                        }
                    }
                }
                catch (IOException ex)
                {
                    logger.Error(ex);
                }
                try
                {
                    sc.Close();
                    clientReq = null;
                }
                catch (IOException ex)
                {
                    logger.Error(ex);
                }
            }
        }

        private static string Receive(Socket socket, int timeout)
        {
            string result = string.Empty;
            socket.ReceiveTimeout = timeout;
            List<byte> data = new List<byte>();
            byte[] buffer = new byte[1024];
            int length = 0;
            try
            {
                while ((length = socket.Receive(buffer)) > 0)
                {
                    for (int j = 0; j < length; j++)
                    {
                        data.Add(buffer[j]);
                    }
                    if (length < buffer.Length)
                    {
                        break;
                    }
                }
            }
            catch(Exception ex) {
                logger.Error(ex);
            }
            var s = "";
            foreach (byte b in data)
            {
                s += b.ToString("X2");
                s += "";
            }
            return s;
        }

        public static void beginChangeData()
        {
            try
            {
                timer.Elapsed += new ElapsedEventHandler(ChangeData);
                timer.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["intervaltime"]);
                timer.Start();
                timer.Enabled = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

        }
        public static void ChangeData(object source, ElapsedEventArgs e)
        {
            if (ConfigurationManager.AppSettings["isTest"] == "true")
            {
                try
                {
                    using (cardbEntities cardb = new cardbEntities())
                    {
                        string sql = @"
                            update CarStatusInfo
                            set Data_LastChangeTime = getdate()
                            where Id in(
                            select t.id from (select *,ROW_NUMBER() over(partition by carno order by Data_LastChangeTime asc) rn from CarStatusInfo) t where rn<=1
                            )";
                        var result = cardb.Database.ExecuteSqlCommand(sql);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }
    
            }
            
        }
    }



}

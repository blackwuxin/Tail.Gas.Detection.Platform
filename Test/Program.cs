using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Test
{
    class Program
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
        static void Main(string[] args)
        {
            Program prgram = new Program();
            prgram.Start();
            Console.ReadLine();
            //ScoketTest.Listen(8889);
            //Console.ReadLine();

        }
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
                            sc.Send(new byte[]{0x55,0xAA,0x03,0x03});
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
                catch (Exception ex)
                {
                    logger.Error(ex);
                }
                try
                {
                    sc.Close();
                    clientReq = null;
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }
            }
        }

        public void Start()
        {
            tcpServer = new TcpListener(new IPEndPoint(IPAddress.Any, 8887));
            newThread = new Thread(new ThreadStart(ThreadFunction));
            newThread.IsBackground = true;
            newThread.Start();
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
            //if (data.Count > 0)
            //{
            //    result = encode.GetString(data.ToArray(), 0, data.Count);
            //}
            var s = "";
            foreach (byte b in data)
            {
                s += b.ToString("X2");
                s += "";
            }
            return s;
        }
        /// <summary>
        /// 销毁Socket对象
        /// </summary>
        /// <param name="socket"></param>
        private static void DestroySocket(Socket socket)
        {
            if (socket.Connected)
            {
                socket.Shutdown(SocketShutdown.Send);
            }
            socket.Close();
        }

    }
}

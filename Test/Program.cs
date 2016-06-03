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
            tcpServer.Start(4);
            while (true)
            {
                sc = tcpServer.AcceptSocket();
                try
                {
                    if (sc.Connected)
                    {
                        try
                        {
                            ns = new NetworkStream(sc);
                            sr = new StreamReader(ns);
                            sw = new StreamWriter(ns);
                            clientReq = sr.ReadLine();
                            Console.WriteLine(clientReq);
                            logger.Info(clientReq);
                            sw.WriteLine("55AA0303");          
                            //string result = inputdata.InputCheckData(clientReq);
                            //logger.Info(result);
                        }
                        catch (OutOfMemoryException ex)
                        {
                            logger.Error(ex);
                        }
                        catch (IOException ex)
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
                    sw.Close();
                    sr.Close();
                    ns.Close();
                    sc.Close();
                    clientReq = null;
                }
                catch (IOException ex)
                {
                    logger.Error(ex);
                }
            }
        }

        public void Start()
        {
            tcpServer = new TcpListener(new IPEndPoint(IPAddress.Any, 8889));
            newThread = new Thread(new ThreadStart(ThreadFunction));
            newThread.IsBackground = true;
            newThread.Start();
        }
    }
}

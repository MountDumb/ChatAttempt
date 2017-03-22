using System;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;
namespace TCPSocketServer
{
    /// 
    /// Summary description for TCPSockServer.
    /// 
    public class Server : Form
    {

        //////////////////////////////////////////////////////
        ///Variables & Properties
        //////////////////////////////////////////////////////
        Button btnStartServer;
        private StreamWriter serverStreamWriter;
        private StreamReader serverStreamReader;

        //////////////////////////////////////////////////////
        ///constructor
        public Server()
        {
            //create StartServer button set its properties & event handlers 
            this.btnStartServer = new Button();
            this.btnStartServer.Text = "Start Server";
            this.btnStartServer.Click += new System.EventHandler(this.btnStartServer_Click);
            //add controls to form
            this.Controls.Add(this.btnStartServer);
        }
        //////////////////////////////////////////////////////
        ///Main Method
        public static void Main(string[] args)
        {
            //I might not even use this example.....
            //creat n display windows form
            Server tcpSockServer = new Server();
            Application.Run(tcpSockServer);
        }
        //////////////////////////////////////////////////////
        ///Start Server
        private bool StartServer()
        {
            //create server's tcp listener for incoming connection
            TcpListener tcpServerListener = new TcpListener(4444);
            tcpServerListener.Start(); //start server
            Console.WriteLine("Server Started");
            this.btnStartServer.Enabled = false;
            //block tcplistener to accept incoming connection
            Socket serverSocket = tcpServerListener.AcceptSocket();

            try
            {
                if (serverSocket.Connected)
                {
                    Console.WriteLine("Client connected");
                    //open network stream on accepted socket
                    NetworkStream serverSockStream = new NetworkStream(serverSocket);
                    serverStreamWriter = new StreamWriter
                    (serverSockStream);
                    serverStreamReader = new StreamReader(serverSockStream);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }

            return true;
        } ////////////////////////////////////////////////////
          ///Event handlers
        //////////////////////////////////////////////////////
        private void btnStartServer_Click(object sender, System.EventArgs e)

        {
            //start server
            if (!StartServer())
                Console.WriteLine("Unable to start server");

            //sending n receiving msgs
            while (true)
            {
                Console.WriteLine
                ("CLIENT: " + serverStreamReader.ReadLine());
                serverStreamWriter.WriteLine("Hi!");
                serverStreamWriter.Flush();
            }//while
        }
    }
}
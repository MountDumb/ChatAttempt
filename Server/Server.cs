using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

//Alias for a string variant of the generic "Package<T>" in the "Domain" namespace
using StringPackage = Domain.Package<string>;

namespace Server
{
    internal class Server
    {        
        int _port;
        IPAddress _localAddress;
        TcpListener _server;
        IFormatter _formatter;
        
        internal Server()
        {
            _port = 12537;
            _localAddress = IPAddress.Parse("127.0.0.1");                      
        }

        internal void Run()
        {
            //Assigning a variable for a standard package in case a Client disconnects and an actual "package" is never created. 
            StringPackage package = new StringPackage(new User("Unknown"), "");
            
            try
            {
                _server = new TcpListener(_localAddress, _port);
                _server.Start();
                Console.WriteLine("Server is running...");

                TcpClient client = _server.AcceptTcpClient();
                _formatter = new BinaryFormatter();
                NetworkStream stream = client.GetStream();
                
                //The stream is set up and it's typecasted to StringPackage, so we can examine it's contents and print the user's name to the console.
                package = (StringPackage)_formatter.Deserialize(stream);

                Console.WriteLine($"{package.User.Name} has logged on");
                
                while (stream.CanRead)
                {
                    package = (StringPackage)_formatter.Deserialize(stream);
                    Console.WriteLine($"{package.User.Name}: {package.Content}");
                    _formatter.Serialize(stream, package);
                }
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine($"{package.User.Name} has disconnected");
            }
            finally
            {
                _server.Stop();
            }            
        }

// MSDN Example:
        
//    {
//        public static void Main()
//        {
//            TcpListener server = null;
//            try
//            {
//                // Set the TcpListener on port 13000.
//                Int32 port = 13000;
//                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

        //                // TcpListener server = new TcpListener(port);
        //                server = new TcpListener(localAddr, port);

        //                // Start listening for client requests.
        //                server.Start();

        //                // Buffer for reading data
        //                Byte[] bytes = new Byte[256];
        //                String data = null;

        //                // Enter the listening loop.
        //                while (true)
        //                {
        //                    Console.Write("Waiting for a connection... ");

        //                    // Perform a blocking call to accept requests.
        //                    // You could also user server.AcceptSocket() here.
        //                    TcpClient client = server.AcceptTcpClient();
        //                    Console.WriteLine("Connected!");

        //                    data = null;

        //                    // Get a stream object for reading and writing
        //                    NetworkStream stream = client.GetStream();

        //                    int i;

        //                    // Loop to receive all the data sent by the client.
        //                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
        //                    {
        //                        // Translate data bytes to a ASCII string.
        //                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
        //                        Console.WriteLine("Received: {0}", data);

        //                        // Process the data sent by the client.
        //                        data = data.ToUpper();

        //                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

        //                        // Send back a response.
        //                        stream.Write(msg, 0, msg.Length);
        //                        Console.WriteLine("Sent: {0}", data);
        //                    }

        //                    // Shutdown and end connection
        //                    client.Close();
        //                }
        //            }
        //            catch (SocketException e)
        //            {
        //                Console.WriteLine("SocketException: {0}", e);
        //            }
        //            finally
        //            {
        //                // Stop listening for new clients.
        //                server.Stop();
        //            }


        //            Console.WriteLine("\nHit enter to continue...");
        //            Console.Read();
        //        }

    }
}

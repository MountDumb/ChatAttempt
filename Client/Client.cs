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

namespace Client
{
    internal class Client
    {
        int _port;
        IPAddress _localAddress;
        IFormatter _formatter;
        NetworkStream stream;

        internal Client()
        {
            _port = 12537;
            _localAddress = IPAddress.Parse("127.0.0.1");
            _formatter = new BinaryFormatter();
        }

        internal void Run()
        {
            Console.WriteLine("Please choose a nickname");
            string name = Console.ReadLine();

            using (TcpClient client = new TcpClient())
            {
                client.Connect(_localAddress, _port);
                stream = client.GetStream();
                User user = new User(name);
                StringPackage package = new StringPackage(user, "");
                _formatter.Serialize(stream, package);

                while (true)
                {
                    package.Content = Console.ReadLine();
                    _formatter.Serialize(stream, package);
                }
            }
        }
    }
}

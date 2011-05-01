using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace IPMulticasting
{
    class Program
    {
        static void Main(string[] args)
        {
            new IPMulticast();
        }
    }

    class NotifyMessageVO
    {
        public String location;
        public String server;
        public String nt;
        public String host;
        public String nts;
        public String usn;

        override public String ToString()
        {
            return "[NotifyMessageVO, server: " + server + ", location: " + location + ", usn: " + usn + "]\n";
        }
    }

    class IPMulticast
    {

        private Socket sockListen;
        private Socket sockSend;
        private Socket sockSubscribe;
        private List<NotifyMessageVO> messages;

        public IPMulticast()
        {

            try
            {
                Thread listenNotify = new Thread(new ThreadStart(ListenNotifyUpnp));
                listenNotify.Start();

                Thread console = new Thread(new ThreadStart(ThreadConsole));
                console.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void ThreadConsole()
        {

            while (true)
            {
                String input = Console.ReadLine();
                if (input.Equals("msearch"))
                {
                    Console.WriteLine("Sending M-SEARCH...");
                    Thread sendMSearch = new Thread(SendMSearch);
                    sendMSearch.Start();
                }

                if (input.Equals("listing"))
                {
                    foreach (NotifyMessageVO m in messages)
                    {
                        Console.WriteLine(m.ToString());
                        Console.WriteLine("");
                    }
                }

                if (input.Equals("subscribe"))
                {
                    Thread sendSubscribe = new Thread(SendSubscribeMessage);
                    sendSubscribe.Start();
                }

            }

        }


        public void ListenNotifyUpnp()
        {
            sockListen = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            sockListen.SetSocketOption(SocketOptionLevel.Socket,SocketOptionName.ReuseAddress, 1); // To be able to bind on 
                                                                                                   // the same port concurrently with another application
            sockListen.ExclusiveAddressUse = false;
            
            IPAddress ip = IPAddress.Parse("239.255.255.250");
            sockListen.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(ip, IPAddress.Any));

            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 1900);
            sockListen.Bind(ipep);


            messages = new List<NotifyMessageVO>();
            

            int ln = 0;

            while (true)
            {
                byte[] b = new byte[800];
                sockListen.Receive(b);
                
                string str = System.Text.Encoding.ASCII.GetString(b, 0, b.Length);
                str = str.Trim();
                // Console.WriteLine( str );

                if (str.IndexOf("M-SEARCH") >= 0) // We skip this message because we are the Control Point that sends this message
                {
                    Console.WriteLine("Received an M-SEARCH message: " + str);
                    continue;
                }

                String[] elements = str.Split( '\n' );
                ln = elements.Length;

                NotifyMessageVO message;
                String server   = "";
                String host     = "";
                String location = "";
                String usn      = "";
                String nt       = "";
                String nts      = "";

                for (int i = 0; i < ln; i++)
                {
                    String line = elements[i];
                    String field;
                    String value;

                    int indexOfColon = line.Trim().IndexOf(":");
                    if (indexOfColon < 0) continue;


                    field = line.Substring(0, indexOfColon).ToLower();
                    indexOfColon += 1;
                    value = line.Substring(indexOfColon, line.Length - indexOfColon).Trim();
                    
                    switch (field)
                    {
                        case "server":
                            server += value;
                            break;
                        case "location":
                            location += value;
                            break;
                        case "usn":
                            usn += value;
                            break;
                        case "nt":
                            nt += value;
                            break;
                        case "nts":
                            nts += value;
                            break;
                        case "host":
                            host += value;
                            break;
                    }
                }

                if (nts.Equals("ssdp:byebye"))
                {
                    Console.WriteLine("Received ssdp:byebye for: " + server + ", " + usn);
                    bool removed = SearchAndRemoveMessage(usn);
                    continue;
                }

                bool exist = SearchMessage(location);

                if (!exist)
                {
                    message = new NotifyMessageVO();
                    message.server = server;
                    message.host = host;
                    message.location = location;
                    message.usn = usn;
                    message.nt = nt;
                    message.nts = nts;

                    messages.Add(message);
                    Console.WriteLine("ListenNotifyUpnp, List changed: " + messages.Count + " devices.");
                }
                else
                {
                    // Console.WriteLine("Already Exist, " + messages.Count + " in the list." );
                }

            }
        }

        public void SendMSearch()
        {
            Console.WriteLine("Starting SendSearch Thread...");

            sockSend = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPAddress ip = IPAddress.Parse("239.255.255.250");
            sockSend.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(ip));
            sockSend.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 1);

            IPEndPoint ipep = new IPEndPoint(ip, 1900);
            sockSend.Connect(ipep);

            String packet = "";
			packet += "M-SEARCH * HTTP/1.1\r\n";
			packet += "HOST: 239.255.255.250:1900\r\n";
            packet += "MAN: \"ssdp:discover\"\r\n";
			packet += "MX: " + "3" +"\r\n";
            packet += "ST: " + "urn:schemas-upnp-org:device:MediaServer:1" + "\r\n" + "\r\n";


            byte[] searchMessage = System.Text.Encoding.ASCII.GetBytes(packet);
            sockSend.Send(searchMessage, searchMessage.Length, SocketFlags.None);
            sockSend.Close();
        }

        public void SendSubscribeMessage()
        {
            Console.WriteLine("Starting SendSubscribeMessage Thread...");


            sockSubscribe = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            // sockSend.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(ip));
            // sockSend.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 1);

            IPEndPoint ipep = new IPEndPoint(ip, 42100);
            sockSubscribe.Connect(ipep);

            String packet = "";
            // packet += "SUBSCRIBE http://localhost:42100/upnp/event/content_directory HTTP/1.1\r\n";
            packet += "SUBSCRIBE http://localhost:42100/upnp/event/connection_manager HTTP/1.1\r\n";
            packet += "HOST: localhost:42100\r\n";
            packet += "CALLBACK: <http://localhost:8000/>\r\n";
            packet += "NT: upnp:event\r\n";
            packet += "TIMEOUT: Second-" + "6000" + "\r\n" + "\r\n";


            byte[] subscribeMessage = System.Text.Encoding.ASCII.GetBytes(packet);
            sockSubscribe.Send(subscribeMessage, subscribeMessage.Length, SocketFlags.None);

            byte[] buffer = new byte[300];

            sockSubscribe.Receive(buffer);
            string str = System.Text.Encoding.ASCII.GetString(buffer, 0, buffer.Length);
            str = str.Trim();

            Console.WriteLine("Received: " + str );
            

            sockSubscribe.Close();
        }



        Boolean SearchMessage(String pLocation)
        {
            foreach (NotifyMessageVO m in messages)
            {
                if (m.location.Equals(pLocation))
                {
                    return true;
                }
            }
            return false;
        }

        Boolean SearchAndRemoveMessage(String pUSN)
        {
            foreach (NotifyMessageVO m in messages)
            {
                if (m.usn.Equals(pUSN))
                {
                    messages.Remove(m);
                    Console.WriteLine("SearchAndRemoveMessage, List changed: " + messages.Count + " devices.");
                    return true;
                }
            }
            return false;
        }


    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.Xml;

namespace MediaRendererDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            new HttpAutoBrowser();
        }
    }


    class HttpAutoBrowser
    {
        public HttpAutoBrowser()
        {
            try
            {
                log("Sur le point de démarrer le thread.");
                Thread mythread = new Thread(new ThreadStart(threadLoop));
                mythread.Start();
                log("Le thread a été démarré sans erreur.");
            }
            catch (Exception ex)
            {
                log("Exception durant le démarrage du thread : " + ex.Message);
            }

            Console.ReadLine();
        }

        public void threadLoop()
        {
            try
            {
                Console.WriteLine("Calling SetAVTransportURI");

                String soap = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?><s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\"><s:Body><u:SetAVTransportURI xmlns:u=\"urn:schemas-upnp-org:service:AVTransport:1\"><InstanceID>0</InstanceID><CurrentURI>http://localhost:42100/getres/2386/babel.avi.avi</CurrentURI><CurrentURIMetaData></CurrentURIMetaData></u:SetAVTransportURI></s:Body></s:Envelope>";
                // String str = this.HttpPost("http://192.168.1.89:59003/ContentDirectory/f55c00e9-7b47-67eb-b26f-a674d4da9a8d/control.xml", soap);
                int result = this.HttpPost("http://192.168.1.89:61818/AVTransport/3986b041-02b4-04f6-8f5d-765229d49ab0/control.xml", soap);

                if (result >= 0)
                {
                    Console.WriteLine("Calling Play");

                    soap = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?><s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\"><s:Body><u:Play xmlns:u=\"urn:schemas-upnp-org:service:AVTransport:1\"><InstanceID>0</InstanceID><Speed>1</Speed></u:Play></s:Body></s:Envelope>";
                    result = this.HttpPost2("http://192.168.1.89:61818/AVTransport/3986b041-02b4-04f6-8f5d-765229d49ab0/control.xml", soap);

                    Console.WriteLine( "Result for Play: " + result );
                }
            }
            catch (Exception ex)
            {
                log("Exception durant l'exécution du thread : " + ex.Message);
            }
        }

        private int HttpPost(string URI, string Parameters)
        {
            
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(URI);
            req.Headers.Add("SOAPAction", "\"urn:schemas-upnp-org:service:AVTransport:1#SetAVTransportURI\"");
            req.Accept = "text/xml";
            req.ContentType = "text/xml; charset=\"utf-8\"";
            req.Method = "POST";
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(Parameters);
            req.ContentLength = bytes.Length;

            System.IO.Stream os = req.GetRequestStream();
            os.Write(bytes, 0, bytes.Length);
            os.Close();
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            if (resp == null) return -1;


            XmlDocument doc = new XmlDocument();
            doc.Load(resp.GetResponseStream());

            log(doc.FirstChild.LocalName);

            return 0;
        }

        private int HttpPost2(string URI, string Parameters)
        {

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(URI);
            req.Headers.Add("SOAPAction", "\"urn:schemas-upnp-org:service:AVTransport:1#Play\"");
            req.Accept = "text/xml";
            req.ContentType = "text/xml; charset=\"utf-8\"";
            req.Method = "POST";
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(Parameters);
            req.ContentLength = bytes.Length;

            System.IO.Stream os = req.GetRequestStream();
            os.Write(bytes, 0, bytes.Length);
            os.Close();
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            if (resp == null) return -1;


            XmlDocument doc = new XmlDocument();
            doc.Load(resp.GetResponseStream());

            log(doc.FirstChild.LocalName);

            return 0;
        }

        private bool logON = true;

        private void log(String str)
        {
            if (this.logON == true) Console.WriteLine(str);
        }

    }


}


using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading;
using System.Xml;
using System.IO;


namespace HttpAutoBrowser
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

            Thread.Sleep(10000);
        }

        public void threadLoop()
        {
            try
            {
                // Requête GET vers Google.Com
                // this.HttpGet("http://www.google.com");

                // Requête POST vers Tversity
                // String soap = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?><s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\"><s:Body><u:Browse xmlns:u=\"urn:schemas-upnp-org:service:ContentDirectory:1\"><ObjectID>0</ObjectID><BrowseFlag>BrowseDirectChildren</BrowseFlag><Filter>*</Filter><StartingIndex>0</StartingIndex><RequestedCount>10</RequestedCount><SortCriteria>*</SortCriteria></u:Browse></s:Body></s:Envelope>";

                String soap = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?><s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\"><s:Body><u:GetSystemUpdateID xmlns:u=\"urn:schemas-upnp-org:service:ContentDirectory:1\"></u:GetSystemUpdateID></s:Body></s:Envelope>";
                
                // String str = this.HttpPost("http://192.168.1.89:59003/ContentDirectory/f55c00e9-7b47-67eb-b26f-a674d4da9a8d/control.xml", soap);
                this.HttpPost("http://192.168.1.89:42100/upnp/control/content_directory", soap);
                
            }
            catch (Exception ex)
            {
                log("Exception durant l'exécution du thread : " + ex.Message);
            }
        }

        private void HttpPost(string URI, string Parameters)
        {

            

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(URI);
            // req.Headers.Add("SOAPAction", "\"urn:schemas-upnp-org:service:ContentDirectory:1#Browse\"");
            // req.Headers.Add("SOAPAction", "\"urn:schemas-upnp-org:service:ContentDirectory:1#GetSearchCapabilities\"");
            req.Headers.Add("SOAPAction", "\"urn:schemas-upnp-org:service:ContentDirectory:1#GetSystemUpdateID\"");
           

            req.Accept = "text/xml";
            req.ContentType = "text/xml; charset=\"utf-8\"";
            req.Method = "POST";
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(Parameters);
            req.ContentLength = bytes.Length;

            System.IO.Stream os = req.GetRequestStream();
            os.Write(bytes, 0, bytes.Length);
            os.Close();
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            if (resp == null) return ;

            Console.WriteLine("Server's response");

            Stream stream = resp.GetResponseStream();
            StreamReader reader = new StreamReader(stream);

            Console.WriteLine("Data: " + reader.ReadToEnd());

        }
        
        private bool logON = true;
        
        private void log(String str)
        {
            if(this.logON==true) Console.WriteLine(str);
        }

    }
}

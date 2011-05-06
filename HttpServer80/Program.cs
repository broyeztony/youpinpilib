using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace HttpServer80
{
    class Program
    {
        static void Main(string[] args)
        {
            new HttpWebServer().Start();

        }
    }

    public class HttpWebServer
    {
        private HttpListener Listener;

        public void Start()
        {
            Console.WriteLine("Creating an HTTP Server on port 8080...");

            Listener = new HttpListener();
            Listener.Prefixes.Add("http://*:8080/");
            Listener.Start();
            Listener.BeginGetContext(ProcessRequest, Listener);
            Console.WriteLine("Connection Started");

            Console.ReadLine();
        }

        public void Stop()
        {
            Listener.Stop();
        }

        private void ProcessRequest(IAsyncResult result)
        {
            HttpListener listener = (HttpListener)result.AsyncState;
            HttpListenerContext context = listener.EndGetContext(result);
            
            HttpListenerRequest request     = context.Request;
            HttpListenerResponse response = context.Response;

            StreamReader reader = new StreamReader(request.InputStream);

            Console.WriteLine("Data: " + reader.ReadToEnd());



            /*
            string responseString = "<html>Hello World</html>";
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);

            context.Response.ContentLength64 = buffer.Length;
            System.IO.Stream output = context.Response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
            */

            Listener.BeginGetContext(ProcessRequest, Listener);
        }
    }


}

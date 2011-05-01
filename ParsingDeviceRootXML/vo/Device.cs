using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParsingDeviceRootXML.vo
{
    class Device
    {

        public Device()
        {
            services = new List<Service>();
        }
             

        override public String ToString()
        {
            string result = "[Device: \n" 
                                                       + "\t" + deviceType + ", \n"
                                                       + "\t" + friendlyName + ", \n"
                                                       + "\t" + manufacturer + ", \n"
                                                       + "\t" + manufacturerURL + ", \n"
                                                       + "\t" + modelDescription + ", \n"
                                                       + "\t" + modelName + ", \n"
                                                       + "\t" + modelNumber + ", \n"
                                                       + "\t" + modelURL + ", \n"
                                                       + "\t" + serialNumber + ", \n"
                                                       + "\t" + UDN + ", \n"
                                                       + "\t" + presentationURL + "\n\n";

            foreach (Service serv in services)
            {
                result += "\t" + serv.ToString() + "\n";
            }

            result += "]\n\n";

            return result;
        }


        public string deviceType { get; set; }
        public string friendlyName { get; set; }
        public string manufacturer { get; set; }
        public string manufacturerURL { get; set; }
        public string modelDescription { get; set; }
        public string modelName { get; set; }
        public string modelNumber { get; set; }
        public string modelURL { get; set; }
        public string UDN { get; set; }
        public string presentationURL { get; set; }
        public string serialNumber { get; set; }


        public List<Service> services;



    }
}

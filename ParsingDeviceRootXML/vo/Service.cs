using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParsingDeviceRootXML.vo
{
    class Service
    {






        override public String ToString()
        {
            return "[Service: \n" 
                                                + "\t" + serviceType + ", \n"
                                                + "\t" + serviceId + ", \n"
                                                + "\t" + SCPDURL + ", \n"
                                                + "\t" + controlURL + ", \n"
                                                + "\t" + eventSubURL + "\n"
                                                + "\t]\n";
        }




        public string serviceType { get; set; }
        public string serviceId { get; set; }
        public string SCPDURL { get; set; }
        public string controlURL { get; set; }
        public string eventSubURL { get; set; }


    }
}

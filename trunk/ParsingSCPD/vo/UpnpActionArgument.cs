using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParsingSCPD.vo
{
    class UpnpActionArgument
    {

        public String name;
        public String direction;
        public String relatedStateVariable;

        public UpnpActionArgument()
        {

        }

        public override string ToString()
        {
            return "[UpnpActionArgument: "   + "\n"
                                             + "\t\t\t" + name + ", \n"
                                             + "\t\t\t" + direction + ", \n"
                                             + "\t\t\t" + relatedStateVariable + ", \n"
                                             + "\t] \n";
        }



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParsingSCPD.vo
{
    class UpnpAction
    {

        public List<UpnpActionArgument> argumentList;
        public String name;

        public UpnpAction()
        {
            argumentList = new List<UpnpActionArgument>();
        }


        public override string ToString()
        {
            String result = "[UpnpAction: "  + "\n"
                                    + name + ", \n";

            foreach (UpnpActionArgument arg in argumentList)
            {
                result += "\t" + arg.ToString() + "\n";
            }

            result += "]\n\n";

            return result;
        }


    }
}

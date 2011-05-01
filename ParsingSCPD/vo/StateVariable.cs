using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParsingSCPD.vo
{
    class StateVariable
    {
        public List<String> allowedValueList;

        public StateVariable()
        {
            allowedValueList = new List<string>();
        }


        public String name;
        public String dataType;
        public bool sendEvents;


        public override string ToString()
        {
            String result = "[StateVariable: " + "\n"
                                                + "\t" + name + ", \n"
                                                + "\t" + dataType + ", \n"
                                                + "\t" + sendEvents + ", \n";

            if (allowedValueList.Count > 0) result += "\n\t\t" + "allowedValueList: \n";
            foreach (String arg in allowedValueList)
            {
                result += "\t\t\t" + arg + "\n";
            }

            result += "]\n\n";

            return result;
        }





    }
}

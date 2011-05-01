using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ParsingSCPD.vo;


namespace ParsingSCPD
{
    class Program
    {
        static void Main(string[] args)
        {
            new ParsingSPCD();
            Console.ReadLine();
        }
    }

    class ParsingSPCD
    {



        XmlTextReader reader;

        public ParsingSPCD()
        {
            String descriptionURL   = "http://localhost:42100/UPnP_AV_ContentDirectory_1.0.xml";
            reader                  = new XmlTextReader(descriptionURL);


            // Creating a new list of UpnpAction
            actions = new List<UpnpAction>();
            serviceStateTable = new List<StateVariable>();

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // Le nœud est un élément.
                        OnStartElement(reader.Name);
                        break;

                    case XmlNodeType.Text: //Affiche le texte de chaque élément.
                        OnTextElement(reader.Value);
                        break;

                    case XmlNodeType.EndElement: //Affiche la fin de l'élément.
                        OnEndElement(reader.Name);
                        break;
                }
            }


            Console.WriteLine("Actions: \n");
            foreach (UpnpAction act in actions)
            {
                Console.WriteLine(act.ToString());
            }

            Console.WriteLine("StateVariables: \n");
            foreach (StateVariable var in serviceStateTable)
            {
                Console.WriteLine(var.ToString());
            }

        }

        String _property;
        UpnpAction currentProcessedAction;
        UpnpActionArgument currentProcessedActionArgument;
        StateVariable currentProcessedStateVariable;
         

        List<UpnpAction> actions;
        List<StateVariable> serviceStateTable;

        bool processingAction = false;
        bool processedActionName = false;

        private void OnStartElement(String nodeName)
        {

            _property = nodeName;
            if (nodeName.Equals("action"))
            {
                processingAction = true;
                processedActionName = false;
                currentProcessedAction = new UpnpAction();
            }

            if (nodeName.Equals("argument"))
            {
                currentProcessedActionArgument = new UpnpActionArgument();
            }

            if (nodeName.Equals("stateVariable"))
            {
                currentProcessedStateVariable = new StateVariable();
                currentProcessedStateVariable.sendEvents = (reader.GetAttribute("sendEvents").Equals("yes")) ? true : false;
            }

        }

        private void OnTextElement(String text)
        {

            switch (_property)
            {
                case "name":

                    if (processingAction)
                    {
                        if (!processedActionName)
                        {
                            processedActionName = true;
                            currentProcessedAction.name = text;
                        }
                        else
                        {
                            currentProcessedActionArgument.name = text;
                        }
                    }
                    else
                    {
                        currentProcessedStateVariable.name = text;
                    }
                    break;
                case "direction":
                    currentProcessedActionArgument.direction = text;
                    break;
                case "relatedStateVariable":
                    currentProcessedActionArgument.relatedStateVariable = text;    
                    break;
                case "dataType":
                    currentProcessedStateVariable.dataType = text;
                    break;
                case "allowedValue":
                    currentProcessedStateVariable.allowedValueList.Add(text);
                    break;
            }

        }

        private void OnEndElement(String nodeName)
        {
            if (nodeName.Equals("action"))
            {
                processingAction = false;
                actions.Add(currentProcessedAction);
            }

            if (nodeName.Equals("argument"))
            {
                currentProcessedAction.argumentList.Add(currentProcessedActionArgument);
            }

            if (nodeName.Equals("stateVariable"))
            {
                serviceStateTable.Add(currentProcessedStateVariable);
            }

        }



    }

}

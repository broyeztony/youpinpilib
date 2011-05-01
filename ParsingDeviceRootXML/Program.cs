using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ParsingDeviceRootXML.vo;


namespace ParsingDeviceRootXML
{
    class Program
    {
        static void Main(string[] args)
        {
            new ParsingDeviceRootXML();


            Console.ReadLine();
        }
    }

    class ParsingDeviceRootXML
    {

        Device dev;
        String _property;
        Service currentProcessedService;


        public ParsingDeviceRootXML()
        {
            String descriptionURL   = "http://192.168.1.89:61818";
            XmlTextReader reader    = new XmlTextReader(descriptionURL);


            // Creates a new Device
            dev = new Device();

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


            

            Console.WriteLine(dev.ToString());

        }


        private void OnStartElement(String nodeName)
        {
            _property = nodeName;

            if (_property.Equals("service"))
            {
                currentProcessedService = new Service();
            }
        }

        private void OnTextElement(String text)
        {

            switch (_property)
            {
                case "deviceType":
                    dev.deviceType = text;
                    break;
                case "friendlyName":
                    dev.friendlyName = text;
                    break;
                case "manufacturer":
                    dev.manufacturer = text;
                    break;
                case "manufacturerURL":
                    dev.manufacturerURL = text;
                    break;
                case "modelDescription":
                    dev.modelDescription = text;
                    break;
                case "modelName":
                    dev.modelName = text;
                    break;
                case "modelURL":
                    dev.modelURL = text;
                    break;
                case "modelNumber":
                    dev.modelNumber = text;
                    break;
                case "serialNumber":
                    dev.serialNumber = text;
                    break;
                case "UDN":
                    dev.UDN = text;
                    break;
                case "presentationURL":
                    dev.presentationURL = text;
                    break;

                // Process Services tags
                case "serviceType":
                    currentProcessedService.serviceType = text;
                    break;
                case "serviceId":
                    currentProcessedService.serviceId = text;
                    break;
                case "SCPDURL":
                    currentProcessedService.SCPDURL = text;
                    break;
                case "controlURL":
                    currentProcessedService.controlURL = text;
                    break;
                case "eventSubURL":
                    currentProcessedService.eventSubURL = text;
                    break;

            }
        }

        private void OnEndElement(String nodeName)
        {
            if (nodeName.Equals("service"))
            {
                dev.services.Add(currentProcessedService);
            }

        }






    }

}

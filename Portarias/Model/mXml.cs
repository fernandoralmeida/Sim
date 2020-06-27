using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;


namespace Sim.Modulos.Portarias.Model
{
    class mXml
    {
        public static List<string> Listar(string file, string elemento, string elementos, string item)
        {
            try
            {
                List<string> lista = new List<string>();
                var docxml = XDocument.Load(string.Format(@"{0}\xml\{1}.xml", AppDomain.CurrentDomain.BaseDirectory, file));
                var items = docxml.Element(XName.Get(elemento)).Elements(XName.Get(elementos));
                var nitems = items.Select(ele => ele.Element(XName.Get(item)).Value);

                foreach (string name in nitems)
                {
                    lista.Add(name);
                }

                return lista;
            }
            catch
            {
                List<string> lista = new List<string>();
                /*
                NovoElemento(file, elemento, elementos, "...");

                
                var docxml = XDocument.Load(string.Format(@"{0}\xml\{1}.xml", AppDomain.CurrentDomain.BaseDirectory, file));
                var items = docxml.Element(XName.Get(elemento)).Elements(XName.Get(elementos));
                var nitems = items.Select(ele => ele.Element(XName.Get(item)).Value);

                foreach (string name in nitems)
                {
                    lista.Add(name);
                }
                */
                lista.Add("...");
                return lista;
            }
        }

        public static bool SaveXml(string valor, string elemento)
        {
            try
            {
                XmlDocument docxml = new XmlDocument();
                docxml.Load(string.Format(@"{0}\xml\sim.apps.xml", AppDomain.CurrentDomain.BaseDirectory));

                XmlNode node;
                node = docxml.DocumentElement;

                foreach (XmlNode node1 in node.ChildNodes)
                    foreach (XmlNode node2 in node1.ChildNodes)
                        if (node2.Name == elemento)
                        {
                            node2.InnerText = valor;
                        }

                docxml.Save(string.Format(@"{0}\xml\sim.apps.xml", AppDomain.CurrentDomain.BaseDirectory));
                return true;
            }
            catch { return false; }
        }

        public static void NovoElemento(string file, string elemento, string novoelemento, string valor)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(string.Format(@"{0}\xml\{1}.xml", AppDomain.CurrentDomain.BaseDirectory, file));

            XmlNode node;
            node = xDoc.DocumentElement;

            XmlElement newelement = xDoc.CreateElement(novoelemento);

            foreach (XmlNode node1 in node.ChildNodes)
                if (node1.Name == elemento)
                {
                    node1.AppendChild(newelement);
                }

            foreach (XmlNode node1 in node.ChildNodes)
                foreach (XmlNode node2 in node1.ChildNodes)
                    if (node2.Name == novoelemento)
                    {
                        node2.InnerText = valor;
                    }


            xDoc.Save(string.Format(@"{0}\xml\{1}.xml", AppDomain.CurrentDomain.BaseDirectory, file));
        }
    }
}

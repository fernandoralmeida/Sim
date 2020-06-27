using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Text;

namespace Sim.App.Model
{
    class mXml
    {
        public static List<string> xList(string url)
        {
            List<string> lista = new List<string>();
            var docxml = XDocument.Load(url);
            var items = docxml.Element(XName.Get("System")).Elements(XName.Get("Version"));
            var nitems = items.Select(ele => ele.Element(XName.Get("Update")).Value);

            foreach (string name in nitems)
            {
                lista.Add(name);
            }

            docxml = null;
            return lista;
        }

        public static List<string> LoadXml(string file, string elemento, string elementos, string item)
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

        public static void SaveXml(string valor, string elemento, string file)
        {

            XmlDocument docxml = new XmlDocument();
            docxml.Load(string.Format(@"{0}\xml\{1}.xml", AppDomain.CurrentDomain.BaseDirectory, file));

            XmlNode node;
            node = docxml.DocumentElement;

            foreach (XmlNode node1 in node.ChildNodes)
                foreach (XmlNode node2 in node1.ChildNodes)
                    if (node2.Name == elemento)
                    {
                        node2.InnerText = valor;
                    }

            docxml.Save(string.Format(@"{0}\xml\{1}.xml", AppDomain.CurrentDomain.BaseDirectory, file));
        }
    }
}

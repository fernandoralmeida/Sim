using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Sim.Modulos.Legislacao.Model
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
                lista.Add(string.Empty);
                return lista;
            }
        }

        public static void SaveXml(string valor, string elemento)
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
            }
            catch { }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Sim.Monitoring.Model
{
    class mXml
    {
        public static List<string> Listar(string file, string elemento, string elementos, string item)
        {
            List<string> lista = new List<string>();
            try
            {                
                var docxml = XDocument.Load(string.Format(@"{0}\{1}.xml", AppDomain.CurrentDomain.BaseDirectory, file));
                var items = docxml.Element(XName.Get(elemento)).Elements(XName.Get(elementos));
                var nitems = items.Select(ele => ele.Element(XName.Get(item)).Value);

                foreach (string name in nitems)
                {
                    lista.Add(name);
                }

                return lista;
            }
            catch{
            lista.Add("...");
            return lista;
            }
        }
    }
}

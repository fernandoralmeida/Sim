using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Sim.Updater.Model
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
    }
}

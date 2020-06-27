using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;

namespace Sim.Data
{
    class Connections
    {
        internal static string NetworkAuthentication()
        {
            return string.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}SimDataBase1_N.mdb;Persist Security Info=True", Listar("sim.data", "Data", "Conexao", "Rede")[0]);
        }

        internal static string NetworkModules()
        { 
            return string.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}SimDataBase2.mdb;Persist Security Info=True", Listar("sim.data", "Data", "Conexao", "Rede")[0]);
        }

        internal static string NetwortModulesTwo
        {
            get
            {
                return string.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}SimDataBase3.mdb;Persist Security Info=True", Listar("sim.data", "Data", "ConexaoTwo", "Rede")[0]);
            }
        }

        internal static string DataBase4
        {
            get
            {
                return string.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}SimDataBase4.mdb;Persist Security Info=True", Listar("sim.data", "Data", "ConexaoTwo", "Rede")[0]);
            }
        }

        internal static string DataBase5
        {
            get
            {
                return string.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}SimDataBase5.mdb;Persist Security Info=True", Listar("sim.data", "Data", "ConexaoTwo", "Rede")[0]);
            }
        }

        static List<string> Listar(string file, string elemento, string elementos, string item)
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
                return null;
            }
        }
    }
}

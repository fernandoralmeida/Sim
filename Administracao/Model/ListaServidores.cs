using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Sim.Modulos.Administracao.Model
{
    using Sim.Data;

    class ListaServidores : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            return ListName().GetEnumerator();
        }

        public List<string> ListName()
        {
            var dataAccess = Instances.DataM();

            List<string> lst = new List<string>();

            try
            {
                dataAccess.ClearParameters();
                //dataAccess.AddParameters("@Nome", name + "%");

                foreach (DataRow dr in dataAccess.Read(@"SELECT Nome FROM Por_Srv_Nomes WHERE (Bloqueado = 0) ORDER BY Nome").Rows)
                {
                    lst.Add(dr["Nome"].ToString());
                }
            }
            catch
            {
                lst.Add("...");
            }

            return lst;
        }
    }
}

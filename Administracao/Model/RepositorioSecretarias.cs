using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Data;

namespace Sim.Modulos.Administracao.Model
{

    using Sim.Data;

    class RepositorioSecretarias : IEnumerable
    {
        
        public IEnumerator GetEnumerator()
        {
            return ListName().GetEnumerator();
        }

        private List<string> ListName()
        {
            var dataAccess = Instances.DataBase5();

            List<string> lst = new List<string>();

            try
            {
                dataAccess.ClearParameters();

                foreach (DataRow dr in dataAccess.Read(@"SELECT Secretaria FROM Secretarias WHERE (Ativo = true) ORDER BY Secretaria").Rows)
                {
                    lst.Add(dr["Secretaria"].ToString());
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

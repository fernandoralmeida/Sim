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

    class RepositorioSetores : IEnumerable
    {
        public static string GetSecretaria = "%";

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
                dataAccess.AddParameters("@Sec", "%" + GetSecretaria + "%");

                foreach (DataRow dr in dataAccess.Read(@"SELECT Setor FROM Setores WHERE (Secretaria LIKE @Sec) AND (Ativo = true) ORDER BY Setor").Rows)
                {
                    lst.Add(dr["Setor"].ToString());
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Data;

namespace Sim.Modulos.Administracao.Model
{

    using Sim.Data;

    class RepositorioCargos
    {
        public ObservableCollection<string> TodosCargosAtivos()
        {
            var lista = new ObservableCollection<string>();

            var dataAccess = Instances.DataBase5();

            try
            {
                dataAccess.ClearParameters();
                //dataAccess.AddParameters("@Nome", name + "%");

                foreach (DataRow dr in dataAccess.Read(@"SELECT Cargo FROM Cargos ORDER BY Cargo").Rows)
                {
                    lista.Add(dr["Cargo"].ToString());
                }
            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }

            return lista; 
        }
    }
}

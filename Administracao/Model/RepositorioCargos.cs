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

                foreach (DataRow dr in dataAccess.Read(@"SELECT Cargo FROM Cargos WHERE (Ativo = true) ORDER BY Cargo").Rows)
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

        public bool AtivarYesNo(int indice, bool yesno)
        {
            try
            {
                var dataAccess = Instances.DataBase5();

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@Ativo", yesno);
                dataAccess.AddParameters("@Indice", indice);

                string sql = @"UPDATE Cargos SET Ativo = @Ativo WHERE (Indice = @Indice)";


                return dataAccess.Write(sql);

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Adiciona novo setor no banco de dados.
        /// </summary>
        /// <param name="obj">grava o obj setores no banco</param>
        /// <returns></returns>
        public bool Gravar(Cargos obj)
        {
            try
            {
                var dataAccess = Instances.DataBase5();

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@Cargo", obj.Cargo);
                dataAccess.AddParameters("@Ativo", obj.Ativo);

                return dataAccess.Write(@"INSERT INTO Cargos (Cargo, Ativo) VALUES (@Cargo, @Ativo)");

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Retorna todos os setores
        /// </summary>
        /// <param name="secretaria">retorna somente setores por secretaria</param>        /// 
        /// <returns></returns>
        public ObservableCollection<Cargos> Cargos(string _nome)
        {
            var dataAccess = Instances.DataBase5();

            ObservableCollection<Cargos> lst = new ObservableCollection<Cargos>();

            try
            {
                dataAccess.ClearParameters();
                dataAccess.AddParameters("@Cargo", "%" + _nome + "%");

                foreach (DataRow dr in dataAccess.Read(@"SELECT * FROM Cargos WHERE (Cargo LIKE @Cargo) ORDER BY Cargo").Rows)
                {
                    var setor = new Cargos
                    {
                        Indice = (int)dr["Indice"],
                        Cargo = (string)dr["Cargo"],
                        Ativo = (bool)dr["Ativo"]
                    };
                    lst.Add(setor);
                }
                return lst;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return null;
            }


        }
    }
}

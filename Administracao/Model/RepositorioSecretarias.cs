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

        public bool AtivarYesNo(int indice, bool yesno)
        {
            try
            {
                var dataAccess = Instances.DataBase5();

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@Ativo", yesno);
                dataAccess.AddParameters("@Indice", indice);

                string sql = @"UPDATE Secretarias SET Ativo = @Ativo WHERE (Indice = @Indice)";


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
        public bool Gravar(Secretarias obj)
        {
            try
            {
                var dataAccess = Instances.DataBase5();

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@Secretaria", obj.Secretaria);
                dataAccess.AddParameters("@Ativo", obj.Ativo);

                return dataAccess.Write(@"INSERT INTO Secretarias (Secretaria, Ativo) VALUES (@Secretaria, @Ativo)");

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
        public ObservableCollection<Secretarias> Secretarias(string secretaria)
        {
            var dataAccess = Instances.DataBase5();

            ObservableCollection<Secretarias> lst = new ObservableCollection<Secretarias>();

            try
            {
                dataAccess.ClearParameters();
                dataAccess.AddParameters("@Sec", "%" + secretaria + "%");

                foreach (DataRow dr in dataAccess.Read(@"SELECT * FROM Secretarias WHERE (Secretaria LIKE @Sec) ORDER BY Secretaria").Rows)
                {
                    var setor = new Secretarias
                    {
                        Indice = (int)dr["Indice"],
                        Secretaria = (string)dr["Secretaria"],
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

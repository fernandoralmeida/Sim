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
       
        public bool AtivarYesNo(int indice, bool yesno)
        {
            try
            {
                var dataAccess = Instances.DataBase5();

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@Ativo", yesno);
                dataAccess.AddParameters("@Indice", indice);

                string sql = @"UPDATE Setores SET Ativo = @Ativo WHERE (Indice = @Indice)";


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
        public bool Gravar(Setores obj)
        {
            try
            {
                var dataAccess = Instances.DataBase5();

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@Setor", obj.Setor);
                dataAccess.AddParameters("@Secretaria", obj.Secretaria);
                dataAccess.AddParameters("@Ativo", obj.Ativo);

                return dataAccess.Write(@"INSERT INTO Setores (Setor, Secretaria, Ativo) VALUES (@Setor, @Secretaria, @Ativo)");

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
        public ObservableCollection<Setores> Setores(string secretaria)
        {
            var dataAccess = Instances.DataBase5();

            ObservableCollection<Setores> lst = new ObservableCollection<Setores>();

            try
            {
                dataAccess.ClearParameters();
                dataAccess.AddParameters("@Sec", "%" + secretaria + "%");

                foreach (DataRow dr in dataAccess.Read(@"SELECT * FROM Setores WHERE (Secretaria LIKE @Sec) ORDER BY Setor").Rows)
                {
                    var setor = new Setores
                    {
                        Indice = (int)dr["Indice"],
                        Setor = (string)dr["Setor"],
                        Secretaria = (string)dr["Secretaria"],
                        Ativo = (bool)dr["Ativo"]
                    };
                    lst.Add(setor);
                }
                return lst;
            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return null;
            }

            
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Sim.Modulos.Administracao.Model
{
    using Sim.Data;
    using System.Collections.ObjectModel;

    class RepositorioServidores : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            return ListName().GetEnumerator();
        }
        
        private List<string> ListName()
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

        public Servidores Indice(string p)
        {
            try
            {
                var _serv = new Servidores();

                var dataAccess = Instances.DataBase5();

                dataAccess.ClearParameters();
                dataAccess.AddParameters("@Indice", p);

                foreach (DataRow dr in dataAccess.Read(@"SELECT * FROM Servidores WHERE (Indice = @Indice) ORDER BY Nome").Rows)
                {
                    _serv.Indice = (int)dr["Indice"];
                    _serv.Codigo = dr["Codigo"].ToString();
                    _serv.Nome = dr["Nome"].ToString();
                    _serv.Cargo = dr["Cargo"].ToString();
                    _serv.Secretaria = dr["Secretaria"].ToString();
                    _serv.Setor = dr["Setor"].ToString();
                    _serv.NivelSalarial = dr["NivelSalarial"].ToString();
                    _serv.Admissao = (DateTime)dr["Admissao"];
                    _serv.Situacao = dr["Situacao"].ToString();
                    _serv.AnoParAnoImpar = dr["AnoParAnoImpar"].ToString();
                    _serv.Ativo = (bool)dr["Ativo"];
                }

                return _serv;

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return null;
            }
        }

        public Servidores Servidor(string p)
        {
            try
            {
                var _serv = new Servidores();

                var dataAccess = Instances.DataBase5();

                dataAccess.ClearParameters();
                dataAccess.AddParameters("@Codigo", p);

                foreach (DataRow dr in dataAccess.Read(@"SELECT * FROM Servidores WHERE (Codigo = @Codigo) ORDER BY Nome").Rows)
                {
                    _serv.Indice = (int)dr["Indice"];
                    _serv.Codigo = dr["Codigo"].ToString();
                    _serv.Nome = dr["Nome"].ToString();
                    _serv.Cargo = dr["Cargo"].ToString();
                    _serv.Secretaria = dr["Secretaria"].ToString();
                    _serv.Setor = dr["Setor"].ToString();
                    _serv.NivelSalarial = dr["NivelSalarial"].ToString();
                    _serv.Admissao = (DateTime)dr["Admissao"];
                    _serv.Situacao = dr["Situacao"].ToString();
                    _serv.AnoParAnoImpar = dr["AnoParAnoImpar"].ToString();
                    _serv.Ativo = (bool)dr["Ativo"];                    
                }

                return _serv;

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return null;
            }
        }

        public ObservableCollection<Servidores> UltimosCadastros()
        {
            try
            {
                var lst = new ObservableCollection<Servidores>();

                var dataAccess = Instances.DataBase5();

                dataAccess.ClearParameters();

                foreach (DataRow dr in dataAccess.Read(@"SELECT TOP 20 * FROM Servidores ORDER BY Nome").Rows)
                {
                    lst.Add(new Servidores()
                    {
                        Indice = (int)dr["Indice"],
                        Codigo = dr["Codigo"].ToString(),
                        Nome = dr["Nome"].ToString(),
                        Cargo = dr["Cargo"].ToString(),
                        Secretaria = dr["Secretaria"].ToString(),
                        Setor = dr["Setor"].ToString(),
                        NivelSalarial = dr["NivelSalarial"].ToString(),
                        Admissao = (DateTime)dr["Admissao"],
                        Situacao = dr["Situacao"].ToString(),
                        AnoParAnoImpar = dr["AnoParAnoImpar"].ToString(),
                        Ativo = (bool)dr["Ativo"]
                    });
                }

                return lst;

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return null;
            }
        }

        public bool Gravar(Servidores obj)
        {

            try
            {
                var dataAccess = Instances.DataBase5();

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@Codigo", obj.Codigo);
                dataAccess.AddParameters("@Nome", obj.Nome);                
                dataAccess.AddParameters("@Secretaria", obj.Secretaria);
                dataAccess.AddParameters("@Setor", obj.Setor);
                dataAccess.AddParameters("@Cargo", obj.Cargo);
                dataAccess.AddParameters("@NivelSalarial", obj.NivelSalarial);
                dataAccess.AddParameters("@Admissao", obj.Admissao.ToString("dd/MM/yyyy"));
                dataAccess.AddParameters("@AnoParImpar", obj.AnoParAnoImpar);
                dataAccess.AddParameters("@Situacao", obj.Situacao);
                dataAccess.AddParameters("@Ativo", obj.Ativo);

                return dataAccess.Write(@"INSERT INTO Servidores (Codigo, Nome, Secretaria, Setor, Cargo, NivelSalarial, Admissao, AnoParAnoImpar, Situacao, Ativo) VALUES (@Codigo, @Nome, @Secretaria, @Setor, @Cargo, @NivelSalarial, @Admissao, @AnoParImpar, @Situacao, @Ativo)");

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool Alterar(Servidores obj)
        {
            try
            {
                var dataAccess = Instances.DataBase5();

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@Codigo", obj.Codigo);
                dataAccess.AddParameters("@Nome", obj.Nome);
                dataAccess.AddParameters("@Secretaria", obj.Secretaria);
                dataAccess.AddParameters("@Setor", obj.Setor);
                dataAccess.AddParameters("@Cargo", obj.Cargo);
                dataAccess.AddParameters("@NivelSalarial", obj.NivelSalarial);
                dataAccess.AddParameters("@Admissao", obj.Admissao.ToString("dd/MM/yyyy"));
                dataAccess.AddParameters("@AnoParImpar", obj.AnoParAnoImpar);
                dataAccess.AddParameters("@Situacao", obj.Situacao);
                dataAccess.AddParameters("@Ativo", obj.Ativo);
                dataAccess.AddParameters("@Indice", obj.Indice);

                string sql = @"UPDATE Servidores SET Codigo=@Codigo, Nome=@Nome, Secretaria=@Secretaria, Setor=@Setor, Cargo=@Cargo, NivelSalarial=@NivelSalarial, Admissao=@Admissao, AnoParAnoImpar=@AnoParImpar, Situacao=@Situacao, Ativo=@Ativo WHERE (Indice = @Indice)";


                return dataAccess.Write(sql);

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool Excluir(int indice)
        {
            try
            {
                var dataAccess = Instances.DataBase5();

                dataAccess.ClearParameters();
                dataAccess.AddParameters("@Indice", indice);

                string sql = @"DELETE FROM Servidores WHERE ((Indice = @Indice))";

                return dataAccess.Write(sql);

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return false;
            }
        }

        #region Listas

        public ObservableCollection<Servidores> Listar_Nome_e_ou_Secretaria(List<string> p)
        {
            try
            {
                var lst = new ObservableCollection<Servidores>();

                var dataAccess = Instances.DataBase5();

                dataAccess.ClearParameters();
                dataAccess.AddParameters("@Nome", p[0]);
                dataAccess.AddParameters("@Secretaria", p[1]);

                foreach (DataRow dr in dataAccess.Read(@"SELECT * FROM Servidores WHERE (Nome LIKE + @Nome + '%') AND (Secretaria LIKE + '%' + @Secretaria + '%') ORDER BY Nome").Rows)
                {
                    lst.Add(new Servidores()
                    {
                        Indice = (int)dr["Indice"],
                        Codigo = dr["Codigo"].ToString(),
                        Nome = dr["Nome"].ToString(),
                        Cargo = dr["Cargo"].ToString(),
                        Secretaria = dr["Secretaria"].ToString(),
                        Setor = dr["Setor"].ToString(),
                        NivelSalarial = dr["NivelSalarial"].ToString(),
                        Admissao = (DateTime)dr["Admissao"],
                        Situacao = dr["Situacao"].ToString(),
                        AnoParAnoImpar = dr["AnoParAnoImpar"].ToString(),
                        Ativo = (bool)dr["Ativo"]
                    });
                }

                return lst;

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return null;
            }
        }

        #endregion

        #region Relatorio
        public ObservableCollection<Servidores> Relatorio(List<string> param)
        {
            try
            {
                var _serv = new ObservableCollection<Servidores>();

                var dataAccess = Instances.DataBase5();

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@Nome", param[0]);
                dataAccess.AddParameters("@Secretaria", param[1]);
                dataAccess.AddParameters("@Setor", param[2]);
                dataAccess.AddParameters("@Situacao", param[3]);
                dataAccess.AddParameters("@AnoParImpar", param[4]);
                dataAccess.AddParameters("@Cargo", param[5]);

                string sql = @"SELECT * FROM Servidores WHERE (Nome LIKE @Nome + '%') 
AND (Secretaria LIKE @Secretaria) 
AND (Setor LIKE @Setor) 
AND (Situacao LIKE @Situacao) 
AND (AnoParImpar LIKE @AnoParImpar) 
AND (Cargo LIKE @Cargo)
ORDER BY Nome";

                int c = 1;

                foreach (DataRow dr in dataAccess.Read(sql).Rows)
                {
                    _serv.Add(new Servidores()
                    {
                        Indice = (int)dr["Indice"],
                        Codigo = dr["Codigo"].ToString(),
                        Nome = dr["Nome"].ToString(),
                        Cargo = dr["Cargo"].ToString(),
                        Secretaria = dr["Secretaria"].ToString(),
                        Setor = dr["Setor"].ToString(),
                        NivelSalarial = dr["NivelSalarial"].ToString(),
                        Admissao = (DateTime)dr["Admissao"],
                        Situacao = dr["Situacao"].ToString(),
                        AnoParAnoImpar = dr["AnoParAnoImpar"].ToString(),
                        Ativo = (bool)dr["Ativo"],
                        Contador = c
                    }); 
                    c++;
                }

                return _serv;

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return null;
            }
        }
        #endregion

    }
}

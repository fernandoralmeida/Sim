using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Data;

namespace Sim.Modulos.Administracao.Model
{

    using Sim.Data;

    class RepositorioAvaliacoes
    {
        public bool Gravar(Avaliacoes obj)
        {
            
            try
            {
                var dataAccess = Instances.DataBase5();

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@Cod_Servidor", obj.Servidor.Codigo);
                dataAccess.AddParameters("@Nome", obj.Servidor.Nome);
                dataAccess.AddParameters("@Cargo", obj.Servidor.Cargo);
                dataAccess.AddParameters("@Secretaria", obj.Servidor.Secretaria);
                dataAccess.AddParameters("@Setor", obj.Servidor.Setor);
                dataAccess.AddParameters("@NivelSalarial", obj.Servidor.NivelSalarial);
                dataAccess.AddParameters("@DataAdmissao", obj.Servidor.Admissao.ToString("dd/MM/yyyy"));
                dataAccess.AddParameters("@Situacao", obj.Servidor.Situacao);
                dataAccess.AddParameters("@DataAvaliacao", obj.DataAvaliacao.ToString("dd/MM/yyyy"));
                dataAccess.AddParameters("@AnoParImpar", obj.Servidor.AnoParAnoImpar);
                dataAccess.AddParameters("@Pontos", obj.Pontos);
                dataAccess.AddParameters("@Resultado", obj.Resultado);                
                dataAccess.AddParameters("@DescricaoResultado", obj.DescricaoResultado);

                return dataAccess.Write(@"INSERT INTO Avaliacoes (Cod_Servidor, Nome, Cargo, Secretaria, Setor, NivelSalarial, DataAdmissao, Situacao, DataAvaliacao, AnoParImpar, Pontos, Resultado, DescricaoResultado) VALUES (@Cod_Servidor, @Nome, @Cargo, @Secretaria, @Setor, @NivelSalarial, @DataAdmissao, @Situacao, @DataAvaliacao, @AnoParImpar, @Pontos, @Resultado, @DescricaoResultado)");

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return false;              
            }            
        }
        
        public bool Alterar(Avaliacoes obj)
        {
            try
            {
                var dataAccess = Instances.DataBase5();

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@Cod_Servidor", obj.Servidor.Codigo);
                dataAccess.AddParameters("@Nome", obj.Servidor.Nome);
                dataAccess.AddParameters("@Cargo", obj.Servidor.Cargo);
                dataAccess.AddParameters("@Secretaria", obj.Servidor.Secretaria);
                dataAccess.AddParameters("@Setor", obj.Servidor.Setor);
                dataAccess.AddParameters("@NivelSalarial", obj.Servidor.NivelSalarial);
                dataAccess.AddParameters("@DataAdmissao", obj.Servidor.Admissao.ToString("dd/MM/yyyy"));
                dataAccess.AddParameters("@Situacao", obj.Servidor.Situacao);
                dataAccess.AddParameters("@DataAvaliacao", obj.DataAvaliacao.ToString("dd/MM/yyyy"));
                dataAccess.AddParameters("@AnoParImpar", obj.Servidor.AnoParAnoImpar);
                dataAccess.AddParameters("@Pontos", obj.Pontos);
                dataAccess.AddParameters("@Resultado", obj.Resultado);
                dataAccess.AddParameters("@DescricaoResultado", obj.DescricaoResultado);
                dataAccess.AddParameters("@Indice", obj.Indice);

                string sql = @"UPDATE Avaliacoes SET Cod_Servidor = @Cod_Servidor, Nome = @Nome, Cargo = @Cargo, Secretaria = @Secretaria, Setor = @Setor, NivelSalarial = @NivelSalarial, DataAdmissao = @DataAdmissao, Situacao = @Situacao, DataAvaliacao = @DataAvaliacao, AnoParImpar = @AnoParImpar, Pontos = @Pontos, Resultado = @Resultado, DescricaoResultado = @DescricaoResultado WHERE (Indice = @Indice)";


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

                string sql = @"DELETE FROM Avaliacoes WHERE ((Indice = @Indice))";

                return dataAccess.Write(sql);

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return false;
            }
        }

        public Avaliacoes Indice(int indice)
        {
            try
            {
                var lst = new Avaliacoes();

                var dataAccess = Instances.DataBase5();

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@Indice", indice);

                foreach (DataRow dr in dataAccess.Read(@"SELECT * FROM Avaliacoes WHERE (Indice = @Indice)").Rows)
                {
                    lst.Indice = (int)dr["Indice"];
                    lst.Servidor.Codigo = dr["Cod_Servidor"].ToString();
                    lst.Servidor.Nome = dr["Nome"].ToString();
                    lst.Servidor.Cargo = dr["Cargo"].ToString();
                    lst.Servidor.Secretaria = dr["Secretaria"].ToString();
                    lst.Servidor.Setor = dr["Setor"].ToString();
                    lst.Servidor.NivelSalarial = dr["NivelSalarial"].ToString();
                    lst.Servidor.Admissao = (DateTime)dr["DataAdmissao"];
                    lst.Servidor.Situacao = dr["Situacao"].ToString();
                    lst.DataAvaliacao = (DateTime)dr["DataAvaliacao"];
                    lst.Servidor.AnoParAnoImpar = dr["AnoParImpar"].ToString();
                    lst.Resultado = dr["Resultado"].ToString();
                    lst.Pontos = (int)dr["Pontos"];
                    lst.DescricaoResultado = dr["DescricaoResultado"].ToString();
                }

                return lst;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return null;
            }
        }

        public ObservableCollection<Avaliacoes> Consulta(List<string> param)
        {
            try
            {
                var lst = new ObservableCollection<Avaliacoes>();

                var dataAccess = Instances.DataBase5();

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@DataAvaliacao", param[0]);
                dataAccess.AddParameters("@Nome", param[1]);
                dataAccess.AddParameters("@Secretaria", param[2]);
                
                foreach (DataRow dr in dataAccess.Read(@"SELECT * FROM Avaliacoes WHERE (DataAvaliacao >= @DataAvaliacao) AND (Nome LIKE @Nome + '%') AND (Secretaria LIKE @Secretaria) ORDER BY Nome").Rows)
                {
                    lst.Add(new Avaliacoes()
                    {
                        Indice = (int)dr["Indice"],
                        Servidor = new Servidores() {
                            Codigo = dr["Cod_Servidor"].ToString(),
                            Nome = dr["Nome"].ToString(),
                            Cargo = dr["Cargo"].ToString(),
                            Secretaria = dr["Secretaria"].ToString(),
                            Setor = dr["Setor"].ToString(),
                            NivelSalarial = dr["NivelSalarial"].ToString(),
                            Admissao = (DateTime)dr["DataAdmissao"],
                            Situacao = dr["Situacao"].ToString(),                            
                            AnoParAnoImpar = dr["AnoParImpar"].ToString(),
                        },
                        DataAvaliacao = (DateTime)dr["DataAvaliacao"],
                        Resultado = dr["Resultado"].ToString(),
                        Pontos = (int)dr["Pontos"],
                        DescricaoResultado = dr["DescricaoResultado"].ToString()
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

        public ObservableCollection<Avaliacoes> PorData(DateTime param)
        {
            try
            {
                var lst = new ObservableCollection<Avaliacoes>();

                var dataAccess = Instances.DataBase5();

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@DataAvaliacao", param.ToString("dd/MM/yyyy"));

                int c = 1;
                foreach (DataRow dr in dataAccess.Read(@"SELECT * FROM Avaliacoes WHERE (DataAvaliacao LIKE @DataAvaliacao) ORDER BY Nome").Rows)
                {
                    lst.Add(new Avaliacoes()
                    {
                        Indice = (int)dr["Indice"],
                        Servidor = new Servidores()
                        {
                            Codigo = dr["Cod_Servidor"].ToString(),
                            Nome = dr["Nome"].ToString(),
                            Cargo = dr["Cargo"].ToString(),
                            Secretaria = dr["Secretaria"].ToString(),
                            Setor = dr["Setor"].ToString(),
                            NivelSalarial = dr["NivelSalarial"].ToString(),
                            Admissao = (DateTime)dr["DataAdmissao"],
                            Situacao = dr["Situacao"].ToString(),
                            AnoParAnoImpar = dr["AnoParImpar"].ToString(),
                        },
                        DataAvaliacao = (DateTime)dr["DataAvaliacao"],
                        Resultado = dr["Resultado"].ToString(),
                        Pontos = (int)dr["Pontos"],
                        DescricaoResultado = dr["DescricaoResultado"].ToString(),
                        Contador = c
                    });
                    c++;
                }

                return lst;

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return null;
                
                //throw new Exception(ex.Message);
            }
        }

        public ObservableCollection<Avaliacoes> Relatorio(List<string> param)
        {
            try
            {
                var lst = new ObservableCollection<Avaliacoes>();

                var dataAccess = Instances.DataBase5();

                dataAccess.ClearParameters();

                dataAccess.AddParameters("@DataAvaliacaoI", param[0]);
                dataAccess.AddParameters("@DataAvaliacaoF", param[1]);
                dataAccess.AddParameters("@Nome", param[2]);
                dataAccess.AddParameters("@Secretaria", param[3]);
                dataAccess.AddParameters("@Setor", param[4]);
                dataAccess.AddParameters("@Situacao", param[5]);
                dataAccess.AddParameters("@AnoParImpar", param[6]);
                dataAccess.AddParameters("@Resultado", param[7]);                
                dataAccess.AddParameters("@DescricaoResultado", param[8]);
                dataAccess.AddParameters("@Pontos", param[9]);

                string sql = @"SELECT * FROM Avaliacoes WHERE (DataAvaliacao BETWEEN @DataAvaliacaoI AND @DataAvaliacaoF) 
AND (Nome LIKE @Nome + '%') 
AND (Secretaria LIKE @Secretaria) 
AND (Setor LIKE @Setor) 
AND (Situacao LIKE @Situacao) 
AND (AnoParImpar LIKE @AnoParImpar) 
AND (Resultado LIKE @Resultado) 
AND (DescricaoResultado LIKE @DescricaoResultado) 
AND (Pontos >= @Pontos)
ORDER BY Nome";

                int c = 1;

                foreach (DataRow dr in dataAccess.Read(sql).Rows)
                {
                    lst.Add(new Avaliacoes()
                    {
                        Indice = (int)dr["Indice"],
                        Servidor = new Servidores()
                        {
                            Codigo = dr["Cod_Servidor"].ToString(),
                            Nome = dr["Nome"].ToString(),
                            Cargo = dr["Cargo"].ToString(),
                            Secretaria = dr["Secretaria"].ToString(),
                            Setor = dr["Setor"].ToString(),
                            NivelSalarial = dr["NivelSalarial"].ToString(),
                            Admissao = (DateTime)dr["DataAdmissao"],
                            Situacao = dr["Situacao"].ToString(),
                            AnoParAnoImpar = dr["AnoParImpar"].ToString(),
                        },
                        DataAvaliacao = (DateTime)dr["DataAvaliacao"],
                        Resultado = dr["Resultado"].ToString(),
                        Pontos = (int)dr["Pontos"],
                        DescricaoResultado = dr["DescricaoResultado"].ToString(),
                        Contador = c
                    });
                    c++;
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

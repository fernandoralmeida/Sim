using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Modulos.Administracao.Model
{

    using Mvvm.Helpers.Notifiers;

    public class Avaliacoes : NotifyProperty
    {
        int indice;
        public int Indice
        {
            get { return indice; }
            set { indice = value; RaisePropertyChanged("Indice"); }
        }
        /// <summary>
        /// Não utilizado no momento
        /// </summary>
        /*
        Servidores servidor;
        public Servidores Servidor { get { return servidor; } set { servidor = value; RaisePropertyChanged("Servidor"); } }
        ^*/

        string codigo;
        public string Codigo
        {
            get { return codigo; }
            set
            {
                codigo = value;
                RaisePropertyChanged("Codigo");
            }
        }

        string nome;
        public string Nome { get { return nome; } set { nome = value; RaisePropertyChanged("Nome"); } }

        string cargo;
        public string Cargo
        {
            get { return cargo; }
            set { cargo = value; RaisePropertyChanged("Cargo"); }
        }

        string secretaria;
        public string Secretaria
        {
            get { return secretaria; }
            set
            {
                secretaria = value;
                RepositorioSetores.GetSecretaria = secretaria;

                RaisePropertyChanged("Secretaria");
            }
        }

        string setor = string.Empty;
        public string Setor { get { return setor; } set { setor = value; RaisePropertyChanged("Setor"); } }

        string nivelsalarial;
        public string NivelSalarial
        {
            get { return nivelsalarial; }
            set
            {
                nivelsalarial = value; RaisePropertyChanged("NivelSalarial");
            }
        }

        DateTime admissao;
        public DateTime Admissao
        {
            get { return admissao; }
            set { admissao = value; RaisePropertyChanged("Admissao"); }
        }
        
        string situacao;
        public string Situacao
        {
            get { return situacao; }
            set { situacao = value; RaisePropertyChanged("Situacao"); }
        }
                
        DateTime dataavaliacao;
        public DateTime DataAvaliacao { get { return dataavaliacao; } set { dataavaliacao = value; RaisePropertyChanged("DataAvaliacao"); } }

        string anoparanoimpar;
        public string AnoParAnoImpar { get { return anoparanoimpar; } set { anoparanoimpar = value; RaisePropertyChanged("AnoParAnoImpar"); } }

        string resultado = string.Empty;
        public string Resultado
        {
            get { return resultado; }
            set { resultado = value;
                RaisePropertyChanged("Resultado");
            }
        }

        int pontosobtido;
        public int Pontos
        {
            get { return pontosobtido; }
            set { pontosobtido = value;
                RaisePropertyChanged("PontosObtido");
            }
        }

        string descricaoresultado = string.Empty;
        public string DescricaoResultado
        {
            get { return descricaoresultado; }
            set { descricaoresultado = value;
                RaisePropertyChanged("DescricaoResultado");
            }
        }

        int contador;
        public int Contador
        {
            get { return contador; }
            set
            {
                contador = value;
                RaisePropertyChanged("Contador");
            }
        }
        
    }
}

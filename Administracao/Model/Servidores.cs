using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Modulos.Administracao.Model
{
    using Mvvm.Helpers.Notifiers;
    using System.Windows.Input.StylusPlugIns;

    public class Servidores : NotifyProperty
    {
        int indice;
        public int Indice { get { return indice; } set { indice = value; RaisePropertyChanged("Indice"); } }

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
                secretaria = value; RaisePropertyChanged("Secretaria");
            }
        }

        string setor;
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

        string anoparanoimpar;
        public string AnoParAnoImpar { get { return anoparanoimpar; } set { anoparanoimpar = value; RaisePropertyChanged("AnoParAnoImpar"); } }

        string situacao;
        public string Situacao
        {
            get { return situacao; }
            set { situacao = value; RaisePropertyChanged("Situacao"); }
        }

        bool ativo;
        public bool Ativo { get { return ativo; } set { ativo = value; RaisePropertyChanged("Ativo"); } }

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Modulos.Administracao.Model
{
    using Mvvm.Helpers.Notifiers;

    public class Cargos : NotifyProperty
    {
        int indice;
        public int Indice { get { return indice; } set { indice = value; RaisePropertyChanged("Indice"); } }

        string cargo;
        public string Cargo { get { return cargo; } set { cargo = value; RaisePropertyChanged("Cargo"); } }

        bool ativo;
        public bool Ativo { get { return ativo; }   set { ativo = value; RaisePropertyChanged("Ativo"); } }
    }
}

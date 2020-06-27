using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Modulos.Administracao.Model
{

    using Mvvm.Helpers.Notifiers;

    public class Secretarias : NotifyProperty
    {
        int indice;
        public int Indice { get { return indice; } set { indice = value; RaisePropertyChanged("Indice"); } }

        string secretaria;
        public string Secretaria { get { return secretaria; } set { secretaria = value; RaisePropertyChanged("Secretaria"); } }

        bool ativo;
        public bool Ativo { get { return ativo; } set { ativo = value; RaisePropertyChanged("Ativo"); } }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Modulos.Legislacao.Model
{

    using Sim.Mvvm.Helpers.Notifiers;

    abstract class mAcoesBase : NotifyProperty
    {
        public int Indice { get; set; }
        public string Acao { get; set; }
        public string TipoAlvo { get; set; }
        public int NumeroAlvo { get; set; }
        public string ComplementoAlvo { get; set; }
        public DateTime DataAlvo { get; set; }
    }
}

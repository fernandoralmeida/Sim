﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Data.OnLine.Correios
{
    public class mEndereco
    {
        public string CEP { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Municipio { get; set; }
        public string UF { get; set; }

        public void Clear()
        {
            Logradouro = string.Empty;
            Bairro = string.Empty;
            Municipio = string.Empty;
            UF = string.Empty;
        }
    }
}

﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Modulos.Empreendedor.Model
{
    using Mvvm.Helpers.Notifiers;

    /// <summary>
    /// Modelo generico para listar/gravar os Tipos: Porte, Origem, UsoLocal, Vinculos, Perfil e Situacao
    /// </summary>
    public class mTiposGenericos : NotifyProperty
    {
        #region Declarations
        private int _indice;
        private int _valor;
        private string _nome;
        private bool _ativo;
        #endregion

        #region Properties
        public int Indice
        {
            get { return _indice; }
            set
            {
                _indice = value;
                RaisePropertyChanged("Indice");
            }
        }
        public int Valor
        {
            get { return _valor; }
            set
            {
                _valor = value;
                RaisePropertyChanged("Valor");
            }
        }
        public string Nome
        {
            get { return _nome; }
            set
            {
                _nome = value;
                RaisePropertyChanged("Nome");
            }
        }
        public bool Ativo
        {
            get { return _ativo; }
            set
            {
                _ativo = value;
                RaisePropertyChanged("Ativo");
            }
        }
        #endregion

        #region Functions

        public void Clear()
        {
            Indice = 0;
            Nome = string.Empty;
            Valor = 0;
            Ativo = true;
        }

        #endregion
    }
}

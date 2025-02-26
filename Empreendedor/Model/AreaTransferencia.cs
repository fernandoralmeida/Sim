﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Modulos.Empreendedor.Model
{

    using Mvvm.Helpers.Notifiers;

    public class AreaTransferencia : GlobalNotifyProperty
    {
        #region Declarations

        private static string _cnpj = string.Empty;
        private static string _cpf = string.Empty;
        private static string _viabilidade = string.Empty;
        private static string _param = string.Empty;
        private static List<string> _inscricoes = new List<string>();
        private static string _evento = string.Empty;
        private static string _cad_ambulante;

        private static bool? _cadpf;
        private static bool? _cadpj;
        private static bool? _cpfon;
        private static bool? _cnpjon;
        private static bool? _meiformalizacao;
        private static bool? _meialterada;
        private static bool? _meibaixada;
        private static bool? _inscricaook;
        private static bool? _viabilidadeok;
        private static bool? _cadambulanteok;
        private static bool? _consambulante;

        #endregion

        #region Properties

        public static string CNPJ
        {
            get { return _cnpj; }
            set
            {
                _cnpj = value;
                OnGlobalPropertyChanged("CNPJ");
            }
        }

        public static string CPF
        {
            get { return _cpf; }
            set
            {
                _cpf = value;
                OnGlobalPropertyChanged("CPF");
            }
        }

        public static string Viabilidade
        {
            get { return _viabilidade; }
            set
            {
                _viabilidade = value;
                OnGlobalPropertyChanged("Viabilidade");
            }
        }

        public static string Parametro
        {
            get { return _param; }
            set
            {
                _param = value;
                OnGlobalPropertyChanged("Parametro");
            }
        }

        public static List<string> Inscricao
        {
            get { return _inscricoes; }
            set
            {
                _inscricoes = value;
                OnGlobalPropertyChanged("Inscricao");
            }
        }

        public static string Evento
        {
            get { return _evento; }
            set
            {
                _evento = value;
                OnGlobalPropertyChanged("Evento");
            }
        }

        public static string CadastroAmbulante
        {
            get { return _cad_ambulante; }
            set
            {
                _cad_ambulante = value;
                OnGlobalPropertyChanged("CadastroAmbulante");
            }
        }

        public static bool? CadPF
        {
            get { return _cadpf; }
            set { _cadpf = value;
                OnGlobalPropertyChanged("CadPF");
            }
        }

        public static bool? CadPJ
        {
            get { return _cadpj; }
            set
            {
                _cadpj = value;
                OnGlobalPropertyChanged("CadPJ");
            }
        }

        public static bool? CPF_On
        {
            get { return _cpfon; }
            set
            {
                _cpfon = value;
                OnGlobalPropertyChanged("CPF_On");
            }
        }

        public static bool? CNPJ_On
        {
            get { return _cnpjon; }
            set
            {
                _cnpjon = value;
                OnGlobalPropertyChanged("CNPJ_On");
            }
        }

        public static bool? MEI_F
        {
            get { return _meiformalizacao; }
            set
            {
                _meiformalizacao = value;
                OnGlobalPropertyChanged("MEI_F");
            }
        }

        public static bool? MEI_A
        {
            get { return _meialterada; }
            set
            {
                _meialterada = value;
                OnGlobalPropertyChanged("MEI_A");
            }
        }

        public static bool? MEI_B
        {
            get { return _meibaixada; }
            set
            {
                _meibaixada = value;
                OnGlobalPropertyChanged("MEI_B");
            }
        }

        public static bool? InscricaoOK
        {
            get { return _inscricaook; }
            set { _inscricaook = value;
                OnGlobalPropertyChanged("InscricaoOK");
            }
        }

        public static bool? ViabilidadeOK
        {
            get { return _viabilidadeok; }
            set
            {
                _viabilidadeok = value;
                OnGlobalPropertyChanged("ViabilidadeOK");
            }
        }

        public static bool? CadAmbulanteOK
        {
            get { return _cadambulanteok; }
            set
            {
                _cadambulanteok = value;
                OnGlobalPropertyChanged("CadAmbulanteOK");
            }
        }

        public static bool? ConsAmbulante
        {
            get { return _consambulante; }
            set { _consambulante = value; OnGlobalPropertyChanged("ConsAmbulante"); }
        }
        #endregion

        #region Functions

        public static void Limpar()
        {
            CNPJ = string.Empty;
            CPF = string.Empty;
            Viabilidade = string.Empty;
            Parametro = string.Empty;
            Inscricao.Clear();
            Evento = string.Empty;
            CadastroAmbulante = string.Empty;
        }
        #endregion
    }
}

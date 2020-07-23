using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics;

namespace Sim.Modulos.Administracao.ViewModel.Servidor
{
    using Sim.Mvvm.Helpers.Notifiers;
    using Sim.Mvvm.Helpers.Commands;
    using Mvvm.Helpers.Observers;
    using Model;

    class Report : NotifyProperty
    {
        #region Declarações
        private NavigationService ns;
        private ObservableCollection<Servidores> _consultaservidores = new ObservableCollection<Servidores>();
        private Visibility _blackbox = Visibility.Collapsed;
        private Visibility _msgbox = Visibility.Collapsed;
        private string _getnome;
        private string _getsecretaria;
        private string _getsetor;
        private string _getsituacao;
        private string _getanoparimpar;
        private string _getcargo;
        private FlowDocument _reportdocument = new FlowDocument();
        private bool _starprogress = false;
        private bool _samedate = false;
        private ObservableCollection<string> _cargos = new ObservableCollection<string>();
        #endregion

        #region Propriedades
        public FlowDocument ReportDocument
        {
            get { return _reportdocument; }
            set
            {
                _reportdocument = value;
                RaisePropertyChanged("ReportDocument");
            }
        }

        public ObservableCollection<Servidores> ConsultaServidores
        {
            get { return _consultaservidores; }
            set
            {
                _consultaservidores = value;
                RaisePropertyChanged("ConsultaServidores");
            }
        }

        public ObservableCollection<string> Cargos
        {
            get { return _cargos; }
            set
            {
                _cargos = value;
                RaisePropertyChanged("ListaCargos");
            }
        }

        public string GetNome
        {
            get { return _getnome; }
            set
            {
                _getnome = value;
                RaisePropertyChanged("GetNome");
            }
        }

        public string GetSecretaria
        {
            get { return _getsecretaria; }
            set
            {
                _getsecretaria = value;
                RaisePropertyChanged("GetSecretaria");
            }
        }

        public string GetSetor
        {
            get { return _getsetor; }
            set
            {
                _getsetor = value;
                RaisePropertyChanged("GetSetor");
            }
        }

        public string GetSituacao
        {
            get { return _getsituacao; }
            set
            {
                _getsituacao = value;
                RaisePropertyChanged("GetSituacao");
            }
        }

        public string GetAnoParImpar
        {
            get { return _getanoparimpar; }
            set
            {
                _getanoparimpar = value;
                RaisePropertyChanged("GetAnoParImpar");
            }
        }

        public string GetCargo
        {
            get { return _getcargo; }
            set
            {
                _getcargo = value;
                RaisePropertyChanged("GetCargo");
            }
        }

        public Visibility BlackBox
        {
            get { return _blackbox; }
            set
            {
                _blackbox = value;
                RaisePropertyChanged("BlackBox");
            }
        }

        public Visibility ShowMsgBox
        {
            get { return _msgbox; }
            set
            {
                _msgbox = value;
                RaisePropertyChanged("ShowMsgBox");
            }
        }

        public bool StartProgress
        {
            get { return _starprogress; }
            set
            {
                _starprogress = value;
                RaisePropertyChanged("StartProgress");
            }
        }

        public bool SameDate
        {
            get { return _samedate; }
            set
            {
                _samedate = value;
                RaisePropertyChanged("SameDate");
            }
        }
        #endregion

        #region Commandos
        public ICommand CommandFiltrar => new RelayCommand(p => {
            Listar().Wait();
            if (Listar().IsCompleted)
            {
                BlackBox = Visibility.Collapsed;
            }
        });

        public ICommand CommandPrint => new RelayCommand(p => {

            try
            {
                StartProgress = true;
                BlackBox = Visibility.Visible;

                // paginador
                ReportDocument.PageHeight = 768;
                ReportDocument.PageWidth = 1104;
                ReportDocument.ColumnWidth = 1104;
                IDocumentPaginatorSource idocument = ReportDocument as IDocumentPaginatorSource;
                idocument.DocumentPaginator.ComputePageCountAsync();

                //Now print using PrintDialog
                var pd = new PrintDialog
                {
                    UserPageRangeEnabled = true
                };
                pd.PrintTicket.PageOrientation = System.Printing.PageOrientation.Landscape;

                if (pd.ShowDialog().Value)
                    pd.PrintDocument(idocument.DocumentPaginator, "Printing....");

                StartProgress = false;
                BlackBox = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                StartProgress = false;
                BlackBox = Visibility.Collapsed;
                MessageBox.Show(ex.Message);
            }

        });

        public ICommand CommandLimpar => new RelayCommand(p => {



        });
        #endregion

        #region Construtor
        public Report()
        {
            GlobalNavigation.Pagina = "RELATÓRIO";
            ns = GlobalNavigation.NavService;
            Cargos = ListarCargos().Result;
        }
        #endregion

        #region Methodos

        private List<string> Parametros()
        {
            var l = new List<string>();

            if (GetNome == string.Empty || GetNome == null)
                l.Add("%");
            else
                l.Add(GetNome);

            if (GetSecretaria == string.Empty || GetSecretaria == null || GetSecretaria == "...")
                l.Add("%");
            else
                l.Add(GetSecretaria);

            if (GetSetor == string.Empty || GetSetor == null)
                l.Add("%");
            else
                l.Add(GetSetor);

            if (GetSituacao == string.Empty || GetSituacao == null || GetSituacao == "...")
                l.Add("%");
            else
                l.Add(GetSituacao);

            if (GetAnoParImpar == string.Empty || GetAnoParImpar == null || GetAnoParImpar == "...")
                l.Add("%");
            else
                l.Add(GetAnoParImpar);

            if (GetCargo == string.Empty || GetCargo == null || GetCargo == "...")
                l.Add("%");
            else
                l.Add(GetCargo);

            return l;
        }

        private Task Listar()
        {
            BlackBox = Visibility.Visible;
            var t = Task.Factory.StartNew(() => {
                ConsultaServidores = new RepositorioServidores().Relatorio(Parametros());

                if (ConsultaServidores.Count >= 0)
                {
                    Application.Current.Dispatcher.BeginInvoke(new System.Threading.ThreadStart(() =>
                    {
                        ReportDocument = ReportPreview(ConsultaServidores);
                    }));
                }
            });
            t.Wait();
            return t;
        }

        public Task<ObservableCollection<string>> ListarCargos()
        {
            var t = Task<ObservableCollection<string>>.Factory.StartNew(() => new RepositorioCargos().TodosCargosAtivos());
            {

            };
            t.Wait();
            return t;
        }

        #endregion

        #region Documento
        private FlowDocument ReportPreview(ObservableCollection<Servidores> obj)
        {

            FlowDocument flow = new FlowDocument();

            flow.ColumnGap = 0;
            flow.Background = Brushes.White;
            //flow.ColumnWidth = 8.5 * 96.0;
            //flow.ColumnWidth =  96.0 * 8.5;
            //flow.PageHeight = 11.5 * 96.0;
            //flow.PageWidth = 8.5 * 96.0;
            flow.PageHeight = 768;
            flow.PageWidth = 1104;
            flow.ColumnWidth = 1104;
            flow.FontFamily = new FontFamily("Segoe UI");
            flow.FontSize = 11;
            flow.PagePadding = new Thickness(40, 20, 40, 20);
            flow.TextAlignment = TextAlignment.Left;

            Paragraph pH = new Paragraph(new Run(new Header().NameOrg));
            pH.Typography.Capitals = FontCapitals.SmallCaps;
            pH.Foreground = Brushes.Black;
            pH.FontSize = 16;
            pH.FontWeight = FontWeights.Bold;
            pH.Margin = new Thickness(0);

            Paragraph pH1 = new Paragraph(new Run(new Header().SloganOrg));
            pH1.Foreground = Brushes.Black;
            pH1.FontSize = 9;
            pH1.Margin = new Thickness(1, 0, 0, 0);

            Paragraph pH2 = new Paragraph(new Run(new Header().DepOrg));
            pH2.Typography.Capitals = FontCapitals.SmallCaps;
            pH2.Foreground = Brushes.Black;
            pH2.FontWeight = FontWeights.Bold;
            pH2.FontSize = 12;
            pH2.Margin = new Thickness(0, 10, 0, 0);

            Paragraph pH3 = new Paragraph(new Run(new Header().SetorOrg));
            pH3.Typography.Capitals = FontCapitals.SmallCaps;
            pH3.FontWeight = FontWeights.Bold;
            pH3.Foreground = Brushes.Black;
            pH3.Margin = new Thickness(0, 0, 0, 20);

            flow.Blocks.Add(pH);
            flow.Blocks.Add(pH1);
            flow.Blocks.Add(pH2);
            flow.Blocks.Add(pH3);

            /*
            var lfiltro = new Rectangle();
            lfiltro.Stroke = new SolidColorBrush(Colors.Silver);
            lfiltro.StrokeThickness = 1;
            lfiltro.Height = 1;
            lfiltro.Width = double.NaN;
            lfiltro.Margin = new Thickness(0, 0, 0, 0);

            var linefiltro = new BlockUIContainer(lfiltro);
            flow.Blocks.Add(linefiltro);            
            */

            //SolidColorBrush bgc1 = Application.Current.Resources["ButtonBackgroundHover"] as SolidColorBrush;

            Table tb = new Table();
            tb.CellSpacing = 0;
            tb.BorderThickness = new Thickness(0.5);
            tb.BorderBrush = Brushes.Black;

            tb.Columns.Add(new TableColumn() { Width = new GridLength(50) });
            tb.Columns.Add(new TableColumn() { Width = GridLength.Auto });
            tb.Columns.Add(new TableColumn() { Width = new GridLength(120) });
            tb.Columns.Add(new TableColumn() { Width = new GridLength(120) });
            tb.Columns.Add(new TableColumn() { Width = new GridLength(120) });
            //tb.Columns.Add(new TableColumn() { Width = new GridLength(200) });

            //tb.Columns.Add(new TableColumn() { Width = new GridLength(10, GridUnitType.Star) }); 

            TableRowGroup rg = new TableRowGroup();

            string f = string.Empty;
            foreach (string filtro in Parametros())
                f += filtro + ";";

            TableRow pheader = new TableRow();
            pheader.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("FILTROS: {0}", f))) { Padding = new Thickness(5) }) { ColumnSpan = 4, BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
            pheader.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("REGISTROS: {0}", obj.Count))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });

            tb.RowGroups.Add(rg);

            Paragraph p = new Paragraph();
            p.Padding = new Thickness(5);

            TableRow header = new TableRow();
            rg.Rows.Add(pheader);
            rg.Rows.Add(header);
            rg.Foreground = Brushes.Black;
            //rw2.Background = bgc1;
            header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run(""))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
            header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("NOME"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
            header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("CARGO"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
            header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("SECRETARIA"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
            header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("SETOR"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
            //header.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("ASSINATURA"))) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(1, 0, 0, 0), BorderBrush = Brushes.Black });

            int alt = 0;

            if (obj != null)
            {

                foreach (Servidores a in obj)
                {

                    TableRow row = new TableRow();
                    rg.Foreground = Brushes.Black;
                    rg.Rows.Add(row);
                    row.Cells.Add(new TableCell(new Paragraph(new Run(a.Contador.ToString())) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(a.Codigo + " - " + a.Nome + "\n" + a.Situacao + ", " + a.AnoParAnoImpar )) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(a.Cargo)) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(a.Secretaria)) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    row.Cells.Add(new TableCell(new Paragraph(new Run(a.Setor)) { Padding = new Thickness(5) }) { BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black });
                    //row.Cells.Add(new TableCell(new Paragraph(new Run("")) { Padding = new Thickness(5, 20, 5, 20) }) { BorderThickness = new Thickness(1, 1, 0, 0), BorderBrush = Brushes.Black });
                    /*
                    if (alt % 2 != 0)
                        row.Background = bgc1;
                        */
                    alt++;
                }
            }

            flow.Blocks.Add(tb);
            /*
            var lrodape = new Rectangle();
            lrodape.Stroke = new SolidColorBrush(Colors.Silver);
            lrodape.StrokeThickness = 1;
            lrodape.Height = 1;
            lrodape.Width = double.NaN;
            lrodape.Margin = new Thickness(0, 10, 0, 0);

            var lineBlock = new BlockUIContainer(lrodape);
            flow.Blocks.Add(lineBlock);
            */

            Paragraph r = new Paragraph();
            r.Margin = new Thickness(0, 0, 0, 0);
            r.FontSize = 10;

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;

            //r.Inlines.Add(lrodape);
            r.Inlines.Add(new Run(AppDomain.CurrentDomain.FriendlyName.ToUpper()));
            r.Inlines.Add(new Run(" / V" + version + " / "));
            r.Inlines.Add(new Bold(new Run(Accounts.AccountOn.Nome)));
            r.Inlines.Add(new Run(" / " + DateTime.Now.ToString("dd.MM.yyyy")));
            r.Inlines.Add(new Run(" / " + DateTime.Now.ToLongTimeString()));


            flow.Blocks.Add(r);

            return flow;
        }
        #endregion
    }
}

using System;
using System.Windows;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Sim.App.ViewModel
{

    using Modulos.Accounts;
    using Common.Helpers;
    using Mvvm.Helpers.Notifiers;
    using Mvvm.Helpers.Commands;
    using Mvvm.Helpers.Observers;
    using ModernUI.Presentation;
    using ModernUI.Model;


    class vmToolsTheme : NotifyProperty
    {
        #region Declarations
        private NavigationService ns;
        private ThemeManager tm = new ThemeManager();
        private PaletteColors pc = new PaletteColors();
        private ObservableCollection<mTheme> _themes = new ObservableCollection<mTheme>();
        private ObservableCollection<mColor> _colorthemes = new ObservableCollection<mColor>();
        private Color selectedAccentColor;
        private string _selectedthememode;
        private string _themename;
        private Visibility _yesnot;
        private Visibility _blackbox;
        private bool _starprogress;
        #endregion

        #region Properties

        public string ThemeName
        {
            get { return _themename; }
            set { _themename = value;
                RaisePropertyChanged("ThemeName");
            }
        }

        public string SelectedThemeMode
        {
            get { return _selectedthememode; }
            set
            {
                _selectedthememode = value;
                RaisePropertyChanged("SelectedThemeMode");
            }
        }

        public Color SelectedAccentColor
        {
            get { return this.selectedAccentColor; }
            set
            {
                if (this.selectedAccentColor != value)
                {
                    this.selectedAccentColor = value;                
                    RaisePropertyChanged("SelectedAccentColor");
                }
            }
        }

        public ObservableCollection<mTheme> ThemeMode
        {
            get { return _themes; }
            set
            {
                _themes = value;
                RaisePropertyChanged("ThemeMode");
            }
        }

        public ObservableCollection<mColor> ColorThemes
        {
            get { return _colorthemes; }
            set
            {
                _colorthemes = value;
                RaisePropertyChanged("ColorThemes");
            }
        }

        public Visibility ViewYesNot
        {
            get { return _yesnot; }
            set
            {
                _yesnot = value;
                RaisePropertyChanged("ViewYesNot");
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

        public bool StartProgress
        {
            get { return _starprogress; }
            set
            {
                _starprogress = value;
                RaisePropertyChanged("StartProgress");
            }
        }
        #endregion

        #region Command
        public ICommand CommandApplyTheme => new RelayCommand(p =>
        {
            //AsyncAccountTheme();
            AsyncSaveTheme();
            if (ns.CanGoBack)
                ns.GoBack();

        });

        public ICommand CommandGoBack => new RelayCommand(p =>
        {
            CheckTheme();
            if (ns.CanGoBack)
                ns.GoBack();
        });
        #endregion

        #region Constructor

        public vmToolsTheme()
        {
            ns = GlobalNavigation.NavService;
            GlobalNavigation.Pagina = "PERSONALIZAR";
            GlobalNavigation.BrowseBack = Visibility.Visible;
            this.PropertyChanged += VmTheme_PropertyChanged;
            AsyncThemes();
            
        }

        #endregion

        #region Methods

        private void VmTheme_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedAccentColor" || e.PropertyName == "SelectedThemeMode")
            {
                tm.Apply(SelectedAccentColor, SelectedThemeMode);
                SelectedAccentColor = (Color)Application.Current.Resources["AccentColor"];
                //AsyncSaveTheme();
            }
        }

        private void CheckTheme()
        {
            try
            {
                Color paleta = new Color();
                paleta = (Color)ColorConverter.ConvertFromString(AccountOn.Color);
                SelectedThemeMode = AccountOn.Thema;
                SelectedAccentColor = paleta;            
            }
            catch
            {
                SelectedAccentColor = Colors.CornflowerBlue;
                SelectedThemeMode = "Light";
            }
                        
        }

        private void AsyncSaveTheme()
        {
            var t = Task.Factory.StartNew(() => {

                var opc = new Modulos.Accounts.Model.mOpcoes
                {
                    Identificador = AccountOn.Identificador,
                    Thema = SelectedThemeMode.ToString(),
                    Color = SelectedAccentColor.ToString()
                };

                AccountOn.Color = opc.Color;
                AccountOn.Thema = opc.Thema;

                if (AccountOn.Identificador.ToLower() != "System".ToLower())
                    new Modulos.Accounts.Model.mData().GravarOpcoes(opc);

            });
        }

        private void AsyncThemes()
        {
            var t = Task.Factory.StartNew(() => {
                ColorThemes = pc.ListColors();
                ThemeMode = tm.ListThemes();
            });

            CheckTheme();
        }

        #endregion
    }
}

using System;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Sim.ModernUI.Presentation
{

    using Presentation;
    using Model;
    using Sim.Resources;


    //public enum Themes { Light, Dark, SmileSnow, GreenGrass, WaterGlass, SnowFlakes }

    public class ThemeManager : NotifyPropertyChanged
    {

        #region Declarations

        public const string KeyAccentColor = "AccentColor";
        public const string KeyAccent = "Accent";

        private static ThemeManager current = new ThemeManager();
        ObservableCollection<mTheme> gthemes = new ObservableCollection<mTheme>();

        #endregion

        #region Properties

        private ObservableCollection<mTheme> GetThemes
        {
            get
            {
                return gthemes;
            }
            set
            {
                gthemes = value;
                OnPropertyChanged("GetThemes");
            }
        }

        #endregion

        #region Methods

        public void Apply(Color color, string theme) 
        {
            ApplyAccentColor(color);
            ApplyTheme(theme);
            ThemeObserver.ThemeSource = true;
        }

        private void ApplyAccentColor(Color accentColor)
        {
            // set accent color and brush resources
            if (accentColor != null) { 
                Application.Current.Resources[KeyAccentColor] = accentColor;
                Application.Current.Resources[KeyAccent] = new SolidColorBrush(accentColor);}
        }

        private void ApplyTheme(string theme)
        {
            if (theme != null)
            {
                //Assembly assembly = Assembly.LoadFrom("Sim.ModernUI.dll");

                var ThemeMode = new ResourceDictionary
                {
                    Source =
                    new Uri(string.Format(@"/Sim.Modernui;component/Themes/Flat/ModernUI.{0}.xaml", theme), UriKind.RelativeOrAbsolute)
                };

                var ThemeBase = new ResourceDictionary
                {
                    Source =
                     new Uri(@"/Sim.Modernui;component/Themes/Flat/ModernUI.xaml", UriKind.RelativeOrAbsolute)
                };

                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(ThemeBase);
                Application.Current.Resources.MergedDictionaries.Add(ThemeMode);
            }
        }

        #endregion

        #region Constructors

        public ThemeManager()
        {   }

        #endregion

        #region Functions
        public ObservableCollection<mTheme> ListThemes()
        {
            ObservableCollection<mTheme> _theme = new ObservableCollection<mTheme>();

            _theme.Clear();
            _theme.Add(new mTheme() { Name = "BingImage", Descricao = "IMGENS DO BING", Imagem = BingImage.UriBing });
            _theme.Add(new mTheme() { Name = "Light", Descricao = "LIGHT", Imagem = new Uri("/Sim.Resources;component/Wallpaper/light.jpg", UriKind.Relative) });
            _theme.Add(new mTheme() { Name = "BoraBora", Descricao = "BORA BORA", Imagem = new Uri("/Sim.Resources;component/Wallpaper/bora_bora.jpg", UriKind.Relative) });
            _theme.Add(new mTheme() { Name = "GreenGrass", Descricao = "GREEN GRASS", Imagem = new Uri("/Sim.Resources;component/Wallpaper/water_drops_on_grass.jpg", UriKind.Relative) });
            _theme.Add(new mTheme() { Name = "SnowFlakes", Descricao = "SNOW FLAKES", Imagem = new Uri("/Sim.Resources;component/Wallpaper/snow_flakes.jpg", UriKind.Relative) });
            _theme.Add(new mTheme() { Name = "SmileSnow", Descricao = "SMILE SNOW", Imagem = new Uri("/Sim.Resources;component/Wallpaper/smile_on_snow.jpg", UriKind.Relative) });
            _theme.Add(new mTheme() { Name = "Dark", Descricao = "DARK", Imagem = new Uri("/Sim.Resources;component/Wallpaper/dark.jpg", UriKind.Relative) });
            _theme.Add(new mTheme() { Name = "WaterGlass", Descricao = "WATER DROPS", Imagem = new Uri("/Sim.Resources;component/Wallpaper/water_drops.jpg", UriKind.Relative) });
            _theme.Add(new mTheme() { Name = "DarkGrass", Descricao = "DARK GRASS", Imagem = new Uri("/Sim.Resources;component/Wallpaper/dark_grass.jpg", UriKind.Relative) });
            _theme.Add(new mTheme() { Name = "Aventador", Descricao = "AVENTADOR", Imagem = new Uri("/Sim.Resources;component/Wallpaper/aventador.jpg", UriKind.Relative) });

            return _theme;
        }
        #endregion

    }
}

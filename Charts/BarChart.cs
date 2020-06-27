using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Sim.Charts
{

    using Sim.Charts.Model;
    using System.Windows.Threading;

    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Sim.Charts"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Sim.Charts;assembly=Sim.Charts"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:BarChart/>
    ///
    /// </summary>
    [TemplatePart(Name = Box_List, Type =typeof(ListBox))]
    public class BarChart : Control, INotifyPropertyChanged
    {
        static BarChart()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BarChart), new FrameworkPropertyMetadata(typeof(BarChart)));
        }

        #region  Fields
        
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(BarChart), new FrameworkPropertyMetadata(string.Empty));
        //public static readonly DependencyProperty ListSourceProperty = DependencyProperty.Register("ListSource", typeof(ObservableCollection<mBarChart>), typeof(BarChart), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty RelativeWidthProperty = DependencyProperty.Register("RelativeWidth", typeof(double), typeof(BarChart), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty LegendasOnOffProperty = DependencyProperty.Register("LegendasOnOff", typeof(Visibility), typeof(BarChart), new FrameworkPropertyMetadata(null));
        /// <summary>
        /// Registers a dependency property as backing store for the Content property
        /// </summary>
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", 
            typeof(object), typeof(BarChart),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        #endregion

        #region Declarations
        public const string Box_List = "BOX_List";
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<mBarChart> _listsource = new ObservableCollection<mBarChart>();
        private List<KeyValuePair<string, int>> _itemssource = new List<KeyValuePair<string, int>>();
        private ListBox _box_list;
        #endregion

        #region Properties

        public string Title
        {
            get { return base.GetValue(TitleProperty) as string; }
            set
            {
                base.SetValue(TitleProperty, value);
                RaisePropertyChanged("Title");
            }
        }

        /// <summary>
        /// Gets or sets the Content.
        /// </summary>
        /// <value>The Content.</value>
        public object Content
        {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public ListBox ListBoxChart
        {
            get { return _box_list; }
            set { _box_list = value; }
        }

        public List<KeyValuePair<string, int>> ItemsSource
        {
            get { return _itemssource; }
            set
            {
                _itemssource = value;
                RaisePropertyChanged("ItemsSource");
            }
        }  

        public Visibility LegendasOnOff
        {
            get { return (Visibility)GetValue(LegendasOnOffProperty); }
            set { SetValue(LegendasOnOffProperty, value);
                RaisePropertyChanged("LegendasOnOff");
            }
        }

        public double RelativeWidth
        {
            get { return (double)GetValue(RelativeWidthProperty); }
            set
            {
                SetValue(RelativeWidthProperty, value);
                RaisePropertyChanged("RelativeWidth");
            }
        }

        #endregion

        #region Methods

        private ObservableCollection<mBarChart> ToListSource()
        {
            _listsource.Clear();

            double max_bar_width = 0;

            foreach (KeyValuePair<string, int> x in ItemsSource)
            {
                max_bar_width += x.Value;
            }

            foreach (KeyValuePair<string, int> x in ItemsSource)
            {
                mBarChart _chart = new mBarChart();

                double _relwidth = RelativeWidth;

                if (_relwidth == 0)
                    _relwidth = 600;

                double _current_wdth = (x.Value * (_relwidth * 0.5)) / max_bar_width;
                                     
                double _percent = x.Value / max_bar_width;

                if (_current_wdth < 1)
                    _current_wdth = 1;

                _chart.Key = x.Key;
                _chart.Width = _current_wdth;
                _chart.Value = x.Value;
                _chart.Percent = _percent;
                _chart.Total = max_bar_width;

                _listsource.Add(_chart);
            }
            
            return _listsource;
        }

        protected void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion

        #region Overrides

        protected override Size MeasureOverride(Size constraint)
        {
            RelativeWidth = constraint.Width;
            return base.MeasureOverride(constraint);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ListBoxChart = Template.FindName(Box_List, this) as ListBox;            
            //RelativeWidth = 1000;
            ListBoxChart.ItemsSource = ToListSource();
        }

        #endregion
    }
    /*
    public static class ExtensionMethods
    {

        private static Action EmptyDelegate = delegate () { };

        public static void Refresh(this UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }

    }*/
}

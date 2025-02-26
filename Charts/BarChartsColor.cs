﻿using System;
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
    [TemplatePart(Name = PartBars, Type = typeof(ItemsControl))]
    [TemplatePart(Name = PartLegendas, Type = typeof(ItemsControl))]
    public class BarChartsColor : Control, INotifyPropertyChanged
    {
        static BarChartsColor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BarChartsColor), new FrameworkPropertyMetadata(typeof(BarChartsColor)));
        }

        #region  Fields
        public const string PartBars = "PART_Bars";
        public const string PartLegendas = "PART_Leg";
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(BarChartsColor), new FrameworkPropertyMetadata(string.Empty));
        //public static readonly DependencyProperty ListSourceProperty = DependencyProperty.Register("ListSource", typeof(ObservableCollection<mBarChart>), typeof(BarChart), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty RelativeWidthProperty = DependencyProperty.Register("RelativeWidth", typeof(double), typeof(BarChartsColor), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty LegendasOnOffProperty = DependencyProperty.Register("LegendasOnOff", typeof(Visibility), typeof(BarChartsColor), new FrameworkPropertyMetadata(null));
        /// <summary>
        /// Registers a dependency property as backing store for the Content property
        /// </summary>
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content",
            typeof(object), typeof(BarChartsColor),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        private ItemsControl _partbars;
        private ItemsControl _partlegs;

        #endregion

        #region Declarations

        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<mBarChart> _listsource = new ObservableCollection<mBarChart>();
        private List<KeyValuePair<string, int>> _itemssource = new List<KeyValuePair<string, int>>();

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

        public ItemsControl ItemsControlBars
        {
            get { return _partbars; }
            set { _partbars = value; }
        }

        public ItemsControl ItemsControlLegs
        {
            get { return _partlegs; }
            set { _partlegs = value; }
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
            set
            {
                SetValue(LegendasOnOffProperty, value);
                RaisePropertyChanged("LegendasOnOff");
            }
        }

        public double RelativeWidth
        {
            get { return (double)GetValue(RelativeWidthProperty); }
            set
            {
                SetValue(RelativeWidthProperty, ItemsControlBars.RenderSize.Width);
                ItemsControlBars.ItemsSource = ToListSource();
                ItemsControlLegs.ItemsSource = ToListSource();
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
                    _relwidth = 500;

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

            if (_listsource.Count < 1)
                LegendasOnOff = Visibility.Collapsed;
            else
                LegendasOnOff = Visibility.Visible;

            return _listsource;
        }

        private void ItemsControlBars_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ItemsControlBars.ItemsSource = ToListSource();
        }

        private void ItemsControlLegs_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ItemsControlLegs.ItemsSource = ToListSource();
        }

        protected void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
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

            ItemsControlBars = Template.FindName(PartBars, this) as ItemsControl;
            ItemsControlLegs = Template.FindName(PartLegendas, this) as ItemsControl;

            ItemsControlBars.SizeChanged += ItemsControlBars_SizeChanged;

            ItemsControlLegs.SizeChanged += ItemsControlLegs_SizeChanged;
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

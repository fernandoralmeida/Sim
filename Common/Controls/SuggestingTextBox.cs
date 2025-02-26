﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sim.Common.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Sim.Common.Controls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Sim.Common.Controls;assembly=Sim.Common.Controls"
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
    ///     <MyNamespace:SuggestingTextBox/>
    ///
    /// </summary>
    [TemplatePart(Name = PartEditor, Type = typeof(TextBox))]
    [TemplatePart(Name = PartPopup, Type = typeof(Popup))]
    [TemplatePart(Name = PartSelector, Type = typeof(Selector))]
    public class SuggestingTextBox : Control
    {

        static SuggestingTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SuggestingTextBox), new FrameworkPropertyMetadata(typeof(SuggestingTextBox)));
        }

        #region Declarações        

        public const string PartEditor = "PART_Editor";
        public const string PartPopup = "PART_Popup";
        public const string PartSelector = "PART_Selector";

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            "ItemsSource", typeof(IEnumerable), typeof(SuggestingTextBox), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty DelayProperty = DependencyProperty.Register(
            "Delay", typeof(int), typeof(SuggestingTextBox), new FrameworkPropertyMetadata(200));

        public static readonly DependencyProperty ThresholdProperty = DependencyProperty.Register(
            "Threshold", typeof(int), typeof(SuggestingTextBox), new FrameworkPropertyMetadata(2));

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(SuggestingTextBox), new FrameworkPropertyMetadata(string.Empty));
        
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            "SelectedItem", typeof(object), typeof(SuggestingTextBox), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(
            "IsDropDownOpen", typeof(bool), typeof(SuggestingTextBox), new FrameworkPropertyMetadata(false));
                
        private TextBox _textbox;
        private Popup _popup;
        private Selector _itemselector;

        private Timer _keypressTimer;
        private delegate void TextChangedCallBack();

        private bool _keyarrow;

        private List<string> _list; 

        #endregion

        #region Propriedades

        public TextBox Editor
        {
            get { return _textbox; }
            set { _textbox = value; }
        }

        public Selector ItemsSelector
        {
            get { return _itemselector; }
            set { _itemselector = value; }
        }

        public Popup Popup
        {
            get { return _popup; }
            set { _popup = value; }
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set
            {
                SetValue(ItemsSourceProperty, value);                
            }
        }

        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }

            set { SetValue(IsDropDownOpenProperty, value); }
        }

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }

            set { SetValue(SelectedItemProperty, value); }
        }

        public int Delay
        {
            get { return (int)GetValue(DelayProperty); }
            set
            {
                SetValue(DelayProperty, value);
            }
        }

        public int Threshold
        {
            get { return (int)GetValue(ThresholdProperty); }
            set { SetValue(ThresholdProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set
            {
                Editor.Text = value;
                SetValue(TextProperty, value);                
            }
        }

        #endregion

        #region Metodos

        private void OnEditorTextChanged(object sender, TextChangedEventArgs e)
        {
            if (Delay > 0)
            {
                _keypressTimer.Interval = Delay;
                _keypressTimer.Start();
            }
            else
                TextChanged();
        }
        
        private void _keypressTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _keypressTimer.Stop();
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new TextChangedCallBack(TextChanged));
        }

        private void TextChanged()
        {
            try
            {

                _list = new List<string>();

                if (Editor.Text.Length >= Threshold)
                {

                    foreach (string word in ItemsSource)
                    {
                        if (word.StartsWith(Editor.Text, StringComparison.CurrentCultureIgnoreCase))
                            _list.Add(word);
                    }                    
                }

                ItemsSelector.ItemsSource = _list;

                if (Editor.Text != (string)SelectedItem)
                    IsDropDownOpen = ItemsSelector.HasItems;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        
        private void OnPopupClosed(object sender, EventArgs e)
        {
            SelectedItem = ItemsSelector.SelectedItem;

            if (SelectedItem != null)
                Text = (string)SelectedItem;

            Editor.Focus();
            _keyarrow = false;
        }

        private void ItemsSelector_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.IsDown && e.Key == Key.Return)
                IsDropDownOpen = false;
        }

        private void Editor_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.IsDown && e.Key == Key.Down)
            {
                ItemsSelector.Focus();
                _keyarrow = true;
            }
        }

        private void ItemsSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ItemsSelector != null)
            {
                SelectedItem = ItemsSelector.SelectedValue;

                if (_keyarrow == false)
                    IsDropDownOpen = false;
            }
        }
        
        private void ItemsSelector_MouseMove(object sender, MouseEventArgs e)
        {
            _keyarrow = false;
        }

        #endregion

        #region Overrides

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _keypressTimer = new Timer();
            _keypressTimer.Elapsed += _keypressTimer_Elapsed;
            
            Editor = Template.FindName(PartEditor, this) as TextBox;
            Popup = Template.FindName(PartPopup, this) as Popup;
            ItemsSelector = Template.FindName(PartSelector, this) as Selector;

            ItemsSelector.SelectionChanged += ItemsSelector_SelectionChanged;
            ItemsSelector.PreviewKeyDown += ItemsSelector_PreviewKeyDown;
            ItemsSelector.MouseMove += ItemsSelector_MouseMove;

            if (Editor != null)
            {
                Editor.TextChanged += OnEditorTextChanged;
                Editor.PreviewKeyDown += Editor_PreviewKeyDown;
            }

            if (Popup != null)
            {
                Popup.StaysOpen = false;
                Popup.Closed += OnPopupClosed;
            }

            _keyarrow = false;
        }


        #endregion

    }
}

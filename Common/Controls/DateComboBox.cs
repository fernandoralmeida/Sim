using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sim.Common.Controls
{
    using Sim.Common.Model;
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
    ///     <MyNamespace:DateComboBox/>
    ///
    /// </summary>
    ///    
    [TemplatePart(Name = PartDay, Type = typeof(ComboBox))]
    [TemplatePart(Name = PartMonth, Type = typeof(ComboBox))]
    [TemplatePart(Name = PartYear, Type = typeof(ComboBox))]
    [TemplatePart(Name = PartText, Type = typeof(TextBox))]
    public class DateComboBox : Control, INotifyPropertyChanged
    {
        static DateComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DateComboBox), new FrameworkPropertyMetadata(typeof(DateComboBox)));
        }

        #region Declarations

        public const string PartDay = "PART_Day";
        public const string PartMonth = "PART_Month";
        public const string PartYear = "PART_Year";
        public const string PartText = "PART_Text";

        //private event EventHandler<SelectionChangedEventArgs> SelectedDateChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        private static readonly DependencyProperty GetDayProperty = DependencyProperty.Register(
            "GetDay", typeof(string), typeof(DateComboBox), new FrameworkPropertyMetadata(string.Empty));

        private static readonly DependencyProperty GetMonthProperty = DependencyProperty.Register(
            "GetMonth", typeof(string), typeof(DateComboBox), new FrameworkPropertyMetadata(string.Empty));

        private static readonly DependencyProperty GetYearProperty = DependencyProperty.Register(
            "GetYear", typeof(string), typeof(DateComboBox), new FrameworkPropertyMetadata(string.Empty));

        public static readonly DependencyProperty SelectedDateProperty = DependencyProperty.Register(
            "SelectedDate", typeof(DateTime), typeof(DateComboBox), new FrameworkPropertyMetadata(null));

        private ComboBox _comboDay;
        private ComboBox _comboMonth;
        private ComboBox _comboYear;
        private TextBox _datetext;
        

        private string _date;
        #endregion

        #region Properties

        private string GetDay
        {
            get { return (string)GetValue(GetDayProperty); }
            set
            {
                SetValue(GetDayProperty, value);
            }
        }

        private string GetMonth
        {
            get { return (string)GetValue(GetMonthProperty); }
            set
            {
                SetValue(GetMonthProperty, value);
            }
        }

        private string GetYear
        {
            get { return (string)GetValue(GetYearProperty); }
            set
            {
                SetValue(GetYearProperty, value);
            }
        }

        public DateTime SelectedDate
        {
            get { return (DateTime)GetValue(SelectedDateProperty); }
            set
            {
                
                SetValue(SelectedDateProperty, value);
                RaisePropertyChanged("SelectedDate");
            }
        }

        private string UpdateSelectedDate
        {
            get { return _date; }
            set
            {
                _date = value;
                RaisePropertyChanged("UpdateSelectedDate");
            }
        }

        #endregion

        #region Methods


        private void _datatext_TextChanged(object sender, TextChangedEventArgs e)
        {
            //MessageBox.Show(_datetext.Text);
            if (_datetext.Text != null || _datetext.Text != string.Empty)
                SelectedDate = Convert.ToDateTime(_datetext.Text);
            
        }

        protected void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));                
            }
        }

        #endregion

        #region Overridies

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _comboDay = Template.FindName(PartDay, this) as ComboBox;
            _comboMonth = Template.FindName(PartMonth, this) as ComboBox;
            _comboYear = Template.FindName(PartYear, this) as ComboBox;
            _datetext = Template.FindName(PartText, this) as TextBox;
            
            List<mDays> dias = new List<mDays>();
            //dias.Add(new mDays() { Day = "Dia", Value = "0" });
            for (int i = 1; i < 32; i++)
                dias.Add(new mDays() { Day = i.ToString(), Value = i.ToString() });

            _comboDay.ItemsSource = dias;

            List<mMonths> mes = new List<mMonths>();
            //mes.Add(new mMonths() { Month = "Mês", Value = "0" });
            mes.Add(new mMonths() { Month = "Janeiro", Value = "1" });
            mes.Add(new mMonths() { Month = "Fevereiro", Value = "2" });
            mes.Add(new mMonths() { Month = "Março", Value = "3" });
            mes.Add(new mMonths() { Month = "Abril", Value = "4" });
            mes.Add(new mMonths() { Month = "Maio", Value = "5" });
            mes.Add(new mMonths() { Month = "Junho", Value = "6" });
            mes.Add(new mMonths() { Month = "Julho", Value = "7" });
            mes.Add(new mMonths() { Month = "Agosto", Value = "8" });
            mes.Add(new mMonths() { Month = "Setembro", Value = "9" });
            mes.Add(new mMonths() { Month = "Outubro", Value = "10" });
            mes.Add(new mMonths() { Month = "Novembro", Value = "11" });
            mes.Add(new mMonths() { Month = "Dezembro", Value = "12" });

            _comboMonth.ItemsSource = mes;

            List<mYears> ano = new List<mYears>();
            //ano.Add(new mYears() { Year = "Ano", Value = "0001" });
            for (int i = DateTime.Now.Year; i > 1852; i--)
                ano.Add(new mYears() { Year = i.ToString(), Value = i.ToString() });

            _comboYear.ItemsSource = ano;
            
            GetDay = SelectedDate.Day.ToString();
            GetMonth = SelectedDate.Month.ToString();
            GetYear = SelectedDate.Year.ToString();

            _datetext.TextChanged += _datatext_TextChanged;
        }


        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == SelectedDateProperty)
            {
                if (GetDay != SelectedDate.Day.ToString())
                    GetDay = SelectedDate.Day.ToString();

                if (GetMonth != SelectedDate.Month.ToString())
                    GetMonth = SelectedDate.Month.ToString();

                if(GetYear != SelectedDate.Year.ToString())
                    GetYear = SelectedDate.Year.ToString();
                
                if (_datetext != null)
                    if (_datetext.Text != SelectedDate.ToShortDateString())
                        _datetext.Text = SelectedDate.ToShortDateString();

            }
            
            if (e.Property == GetDayProperty)
            {
                if (GetDay != SelectedDate.Day.ToString())
                    _datetext.Text = GetDay + "/" + GetMonth + "/" + GetYear;
            }

            if (e.Property == GetMonthProperty)
            {
                if (GetMonth != SelectedDate.Month.ToString())
                    _datetext.Text = GetDay + "/" + GetMonth + "/" + GetYear;
            }

            if (e.Property == GetYearProperty)
            {
                if (GetYear != SelectedDate.Year.ToString())
                    _datetext.Text = GetDay + "/" + GetMonth + "/" + GetYear;
            }

        }


        #endregion

    }
}

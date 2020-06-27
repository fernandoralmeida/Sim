using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;

namespace Sim.ModernUI.Model
{
    public class mColor
    {
        private string _name = string.Empty;
        public string Name
        {
            get { return _name.ToUpper(); }
            set { _name = value; }
        }
        public Color Value { get; set; }
    }
}

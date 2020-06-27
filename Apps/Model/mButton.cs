using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Sim.App.Model
{
    class mButton
    {
        public string Rotulo { get; set; }
        public ICommand CommandExecute { get; set; }
    }
}

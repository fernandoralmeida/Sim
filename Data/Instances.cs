using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sim.Data
{
    public class Instances
    {
        public static IData DataM()
        {
            return new DataB();
        }

        public static IData DataA()
        {
            return new DataA();
        }

        public static IData DataM1()
        {
            return new DataC();
        }

        public static IData DataBase4() { return new Data(Connections.DataBase4); }

        public static IData DataBase5() { return new Data(Connections.DataBase5); }
    }
}

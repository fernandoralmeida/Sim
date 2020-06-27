using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Sim.Modulos.Legislacao.Model
{
    using Sim.Data;

    class mData
    {
        public bool BlockNoBlock(string commandsql, mTiposGenericos obj)
        {
            
            IData mdata = Instances.DataM();
            
            mdata.ClearParameters();            
            mdata.AddParameters("@Bloqueado", obj.Bloqueado);
            mdata.AddParameters("@Codigo", obj.Codigo);

            return mdata.Write(commandsql);
        }

        public bool InsertTiposGenericos(string sqlcommand1, string sqlcommand2, mTiposGenericos obj)
        {

            IData mdata = Instances.DataM();

            int last_codigo = 0;

            foreach (DataRow dr in mdata.Read(sqlcommand2).Rows)
                last_codigo = (int)dr[0] + 1;

            mdata.ClearParameters();

            mdata.AddParameters("@Codigo", last_codigo);
            mdata.AddParameters("@Nome", obj.Nome);
            mdata.AddParameters("@Cadastro", obj.Cadastro.ToShortDateString());
            mdata.AddParameters("@Alterado", obj.Alterado.ToShortDateString());
            mdata.AddParameters("@Bloqueado", obj.Bloqueado);
            
            return mdata.Write(sqlcommand1);
        }
    }
}

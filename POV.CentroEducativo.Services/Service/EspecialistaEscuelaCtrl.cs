using Framework.Base.DataAccess;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.DAO;
using System;
using System.Data;

namespace POV.CentroEducativo.Service
{

    public class EspecialistaEscuelaCtrl
    {
        public DataSet Retrieve(IDataContext dctx, EspecialistaEscuela especialistaEscuela)
        {
            EspecialistaEscuelaRetHlp da = new EspecialistaEscuelaRetHlp();
            DataSet ds = da.Action(dctx, especialistaEscuela);
            return ds;
        }

        public void Insert(IDataContext dctx, EspecialistaEscuela especialistaEscuela)
        {
            EspecialistaEscuelaInsHlp da = new EspecialistaEscuelaInsHlp();
            da.Action(dctx, especialistaEscuela);
        }

        public void Update(IDataContext dctx, EspecialistaEscuela especialistaEscuela, EspecialistaEscuela previous)
        {
            EspecialistaEscuelaUpdHlp da = new EspecialistaEscuelaUpdHlp();
            da.Action(dctx, especialistaEscuela, previous);
        }

        public void Delete(IDataContext dctx, EspecialistaEscuela especialistaEscuela)
        {
            EspecialistaEscuelaDelHlp da = new EspecialistaEscuelaDelHlp();
            da.Action(dctx, especialistaEscuela);
        }

        public EspecialistaEscuela LastDataRowToEspecialistaEscuela(DataSet ds)
        {
            if (!ds.Tables.Contains("EspecialistaEscuela"))
                throw new Exception("LastDataRowToEspecialistaEscuela: DataSet no tiene la tabla EspecialistaEscuelaEscuela");
            int index = ds.Tables["EspecialistaEscuela"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToEspecialistaEscuela: El DataSet no tiene filas");
            return this.DataRowToEspecialistaEscuela(ds.Tables["EspecialistaEscuela"].Rows[index - 1]);
        }
        
        public EspecialistaEscuela DataRowToEspecialistaEscuela(DataRow row)
        {
            EspecialistaEscuela especialistaEscuela = new EspecialistaEscuela();
            if (row.IsNull("EspecialistaEscuelaID"))
                especialistaEscuela.EspecialistaEscuelaID = null;
            else
                especialistaEscuela.EspecialistaEscuelaID = (long)Convert.ChangeType(row["EspecialistaEscuelaID"], typeof(long));
            if (row.IsNull("EspecialistaID"))
                especialistaEscuela.EspecialistaEscuelaID = null;
            else
                especialistaEscuela.EspecialistaID = (int)Convert.ChangeType(row["EspecialistaID"], typeof(int));
            if (row.IsNull("EscuelaID"))
                especialistaEscuela.EscuelaID = null;
            else
                especialistaEscuela.EscuelaID = (int)Convert.ChangeType(row["EscuelaID"], typeof(int));
            if (row.IsNull("Estatus"))
                especialistaEscuela.Estatus = null;
            else
                especialistaEscuela.Estatus = (bool)Convert.ChangeType(row["Estatus"], typeof(bool));
            return especialistaEscuela;
        }      
    }
}

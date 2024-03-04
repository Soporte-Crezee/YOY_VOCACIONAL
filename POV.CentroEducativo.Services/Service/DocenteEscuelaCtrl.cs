// Clase motor de red social
using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.DAO;
using POV.Localizacion.Service;
using POV.Localizacion.BO;

namespace POV.CentroEducativo.Service { 
   /// <summary>
   /// Controlador del objeto DocenteEscuela
   /// </summary>
   public class DocenteEscuelaCtrl { 
      /// <summary>
      /// Consulta registros de DocenteEscuelaRetHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="docenteEscuelaRetHlp">DocenteEscuelaRetHlp que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de DocenteEscuelaRetHlp generada por la consulta</returns>
      public DataSet Retrieve(IDataContext dctx, DocenteEscuela docenteEscuela){
         DocenteEscuelaRetHlp da = new DocenteEscuelaRetHlp();
         DataSet ds = da.Action(dctx, docenteEscuela);
         return ds;
      }
      /// <summary>
      /// Crea un registro de DocenteEscuelaInsHlp en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="docenteEscuelaInsHlp">DocenteEscuelaInsHlp que desea crear</param>
      public void Insert(IDataContext dctx, DocenteEscuela  docenteEscuela){
         DocenteEscuelaInsHlp da = new DocenteEscuelaInsHlp();
         da.Action(dctx,  docenteEscuela);
      }
      /// <summary>
      /// Actualiza de manera optimista un registro de DocenteEscuelaUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="docenteEscuelaUpdHlp">DocenteEscuelaUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">DocenteEscuelaUpdHlp que tiene los datos anteriores</param>
      public void Update(IDataContext dctx, DocenteEscuela  docenteEscuela, DocenteEscuela previous){
         DocenteEscuelaUpdHlp da = new DocenteEscuelaUpdHlp();
         da.Action(dctx,  docenteEscuela, previous);
      }
      /// <summary>
      /// Elimina un registro de DocenteEscuelaDelHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="docenteEscuelaDelHlp">DocenteEscuelaDelHlp que desea eliminar</param>
      public void Delete(IDataContext dctx, DocenteEscuela  docenteEscuela){
         DocenteEscuelaDelHlp da = new DocenteEscuelaDelHlp();
         da.Action(dctx,  docenteEscuela);
      }
      /// <summary>
      /// Crea un objeto de DocenteEscuela a partir de los datos del último DataRow del DataSet.
      /// </summary>
      /// <param name="ds">El DataSet que contiene la información de DocenteEscuela</param>
      /// <returns>Un objeto de DocenteEscuela creado a partir de los datos</returns>
      public DocenteEscuela LastDataRowToDocenteEscuela(DataSet ds) {
         if (!ds.Tables.Contains("DocenteEscuela"))
            throw new Exception("LastDataRowToDocenteEscuela: DataSet no tiene la tabla DocenteEscuela");
         int index = ds.Tables["DocenteEscuela"].Rows.Count;
         if (index < 1)
            throw new Exception("LastDataRowToDocenteEscuela: El DataSet no tiene filas");
         return this.DataRowToDocenteEscuela(ds.Tables["DocenteEscuela"].Rows[index - 1]);
      }
      /// <summary>
      /// Crea un objeto de DocenteEscuela a partir de los datos de un DataRow.
      /// </summary>
      /// <param name="row">El DataRow que contiene la información de DocenteEscuela</param>
      /// <returns>Un objeto de DocenteEscuela creado a partir de los datos</returns>
      public DocenteEscuela DataRowToDocenteEscuela(DataRow row){
         DocenteEscuela docenteEscuela = new DocenteEscuela();
         if (row.IsNull("DocenteEscuelaID"))
            docenteEscuela.DocenteEscuelaID = null;
         else
            docenteEscuela.DocenteEscuelaID = (long)Convert.ChangeType(row["DocenteEscuelaID"], typeof(long));
         if (row.IsNull("DocenteID"))
             docenteEscuela.DocenteID = null;
         else
             docenteEscuela.DocenteID = ( int )Convert.ChangeType( row[ "DocenteID" ] , typeof( int ) );
         if (row.IsNull("EscuelaID"))
             docenteEscuela.EscuelaID = null;
         else
             docenteEscuela.EscuelaID = ( int )Convert.ChangeType( row[ "EscuelaID" ] , typeof( int ) );
         if (row.IsNull("Estatus"))
            docenteEscuela.Estatus = null;
         else
            docenteEscuela.Estatus = (bool)Convert.ChangeType(row["Estatus"], typeof(bool));
         return docenteEscuela;
      }
   } 
}

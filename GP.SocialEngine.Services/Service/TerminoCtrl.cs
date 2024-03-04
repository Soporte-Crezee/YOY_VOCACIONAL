using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;
using GP.SocialEngine.DAO;

namespace GP.SocialEngine.Service { 
   /// <summary>
   /// Controlador del objeto Termino
   /// </summary>
   public class TerminoCtrl { 
      /// <summary>
      /// Consulta registros de TerminoRetHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="terminoRetHlp">TerminoRetHlp que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de TerminoRetHlp generada por la consulta</returns>
      public DataSet Retrieve(IDataContext dctx, Termino termino){
         TerminoRetHlp da = new TerminoRetHlp();
         DataSet ds = da.Action(dctx, termino);
         return ds;
      }
      /// <summary>
      /// Crea un registro de TerminoInsHlp en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="terminoInsHlp">TerminoInsHlp que desea crear</param>
      public void Insert(IDataContext dctx, Termino  termino){
         TerminoInsHlp da = new TerminoInsHlp();
         da.Action(dctx,  termino);
      }
      /// <summary>
      /// Actualiza de manera optimista un registro de TerminoUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="terminoUpdHlp">TerminoUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">TerminoUpdHlp que tiene los datos anteriores</param>
      public void Update(IDataContext dctx, Termino  termino, Termino previous){
         TerminoUpdHlp da = new TerminoUpdHlp();
         da.Action(dctx,  termino, previous);
      }
      /// <summary>
      /// Crea un objeto de Termino a partir de los datos del último DataRow del DataSet.
      /// </summary>
      /// <param name="ds">El DataSet que contiene la información de Termino</param>
      /// <returns>Un objeto de Termino creado a partir de los datos</returns>
      public Termino LastDataRowToTermino(DataSet ds) {
         if (!ds.Tables.Contains("Termino"))
            throw new Exception("LastDataRowToTermino: DataSet no tiene la tabla Termino");
         int index = ds.Tables["Termino"].Rows.Count;
         if (index < 1)
            throw new Exception("LastDataRowToTermino: El DataSet no tiene filas");
         return this.DataRowToTermino(ds.Tables["Termino"].Rows[index - 1]);
      }
      /// <summary>
      /// Crea un objeto de Termino a partir de los datos de un DataRow.
      /// </summary>
      /// <param name="row">El DataRow que contiene la información de Termino</param>
      /// <returns>Un objeto de Termino creado a partir de los datos</returns>
      public Termino DataRowToTermino(DataRow row){
         Termino termino = new Termino();
         if (row.IsNull("TerminoID"))
            termino.TerminoID = null;
         else
            termino.TerminoID = (int)Convert.ChangeType(row["TerminoID"], typeof(int));
         if (row.IsNull("Cuerpo"))
            termino.Cuerpo = null;
         else
            termino.Cuerpo = (string)Convert.ChangeType(row["Cuerpo"], typeof(string));
         if (row.IsNull("FechaCreacion"))
            termino.FechaCreacion = null;
         else
            termino.FechaCreacion = (DateTime)Convert.ChangeType(row["FechaCreacion"], typeof(DateTime));
         if (row.IsNull("Estatus"))
            termino.Estatus = null;
         else
            termino.Estatus = (bool)Convert.ChangeType(row["Estatus"], typeof(bool));
         return termino;
      }
   } 
}

using System;
using System.Collections.Generic;
using System.Data;
using Framework.Base.DataAccess;
using POV.Reactivos.BO;
using POV.Reactivos.DAO;

namespace POV.Reactivos.Service { 
   /// <summary>
   /// Controlador del objeto EvaluacionRango
   /// </summary>
   public class EvaluacionRangoCtrl { 
      /// <summary>
      /// Consulta registros de EvaluacionRangoRetHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="evaluacionRangoRetHlp">EvaluacionRangoRetHlp que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de EvaluacionRangoRetHlp generada por la consulta</returns>
      public DataSet Retrieve(IDataContext dctx, EvaluacionRango evaluacionRango, Reactivo reactivo){
         EvaluacionRangoRetHlp da = new EvaluacionRangoRetHlp();
         DataSet ds = da.Action(dctx, evaluacionRango, reactivo);
         return ds;
      }

      public void Insert(IDataContext dctx, EvaluacionRango evaluacionRango, Reactivo reactivo)
      {
          EvaluacionRangoInsHlp da = new EvaluacionRangoInsHlp();
          da.Action(dctx, evaluacionRango, reactivo);
      }

      public void Update(IDataContext dctx, EvaluacionRango evaluacionRango, EvaluacionRango anterior)
      {
          EvaluacionRangoUpdHlp da = new EvaluacionRangoUpdHlp();
          da.Action(dctx,evaluacionRango, anterior);
      }
      /// <summary>
      /// Crea un objeto de EvaluacionRango a partir de los datos del último DataRow del DataSet.
      /// </summary>
      /// <param name="ds">El DataSet que contiene la información de EvaluacionRango</param>
      /// <returns>Un objeto de EvaluacionRango creado a partir de los datos</returns>
      public EvaluacionRango LastDataRowToEvaluacionRango(DataSet ds) {
         if (!ds.Tables.Contains("EvaluacionRango"))
            throw new Exception("LastDataRowToEvaluacionRango: DataSet no tiene la tabla EvaluacionRango");
         int index = ds.Tables["EvaluacionRango"].Rows.Count;
         if (index < 1)
            throw new Exception("LastDataRowToEvaluacionRango: El DataSet no tiene filas");
         return this.DataRowToEvaluacionRango(ds.Tables["EvaluacionRango"].Rows[index - 1]);
      }
      /// <summary>
      /// Crea un objeto de EvaluacionRango a partir de los datos de un DataRow.
      /// </summary>
      /// <param name="row">El DataRow que contiene la información de EvaluacionRango</param>
      /// <returns>Un objeto de EvaluacionRango creado a partir de los datos</returns>
      public EvaluacionRango DataRowToEvaluacionRango(DataRow row){
         
         EvaluacionRango evaluacionRango = new EvaluacionRango();
         if (row.IsNull("EvaluacionRangoID"))
            evaluacionRango.EvaluacionID = null;
         else
            evaluacionRango.EvaluacionID = (Guid)Convert.ChangeType(row["EvaluacionRangoID"], typeof(Guid));
         if (row.IsNull("Inicio"))
            evaluacionRango.Inicio = null;
         else
            evaluacionRango.Inicio = (short)Convert.ChangeType(row["Inicio"], typeof(short));
         if (row.IsNull("Fin"))
            evaluacionRango.Fin = null;
         else
            evaluacionRango.Fin = (short)Convert.ChangeType(row["Fin"], typeof(short));
         if (row.IsNull("PorcentajeCalificacion"))
            evaluacionRango.PorcentajeCalificacion = null;
         else
            evaluacionRango.PorcentajeCalificacion = (decimal)Convert.ChangeType(row["PorcentajeCalificacion"], typeof(decimal));

         return evaluacionRango;
      }
   } 
}

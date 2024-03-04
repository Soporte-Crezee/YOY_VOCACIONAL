using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Expediente.BO;
using POV.Modelo.BO;
using POV.Expediente.DAO;

namespace POV.Expediente.Service { 
   /// <summary>
   /// Controlador abstracto del objeto generico AResultadoClasificador
   /// </summary>
   public abstract class AResultadoClasificadorCtrl { 
      /// <summary>
      /// Consulta registros de AResultadoClasificadorRetHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="aResultadoClasificadorRetHlp">AResultadoClasificadorRetHlp que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de AResultadoClasificadorRetHlp generada por la consulta</returns>
       protected DataSet RetrieveAResultadoClasificador(IDataContext dctx, AResultadoClasificador resultadoClasificador)
       {
         ResultadoClasificadorRetHlp da = new ResultadoClasificadorRetHlp();
         DataSet ds = da.Action(dctx, resultadoClasificador);
         return ds;
      }
      /// <summary>
      /// Crea un registro de AResultadoClasificadorInsHlp en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="aResultadoClasificadorInsHlp">AResultadoClasificadorInsHlp que desea crear</param>
      protected void InsertAResultadoClasificador(IDataContext dctx, AResultadoClasificador resultadoClasificador)
      {
         ResultadoClasificadorInsHlp da = new ResultadoClasificadorInsHlp();
         da.Action(dctx,  resultadoClasificador);
      }

      /// <summary>
      /// Consulta un registro completo de ResultadoClasificador
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="resultadoClasificador">AResultadoClasificador que provee el criterio de selección para realizar la consulta</param>
      /// <returns>AResultadoClasificador concreto</returns>
      public abstract AResultadoClasificador RetrieveComplete(IDataContext dctx, AResultadoClasificador resultadoClasificador);
      /// <summary>
      /// Inserta un registro completo de resultado clasificador
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="resultadoClasificador">AResultadoClasificador concreto</param>
      public abstract void InsertComplete(IDataContext dctx, AResultadoClasificador resultadoClasificador);
      public abstract void UpdateComplete(IDataContext dctx, AResultadoClasificador resultadoClasificador, AResultadoClasificador previous);
      public abstract void DeleteComplete(IDataContext dctx, AResultadoClasificador resultadoClasificador);

      public abstract AResultadoClasificador LastDataRowToResultadoClasificador(DataSet ds);
      public abstract AResultadoClasificador DataRowToResultadoClasificador(DataRow dr);

   } 
}

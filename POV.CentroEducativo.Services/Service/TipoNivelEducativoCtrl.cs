using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.DAO;

namespace POV.CentroEducativo.Service { 
   /// <summary>
   /// Controlador del objeto TipoNivelEducativo
   /// </summary>
   public class TipoNivelEducativoCtrl { 
      /// <summary>
      /// Consulta registros de TipoNivelEducativoRetHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="tipoNivelEducativoRetHlp">TipoNivelEducativoRetHlp que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de TipoNivelEducativoRetHlp generada por la consulta</returns>
      public DataSet Retrieve(IDataContext dctx, TipoNivelEducativo tipoNivelEducativo){
         TipoNivelEducativoRetHlp da = new TipoNivelEducativoRetHlp();
         DataSet ds = da.Action(dctx, tipoNivelEducativo);
         return ds;
      }
      /// <summary>
      /// Crea un registro de TipoNivelEducativoInsHlp en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="tipoNivelEducativoInsHlp">TipoNivelEducativoInsHlp que desea crear</param>
      public void Insert(IDataContext dctx, TipoNivelEducativo tipoNivelEducativo){
         TipoNivelEducativoInsHlp da = new TipoNivelEducativoInsHlp();
         da.Action(dctx, tipoNivelEducativo);
      }
      /// <summary>
      /// Crea un objeto de TipoNivelEducativo a partir de los datos del último DataRow del DataSet.
      /// </summary>
      /// <param name="ds">El DataSet que contiene la información de TipoNivelEducativo</param>
      /// <returns>Un objeto de TipoNivelEducativo creado a partir de los datos</returns>
      public TipoNivelEducativo LastDataRowToTipoNivelEducativo(DataSet ds) {
         if (!ds.Tables.Contains("TipoNivelEducativo"))
            throw new Exception("LastDataRowToTipoNivelEducativo: DataSet no tiene la tabla TipoNivelEducativo");
         int index = ds.Tables["TipoNivelEducativo"].Rows.Count;
         if (index < 1)
            throw new Exception("LastDataRowToTipoNivelEducativo: El DataSet no tiene filas");
         return this.DataRowToTipoNivelEducativo(ds.Tables["TipoNivelEducativo"].Rows[index - 1]);
      }
      /// <summary>
      /// Crea un objeto de TipoNivelEducativo a partir de los datos de un DataRow.
      /// </summary>
      /// <param name="row">El DataRow que contiene la información de TipoNivelEducativo</param>
      /// <returns>Un objeto de TipoNivelEducativo creado a partir de los datos</returns>
      public TipoNivelEducativo DataRowToTipoNivelEducativo(DataRow row){
         TipoNivelEducativo tipoNivelEducativo = new TipoNivelEducativo();
         if (row.IsNull("TipoNivelEducativoID"))
            tipoNivelEducativo.TipoNivelEducativoID = null;
         else
            tipoNivelEducativo.TipoNivelEducativoID = (int)Convert.ChangeType(row["TipoNivelEducativoID"], typeof(int));
         if (row.IsNull("Clave"))
            tipoNivelEducativo.Clave = null;
         else
            tipoNivelEducativo.Clave = (string)Convert.ChangeType(row["Clave"], typeof(string));
         if (row.IsNull("Nombre"))
            tipoNivelEducativo.Nombre = null;
         else
            tipoNivelEducativo.Nombre = (string)Convert.ChangeType(row["Nombre"], typeof(string));
         return tipoNivelEducativo;
      }
   } 
}

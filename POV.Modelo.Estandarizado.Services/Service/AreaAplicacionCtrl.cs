using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Modelo.Estandarizado.BO;
using POV.Modelo.Estandarizado.DAO;

namespace POV.Modelo.Estandarizado.Service
{ 
   /// <summary>
   /// Controlador del objeto AreaAplicacion
   /// </summary>
   public class AreaAplicacionCtrl { 
      /// <summary>
      /// Consulta registros de AreaAplicacionRetHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="areaAplicacionRetHlp">AreaAplicacionRetHlp que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de AreaAplicacionRetHlp generada por la consulta</returns>
      public DataSet Retrieve(IDataContext dctx, AreaAplicacion areaAplicacion){
         AreaAplicacionRetHlp da = new AreaAplicacionRetHlp();
         DataSet ds = da.Action(dctx, areaAplicacion);
         return ds;
      }
      /// <summary>
      /// Crea un objeto de AreaAplicacion a partir de los datos del último DataRow del DataSet.
      /// </summary>
      /// <param name="ds">El DataSet que contiene la información de AreaAplicacion</param>
      /// <returns>Un objeto de AreaAplicacion creado a partir de los datos</returns>
      public AreaAplicacion LastDataRowToAreaAplicacion(DataSet ds) {
         if (!ds.Tables.Contains("AreaAplicacion"))
            throw new Exception("LastDataRowToAreaAplicacion: DataSet no tiene la tabla AreaAplicacion");
         int index = ds.Tables["AreaAplicacion"].Rows.Count;
         if (index < 1)
            throw new Exception("LastDataRowToAreaAplicacion: El DataSet no tiene filas");
         return this.DataRowToAreaAplicacion(ds.Tables["AreaAplicacion"].Rows[index - 1]);
      }
      /// <summary>
      /// Crea un objeto de AreaAplicacion a partir de los datos de un DataRow.
      /// </summary>
      /// <param name="row">El DataRow que contiene la información de AreaAplicacion</param>
      /// <returns>Un objeto de AreaAplicacion creado a partir de los datos</returns>
      public AreaAplicacion DataRowToAreaAplicacion(DataRow row){
         AreaAplicacion areaAplicacion = new AreaAplicacion();
         if (row.IsNull("AreaAplicacionID"))
            areaAplicacion.AreaAplicacionID = null;
         else
            areaAplicacion.AreaAplicacionID = (int)Convert.ChangeType(row["AreaAplicacionID"], typeof(int));
         if (row.IsNull("Descripcion"))
            areaAplicacion.Descripcion = null;
         else
            areaAplicacion.Descripcion = (string)Convert.ChangeType(row["Descripcion"], typeof(string));
         return areaAplicacion;
      }
   } 
}

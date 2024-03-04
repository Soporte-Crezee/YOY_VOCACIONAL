using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.DAO;
using POV.Localizacion.BO;

namespace POV.CentroEducativo.Service { 
   /// <summary>
   /// Controlador del objeto Zona
   /// </summary>
   public class ZonaCtrl { 
      /// <summary>
      /// Consulta registros de ZonaRetHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="zonaRetHlp">ZonaRetHlp que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de ZonaRetHlp generada por la consulta</returns>
      public DataSet Retrieve(IDataContext dctx, Zona zona){
         ZonaRetHlp da = new ZonaRetHlp();
         DataSet ds = da.Action(dctx, zona);
         return ds;
      }
      /// <summary>
      /// Crea un registro de ZonaInsHlp en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="zonaInsHlp">ZonaInsHlp que desea crear</param>
      public void Insert(IDataContext dctx, Zona  zona){
         ZonaInsHlp da = new ZonaInsHlp();
         da.Action(dctx,  zona);
      }
      /// <summary>
      /// Actualiza de manera optimista un registro de ZonaUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="zonaUpdHlp">ZonaUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">ZonaUpdHlp que tiene los datos anteriores</param>
      public void Update(IDataContext dctx, Zona  zona, Zona previous){
         ZonaUpdHlp da = new ZonaUpdHlp();
         da.Action(dctx,  zona, previous);
      }
      /// <summary>
      /// Crea un objeto de Zona a partir de los datos del último DataRow del DataSet.
      /// </summary>
      /// <param name="ds">El DataSet que contiene la información de Zona</param>
      /// <returns>Un objeto de Zona creado a partir de los datos</returns>
      public Zona LastDataRowToZona(DataSet ds) {
         if (!ds.Tables.Contains("Zona"))
            throw new Exception("LastDataRowToZona: DataSet no tiene la tabla Zona");
         int index = ds.Tables["Zona"].Rows.Count;
         if (index < 1)
            throw new Exception("LastDataRowToZona: El DataSet no tiene filas");
         return this.DataRowToZona(ds.Tables["Zona"].Rows[index - 1]);
      }
      /// <summary>
      /// Crea un objeto de Zona a partir de los datos de un DataRow.
      /// </summary>
      /// <param name="row">El DataRow que contiene la información de Zona</param>
      /// <returns>Un objeto de Zona creado a partir de los datos</returns>
      public Zona DataRowToZona(DataRow row){
         Zona zona = new Zona();
         if (zona.UbicacionID == null)
             zona.UbicacionID = new Ubicacion();
         if (row.IsNull("ZonaID"))
            zona.ZonaID = null;
         else
            zona.ZonaID = (int)Convert.ChangeType(row["ZonaID"], typeof(int));
         if (row.IsNull("Clave"))
            zona.Clave = null;
         else
            zona.Clave = (string)Convert.ChangeType(row["Clave"], typeof(string));
         if (row.IsNull("Nombre"))
            zona.Nombre = null;
         else
            zona.Nombre = (string)Convert.ChangeType(row["Nombre"], typeof(string));
         if (row.IsNull("UbicacionID"))
            zona.UbicacionID = null;
         else
            zona.UbicacionID.UbicacionID = (long)Convert.ChangeType(row["UbicacionID"], typeof(long));
         return zona;
      }
   } 
}

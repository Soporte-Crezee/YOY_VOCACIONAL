using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Seguridad.BO;
using POV.Seguridad.DAO;

namespace POV.Seguridad.Service
{ 
   /// <summary>
   /// Servicios para los PerfilPermiso del sistema
   /// </summary>
   public class PerfilPermisoCtrl { 
      /// <summary>
      /// Crea un registro de PerfilPermiso en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="perfilPermiso">PerfilPermiso que desea crear</param>
      public void Insert(IDataContext dctx, int? perfilID, int? permisoID){
         PerfilPermisoInsHlp da = new PerfilPermisoInsHlp();
         da.Action(dctx, perfilID, permisoID);
      }
      /// <summary>
      /// Consulta registros de PerfilPermiso en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="perfilPermiso">PerfilPermiso que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de PerfilPermiso generada por la consulta</returns>
      public DataSet Retrieve(IDataContext dctx, int? perfilPermisoID, int? perfilID, int? permisoID){
         PerfilPermisoRetHlp da = new PerfilPermisoRetHlp();
         DataSet ds = da.Action(dctx, perfilPermisoID, perfilID, permisoID);
         return ds;
      }
      /// <summary>
      /// Consulta registros de PerfilPermiso en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="perfilPermiso">PerfilPermiso que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de PerfilPermiso generada por la consulta</returns>
      public DataSet RetrieveComplete(IDataContext dctx, int perfilID){
         PerfilPermisoRetCompleteHlp da = new PerfilPermisoRetCompleteHlp();
         DataSet ds = da.Action(dctx, perfilID);
         return ds;
      }
      /// <summary>
      /// Elimina un registro de PerfilPermiso en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="perfilPermiso">PerfilPermiso que desea eliminar</param>
      public void DeleteByPerfil(IDataContext dctx, int perfilID){
         PerfilPermisoByPerfilDelHlp da = new PerfilPermisoByPerfilDelHlp();
         da.Action(dctx, perfilID);
      }
      /// <summary>
      /// Elimina un registro de PerfilPermiso en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="perfilPermiso">PerfilPermiso que desea eliminar</param>
      public void Delete(IDataContext dctx, int? perfilPermisoID){
         PerfilPermisoDelHlp da = new PerfilPermisoDelHlp();
         da.Action(dctx, perfilPermisoID);
      }
      /// <summary>
      /// Crea un objeto de Permiso a partir de los datos del último DataRow del DataSet.
      /// </summary>
      /// <param name="ds">El DataSet que contiene la información de Permiso</param>
      /// <returns>Un objeto de Permiso creado a partir de los datos</returns>
      public Permiso LastDataRowToPermiso(DataSet ds) {
         if (!ds.Tables.Contains("Permiso"))
             throw new Exception("Error CT2404.- LastDataRowToPermiso: DataSet no tiene la tabla Permiso");
         int index = ds.Tables["Permiso"].Rows.Count;
         if (index < 1)
             throw new Exception("Error CT2405.- LastDataRowToPermiso: El DataSet no tiene filas");
         return this.DataRowToPermiso(ds.Tables["Permiso"].Rows[index - 1]);
      }
      /// <summary>
      /// Crea un objeto de Permiso a partir de los datos de un DataRow.
      /// </summary>
      /// <param name="row">El DataRow que contiene la información de Permiso</param>
      /// <returns>Un objeto de Permiso creado a partir de los datos</returns>
      public Permiso DataRowToPermiso(DataRow row){
         Permiso permiso = new Permiso();
         permiso.Aplicacion = new Aplicacion();
         permiso.Accion = new Accion();
         if (row.IsNull("Permiso_PermisoID"))
            permiso.PermisoID = null;
         else
            permiso.PermisoID = (int)Convert.ChangeType(row["Permiso_PermisoID"], typeof(int));
         if (row.IsNull("Aplicacion_AplicacionID"))
            permiso.Aplicacion.AplicacionID = null;
         else
            permiso.Aplicacion.AplicacionID = (int)Convert.ChangeType(row["Aplicacion_AplicacionID"], typeof(int));
         if (row.IsNull("Aplicacion_Descripcion"))
             permiso.Aplicacion.Nombre = null;
         else
             permiso.Aplicacion.Nombre = (string)Convert.ChangeType(row["Aplicacion_Descripcion"], typeof(string));
         if (row.IsNull("Accion_AccionID"))
            permiso.Accion.AccionID = null;
         else
            permiso.Accion.AccionID = (int)Convert.ChangeType(row["Accion_AccionID"], typeof(int));
         if (row.IsNull("Accion_Descripcion"))
             permiso.Accion.Nombre = null;
         else
            permiso.Accion.Nombre = (string)Convert.ChangeType(row["Accion_Descripcion"], typeof(string));
         return permiso;
      }
   } 
}

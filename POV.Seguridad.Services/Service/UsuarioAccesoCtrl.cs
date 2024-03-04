using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Seguridad.BO;
using POV.Seguridad.DAO;

namespace POV.Seguridad.Service
{ 
   /// <summary>
   /// Servicios para los Usuarios del sistema
   /// </summary>
   public class UsuarioAccesoCtrl { 
      /// <summary>
      /// Crea un registro de UsuarioAcceso en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="usuarioAcceso">UsuarioAcceso que desea crear</param>
      public void Insert(IDataContext dctx, UsuarioPrivilegios usuarioPrivilegios, UsuarioAcceso usuarioAcceso){
         UsuarioAccesoInsHlp da = new UsuarioAccesoInsHlp();
         da.Action(dctx, usuarioPrivilegios, usuarioAcceso);
      }
      /// <summary>
      /// Consulta registros de UsuarioAcceso en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="usuarioAcceso">UsuarioAcceso que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de UsuarioAcceso generada por la consulta</returns>
      public DataSet Retrieve(IDataContext dctx, UsuarioPrivilegios usuarioPrivilegios, UsuarioAcceso usuarioAcceso){
         UsuarioAccesoRetHlp da = new UsuarioAccesoRetHlp();
         DataSet ds = da.Action(dctx, usuarioPrivilegios, usuarioAcceso);
         return ds;
      }

      public DataSet RetrieveWithPrivilegio(IDataContext dctx, UsuarioPrivilegios usuarioPrivilegios, UsuarioAcceso usuarioAcceso)
      {
          UsuarioAccesoRetHlp da = new UsuarioAccesoRetHlp();
          DataSet ds = da.ActionPrivilegio(dctx, usuarioPrivilegios, usuarioAcceso);
          return ds;
      }

      public UsuarioAcceso RetrieveComplete(IDataContext dctx, UsuarioPrivilegios usuarioPrivilegios, UsuarioAcceso usuarioAcceso)
      {
          UsuarioAcceso usuarioAccesoAux = new UsuarioAcceso();

          DataSet ds = RetrieveWithPrivilegio(dctx, usuarioPrivilegios, usuarioAcceso);

          if (ds.Tables[0].Rows.Count < 1)
              return usuarioAccesoAux;
          int index = ds.Tables["UsuarioAcceso"].Rows.Count;
          DataRow dr = ds.Tables["UsuarioAcceso"].Rows[index - 1];

          PerfilCtrl perfilServ = new PerfilCtrl();
          PermisoCtrl permisoServ = new PermisoCtrl();
          Perfil perfil;
          Permiso permiso;
          IPrivilegio privilegio = null;

          usuarioAccesoAux = DataRowToUsuarioAcceso(dr);


          if (!dr.IsNull("Privilegio_PerfilID"))
          {
              perfil = perfilServ.RetrieveComplete(dctx, (int)dr["Privilegio_PerfilID"]);
              privilegio = perfil;
          }
          else if (!dr.IsNull("Privilegio_PermisoID"))
          {
              permiso = new Permiso();
              permiso.PermisoID = (int?)dr["Privilegio_PermisoID"];
              permiso.Accion = new Accion();
              permiso.Aplicacion = new Aplicacion();
              permiso = permisoServ.LastDataRowToPermiso(permisoServ.Retrieve(dctx, permiso));
              privilegio = permiso;
          }
          usuarioAccesoAux.Privilegio = privilegio;


          return usuarioAccesoAux;
      }
      /// <summary>
      /// Consulta registros de UsuarioAcceso en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="usuarioAcceso">UsuarioAcceso que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de UsuarioAcceso generada por la consulta</returns>
      public DataSet RetrieveByUsuarioPrivilegiosID(IDataContext dctx, UsuarioPrivilegios usuarioPrivilegios){
         UsuarioAccesoPrivilegiosRetHlp da = new UsuarioAccesoPrivilegiosRetHlp();
         DataSet ds = da.Action(dctx, usuarioPrivilegios);
         return ds;
      }
      /// <summary>
      /// Elimina un registro de UsuarioAcceso en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="usuarioAcceso">UsuarioAcceso que desea eliminar</param>
      public void Delete(IDataContext dctx, UsuarioAcceso usuarioAcceso){
         UsuarioAccesoDelHlp da = new UsuarioAccesoDelHlp();
         da.Action(dctx, usuarioAcceso);
      }
      /// <summary>
      /// Elimina un registro de UsuarioAcceso en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="usuarioAcceso">UsuarioAcceso que desea eliminar</param>
      public void DeleteByUsuarioPrivilegios(IDataContext dctx, UsuarioPrivilegios usuarioPrivilegios){
         UsuarioAccesoUsuarioPrivilegiosDelHlp da = new UsuarioAccesoUsuarioPrivilegiosDelHlp();
         da.Action(dctx, usuarioPrivilegios);
      }
      /// <summary>
      /// Crea un objeto de UsuarioAcceso a partir de los datos del último DataRow del DataSet.
      /// </summary>
      /// <param name="ds">El DataSet que contiene la información de UsuarioAcceso</param>
      /// <returns>Un objeto de UsuarioAcceso creado a partir de los datos</returns>
      public UsuarioAcceso LastDataRowToUsuarioAcceso(DataSet ds) {
         if (!ds.Tables.Contains("UsuarioAcceso"))
             throw new Exception("Error CT2410.- LastDataRowToUsuarioAcceso: DataSet no tiene la tabla UsuarioAcceso");
         int index = ds.Tables["UsuarioAcceso"].Rows.Count;
         if (index < 1)
             throw new Exception("Error CT2411.- LastDataRowToUsuarioAcceso: El DataSet no tiene filas");
         return this.DataRowToUsuarioAcceso(ds.Tables["UsuarioAcceso"].Rows[index - 1]);
      }
      /// <summary>
      /// Crea un objeto de UsuarioAcceso a partir de los datos de un DataRow.
      /// </summary>
      /// <param name="row">El DataRow que contiene la información de UsuarioAcceso</param>
      /// <returns>Un objeto de UsuarioAcceso creado a partir de los datos</returns>
      public UsuarioAcceso DataRowToUsuarioAcceso(DataRow row){
         UsuarioAcceso usuarioAcceso = new UsuarioAcceso();
         usuarioAcceso.UsuarioAsigno = new Usuario();
         if (row.IsNull("UsuarioAccesoID"))
            usuarioAcceso.UsuarioAccesoID = null;
         else
            usuarioAcceso.UsuarioAccesoID = (int)Convert.ChangeType(row["UsuarioAccesoID"], typeof(int));
         if (row.IsNull("FechaAsignacion"))
            usuarioAcceso.FechaAsignacion = null;
         else
            usuarioAcceso.FechaAsignacion = (DateTime)Convert.ChangeType(row["FechaAsignacion"], typeof(DateTime));
         if (row.IsNull("UsuarioAsignoID"))
            usuarioAcceso.UsuarioAsigno.UsuarioID = null;
         else
            usuarioAcceso.UsuarioAsigno.UsuarioID = (int)Convert.ChangeType(row["UsuarioAsignoID"], typeof(int));
         return usuarioAcceso;
      }
   } 
}

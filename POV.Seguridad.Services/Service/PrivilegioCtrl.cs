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
   /// Servicios para los Privilegios del sistema
   /// </summary>
   public class PrivilegioCtrl { 
      /// <summary>
      /// Crea un registro de Privilegio en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="privilegio">Privilegio que desea crear</param>
      public void Insert(IDataContext dctx, UsuarioAcceso usuarioAcceso, int? perfilID, int? permisoID){
         PrivilegioInsHlp da = new PrivilegioInsHlp();
         da.Action(dctx, usuarioAcceso, perfilID, permisoID);
      }
      /// <summary>
      /// Consulta registros de Privilegio en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="privilegio">Privilegio que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de Privilegio generada por la consulta</returns>
      public DataSet Retrieve(IDataContext dctx, UsuarioAcceso usuarioAcceso, int? privilegioID, int? perfilID, int? permisoID){
         PrivilegioRetHlp da = new PrivilegioRetHlp();
         DataSet ds = da.Action(dctx, usuarioAcceso, privilegioID, perfilID, permisoID);
         return ds;
      }
      /// <summary>
      /// Elimina un registro de Privilegio en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="privilegio">Privilegio que desea eliminar</param>
      public void Delete(IDataContext dctx, UsuarioAcceso usuarioAcceso){
         PrivilegioDelHlp da = new PrivilegioDelHlp();
         da.Action(dctx, usuarioAcceso);
      }
   } 
}

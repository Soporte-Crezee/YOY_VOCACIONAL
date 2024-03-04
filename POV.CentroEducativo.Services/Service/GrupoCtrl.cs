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
   /// Controlador del objeto Grupo
   /// </summary>
   public class GrupoCtrl { 
      /// <summary>
      /// Consulta registros de GrupoRetHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="grupoRetHlp">GrupoRetHlp que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de GrupoRetHlp generada por la consulta</returns>
      public DataSet Retrieve(IDataContext dctx, Grupo grupo, Escuela escuela){
         GrupoRetHlp da = new GrupoRetHlp();
         DataSet ds = da.Action(dctx, grupo, escuela);
         return ds;
      }
      /// <summary>
      /// Crea un registro de GrupoInsHlp en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="grupoInsHlp">GrupoInsHlp que desea crear</param>
      public void Insert(IDataContext dctx, Grupo  grupo, Escuela escuela){
         GrupoInsHlp da = new GrupoInsHlp();
         da.Action(dctx,  grupo, escuela);
      }
      /// <summary>
      /// Actualiza de manera optimista un registro de GrupoUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="grupoUpdHlp">GrupoUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">GrupoUpdHlp que tiene los datos anteriores</param>
      public void Update(IDataContext dctx, Grupo  grupo, Escuela escuela, Grupo previous){
         GrupoUpdHlp da = new GrupoUpdHlp();
         da.Action(dctx,  grupo,previous, escuela);
      }
      /// <summary>
      /// Elimina un registro de GrupoDelHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="grupoDelHlp">GrupoDelHlp que desea eliminar</param>
      public void Delete(IDataContext dctx, Grupo  grupo, Escuela escuela){
         GrupoDelHlp da = new GrupoDelHlp();
         da.Action(dctx,  grupo, escuela);
      }
      /// <summary>
      /// Crea un objeto de Grupo a partir de los datos del último DataRow del DataSet.
      /// </summary>
      /// <param name="ds">El DataSet que contiene la información de Grupo</param>
      /// <returns>Un objeto de Grupo creado a partir de los datos</returns>
      public Grupo LastDataRowToGrupo(DataSet ds) {
         if (!ds.Tables.Contains("Grupo"))
            throw new Exception("LastDataRowToGrupo: DataSet no tiene la tabla Grupo");
         int index = ds.Tables["Grupo"].Rows.Count;
         if (index < 1)
            throw new Exception("LastDataRowToGrupo: El DataSet no tiene filas");
         return this.DataRowToGrupo(ds.Tables["Grupo"].Rows[index - 1]);
      }
      /// <summary>
      /// Crea un objeto de Grupo a partir de los datos de un DataRow.
      /// </summary>
      /// <param name="row">El DataRow que contiene la información de Grupo</param>
      /// <returns>Un objeto de Grupo creado a partir de los datos</returns>
      public Grupo DataRowToGrupo(DataRow row){
         Grupo grupo = new Grupo();
         if (row.IsNull("GrupoID"))
            grupo.GrupoID = null;
         else
            grupo.GrupoID = (Guid)Convert.ChangeType(row["GrupoID"], typeof(Guid));
         if (row.IsNull("Nombre"))
            grupo.Nombre = null;
         else
            grupo.Nombre = (String)Convert.ChangeType(row["Nombre"], typeof(String));
         if (row.IsNull("Grado"))
            grupo.Grado = null;
         else
            grupo.Grado = (byte)Convert.ChangeType(row["Grado"], typeof(byte));
         return grupo;
      }
   } 
}

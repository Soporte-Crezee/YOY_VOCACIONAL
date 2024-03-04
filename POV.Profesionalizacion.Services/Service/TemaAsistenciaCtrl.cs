using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Profesionalizacion.BO;
using POV.Profesionalizacion.DAO;

namespace POV.Profesionalizacion.Service { 
   /// <summary>
   /// Controlador del objeto TemaAsistencia
   /// </summary>
   public class TemaAsistenciaCtrl { 
      /// <summary>
      /// Consulta registros de TemaAsistenciaRetHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="temaAsistenciaRetHlp">TemaAsistenciaRetHlp que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de TemaAsistenciaRetHlp generada por la consulta</returns>
      public DataSet Retrieve(IDataContext dctx, TemaAsistencia temaAsistencia){
         TemaAsistenciaRetHlp da = new TemaAsistenciaRetHlp();
         DataSet ds = da.Action(dctx, temaAsistencia);
         return ds;
      }
      public TemaAsistencia RetrieveSimple(IDataContext dctx, TemaAsistencia temaAsistencia)
      {
          TemaAsistencia temaAsitenciaContenido = null;
           if(temaAsistencia==null) throw new ArgumentNullException("temaAsistencia","TemaAsistenciaCtrl:TemaAsistencia no puede ser nulo");
          DataSet ds = Retrieve(dctx,temaAsistencia);
          if(ds.Tables[0].Rows.Count>1)
              throw new Exception("TemaAsistenciaCtrl:La consulta devolvió más de un registro para las condiciones proporcionadas");
           if (ds.Tables[0].Rows.Count > 0)
           {
               temaAsitenciaContenido = this.LastDataRowToTemaAsistencia(ds);
           }

           return temaAsitenciaContenido;
      }
      /// <summary>
      /// Crea un registro de TemaAsistenciaRetHlp en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="temaAsistenciaRetHlp">TemaAsistenciaRetHlp que desea crear</param>
      public void Insert(IDataContext dctx,TemaAsistencia temaAsistencia)
      {
          if (ValidateDataForInsertOrUpdate(dctx, temaAsistencia))
          {
              TemaAsistenciaInsHlp da = new TemaAsistenciaInsHlp();
              da.Action(dctx, temaAsistencia);
          }
          else
          {
              throw new DuplicateNameException("El nombre del tema ya está registrado en el sistema, por favor verifique");

          }
      }
      /// <summary>
      /// Actualiza de manera optimista un registro de TemaAsistenciaRetHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="temaAsistenciaRetHlp">TemaAsistenciaRetHlp que tiene los datos nuevos</param>
      /// <param name="previous">TemaAsistenciaRetHlp que tiene los datos anteriores</param>
      public void Update(IDataContext dctx, TemaAsistencia temaAsistencia, TemaAsistencia previous)
      {
          TemaAsistenciaUpdHlp da = new TemaAsistenciaUpdHlp();
          if (temaAsistencia.Nombre != previous.Nombre)
          {
              if (ValidateDataForInsertOrUpdate(dctx, temaAsistencia))
              {
                 
                  da.Action(dctx, temaAsistencia, previous);
              }
              else
                  throw new DuplicateNameException("El nombre del tema ya está registrado en el sistema, por favor verifique");
          }
          else
          {
              da.Action(dctx, temaAsistencia, previous);
          }
      }
      /// <summary>
      /// Elimina un registro de TemaAsistenciaRetHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="temaAsistenciaRetHlp">TemaAsistenciaRetHlp que desea eliminar</param>
      public void Delete(IDataContext dctx, TemaAsistencia temaAsistencia)
      {
          TemaAsistenciaDelHlp da = new TemaAsistenciaDelHlp();
          da.Action(dctx,temaAsistencia);
      }
      /// <summary>
      /// Crea un objeto de TemaAsistencia a partir de los datos del último DataRow del DataSet.
      /// </summary>
      /// <param name="ds">El DataSet que contiene la información de TemaAsistencia</param>
      /// <returns>Un objeto de TemaAsistencia creado a partir de los datos</returns>
      public TemaAsistencia LastDataRowToTemaAsistencia(DataSet ds) {
         if (!ds.Tables.Contains("TemaAsistencia"))
            throw new Exception("LastDataRowToTemaAsistencia: DataSet no tiene la tabla TemaAsistencia");
         int index = ds.Tables["TemaAsistencia"].Rows.Count;
         if (index < 1)
            throw new Exception("LastDataRowToTemaAsistencia: El DataSet no tiene filas");
         return this.DataRowToTemaAsistencia(ds.Tables["TemaAsistencia"].Rows[index - 1]);
      }
      /// <summary>
      /// Crea un objeto de TemaAsistencia a partir de los datos de un DataRow.
      /// </summary>
      /// <param name="row">El DataRow que contiene la información de TemaAsistencia</param>
      /// <returns>Un objeto de TemaAsistencia creado a partir de los datos</returns>
      public TemaAsistencia DataRowToTemaAsistencia(DataRow row){
         TemaAsistencia temaAsistencia = new TemaAsistencia();
         if (row.IsNull("TemaAsistenciaID"))
            temaAsistencia.TemaAsistenciaID = null;
         else
            temaAsistencia.TemaAsistenciaID = (int)Convert.ChangeType(row["TemaAsistenciaID"], typeof(int));
         if (row.IsNull("Nombre"))
            temaAsistencia.Nombre = null;
         else
            temaAsistencia.Nombre = (string)Convert.ChangeType(row["Nombre"], typeof(string));
         if (row.IsNull("Descripcion"))
            temaAsistencia.Descripcion = null;
         else
            temaAsistencia.Descripcion = (string)Convert.ChangeType(row["Descripcion"], typeof(string));
         if (row.IsNull("Activo"))
            temaAsistencia.Activo = null;
         else
            temaAsistencia.Activo = (bool)Convert.ChangeType(row["Activo"], typeof(bool));
         if (row.IsNull("FechaRegistro"))
            temaAsistencia.FechaRegistro = null;
         else
            temaAsistencia.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
         return temaAsistencia;
      }
      /// <summary>
      /// Valida si existe un tema de asistencia con el mismo nombre en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="temaAsistencia">Tema de Asistencia</param>
      /// <returns></returns>
      private bool ValidateDataForInsertOrUpdate(IDataContext dctx, TemaAsistencia temaAsistencia)
      {
          DataSet ds = Retrieve(dctx, new TemaAsistencia { Nombre = temaAsistencia.Nombre });
          return ds.Tables["TemaAsistencia"].Rows.Count <= 0;
      }
   } 
}

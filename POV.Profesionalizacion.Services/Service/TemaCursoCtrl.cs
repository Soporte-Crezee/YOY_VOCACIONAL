using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Profesionalizacion.BO;
using POV.Profesionalizacion.DAO;

namespace POV.Profesionalizacion.Service { 
   /// <summary>
   /// Controlador del objeto TemaCurso
   /// </summary>
   public class TemaCursoCtrl { 
      /// <summary>
      /// Consulta registros de TemaCurso en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="temaCurso">TemaCurso que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de TemaCurso generada por la consulta</returns>
      public DataSet Retrieve(IDataContext dctx, TemaCurso temaCurso){
         TemaCursoRetHlp da = new TemaCursoRetHlp();
         DataSet ds = da.Action(dctx, temaCurso);
         return ds;
      }
      /// <summary>
      /// Crea un objeto de TemaCurso a partir de los datos del último DataRow del DataSet.
      /// </summary>
      /// <param name="ds">El DataSet que contiene la información de TemaCurso</param>
      /// <returns>Un objeto de TemaCurso creado a partir de los datos</returns>
      public TemaCurso LastDataRowToTemaCurso(DataSet ds) {
         if (!ds.Tables.Contains("TemaCurso"))
            throw new Exception("LastDataRowToTemaCurso: DataSet no tiene la tabla TemaCurso");
         int index = ds.Tables["TemaCurso"].Rows.Count;
         if (index < 1)
            throw new Exception("LastDataRowToTemaCurso: El DataSet no tiene filas");
         return this.DataRowToTemaCurso(ds.Tables["TemaCurso"].Rows[index - 1]);
      }
      /// <summary>
      /// Crea un objeto de TemaCurso a partir de los datos de un DataRow.
      /// </summary>
      /// <param name="row">El DataRow que contiene la información de TemaCurso</param>
      /// <returns>Un objeto de TemaCurso creado a partir de los datos</returns>
      public TemaCurso DataRowToTemaCurso(DataRow row){
         TemaCurso temaCurso = new TemaCurso();
         if (row.IsNull("TemaCursoID"))
            temaCurso.TemaCursoID = null;
         else
            temaCurso.TemaCursoID = (int)Convert.ChangeType(row["TemaCursoID"], typeof(int));
         if (row.IsNull("Nombre"))
            temaCurso.Nombre = null;
         else
            temaCurso.Nombre = (String)Convert.ChangeType(row["Nombre"], typeof(String));
         if (row.IsNull("Descripcion"))
            temaCurso.Descripcion = null;
         else
            temaCurso.Descripcion = (String)Convert.ChangeType(row["Descripcion"], typeof(String));
         if (row.IsNull("Activo"))
            temaCurso.Activo = null;
         else
            temaCurso.Activo = (bool)Convert.ChangeType(row["Activo"], typeof(bool));
         if (row.IsNull("FechaRegistro"))
            temaCurso.FechaRegistro = null;
         else
            temaCurso.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
         return temaCurso;
      }
      /// <summary>
      /// Crea un registro de TemaCursoInsHlp en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="temaCursoInsHlp">TemaCursoInsHlp que desea crear</param>
      public void Insert(IDataContext dctx, TemaCurso temacurso)
      {
          TemaCursoInsHlp da = new TemaCursoInsHlp();
          da.Action(dctx, temacurso);
      }
      /// <summary>
      /// Actualiza de manera optimista un registro de TemaCursoUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="temaCursoUpdHlp">TemaCursoUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">TemaCursoUpdHlp que tiene los datos anteriores</param>
      public void Update(IDataContext dctx, TemaCurso temacurso, TemaCurso previous)
      {
          TemaCursoUpdHlp da = new TemaCursoUpdHlp();
          da.Action(dctx, temacurso, previous);
      }
      /// <summary>
      /// Elimina un registro de TemaCursoUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
      /// <param name="TemaCursoDelHlp">TemaCursoDelHlp que desea eliminar</param>
      public void Delete(IDataContext dctx, TemaCurso temacurso)
      {
          if (temacurso == null) throw new Exception("TemaCursoCtrl: El parámetro 'temacurso' no puede ser vacio.");
          TemaCursoDelHlp da = new TemaCursoDelHlp();
          da.Action(dctx, temacurso);          
      }
   } 
}

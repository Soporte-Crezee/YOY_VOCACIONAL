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
   /// Controlador del objeto Director
   /// </summary>
   public class DirectorCtrl { 
      /// <summary>
      /// Consulta registros de DirectorRetHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="directorRetHlp">DirectorRetHlp que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de DirectorRetHlp generada por la consulta</returns>
      public DataSet Retrieve(IDataContext dctx, Director director){
         DirectorRetHlp da = new DirectorRetHlp();
         DataSet ds = da.Action(dctx, director);
         return ds;
      }
      /// <summary>
      /// Crea un registro de DirectorInsHlp en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="directorInsHlp">DirectorInsHlp que desea crear</param>
      public void Insert(IDataContext dctx, Director  director){
         DirectorInsHlp da = new DirectorInsHlp();
         da.Action(dctx,  director);
      }
      /// <summary>
      /// Actualiza un registro de Director en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="director">Director que tiene los datos nuevos</param>
      /// <param name="previous">Director que tiene los datos anteriores</param>
      public void Update(IDataContext dctx, Director director, Director previous)
      {
          DirectorUpdHlp da = new DirectorUpdHlp();
          if (director.Curp != previous.Curp)
          {
              if (NoRepetido(dctx, director))
                  da.Action(dctx, director, previous);
              else
                  throw new Exception("Update: El curp que intenta agregar ya esta asignado a otro Director.");
          }
          else
              da.Action(dctx,director,previous);
      }

      public void Delete(IDataContext dctx, Director director)
      {
          DirectorDelHlp da = new DirectorDelHlp();
          da.Action(dctx, director);
      }

      /// <summary>
      /// Crea un objeto de Director a partir de los datos del último DataRow del DataSet.
      /// </summary>
      /// <param name="ds">El DataSet que contiene la información de Director</param>
      /// <returns>Un objeto de Director creado a partir de los datos</returns>
      public Director LastDataRowToDirector(DataSet ds) {
         if (!ds.Tables.Contains("Director"))
            throw new Exception("LastDataRowToDirector: DataSet no tiene la tabla Director");
         int index = ds.Tables["Director"].Rows.Count;
         if (index < 1)
            throw new Exception("LastDataRowToDirector: El DataSet no tiene filas");
         return this.DataRowToDirector(ds.Tables["Director"].Rows[index - 1]);
      }
      /// <summary>
      /// Crea un objeto de Director a partir de los datos de un DataRow.
      /// </summary>
      /// <param name="row">El DataRow que contiene la información de Director</param>
      /// <returns>Un objeto de Director creado a partir de los datos</returns>
      public Director DataRowToDirector(DataRow row){
         Director director = new Director();

         if (row.IsNull("DirectorID"))
            director.DirectorID = null;
         else
            director.DirectorID = (int)Convert.ChangeType(row["DirectorID"], typeof(int));
         if (row.IsNull("Curp"))
             director.Curp = null;
         else
             director.Curp = (string)Convert.ChangeType(row["Curp"], typeof(string));
         if (row.IsNull("Nombre"))
             director.Nombre = null;
         else
             director.Nombre = (string)Convert.ChangeType(row["Nombre"], typeof(string));
         if (row.IsNull("PrimerApellido"))
             director.PrimerApellido = null;
         else
             director.PrimerApellido = (string)Convert.ChangeType(row["PrimerApellido"], typeof(string));
         if (row.IsNull("SegundoApellido"))
             director.SegundoApellido = null;
         else
             director.SegundoApellido = (string)Convert.ChangeType(row["SegundoApellido"], typeof(string));
         if (row.IsNull("FechaNacimiento"))
             director.FechaNacimiento = null;
         else
             director.FechaNacimiento = (DateTime)Convert.ChangeType(row["FechaNacimiento"], typeof(DateTime));
         if (row.IsNull("Sexo"))
             director.Sexo = null;
         else
             director.Sexo = (bool)Convert.ChangeType(row["Sexo"], typeof(bool));
         if (row.IsNull("NivelEscolar"))
             director.NivelEscolar = null;
         else
             director.NivelEscolar = (string)Convert.ChangeType(row["NivelEscolar"], typeof(string));
         if (row.IsNull("Correo"))
             director.Correo = null;
         else
             director.Correo = (string)Convert.ChangeType(row["Correo"], typeof(string));
         if (row.IsNull("Telefono"))
             director.Telefono = null;
         else
             director.Telefono = (string)Convert.ChangeType(row["Telefono"], typeof(string));
         if (row.IsNull("FechaRegistro"))
             director.FechaRegistro = null;
         else
             director.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
         if (row.IsNull("Estatus"))
             director.Estatus = null;
         else
             director.Estatus = (bool)Convert.ChangeType(row["Estatus"], typeof(bool));
         if (row.IsNull("Clave"))
             director.Clave = null;
         else
             director.Clave = (string)Convert.ChangeType(row["Clave"], typeof(string));
         if (row.IsNull("EstatusIdentificacion"))
             director.EstatusIdentificacion = null;
         else
             director.EstatusIdentificacion = (bool)Convert.ChangeType(row["EstatusIdentificacion"], typeof(bool));
         return director;
      }

      /// <summary>
      /// Identifica si existe un docente con identificado con la misma CURP
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="director">El Director </param>
      /// <returns></returns>
      private bool NoRepetido(IDataContext dctx, Director director)
      {
          DataSet ds = Retrieve(dctx, new Director {Curp = director.Curp});
          return ds.Tables[0].Rows.Count <= 0;
      }
   } 
}
